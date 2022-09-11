#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.Objectives.Entities;

using UI.Areas.Objective.Controllers.EntitiesRoots;
using UI.Extensions;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.Objective.Entities;

#endregion

namespace UI.Areas.Objective.Controllers.ValueObjects
{
    public class ObjectiveStepController : ObjectiveAggregateController, IRule<ActionPlan>
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

        #region Contact

        private ActionPlan _objectiveStep;

        public ActionPlan SecondEntity
        {
            get
            {
                return _objectiveStep ??
                       (_objectiveStep =
                        FirstEntity.ActionPlans.SingleOrDefault(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #endregion

        #region IRule<ObjectiveStep> Members

        public ObjectRules<ActionPlan> Rules
        {
            get { return new ActionPlanRules(); }
        }

        #endregion

        #region Overrides of ObjectiveAggregateController

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                FirstEntity.Kpis.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FirstEntity.ActionPlans != null
                       ? Rules.GetExpiredRules(FirstEntity.ActionPlans)
                       : new List<BrokenBusinessRule>();
        }

        public override void CleanUpModelState()
        {
            ModelState.Remove("Status.Name");
        }

        #endregion

        #region CRUD

        #region Read

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Second, selectedSubRowId);
            CurrentlyInSecondLevel = selectedSubRowId;

            PrePublish();

            SaveTabIndex(1);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new ActionPlan() {Objective = FirstEntity});
        }

        #endregion

        #region Create

        public JsonResult Save(ActionPlan objectiveStep)
        {
            PrePublish();

            #region Permission Check

            if (objectiveStep.IsTransient())
            {
                if (ViewData["CanCreate"] != null && !(bool) ViewData["CanCreate"])
                {
                    ErrorPartialMessage(Resources.Shared.Messages.General.CanCreateMessage);
                    return Json(new
                                    {
                                        Success = false,
                                        PartialViewHtml = RenderPartialViewToString("Error")
                                    });
                }
            }
            else
            {
                if (ViewData["CanUpdate"] != null && !(bool) ViewData["CanUpdate"])
                {
                    ErrorPartialMessage(Resources.Shared.Messages.General.CanUpdateMessage);
                    return Json(new
                                    {
                                        Success = false,
                                        PartialViewHtml = RenderPartialViewToString("Error")
                                    });
                }
            }

            #endregion

            #region Dates

            if (objectiveStep.ActualStartingDate == DateTime.MinValue)
            {
                objectiveStep.ActualStartingDate = new DateTime(1800, 1, 1);

                ModelState.Remove("ActualStartingDate");
            }

            if (objectiveStep.ActualClosingDate == DateTime.MinValue)
            {
                objectiveStep.ActualClosingDate = new DateTime(1800, 1, 1);

                ModelState.Remove("ActualClosingDate");
            }

            #endregion

            #region StepNumber

            objectiveStep.Number = FirstEntity.ActionPlans.Count + 1;

            #endregion

            if (objectiveStep.IsTransient())
            {
                FirstEntity.AddActionPlan(objectiveStep);
            }
            else
            {
                #region Retrieve Lists

                objectiveStep.Objective = SecondEntity.Objective;

                #endregion

                this.UpdateValueObject(objectiveStep, SecondEntity);
            }

            if ((Rules.GetBrokenRules(objectiveStep).Count == 0)) //&& (TryValidateModel(objectiveStep)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(objectiveStep));

                FirstEntity.ActionPlans.Remove(objectiveStep);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", objectiveStep)
                                });
            }

            SetMasterRecordValue(MasterRecordOrder.Second, objectiveStep.Id);

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", objectiveStep)
                            });
        }

        #endregion

        #region Update

        //[HttpPost]
        public ActionResult JsonEdit()
        {
            return PartialView("Edit", SecondEntity);

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("Edit", SecondEntity)
                            });
        }

        #endregion

        #region Delete

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (ViewData["CanDelete"] != null && !(bool) ViewData["CanDelete"])
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
                ActionPlan objectiveStep = FirstEntity.ActionPlans.SingleOrDefault(x => x.Id == id);

                FirstEntity.ActionPlans.Remove(objectiveStep);

                Service.Update(FirstEntity);

                PrePublish();

                return RedirectToAction("Index", "BasicObjective");
            }
            catch (Exception)
            {
                return ErrorPartialMessage("Error During Delete ! Please try Again");
            }
        }

        #endregion

        #endregion
    }
}