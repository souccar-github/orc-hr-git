#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.OrgChart.Entities;
using HRIS.Domain.OrgChart.ValueObjects;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.ProjectManagment.Entities;
using HRIS.Domain.ProjectManagment.ValueObjects;
using Service;
using UI.Areas.ProjectManagement.Controllers.EntitiesRoots;
using UI.Extensions;
using UI.Helpers;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.ProjectManagement.ValueObjects;

#endregion

namespace UI.Areas.ProjectManagement.Controllers.ValueObjects
{
    public class TeamMemberController : ProjectAggregateController, IRule<TeamMember>
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

        #region TeamRole

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

        #region Team Member

        private TeamMember _teamMember;

        public TeamMember FourthEntity
        {
            get
            {
                return _teamMember ??
                       (_teamMember =
                        ThirdEntity.TeamMembers.SingleOrDefault(
                            k => k.Id == GetMasterRecordValue(MasterRecordOrder.Fourth)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<TeamMembers>

        public ObjectRules<TeamMember> Rules
        {
            get { return new TeamMembersRules(); }
        }

        #endregion

        #region Overrides of ProjectAggregateController

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                ThirdEntity.TeamMembers.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Fourth));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return ThirdEntity.TeamMembers.Count != 0
                       ? Rules.GetExpiredRules(ThirdEntity.TeamMembers)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Fourth, selectedSubRowId);

            PrePublish();

            SaveTabIndexSecondLevel(0);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            SetRelatedNodeToTheSession(0);

            CurrentlyInSecondLevel = 1;

            return PartialView("Edit", new TeamMember());
        }

        [HttpPost]
        public ActionResult Save(TeamMember teamMember)
        {
            PrePublish();

            #region Check Entity Readiness

            if (RelatedNode == 0 | RelatedPosition == 0)
            {
                var error = new List<BrokenBusinessRule>
                                {
                                    new BrokenBusinessRule("Id", Resources.Areas.ProjectManagment.ValueObjects.TeamMember.TeamMemberRules.NoNodeSelected.ToLower())
                                };

                ModelState.AddModelErrors(error);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", teamMember)
                                });
            }

            #endregion

            teamMember.Node = new EntityService<Node>().LoadById(RelatedNode);
            teamMember.Position = new EntityService<Position>().LoadById(RelatedPosition);
            teamMember.Employee = new EntityService<Employee>().LoadById(RelatedEmployee);

            #region Check Employee Existance

            if (ThirdEntity.TeamMembers.Any(exist => exist.Employee.Id == teamMember.Employee.Id) && teamMember.IsTransient())
            {
                var error = new List<BrokenBusinessRule>
                                {
                                    new BrokenBusinessRule("Id",
                                                           Resources.Areas.ProjectManagment.ValueObjects.TeamMember.TeamMemberRules.EmployeeAlreadyDefined.ToLower())
                                };

                ModelState.AddModelErrors(error);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", teamMember)
                                });
            }

            #endregion

            if (teamMember.IsTransient())
            {
                ThirdEntity.AddIndirectRole(teamMember);
            }
            else
            {
                #region Retrieve Direct Parent

                teamMember.ProjectTeamRole = FourthEntity.ProjectTeamRole;
                teamMember.Employee = FourthEntity.Employee;

                #endregion

                this.UpdateValueObject(teamMember, FourthEntity);

                this.StringDecode(FourthEntity);
            }


            teamMember.IsCross = teamMember.Node.Id != FirstEntity.Node.Id;

            if ((Rules.GetBrokenRules(teamMember).Count == 0)) // && (TryValidateModel(teamMember)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(teamMember));

                ThirdEntity.TeamMembers.Remove(teamMember);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", teamMember)
                                });
            }

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", teamMember)
                            });
        }

        public ActionResult JsonEdit()
        {
            return PartialView("Edit", FourthEntity);
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                try
                {
                    TeamMember teamMember = ThirdEntity.TeamMembers.Single(k => k.Id == id);

                    ThirdEntity.TeamMembers.Remove(teamMember);

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

        #region Tree

        public ActionResult NodeToJson()
        {
            Node node = RelatedNode != 0
                            ? new EntityService<Node>().LoadById(RelatedNode)
                            : new EntityService<Organization>().GetAll().Single().RootNode.Single();

            string result = node.ToString();

            return Json(new
                            {
                                Success = true,
                                NodeId = node.Id,
                                NodeCode = node.Code,
                                Message = result
                            });
        }

        public ActionResult BackOneLevel(int reset = 0)
        {
            if (RelatedNode != 0 & reset == 0)
            {
                Node node = new EntityService<Node>().LoadById(RelatedNode);
                SetRelatedNodeToTheSession(node.Parent.Id);
            }
            else
            {
                SetRelatedNodeToTheSession(0);
            }

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("Components/Tree")
                            });
        }

        #endregion

        #region DropDownList Helpers

        public ActionResult GetEmployees(int positionId)
        {
            DropDownListHelpers.ListOfSelectedPositionEmployees(positionId);

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("Components/PositionEmployeesList")
                            });
        }

        public ActionResult GetPositions(int nodeId)
        {
            DropDownListHelpers.ListOfSelectedNodePosition(nodeId);

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("Components/NodePositionList")
                            });
        }

        #endregion
    }
}