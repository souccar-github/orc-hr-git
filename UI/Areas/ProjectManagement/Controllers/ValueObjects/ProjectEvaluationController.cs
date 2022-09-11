#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.ProjectManagment.Entities;
using HRIS.Domain.ProjectManagment.ValueObjects;
using Resources.Areas.ProjectManagment.Views.Shared;
using Resources.Shared.Messages;
using Service.PMSComprehensive;
using Service.OrgChart;
using Service.Personnel;
using UI.Areas.ProjectManagement.Controllers.EntitiesRoots;
using UI.Extensions;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.ProjectManagement.ValueObjects;

#endregion

namespace UI.Areas.ProjectManagement.Controllers.ValueObjects
{
    public class ProjectEvaluationController : ProjectAggregateController, IRule<ProjectEvaluation>
    {
        #region Parents Chain

        #region Objective

        private Project _project;

        public Project FirstEntity
        {
            get
            {
                return _project ??
                       (_project = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First)));
            }
        }

        #endregion

        #region ProjectEvaluation

        private ProjectEvaluation _projectEvaluation;

        public ProjectEvaluation SecondEntity
        {
            get
            {
                return _projectEvaluation ??
                       (_projectEvaluation =
                        FirstEntity.ProjectEvaluations.SingleOrDefault(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<ProjectEvaluation>

        public ObjectRules<ProjectEvaluation> Rules
        {
            get { return new ProjectEvaluationRules(); }
        }

        #endregion

        #region Overrides of ProjectAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("Status.Name");
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                FirstEntity.ProjectEvaluations.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FirstEntity.ProjectEvaluations.Count != 0
                       ? Rules.GetExpiredRules(FirstEntity.ProjectEvaluations)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region Utilities

        public ActionResult ClearSelection()
        {
            SetMasterRecordValue(MasterRecordOrder.Second, 0);

            return RedirectToAction("MasterIndex");
        }

        #endregion

        #region Master Project Evaluation

        public ActionResult MasterIndex(int id = 0)
        {
            if (id != 0)
            {
                SetMasterRecordValue(MasterRecordOrder.Second, id);
                CurrentlyInSecondLevel = id;
            }

            PrePublish();

            AddToPath(masterRecordOrder: MasterRecordOrder.Second, level: RibbonLevels.C, actionName: "MasterIndex",
                      stepId: GetMasterRecordValue(MasterRecordOrder.Second),
                      areaName: ProjectManagementAreaRegistration.GetAreaName, nodeName: Navigator.ProjectEvaluation);

            #region Get Data

            int pageNo = 1;
            if (GetMasterRecordValue(MasterRecordOrder.Second) != 0)
            {
                int masterRecordValue = GetMasterRecordValue(MasterRecordOrder.Second);

                int count =
                    FirstEntity.ProjectEvaluations.Where(
                        projectEvaluation => (projectEvaluation.Id >= masterRecordValue)).Count();

                pageNo = count/5;

                if (count%5 > 0)
                {
                    pageNo++;
                }
            }

            ViewData["evaluations"] = FirstEntity.ProjectEvaluations;
            ViewData["SelectedRow"] = GetMasterRecordValue(MasterRecordOrder.Second);
            ViewData["EvaluatedPhaseSelectedRow"] = GetMasterRecordValue(MasterRecordOrder.Third);
            ViewData["PageTo"] = pageNo;

            #endregion

            return View("MasterIndex");
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Second, selectedSubRowId);

            CurrentlyInSecondLevel = selectedSubRowId;

            SaveTabIndex(6);

            PrePublish();

            if (FirstEntity.ProjectEvaluations.Count > 1)
            {
                if (FirstEntity.ProjectEvaluations.Max(i => i.Id) != selectedSubRowId)
                {
                    ViewData["CanUpdate"] = false;
                }
            }

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            #region Check Permission

            Employee employee;

            try
            {
                employee =  EmployeeHelpers.GetByLoginName(HttpContext.User.Identity.Name);
            }
            catch (InvalidOperationException exception)
            {
                return
                    ErrorPartialMessage(
                        Resources.Areas.ProjectManagment.ValueObjects.ProjectEvaluation.ProjectEvaluationRules.
                            NoLoginName);
            }


            if (employee == null)
            {
                return
                    ErrorPartialMessage(
                        Resources.Areas.ProjectManagment.ValueObjects.ProjectEvaluation.ProjectEvaluationRules.
                            ErrorLoginInfo);
            }

            TeamMember teamMember =  EmployeeHelpers.GetProjectMembership(employee.Id,
                                                                               FirstEntity.Id);

            if (teamMember == null)
            {
                return
                    ErrorPartialMessage(
                        Resources.Areas.ProjectManagment.ValueObjects.ProjectEvaluation.ProjectEvaluationRules.
                            NoProjectMembershipAssigned);
            }

            if (teamMember.IsEvaluator == false)
            {
                return
                    ErrorPartialMessage(
                        Resources.Areas.ProjectManagment.ValueObjects.ProjectEvaluation.ProjectEvaluationRules.
                            NotAuthorizedToEvaluate);
            }

            if (FirstEntity.ProjectPhases.Count == 0)
            {
                return
                    ErrorPartialMessage(
                        Resources.Areas.ProjectManagment.ValueObjects.ProjectEvaluation.ProjectEvaluationRules.
                            NoProjectPhases);
            }

            #endregion

            #region Defaults Values

            var projectEvaluation = new ProjectEvaluation {Date = DateTime.Now};

            #region Employee

            projectEvaluation.Evaluator = employee;

            #endregion

            #region Quarter

            if (DateTime.Now.Month <= 3)
            {
                projectEvaluation.Quarter = 1;
            }
            else if (DateTime.Now.Month <= 6)
            {
                projectEvaluation.Quarter = 2;
            }
            else if (DateTime.Now.Month <= 9)
            {
                projectEvaluation.Quarter = 3;
            }
            else if (DateTime.Now.Month <= 12)
            {
                projectEvaluation.Quarter = 4;
            }

            #endregion

            #region EvaluatorProjectRole

            projectEvaluation.EvaluatorProjectRole = teamMember.ProjectTeamRole.Role.Name;

            #endregion

            #endregion

            return PartialView("Edit", projectEvaluation);
        }

        public ActionResult Save(ProjectEvaluation projectEvaluation)
        {
            PrePublish();

            bool insertMode = true;
            if (projectEvaluation.IsTransient())
            {
                FirstEntity.AddEvaluation(projectEvaluation);
            }
            else
            {
                insertMode = false;

                #region Retrieve Direct Parent

                projectEvaluation.Project = SecondEntity.Project;

                #endregion

                this.UpdateValueObject(projectEvaluation, SecondEntity);

                this.StringDecode(SecondEntity);
            }

            if ((Rules.GetBrokenRules(projectEvaluation).Count == 0)) //&& (TryValidateModel(projectEvaluation)))
            {
                Service.Update(FirstEntity);

                if (insertMode)
                {
                    ProjectPhaseToEvaluatedPhase.Clone(FirstEntity.Id, projectEvaluation.Id);
                }
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(projectEvaluation));

                FirstEntity.ProjectEvaluations.Remove(projectEvaluation);

                #region Evaluator Details

                Employee employee =  EmployeeHelpers.GetByLoginName(HttpContext.User.Identity.Name);
                projectEvaluation.Evaluator = employee;

                //TODO ProjectEvaluationController -- Need Test -- The implementation covers single position case only 
                projectEvaluation.EvaluatorPosition =
                     EmployeeHelpers.GetPositions(employee.Id).First().Position.JobTitle.Name;

                #endregion

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", projectEvaluation)
                                });
            }

            SetMasterRecordValue(MasterRecordOrder.Second, projectEvaluation.Id);

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", projectEvaluation)
                            });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                ProjectEvaluation projectEvaluation = FirstEntity.ProjectEvaluations.SingleOrDefault(c => c.Id == id);

                try
                {
                    if (FirstEntity.ProjectEvaluations.Max(x => x.Id) != id)
                    {
                        return
                            ErrorPartialMessage(
                                Resources.Areas.ProjectManagment.ValueObjects.ProjectEvaluation.ProjectEvaluationRules.
                                    CanDeleteOnlyLastestEvaluationRecord);
                    }

                    FirstEntity.ProjectEvaluations.Remove(projectEvaluation);

                    Service.Update(FirstEntity);

                    PrePublish();

                    SetMasterRecordValue(MasterRecordOrder.Second, 0);

                    return RedirectToAction("Index", "Project");
                }
                catch (Exception)
                {
                    return ErrorPartialMessage(General.ErrorDuringDelete);
                }
            }

            return ErrorPartialMessage(General.ErrorDuringDelete);
        }

        public ActionResult JsonEdit()
        {
            return PartialView("Edit", SecondEntity);
        }

        #endregion
    }
}