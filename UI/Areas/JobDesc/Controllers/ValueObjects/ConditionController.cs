#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.JobDesc.Entities;
using HRIS.Domain.JobDesc.ValueObjects;
using Infrastructure.Validation;
using UI.Areas.JobDesc.Controllers.EntitiesRoots;
using UI.Extensions;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.JobDesc.ValueObjects;

#endregion

namespace UI.Areas.JobDesc.Controllers.ValueObjects
{
    public class ConditionController : JobDescAggregateController, IRule<Condition>
    {
        #region Parents Chain

        #region JobDescription

        private JobDescription _jobDescription;

        public JobDescription FirstEntity
        {
            get
            {
                return _jobDescription ??
                       (_jobDescription = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First)));
            }
        }

        #endregion

        #region Specification

        private Specification _specification;

        public Specification SecondEntity
        {
            get { return _specification ?? (_specification = FirstEntity.Specification.First()); }
        }

        #endregion

        #region Working Condition

        private WorkingCondition _workingCondition;

        public WorkingCondition ThirdEntity
        {
            get
            {
                return _workingCondition ??
                       (_workingCondition =
                        SecondEntity.WorkingConditions.SingleOrDefault(c => c.Id == GetMasterRecordValue(MasterRecordOrder.Third)));
            }
        }

        #endregion

        #region Condition

        private Condition _condition;

        public Condition FourthEntity
        {
            get
            {
                return _condition ??
                       (_condition =
                        ThirdEntity.Conditions.Single(c => c.Id == GetMasterRecordValue(MasterRecordOrder.Fourth)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<Condition>

        public ObjectRules<Condition> Rules
        {
            get { return new ConditionRules(); }
        }

        #endregion

        #region Overrides of JobDescAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("Type.Name");
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                ThirdEntity.Conditions.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Fourth));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return
                ThirdEntity.Conditions.Count != 0
                    ? Rules.GetExpiredRules(ThirdEntity.Conditions)
                    : new List<BrokenBusinessRule>();
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Fourth, selectedSubRowId);

            PrePublish();

            SaveTabIndexSecondLevel(7);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new Condition());
        }

        [HttpPost]
        public ActionResult Save(Condition condition)
        {
            PrePublish();

            if (condition.IsTransient())
            {
                ThirdEntity.AddCondition(condition);
            }
            else
            {
                condition.WorkingCondition = FourthEntity.WorkingCondition;

                this.UpdateValueObject(condition, FourthEntity);
                this.StringDecode(FourthEntity);
            }

            if ((Rules.GetBrokenRules(condition).Count == 0) && (TryValidateModel(condition)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(condition));

                ThirdEntity.Conditions.Remove(condition);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", condition)
                                });
            }

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", condition)
                            });
        }

        [HttpPost]
        public JsonResult JsonEdit()
        {
            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("Edit", FourthEntity)
                            });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Condition condition = ThirdEntity.Conditions.Single(c => c.Id == id);

                ThirdEntity.Conditions.Remove(condition);

                Service.Update(FirstEntity);

                PrePublish();

                return RedirectToAction("Index", "Specification");
            }
            catch (Exception)
            {
                return ErrorPartialMessage(Resources.Shared.Messages.General.ErrorDuringDelete);
            }
        }

        #endregion
    }
}