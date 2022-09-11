#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.ProjectManagment.Entities;
using HRIS.Domain.ProjectManagment.ValueObjects;
using UI.Areas.ProjectManagement.Controllers.EntitiesRoots;
using UI.Areas.ProjectManagement.Helpers;
using UI.Extensions;
using UI.Helpers.Cache;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.ProjectManagement.ValueObjects;

#endregion

namespace UI.Areas.ProjectManagement.Controllers.ValueObjects
{
    public class ProjectPhaseController : ProjectAggregateController, IRule<ProjectPhase>
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

        #region Project Phases

        private ProjectPhase _projectPhase;

        public ProjectPhase SecondEntity
        {
            get
            {
                return _projectPhase ??
                       (_projectPhase =
                        FirstEntity.ProjectPhases.SingleOrDefault(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<ProjectPhase>

        public ObjectRules<ProjectPhase> Rules
        {
            get { return new ProjectPhaseRules(); }
        }

        #endregion

        #region Overrides of ObjectiveAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("Status.Name");
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                FirstEntity.ProjectPhases.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FirstEntity.ProjectPhases.Count != 0
                       ? Rules.GetExpiredRules(FirstEntity.ProjectPhases)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region Utilities

        public ActionResult ClearSelection()
        {
            SetMasterRecordValue(MasterRecordOrder.Second, 0);

            return RedirectToAction("MasterIndex");
        }

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

        #region Master Project Phase

        public ActionResult MasterIndex(int id = 0)
        {
            if (id != 0)
            {
                SetMasterRecordValue(MasterRecordOrder.Second, id);
                CurrentlyInSecondLevel = id;
            }

            PrePublish();

            AddToPath(masterRecordOrder: MasterRecordOrder.Second, level: RibbonLevels.B, actionName: "MasterIndex",
                      stepId: GetMasterRecordValue(MasterRecordOrder.Second),
                      areaName: ProjectManagementAreaRegistration.GetAreaName, nodeName: Resources.Areas.ProjectManagment.Views.Shared.Navigator.ProjectPhase);

            #region Get Data

            int pageNo = 1;
            if (GetMasterRecordValue(MasterRecordOrder.Second) != 0)
            {
                int masterRecordValue = GetMasterRecordValue(MasterRecordOrder.Second);

                int count =
                    FirstEntity.ProjectPhases.Where(projectPhase => (projectPhase.Id >= masterRecordValue)).Count();

                pageNo = count/5;

                if (count%5 > 0)
                {
                    pageNo++;
                }
            }

            ViewData["projectPhases"] = FirstEntity.ProjectPhases;
            ViewData["SelectedRow"] = GetMasterRecordValue(MasterRecordOrder.Second);
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

            SaveTabIndex(1);

            PrePublish();

            CacheProvider.Set(ProjectCacheKeys.TeamMembers.ToString(), new List<TeamMember>());
            CacheProvider.Set(ProjectCacheKeys.ProjectTeamRole.ToString(), new List<ProjectTeamRole>());

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            GetProjectIdValue();

            return PartialView("Edit", new ProjectPhase {Project = FirstEntity});
        }

        public ActionResult Save(ProjectPhase projectPhase)
        {
            PrePublish();

            if (projectPhase.IsTransient())
            {
                FirstEntity.AddProjectPhase(projectPhase);
            }
            else
            {
                #region Retrieve Direct Parent

                projectPhase.Project = SecondEntity.Project;

                #endregion

                this.UpdateValueObject(projectPhase, SecondEntity);

                this.StringDecode(SecondEntity);
            }

            if ((Rules.GetBrokenRules(projectPhase).Count == 0) && (TryValidateModel(projectPhase)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(projectPhase));

                FirstEntity.ProjectPhases.Remove(projectPhase);

                GetProjectIdValue();

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", projectPhase)
                                });
            }

            SetMasterRecordValue(MasterRecordOrder.Second, projectPhase.Id);

            PrePublish();

            GetProjectIdValue();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", projectPhase)
                            });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                ProjectPhase projectPhase = FirstEntity.ProjectPhases.SingleOrDefault(c => c.Id == id);

                try
                {
                    FirstEntity.ProjectPhases.Remove(projectPhase);

                    Service.Update(FirstEntity);

                    PrePublish();

                    SetMasterRecordValue(MasterRecordOrder.Second, 0);

                    return RedirectToAction("Index", "Project");
                }
                catch (Exception)
                {
                    return ErrorPartialMessage(Resources.Shared.Messages.General.ErrorDuringDelete);
                }
            }

            return ErrorPartialMessage(Resources.Shared.Messages.General.ErrorDuringDelete);
        }

        public ActionResult JsonEdit()
        {
            GetProjectIdValue();

            return PartialView("Edit", SecondEntity);
        }

        #endregion
    }
}