#region
using Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.Objectives.Entities;
using HRIS.Domain.OrgChart.Entities;
using HRIS.Domain.OrgChart.ValueObjects;
using Repository.NHibernate;
using Service;
using Telerik.Web.Mvc;
using UI.Areas.Objective.Controllers.EntitiesRoots;
using UI.Areas.Objective.Helpers;
using UI.Extensions;
using UI.Helpers.Cache;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.Objective.Entities;
using HRIS.Domain.Objectives.RootEntities;
using DropDownListHelpers = UI.Helpers.DropDownListHelpers;


#endregion

namespace UI.Areas.Objective.Controllers.Entities
{
    public class ObjectiveController : ObjectiveAggregateController, IRule<HRIS.Domain.Objectives.RootEntities.Objective>
    {
        #region Parents Chain

        #region Objective

        private HRIS.Domain.Objectives.RootEntities.Objective _basicObjective;
        private Repository<HRIS.Domain.Objectives.RootEntities.Objective> _repository;

        public HRIS.Domain.Objectives.RootEntities.Objective FirstEntity
        {
            get
            {
                return _basicObjective ??
                       (_basicObjective = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First)));
            }
        }

        public Repository<HRIS.Domain.Objectives.RootEntities.Objective> Repository
        {
            get
            {
                return _repository ?? new Repository<HRIS.Domain.Objectives.RootEntities.Objective>();
            }
        }


        #endregion

        #endregion

        #region IRule<BasicObjective> Members

        public ObjectRules<HRIS.Domain.Objectives.RootEntities.Objective> Rules
        {
            get { return new ObjectiveRules(); }
        }

        #endregion

        #region Overrides of ObjectiveAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("Priority.Name");
            ModelState.Remove("Type.Name");
        }

        #endregion

        #region CRUD

        #region Read

        [GridAction]
        public ActionResult Index(int id = 0, int selectedTabOrder = 0, bool ribbon = false)
        {
            PrePublish();

            #region Security

            if (ViewData["CanRead"] != null && !(bool)ViewData["CanRead"])
            {
                return ErrorPartialMessage(Resources.Shared.Messages.General.CanReadMessage);
            }

            #endregion

            #region Manage Tab, Path, and MastersList

            {
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
            }

            AddToPath(MasterRecordOrder.First, RibbonLevels.Root, stepId: id,
                      areaName: ObjectiveAreaRegistration.GetAreaName, nodeName: Resources.Areas.Objective.Views.Shared.Navigator.Objectives);

            CurrentlyInSecondLevel = GetMasterRecordValue(MasterRecordOrder.First);

            #endregion

            #region Get Data

            var objectives = RelatedPosition != 0
                                 ? Service.GetAll().Where(w => w.Owner.Id == RelatedPosition)
                                 : Service.GetAll();

            var pageNo = 1;

            if (GetMasterRecordValue(MasterRecordOrder.First) != 0)
            {
                int masterRecordValue = GetMasterRecordValue(MasterRecordOrder.First);

                int count = objectives.Where(objective => (objective.Id >= masterRecordValue)).Count();

                pageNo = count != 0 ? count : 1;
            }

            ViewData["objectives"] = objectives;
            ViewData["SelectedRow"] = GetMasterRecordValue(MasterRecordOrder.First);
            ViewData["PageTo"] = pageNo;
            ViewData["ReadyToAddObjective"] = RelatedPosition != 0;

            #endregion

            return View();
        }

        public ActionResult PartialMasterInfo(int selectedRowId = 0)
        {
            PrePublish();

            if (selectedRowId != 0)
            {
                SetMasterRecordValue(MasterRecordOrder.First, selectedRowId);
                CurrentlyInSecondLevel = selectedRowId;
            }

            HRIS.Domain.Objectives.RootEntities.Objective basicObjective =
                Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First));

            return PartialView("BasicInfo", basicObjective);
        }

        #endregion

        #region Create

        public ActionResult Insert()
        {
            LoadStepsList();

            AddToPath(MasterRecordOrder.First, RibbonLevels.Root, areaName: ObjectiveAreaRegistration.GetAreaName,
                      nodeName: Resources.Areas.Objective.Views.Shared.Navigator.Objectives);

            var objective = new HRIS.Domain.Objectives.RootEntities.Objective
                                {
                                    Owner = new EntityService<Position>().LoadById(RelatedPosition)
                                    //,Node = new EntityService<Node>().LoadById(RelatedNode)
                                };

            return View("Insert", objective);
        }

        [HttpPost]
        public ActionResult JsonInsert(HRIS.Domain.Objectives.RootEntities.Objective basicObjective)
        {
            basicObjective.CreatedBy = User.Identity.Name;

            PrePublish();

            #region Permission Check

            if (ViewData["CanCreate"] != null && !(bool)ViewData["CanCreate"])
            {
                ErrorPartialMessage(Resources.Shared.Messages.General.CanCreateMessage);
                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("Error")
                                });
            }

            #endregion

            #region Dates

            if (basicObjective.PlannedClosingDate == DateTime.MinValue)
            {
                basicObjective.PlannedClosingDate = new DateTime(1800, 1, 1);

                ModelState.Remove("PlannedClosingDate");
            }

            if (basicObjective.PlannedStartingDate == DateTime.MinValue)
            {
                basicObjective.PlannedStartingDate = new DateTime(1800, 1, 1);

                ModelState.Remove("PlannedStartingDate");
            }

            #endregion

            #region Check Weight

            //  var list = Service.GetAll().Where(o => o.Owner.Id == basicObjective.Owner.Id);

            // float totalWeight = Enumerable.Sum(list, objective => objective.Weight);

            //  totalWeight += basicObjective.Weight;

            /* if (totalWeight > 100)
             {
                 var error = new List<BrokenBusinessRule>
                                 {
                                     new BrokenBusinessRule("Id",
                                                            Resources.Areas.Objective.Entities.BasicObjective.BasicObjectiveModel.TotalWeightMessage)
                                 };

                 ModelState.AddModelErrors(error);
             }*/

            #endregion
            /*

            if ((Rules.GetBrokenRules(basicObjective).Count == 0))// && (TryValidateModel(basicObjective)))
            {
                Service.Update(basicObjective);
            }
            else
            {
                if (!ModelState.ContainsKey("ActualStartingDate"))
                {
                    basicObjective.ActualStartingDate = DateTime.MinValue;
                    basicObjective.ActualClosingDate = DateTime.MinValue;
                }

                ModelState.AddModelErrors(Rules.GetBrokenRules(basicObjective));

                return View("Insert", basicObjective);
            }
*/
            Repository.Save(basicObjective);


            SetMasterRecordValue(MasterRecordOrder.First, basicObjective.Id);

            PrePublish();

            CacheProvider.ForceUpdate(ObjectiveCacheKeys.Objective.ToString());

            return RedirectToAction("Index", new { id = basicObjective.Id });
        }

        #endregion

        #region Update

        public ActionResult Edit(int id)
        {
            HRIS.Domain.Objectives.RootEntities.Objective basicObjective = Service.LoadById(id);

            return PartialView("Edit", basicObjective);
        }

        [HttpPost]
        public ActionResult JsonEdit(HRIS.Domain.Objectives.RootEntities.Objective basicObjective)
        {
            PrePublish();

            #region Permission Check

            if (ViewData["CanUpdate"] != null && !(bool)ViewData["CanUpdate"])
            {
                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = ErrorPartialMessage(Resources.Shared.Messages.General.CanUpdateMessage)
                                });
            }

            #endregion

            /* #region Dates

            //if (basicObjective.ActualStartingDate == DateTime.MinValue)
            //{
            //    basicObjective.ActualClosingDate = new DateTime(1800, 1, 1);
            //    basicObjective.ActualStartingDate = new DateTime(1800, 1, 1);

            //    ModelState.Remove("ActualStartingDate");
            //    ModelState.Remove("ActualClosingDate");
            //}

            #endregion

            #region Check Weight

            var list =
                new EntityService<HRIS.Domain.Objectives.RootEntities.Objective>().GetAll().Where(o => o.Owner.Id == basicObjective.Owner.Id);

            float totalWeight = Enumerable.Sum(list, objective => objective.Weight);

            totalWeight += basicObjective.Weight;

            if (totalWeight > 100)
            {
                var error = new List<BrokenBusinessRule>
                                {
                                    new BrokenBusinessRule("Id",
                                                          Resources.Areas.Objective.Entities.BasicObjective.BasicObjectiveModel.TotalWeightMessage)
                                };

                ModelState.AddModelErrors(error);
            }

            #endregion

            #region Retrieve Lists
            

            //basicObjective.OrganizationalObjectives = FirstEntity.OrganizationalObjectives;
            //basicObjective.EvaluationCriterias = FirstEntity.EvaluationCriterias;
            //basicObjective.Evaluations = FirstEntity.Evaluations;
            //basicObjective.ObjectiveKpis = FirstEntity.ObjectiveKpis;
            //basicObjective.ObjectiveSteps = FirstEntity.ObjectiveSteps;
            basicObjective.SharedWiths = FirstEntity.SharedWiths;
            basicObjective.Constraints = FirstEntity.Constraints;


            this.UpdateValueObject(basicObjective, FirstEntity);

            #endregion

            if ((Rules.GetBrokenRules(basicObjective).Count == 0) && (TryValidateModel(basicObjective)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                if (!ModelState.ContainsKey("ActualStartingDate"))
                {
                    //basicObjective.ActualStartingDate = DateTime.MinValue;
                    //basicObjective.ActualClosingDate = DateTime.MinValue;
                }

                ModelState.AddModelErrors(Rules.GetBrokenRules(basicObjective));

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("Edit", basicObjective)
                                });
            }*/

            Repository.Update(basicObjective);

            SetMasterRecordValue(MasterRecordOrder.First, basicObjective.Id);

            PrePublish();

            CacheProvider.ForceUpdate(ObjectiveCacheKeys.Objective.ToString());

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("BasicInfo", basicObjective)
                            });
        }

        #endregion

        #region Delete

        [HttpPost]
        public ActionResult Delete(int id)
        {
            PrePublish();

            if (ViewData["CanDelete"] != null && !(bool)ViewData["CanDelete"])
            {
                return RedirectToAction("Index");
            }

            HRIS.Domain.Objectives.RootEntities.Objective basicObjective = Service.LoadById(id);

            if (TryUpdateModel(basicObjective))
            {
                Service.Delete(basicObjective);
            }

            SetMasterRecordValue(MasterRecordOrder.First, 0);

            return RedirectToAction("Index");
        }

        #endregion

        #endregion

        #region Back To Master

        public ActionResult GoToObjective(int id = 0, int selectedTabOrder = 0, bool ribbon = false,
                                          bool ribbonSubEntity = false)
        {
            if (selectedTabOrder > 0)
            {
                SaveTabIndex(selectedTabOrder == 11 ? 0 : selectedTabOrder);
            }

            return RedirectToAction("Index", "Objective", new { id, ribbon, ribbonSubEntity });
        }

        #endregion

        #region Go To Details

        public ActionResult GoToSharedWith(int selectedTabOrder = 0)
        {
            SaveTabIndexSecondLevel(selectedTabOrder);

            AddToPath(MasterRecordOrder.First, RibbonLevels.Root, stepId: GetMasterRecordValue(MasterRecordOrder.First),
                      areaName: ObjectiveAreaRegistration.GetAreaName, nodeName: Resources.Areas.Objective.Views.Shared.Navigator.Objectives,
                      actionName: "GoToObjective");

            return RedirectToAction("MasterIndex", "SharedWith");
        }

        public ActionResult GoToEvaluations(int selectedTabOrder = 0)
        {
            SaveTabIndexSecondLevel(selectedTabOrder);

            if (GetMasterRecordValue(MasterRecordOrder.Second) == 0)
            {
                return RedirectToAction("Index", new { selectedTabOrder = 5 });
            }

            AddToPath(MasterRecordOrder.First, RibbonLevels.Root, stepId: GetMasterRecordValue(MasterRecordOrder.First),
                      areaName: ObjectiveAreaRegistration.GetAreaName, nodeName: Resources.Areas.Objective.Views.Shared.Navigator.Objectives,
                      actionName: "GoToObjective");

            //return RedirectToAction("MasterIndex", "Evaluation");
            return View("Index");
        }

        #endregion

        #region Render Tree

        public ActionResult NodeToJson()
        {
            Node node;
            if (RelatedNode != 0)
            {
                node = new EntityService<Node>().LoadById(RelatedNode);
            }
            else
            {
                node = new EntityService<Organization>().GetAll().Single().RootNode.Single();
            }

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
                                PartialViewHtml = RenderPartialViewToString("OrgTree/Tree")
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
                                PartialViewHtml = RenderPartialViewToString("DropDownLists/PositionEmployeesList")
                            });
        }

        public ActionResult GetPositions(int nodeId)
        {
            DropDownListHelpers.ListOfSelectedNodePosition(nodeId);

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("DropDownLists/NodePositionsList")
                            });
        }

        #endregion
    }
}