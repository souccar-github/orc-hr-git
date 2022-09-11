#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.ProjectManagment.Entities;
using HRIS.Domain.ProjectManagment.ValueObjects;
using UI.Areas.ProjectManagement.Controllers.EntitiesRoots;
using UI.Extensions;
using UI.Helpers.Cache;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.ProjectManagement.ValueObjects;
using UI.Areas.ProjectManagement.Helpers;

#endregion

namespace UI.Areas.ProjectManagement.Controllers.ValueObjects
{
    public class PhaseTaskController : ProjectAggregateController, IRule<PhaseTask>
    {
        #region Parents Chain

        #region Project

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

        #region ProjectPhase

        private ProjectPhase _projectPhase;

        public ProjectPhase SecondEntity
        {
            get
            {
                return _projectPhase ??
                       (_projectPhase =
                        FirstEntity.ProjectPhases.Single(r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #region PhaseTask

        private PhaseTask _phaseTask;

        public PhaseTask ThirdEntity
        {
            get
            {
                return _phaseTask ??
                       (_phaseTask =
                        SecondEntity.Tasks.SingleOrDefault(
                            k => k.Id == GetMasterRecordValue(MasterRecordOrder.Third)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<EvaluatedObjectiveStep>

        public ObjectRules<PhaseTask> Rules
        {
            get { return new PhaseTaskRules(); }
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
                SecondEntity.Tasks.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Third));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return SecondEntity.Tasks.Count != 0
                       ? Rules.GetExpiredRules(SecondEntity.Tasks)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region Utilities

        private void GetProjectIdValue()
        {
            TempData["projectId"] = FirstEntity.Id;
        }

        public ActionResult GetProjectTeamRoles(int projectTeamId)
        {
            DropDownListHelpers.ListOfProjectTeamRoles(projectTeamId);

            CacheProvider.Set(ProjectCacheKeys.TeamMembers.ToString(), new List<TeamMember>());

            return Json(new
            {
                Success = true,
                PartialViewHtml1 = RenderPartialViewToString("Components/TeamRoleList"),
                PartialViewHtml2 = RenderPartialViewToString("Components/TeamMemberList")

            });
        }

        public ActionResult GetTeamMembers(int projectTeamRoleId)
        {
            DropDownListHelpers.ListOfTeamMembers(projectTeamRoleId);

            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("Components/TeamMemberList")
            });
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Third, selectedSubRowId);

            PrePublish();

            CacheProvider.Set(ProjectCacheKeys.TeamMembers.ToString(), new List<TeamMember>());
            CacheProvider.Set(ProjectCacheKeys.ProjectTeamRole.ToString(), new List<ProjectTeamRole>());

            SaveTabIndexSecondLevel(0);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            GetProjectIdValue();

            return PartialView("Edit", new PhaseTask());
        }

        public ActionResult Save(PhaseTask phaseTask)
        {
            PrePublish();

            if (phaseTask.IsTransient())
            {
                SecondEntity.AddTask(phaseTask);
            }
            else
            {
                #region Retrieve Direct Parent

                phaseTask.ProjectPhase = ThirdEntity.ProjectPhase;

                #endregion

                this.UpdateValueObject(phaseTask, ThirdEntity);

                this.StringDecode(ThirdEntity);
            }

            #region Dates

            if (phaseTask.ActualClosingDate == DateTime.MinValue)
            {
                phaseTask.ActualClosingDate = new DateTime(1800, 1, 1);

                ModelState.Remove("ActualClosingDate");
            }

            if (phaseTask.DeadLine == DateTime.MinValue)
            {
                phaseTask.DeadLine = new DateTime(1800, 1, 1);

                ModelState.Remove("DeadLine");
            }

            #endregion

            if ((Rules.GetBrokenRules(phaseTask).Count == 0) && (TryValidateModel(phaseTask)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(phaseTask));

                SecondEntity.Tasks.Remove(phaseTask);

                GetProjectIdValue();

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", phaseTask)
                                });
            }

            SetMasterRecordValue(MasterRecordOrder.Third, phaseTask.Id);

            PrePublish();

            GetProjectIdValue();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", phaseTask)
                            });
        }

        public ActionResult JsonEdit()
        {
            GetProjectIdValue();

            return PartialView("Edit", ThirdEntity);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                PhaseTask phaseTask =
                    SecondEntity.Tasks.SingleOrDefault(i => i.Id == id);

                try
                {
                    SecondEntity.Tasks.Remove(phaseTask);

                    Service.Update(FirstEntity);

                    PrePublish();

                    return RedirectToAction("MasterIndex", "ProjectPhase");
                }
                catch (Exception)
                {
                    return ErrorPartialMessage(Resources.Shared.Messages.General.ErrorDuringDelete);
                }
            }

            return ErrorPartialMessage(Resources.Shared.Messages.General.ErrorDuringDelete);
        }

        #endregion
    }
}