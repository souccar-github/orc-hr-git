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
using UI.Helpers.Model;
using UI.Utilities;
using Validation.ProjectManagement.ValueObjects;

#endregion

namespace UI.Areas.ProjectManagement.Controllers.ValueObjects
{
    public class ProjectTeamController : ProjectAggregateController, IRule<ProjectTeam>
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

        #region ProjectTeam

        private ProjectTeam _projectTeam;

        public ProjectTeam SecondEntity
        {
            get
            {
                return _projectTeam ??
                       (_projectTeam =
                        FirstEntity.ProjectTeams.SingleOrDefault(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<Evaluation>

        public ObjectRules<ProjectTeam> Rules
        {
            get { return new ProjectTeamRules(); }
        }

        #endregion

        #region Overrides of ProjectAggregateController

        public override void CleanUpModelState()
        {
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                FirstEntity.ProjectTeams.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FirstEntity != null && FirstEntity.ProjectTeams.Count != 0
                       ? Rules.GetExpiredRules(FirstEntity.ProjectTeams)
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

        #region Master Project Team

        public ActionResult MasterIndex(int id = 0)
        {
            if (id != 0)
            {
                SetMasterRecordValue(MasterRecordOrder.Second, id);
                CurrentlyInSecondLevel = id;
            }

            PrePublish();

            AddToPath(masterRecordOrder: MasterRecordOrder.Second, level: RibbonLevels.A, actionName: "MasterIndex",
                      stepId: GetMasterRecordValue(MasterRecordOrder.Second),
                      areaName: ProjectManagementAreaRegistration.GetAreaName, nodeName: Resources.Areas.ProjectManagment.Views.Shared.Navigator.ProjectTeam);

            #region Get Data

            int pageNo = 1;
            if (GetMasterRecordValue(MasterRecordOrder.Second) != 0)
            {
                int masterRecordValue = GetMasterRecordValue(MasterRecordOrder.Second);

                int count = FirstEntity.ProjectTeams.Where(team => (team.Id >= masterRecordValue)).Count();

                pageNo = count/5;

                if (count%5 > 0)
                {
                    pageNo++;
                }
            }

            ViewData["ProjectTeams"] = FirstEntity.ProjectTeams;
            ViewData["SelectedRow"] = GetMasterRecordValue(MasterRecordOrder.Second);
            ViewData["ProjectTeamRolesSelectedRow"] = GetMasterRecordValue(MasterRecordOrder.Third);
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

            SaveTabIndex(0);

            PrePublish();

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new ProjectTeam());
        }

        public ActionResult Save(ProjectTeam projectTeam)
        {
            PrePublish();

            if (projectTeam.IsTransient())
            {
                FirstEntity.AddProjectTeam(projectTeam);
            }
            else
            {
                #region Retrieve Direct Parent

                projectTeam.Project = SecondEntity.Project;

                #endregion

                this.UpdateValueObject(projectTeam, SecondEntity);

                this.StringDecode(SecondEntity);
            }

            if ((Rules.GetBrokenRules(projectTeam).Count == 0) && (TryValidateModel(projectTeam)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(projectTeam));

                FirstEntity.ProjectTeams.Remove(projectTeam);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", projectTeam)
                                });
            }

            SetMasterRecordValue(MasterRecordOrder.Second, projectTeam.Id);

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", projectTeam)
                            });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                ProjectTeam projectTeam = FirstEntity.ProjectTeams.SingleOrDefault(c => c.Id == id);

                try
                {
                    FirstEntity.ProjectTeams.Remove(projectTeam);

                    Service.Update(FirstEntity);

                    PrePublish();

                    SetMasterRecordValue(MasterRecordOrder.Second, 0);

                    return RedirectToAction("Index", "Project");
                }
                catch (Exception)
                {
                    SetGlobalErrorMessage(Resources.Shared.Messages.General.EntityCurrentlyInUse);

                    return RedirectToAction("Index", "Project");
                }
            }

            SetGlobalErrorMessage(Resources.Shared.Messages.General.ErrorDuringDelete);

            return RedirectToAction("Index", "Project");
        }

        public ActionResult JsonEdit()
        {
            return PartialView("Edit", SecondEntity);
        }

        #endregion
    }
}