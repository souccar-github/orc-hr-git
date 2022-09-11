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
using Resources.Areas.ProjectManagment.Views.Shared;
using Resources.Shared.Messages;
using Service;
using Telerik.Web.Mvc;
using UI.Areas.ProjectManagement.Controllers.EntitiesRoots;
using UI.Areas.ProjectManagement.Helpers;
using UI.Helpers.Cache;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.ProjectManagement.Entities;

#endregion

namespace UI.Areas.ProjectManagement.Controllers.Entities
{
    public class ProjectController : ProjectAggregateController, IRule<Project>
    {
        #region IRule<Project> Members

        public ObjectRules<Project> Rules
        {
            get { return new ProjectRules(); }
        }

        #endregion

        #region Overrides of ProjectAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("Type.Name");
        }

        #endregion

        #region CRUD

        #region Read

        [GridAction]
        public ActionResult Index(int id = 0, int selectedTabOrder = 0, bool ribbon = false,
                                  bool ribbonSubEntity = false)
        {
            PrePublish();

            #region Security

            if (ViewData["CanRead"] != null && !(bool) ViewData["CanRead"])
            {
                return ErrorPartialMessage(General.CanReadMessage);
            }

            #endregion

            #region Manage Tab, Path, and MastersList

            if (ribbonSubEntity)
            {
                SetMasterRecordValue(MasterRecordOrder.Second, 0);
                CurrentlyInSecondLevel = 0;
            }

            if (ribbon)
            {
                ClearMasterRecords();
                SaveTabIndex(0);
                SetRelatedNodeToTheSession(0);
            }
            else
            {
                if (id != 0)
                {
                    SetMasterRecordValue(MasterRecordOrder.First, id);
                }
            }


            AddToPath(MasterRecordOrder.First, RibbonLevels.Root, stepId: id,
                      areaName: ProjectManagementAreaRegistration.GetAreaName, nodeName: Navigator.ProjectManagement);

            #endregion

            #region Get Data

            IQueryable<Project> projects = RelatedPosition != 0
                                               ? Service.GetAll().Where(w => w.Position.Id == RelatedPosition)
                                               : Service.GetAll();

            int pageNo = 1;

            if (GetMasterRecordValue(MasterRecordOrder.First) != 0)
            {
                int masterRecordValue = GetMasterRecordValue(MasterRecordOrder.First);

                int count = projects.Count(project => (project.Id >= masterRecordValue));

                pageNo = count/5;

                if (count%5 > 0)
                {
                    pageNo++;
                }

                pageNo = pageNo == 0 ? 1 : pageNo;
            }

            ViewData["projects"] = projects;
            ViewData["SelectedRow"] = GetMasterRecordValue(MasterRecordOrder.First);
            ViewData["PageTo"] = pageNo;
            ViewData["ReadyToAddProject"] = RelatedPosition != 0;

            #endregion

            return View();
        }

