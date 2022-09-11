#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.Objectives.Entities;
//using HRIS.Domain.Objectives.ValueObjects;
using HRIS.Domain.OrgChart.Entities;
using HRIS.Domain.OrgChart.ValueObjects;
using Service;
using UI.Areas.Objective.Controllers.EntitiesRoots;
using UI.Extensions;
using UI.Helpers;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.Objective.Entities;

#endregion

namespace UI.Areas.Objective.Controllers.ValueObjects
{
    public class SharedWithController : ObjectiveAggregateController, IRule<SharedWith>
    {
        #region Parents Chain

        #region Objective

        private HRIS.Domain.Objectives.RootEntities.Objective _basicObjective;

        public HRIS.Domain.Objectives.RootEntities.Objective FirstEntity
        {
            get
            {
                return _basicObjective ??
                       (_basicObjective = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First)));
            }
        }

        #endregion

        #region SharedWith

        private SharedWith _sharedWith;

        public SharedWith SecondEntity
        {
            get
            {
                return _sharedWith ??
                       (_sharedWith =
                        FirstEntity.SharedWiths.SingleOrDefault(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<SharedWith>

        public ObjectRules<SharedWith> Rules
        {
            get { return new SharedWithRules(); }
        }

        #endregion

        #region Overrides of ObjectiveAggregateController

        public override void FillList()
        {
            try
            {
                ViewData["ValueObjectsList"] =
                    Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First)).SharedWiths.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
            }
            catch (Exception)
            {
                ViewData["ValueObjectsList"] = null;
            }
        }

        #endregion

        #region Utilities

        public ActionResult ClearSelection()
        {
            SetMasterRecordValue(MasterRecordOrder.Second, 0);

            return RedirectToAction("Index");
        }

        #endregion

        #region Master Shared With

        public ActionResult MasterIndex(int id = 0)
        {
            if (id != 0)
            {
                SetMasterRecordValue(MasterRecordOrder.Second, id);
                CurrentlyInSecondLevel = id;
            }

            PrePublish();

            AddToPath(masterRecordOrder: MasterRecordOrder.Second, level: RibbonLevels.B, actionName: "MasterIndex",
                      stepId: GetMasterRecordValue(MasterRecordOrder.Second), nodeName: Resources.Areas.Objective.Views.Shared.Navigator.SharedWith,
                      areaName: ObjectiveAreaRegistration.GetAreaName);

            #region Get Data

            int pageNo = 1;
            if (GetMasterRecordValue(MasterRecordOrder.Second) != 0)
            {
                int masterRecordValue = GetMasterRecordValue(MasterRecordOrder.Second);

                int count = FirstEntity.SharedWiths.Where(sharedWith => (sharedWith.Id >= masterRecordValue)).Count();

                pageNo = count / 5;

                if (count % 5 > 0)
                {
                    pageNo++;
                }
            }

            ViewData["sharedWiths"] = FirstEntity.SharedWiths;
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

            //CurrentSecondLevelRowIdInSession = selectedSubRowId;

            SaveTabIndex(5);

            PrePublish();

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            SharedWithRelatedNode = 0;
            SharedWithRelatedPosition = 0;

            return PartialView("Edit", new SharedWith());
        }

        public ActionResult Save(SharedWith sharedWith)
        {
            PrePublish();

            /*            #region Check Entity Readiness

                        if (SharedWithRelatedNode == 0 | SharedWithRelatedPosition == 0)
                        {
                            var error = new List<BrokenBusinessRule>
                                            {
                                                new BrokenBusinessRule("Id", "No Node And/Or Position has been Selected !!")
                                            };

                            ModelState.AddModelErrors(error);

                            return Json(new
                                            {
                                                Success = false,
                                                PartialViewHtml = RenderPartialViewToString("List", sharedWith)
                                            });
                        }

                        #endregion

                        #region Check If Position Reselected

                        if (FirstEntity.Owner.Id == SharedWithRelatedPosition)
                        {
                            var error = new List<BrokenBusinessRule>
                                            {
                                                new BrokenBusinessRule("Id", "Position Already Selected As An Owner !!")
                                            };

                            ModelState.AddModelErrors(error);

                            return Json(new
                                            {
                                                Success = false,
                                                PartialViewHtml = RenderPartialViewToString("List", sharedWith)
                                            });
                        }

                        if (FirstEntity.SharedWiths.Any(with => with.Position.Id == SharedWithRelatedPosition))
                        {
                            var error = new List<BrokenBusinessRule>
                                            {
                                                new BrokenBusinessRule("Id", "Position Already Selected !!")
                                            };

                            ModelState.AddModelErrors(error);

                            return Json(new
                                            {
                                                Success = false,
                                                PartialViewHtml = RenderPartialViewToString("List", sharedWith)
                                            });
                        }

                        #endregion*/

            //  sharedWith.Position = new EntityService<Position>().LoadById(SharedWithRelatedPosition);
            // sharedWith.Node = new EntityService<Node>().LoadById(SharedWithRelatedNode);

            if (sharedWith.IsTransient())
            {
                FirstEntity.AddSharedWith(sharedWith);
            }
            else
            {
                #region Retrieve Lists

                sharedWith.Objective = SecondEntity.Objective;

                #endregion

                this.UpdateValueObject(sharedWith, SecondEntity);

                this.StringDecode(SecondEntity);
            }




            Service.Update(FirstEntity);

            /*
                        if ((Rules.GetBrokenRules(sharedWith).Count == 0)) // && (TryValidateModel(sharedWith)))
                        {
                            Service.Update(FirstEntity);
                        }
                        else
                        {
                            ModelState.AddModelErrors(Rules.GetBrokenRules(sharedWith));

                            FirstEntity.SharedWiths.Remove(sharedWith);

                            return Json(new
                                            {
                                                Success = false,
                                                PartialViewHtml = RenderPartialViewToString("List", sharedWith)
                                            });
                        }
            */

            SetMasterRecordValue(MasterRecordOrder.Second, sharedWith.Id);

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", sharedWith)
                            });
        }

        [HttpPost]
        public JsonResult JsonEdit()
        {


            //DropDownListHelpers.ListOfSelectedNodePosition(SharedWithRelatedNode);

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("Edit", SecondEntity)
                            });
        }

        #region Delete

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (ViewData["CanDelete"] != null && !(bool)ViewData["CanDelete"])
            {
                ErrorPartialMessage("You Are Not Allowed To Delete !!");

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("Error")
                                });
            }

            try
            {
                SharedWith sharedWith = FirstEntity.SharedWiths.SingleOrDefault(c => c.Id == id);

                FirstEntity.SharedWiths.Remove(sharedWith);

                Service.Update(FirstEntity);

                PrePublish();

                SetMasterRecordValue(MasterRecordOrder.Second, 0);

                return RedirectToAction("Index", "Objective");
            }
            catch (Exception)
            {
                return ErrorPartialMessage("Error During Delete ! Please try Again");
            }
        }

        [HttpPost]
        public ActionResult MasterDelete(int id)
        {
            if (ViewData["CanDelete"] != null && !(bool)ViewData["CanDelete"])
            {
                ErrorPartialMessage("You Are Not Allowed To Delete !!");

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("Error")
                                });
            }

            try
            {
                SharedWith sharedWith = FirstEntity.SharedWiths.SingleOrDefault(c => c.Id == id);

                FirstEntity.SharedWiths.Remove(sharedWith);

                Service.Update(FirstEntity);

                PrePublish();

                SetMasterRecordValue(MasterRecordOrder.Second, 0);

                return RedirectToAction("MasterIndex");
            }
            catch (Exception)
            {
                return ErrorPartialMessage("Error During Delete ! Please try Again");
            }
        }

        #endregion

        #endregion

        #region Render Tree

        public ActionResult NodeToJson()
        {
            Node node;
            if (SharedWithRelatedNode != 0)
            {
                node = new EntityService<Node>().LoadById(SharedWithRelatedNode);
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
            if (SharedWithRelatedNode != 0 & reset == 0)
            {
                Node node = new EntityService<Node>().LoadById(SharedWithRelatedNode);
                SharedWithRelatedNode = node.Parent.Id;
            }
            else
            {
                SharedWithRelatedNode = 0;
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
                                PartialViewHtml = RenderPartialViewToString("Components/NodePositionsList")
                            });
        }

        #endregion
    }
}