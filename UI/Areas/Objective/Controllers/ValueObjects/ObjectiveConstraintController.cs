#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.Objectives.Entities;
//using HRIS.Domain.Objectives.ValueObjects;
using UI.Areas.Objective.Controllers.EntitiesRoots;
using UI.Extensions;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.Objective.Entities;

#endregion

namespace UI.Areas.Objective.Controllers.ValueObjects
{
    public class ObjectiveConstraintController : ObjectiveAggregateController, IRule<ObjectiveConstraint>
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

        #region ObjectiveConstraint

        private ObjectiveConstraint _objectiveConstraint;

        public ObjectiveConstraint SecondEntity
        {
            get
            {
                return _objectiveConstraint ??
                       (_objectiveConstraint =
                        FirstEntity.Constraints.SingleOrDefault(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<ObjectiveConstraint>

        public ObjectRules<ObjectiveConstraint> Rules
        {
            get { return new ObjectiveConstraintRules(); }
        }

        #endregion

        #region Overrides of ObjectiveAggregateController

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                FirstEntity.Constraints.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FirstEntity.Constraints.Count != 0
                       ? Rules.GetExpiredRules(FirstEntity.Constraints)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Second, selectedSubRowId);

            CurrentlyInSecondLevel = 0;

            SaveTabIndex(4);

            PrePublish();

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new ObjectiveConstraint());
        }

        public ActionResult Save(ObjectiveConstraint objectiveConstraint)
        {
            PrePublish();

            if (objectiveConstraint.IsTransient())
            {
                FirstEntity.Addconstraint(objectiveConstraint);
            }
            else
            {
                #region Retrieve Lists

                objectiveConstraint.Objective = SecondEntity.Objective;

                #endregion

                this.UpdateValueObject(objectiveConstraint, SecondEntity);

                this.StringDecode(SecondEntity);
            }

            if ((Rules.GetBrokenRules(objectiveConstraint).Count == 0) && (TryValidateModel(objectiveConstraint)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(objectiveConstraint));

                FirstEntity.Constraints.Remove(objectiveConstraint);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", objectiveConstraint)
                                });
            }

            SetMasterRecordValue(MasterRecordOrder.Second, objectiveConstraint.Id);

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", objectiveConstraint)
                            });
        }

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
                ObjectiveConstraint objectiveConstraint = FirstEntity.Constraints.SingleOrDefault(c => c.Id == id);

                FirstEntity.Constraints.Remove(objectiveConstraint);

                Service.Update(FirstEntity);

                PrePublish();

                return RedirectToAction("Index", "BasicObjective");
            }
            catch (Exception)
            {
                return ErrorPartialMessage("Error During Delete ! Please try Again");
            }
        }

        [HttpPost]
        public JsonResult JsonEdit()
        {
            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("Edit", SecondEntity)
                            });
        }

        #endregion
    }
}