        public ActionResult PartialMasterInfo(int selectedRowId = 0)
        {
            PrePublish();

            if (selectedRowId != 0)
            {
                SetMasterRecordValue(MasterRecordOrder.First, selectedRowId);
                CurrentlyInFirstLevel = true;
            }

            Project project =
                Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First));

            return PartialView("BasicInfo", project);
        }

        #endregion

        #region Create

        public ActionResult Insert()
        {
            LoadStepsList();

            ViewData["NodeChooserPosition"] = "Test";

            AddToPath(MasterRecordOrder.First, RibbonLevels.Root,
                      areaName: ProjectManagementAreaRegistration.GetAreaName,
                      nodeName: Navigator.ProjectManagement);

            var project = new Project {Node = new Node(), Position = new Position(), Owner = new Employee()};

            return View("Insert", project);
        }

        [HttpPost]
        public ActionResult JsonInsert(Project project)
        {
            PrePublish();

            #region Check Core Entity Relations

            #region Related Node

            if (RelatedNode != 0)
            {
                project.Node = new Node { Id = RelatedNode};
                
            }
            else
            {
                var error = new List<BrokenBusinessRule>
                                {
                                    new BrokenBusinessRule("Id",
                                                          Resources.Shared.Messages.General.RelatedNode)
                                };

                ModelState.AddModelErrors(error);
            }

            #endregion

            #region Related Position

            if (RelatedPosition != 0)
            {
                project.Position = new Position {Id = RelatedPosition};
            }
            else
            {
                var error = new List<BrokenBusinessRule>
                                {
                                    new BrokenBusinessRule("Id",
                                                          Resources.Shared.Messages.General.RelatedPosition)
                                };

                ModelState.AddModelErrors(error);
            }

            #endregion

            #region Related Employee

            if (RelatedEmployee != 0)
            {
                project.Owner = new Employee { Id = RelatedEmployee};
            }
            else
            {
                var error = new List<BrokenBusinessRule>
                                {
                                    new BrokenBusinessRule("Id",
                                                           Resources.Shared.Messages.General.RelatedEmployee)
                                };

                ModelState.AddModelErrors(error);
            }

            #endregion

            #endregion

            #region Permission Check

            if (ViewData["CanCreate"] != null && !(bool) ViewData["CanCreate"])
            {
                ErrorPartialMessage(General.CanCreateMessage);
                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("Error")
                                });
            }

            #endregion

            #region Dates

            if (project.ActualStartingDate == DateTime.MinValue)
            {
                project.ActualStartingDate = new DateTime(1800, 1, 1);

                ModelState.Remove("ActualStartingDate");
            }

            if (project.ActualClosingDate == DateTime.MinValue)
            {
                project.ActualClosingDate = new DateTime(1800, 1, 1);

                ModelState.Remove("ActualClosingDate");
            }

            #endregion

            if ((Rules.GetBrokenRules(project).Count == 0) && (TryValidateModel(project)))
            {
                Service.Update(project);
            }
            else
            {
                if (!ModelState.ContainsKey("ActualStartingDate"))
                {
                    project.ActualStartingDate = DateTime.MinValue;
                    project.ActualClosingDate = DateTime.MinValue;
                }

                ModelState.AddModelErrors(Rules.GetBrokenRules(project));

                return View("Insert", project);
            }

            SetMasterRecordValue(MasterRecordOrder.First, project.Id);

            PrePublish();

            CacheProvider.ForceUpdate(ProjectCacheKeys.Project.ToString());

            return RedirectToAction("Index", new {id = project.Id});
        }

        #endregion

        #region Update

        public ActionResult Edit(int id)
        {
            Project project = Service.LoadById(id);

            return PartialView("Edit", project);
        }

        [HttpPost]
        public ActionResult JsonEdit(Project project)
        {
            PrePublish();

            #region Permission Check

            if (ViewData["CanUpdate"] != null && !(bool) ViewData["CanUpdate"])
            {
                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = ErrorPartialMessage(General.CanUpdateMessage)
                                });
            }

            #endregion

            #region Dates

            if (project.ActualStartingDate == DateTime.MinValue)
            {
                project.ActualClosingDate = new DateTime(1800, 1, 1);
                project.ActualStartingDate = new DateTime(1800, 1, 1);

                ModelState.Remove("ActualStartingDate");
                ModelState.Remove("ActualClosingDate");
            }

            #endregion

            #region Check Core Entity Relations

            #region Related Node

            if (RelatedNode != 0)
            {
                project.Node = new Node { Id = RelatedNode };

            }
            else
            {
                var error = new List<BrokenBusinessRule>
                                {
                                    new BrokenBusinessRule("Id",
                                                          Resources.Shared.Messages.General.RelatedNode)
                                };

                ModelState.AddModelErrors(error);
            }

            #endregion

            #region Related Position

            if (RelatedPosition != 0)
            {
                project.Position = new Position { Id = RelatedPosition };
            }
            else
            {
                var error = new List<BrokenBusinessRule>
                                {
                                    new BrokenBusinessRule("Id",
                                                          Resources.Shared.Messages.General.RelatedPosition)
                                };

                ModelState.AddModelErrors(error);
            }

            #endregion

            #region Related Employee

            if (RelatedEmployee != 0)
            {
                project.Owner = new Employee { Id = RelatedEmployee };
            }
            else
            {
                var error = new List<BrokenBusinessRule>
                                {
                                    new BrokenBusinessRule("Id",
                                                           Resources.Shared.Messages.General.RelatedEmployee)
                                };

                ModelState.AddModelErrors(error);
            }

            #endregion

            #endregion

            if ((Rules.GetBrokenRules(project).Count == 0) && (TryValidateModel(project)))
            {
                Service.Update(project);
            }
            else
            {
                if (!ModelState.ContainsKey("ActualStartingDate"))
                {
                    project.ActualStartingDate = DateTime.MinValue;
                    project.ActualClosingDate = DateTime.MinValue;
                }

                ModelState.AddModelErrors(Rules.GetBrokenRules(project));

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("Edit", project)
                                });
            }

            SetMasterRecordValue(MasterRecordOrder.First, project.Id);

            PrePublish();

            CacheProvider.ForceUpdate(ProjectCacheKeys.Project.ToString());

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("BasicInfo", project)
                            });
        }

        #endregion

        #region Delete

        [HttpPost]
        public ActionResult Delete(int id)
        {
            PrePublish();

            if (ViewData["CanDelete"] != null && !(bool) ViewData["CanDelete"])
            {
                return RedirectToAction("Index");
            }

            Project project = Service.LoadById(id);

            if (TryUpdateModel(project))
            {
                Service.Delete(project);
            }

            SetMasterRecordValue(MasterRecordOrder.First, 0);

            return RedirectToAction("Index");
        }

        #endregion

        #endregion

        #region Back To Master

        public ActionResult GoToProject(int id = 0, int selectedTabOrder = 0, bool ribbon = false,
                                        bool ribbonSubEntity = false)
        {
            if (selectedTabOrder > 0)
            {
                SaveTabIndex(selectedTabOrder == 11 ? 0 : selectedTabOrder);
            }

            CurrentlyInSecondLevel = 0;

            return RedirectToAction("Index", "Project", new {id, ribbon, ribbonSubEntity});
        }

        #endregion

        #region Go To Details

        public ActionResult GoToProjectTeams(int selectedTabOrder = 0)
        {
            SaveTabIndexSecondLevel(selectedTabOrder);

            if (GetMasterRecordValue(MasterRecordOrder.Second) == 0)
            {
                return RedirectToAction("Index", new {selectedTabOrder = 1});
            }

            AddToPath(MasterRecordOrder.First, RibbonLevels.Root, stepId: GetMasterRecordValue(MasterRecordOrder.First),
                      areaName: ProjectManagementAreaRegistration.GetAreaName, nodeName: Navigator.ProjectManagement,
                      actionName: "GoToProject");

            return RedirectToAction("MasterIndex", "ProjectTeam");
        }

        public ActionResult GoToProjectPhases(int selectedTabOrder = 0)
        {
            SaveTabIndexSecondLevel(selectedTabOrder);

            if (GetMasterRecordValue(MasterRecordOrder.Second) == 0)
            {
                return RedirectToAction("Index", new {selectedTabOrder = 1});
            }

            AddToPath(MasterRecordOrder.First, RibbonLevels.Root, stepId: GetMasterRecordValue(MasterRecordOrder.First),
                      areaName: ProjectManagementAreaRegistration.GetAreaName, nodeName: Navigator.ProjectManagement,
                      actionName: "GoToProject");

            return RedirectToAction("MasterIndex", "ProjectPhase");
        }

        public ActionResult GoToEvaluations(int selectedTabOrder = 0)
        {
            SaveTabIndexSecondLevel(selectedTabOrder);

            if (GetMasterRecordValue(MasterRecordOrder.Second) == 0)
            {
                return RedirectToAction("Index", new {selectedTabOrder = 5});
            }

            AddToPath(MasterRecordOrder.First, RibbonLevels.Root, stepId: GetMasterRecordValue(MasterRecordOrder.First),
                      areaName: ProjectManagementAreaRegistration.GetAreaName, nodeName: Navigator.ProjectManagement,
                      actionName: "GoToProject");

            return RedirectToAction("MasterIndex", "ProjectEvaluation");
        }

        #endregion

        #region Render Tree

        public ActionResult NodeToJson()
        {
            try
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
            catch (Exception)
            {
                return RedirectToAction("Index", "Organization", new {area = "OrganizationChart"});
            }
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
    }
}