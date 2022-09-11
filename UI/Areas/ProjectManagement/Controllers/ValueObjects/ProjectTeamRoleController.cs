#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.OrgChart.Entities;
using HRIS.Domain.ProjectManagment.Entities;
using HRIS.Domain.ProjectManagment.ValueObjects;
using Service;
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
    public class ProjectTeamRoleController : ProjectAggregateController, IRule<ProjectTeamRole>
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

        #region Project Team

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

        #region Project Team Role

        private ProjectTeamRole _projectTeamRole;

        public ProjectTeamRole ThirdEntity
        {
            get
            {
                return _projectTeamRole ??
                       (_projectTeamRole =
                        SecondEntity.ProjectTeamRoles.SingleOrDefault(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Third)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<Responsibility>

        public ObjectRules<ProjectTeamRole> Rules
        {
            get { return new ProjectTeamRoleRules(); }
        }

        #endregion

        #region Overrides of JobDescAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("ParentRole.Name");
            ModelState.Remove("Role.Name");
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                SecondEntity.ProjectTeamRoles.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Third));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return SecondEntity.ProjectTeamRoles.Count != 0
                       ? Rules.GetExpiredRules(SecondEntity.ProjectTeamRoles)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Third, selectedSubRowId);

            PrePublish();
            CacheProvider.Set(ProjectCacheKeys.TeamMembers.ToString(),new List<TeamMember>());
            CacheProvider.Set(ProjectCacheKeys.ProjectTeamRole.ToString(),new List<ProjectTeamRole>());

            SaveTabIndexSecondLevel(0);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            GetProjectIdValue();

            return PartialView("Edit", new ProjectTeamRole());
        }

        public ActionResult Save(ProjectTeamRole projectTeamRole)
        {
            PrePublish();

            if (projectTeamRole.IsTransient())
            {
                #region CheckWeight

                CheckWeight(projectTeamRole, false);

                #endregion

                SecondEntity.AddProjectTeamRole(projectTeamRole);
            }
            else
            {
                #region Retrieve Direct Parent

                projectTeamRole.ProjectTeam = ThirdEntity.ProjectTeam;

                #endregion

                this.UpdateValueObject(projectTeamRole, ThirdEntity);

                #region Check Weights

                CheckWeight(projectTeamRole, true);

                #endregion

                this.StringDecode(ThirdEntity);
            }

            #region Indirect Member

            if (projectTeamRole.IndirectProjectTeam.IsTransient())
            {
                projectTeamRole.IndirectProjectTeam = null;
                ThirdEntity.IndirectProjectTeam = null;
            } 
            


            if (projectTeamRole.IndirectProjectTeamRole.IsTransient())
            {
                projectTeamRole.IndirectProjectTeamRole = null;
                ThirdEntity.IndirectProjectTeamRole = null;
            }

            if (projectTeamRole.IndirectTeamMember.IsTransient())
            {
                projectTeamRole.IndirectTeamMember = null;
                ThirdEntity.IndirectTeamMember = null;
            }

            #endregion

            if ((Rules.GetBrokenRules(projectTeamRole).Count == 0)) //&& (TryValidateModel(projectTeamRole)))
            {
                projectTeamRole.ProjectTeam = ThirdEntity.ProjectTeam;

                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(projectTeamRole));

                SecondEntity.ProjectTeamRoles.Remove(projectTeamRole);

                GetProjectIdValue();

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", projectTeamRole)
                                });
            }

            SetMasterRecordValue(MasterRecordOrder.Third, projectTeamRole.Id);

            PrePublish();

            GetProjectIdValue();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", projectTeamRole)
                            });
        }

        public ActionResult JsonEdit()
        {
            GetProjectIdValue();


            //CacheProvider.Set(ProjectCacheKeys.TeamMembers.ToString(), new List<TeamMember>());

            return PartialView("Edit", ThirdEntity);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                ProjectTeamRole projectTeamRole = SecondEntity.ProjectTeamRoles.SingleOrDefault(i => i.Id == id);

                try
                {
                    SecondEntity.ProjectTeamRoles.Remove(projectTeamRole);

                    Service.Update(FirstEntity);

                    PrePublish();

                    return RedirectToAction("MasterIndex", "ProjectTeam");
                }
                catch (Exception)
                {
                    SetGlobalErrorMessage(Resources.Shared.Messages.General.EntityCurrentlyInUse);

                    return RedirectToAction("MasterIndex", "ProjectTeam");
                }
            }

            SetGlobalErrorMessage(Resources.Shared.Messages.General.ErrorDuringDelete);

            return RedirectToAction("MasterIndex", "ProjectTeam");
        }

        #endregion

        #region Utilities

        public ActionResult GetProjectTeamRoles(int id)
        {
            DropDownListHelpers.ListOfProjectTeamRoles(id, GetMasterRecordValue(MasterRecordOrder.Third));

            CacheProvider.Set(ProjectCacheKeys.TeamMembers.ToString(), new List<TeamMember>());

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml1 = RenderPartialViewToString("Components/TeamRoleList"),
                                PartialViewHtml2 = RenderPartialViewToString("Components/TeamMemberList")
                                
                            });
        }

        public ActionResult GetTeamMembers(int id)
        {
            DropDownListHelpers.ListOfTeamMembers(id);

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("Components/TeamMemberList")
                            });
        }

        public ActionResult NodeToJson()
        {
            var node = new EntityService<Organization>().GetAll().Single().RootNode.Single();

            string result = node.ToString();

            return Json(new
                            {
                                Success = true,
                                NodeId = node.Id,
                                NodeCode = node.Code,
                                Message = result
                            });
        }

        private void GetProjectIdValue()
        {
            TempData["projectId"] = FirstEntity.Id;
            TempData["projectTeamId"] = SecondEntity.Id;
            TempData["projectTeamRoleId"] = GetMasterRecordValue(MasterRecordOrder.Third);
        }


        public void CheckWeight(ProjectTeamRole projectTeamRole, bool isUpdate)
        {
            var list = SecondEntity.ProjectTeamRoles.ToList();
            float totalWeigh = 0;

            if (isUpdate)
            {
                totalWeigh = list.Sum(teamRole => teamRole.Weight);
            }
            else
            {
                totalWeigh = list.Sum(teamRole => teamRole.Weight);
                totalWeigh += projectTeamRole.Weight;
            }
            if (totalWeigh > 100)
            {
                var error = new List<BrokenBusinessRule>
                                {
                                    new BrokenBusinessRule("Weight",
                                                           Resources.Areas.ProjectManagment.ValueObjects.ProjectTeamRole.ProjectTeamRoleRules.WeightRule1.ToLower())
                                };

                ModelState.AddModelErrors(error);
            }
        }

        #endregion
    }
}