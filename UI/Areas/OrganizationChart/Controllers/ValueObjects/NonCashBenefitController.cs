#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.OrgChart.Entities;
using HRIS.Domain.OrgChart.ValueObjects;
using UI.Areas.OrganizationChart.Controllers.EntitiesRoots;
using UI.Extensions;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.OrganizationChart.ValueObjects;

#endregion

namespace UI.Areas.OrganizationChart.Controllers.ValueObjects
{
    public class NonCashBenefitController : GradeAggregateController, IRule<NonCashBenefit>
    {
        #region Overrides of GradeAggregateController

        public override void FillList()
        {
            Grade grade = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First));
            if (grade != null)
                ViewData["ValueObjectsList"] =
                    grade.NonCashBenefits.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override void CleanUpModelState()
        {
            ModelState.Remove("Status.Name");
            ModelState.Remove("Type.Name");
            ModelState.Remove("Occurrence.Name");
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            Grade grade = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First));
            return grade.NonCashBenefits != null
                       ? Rules.GetExpiredRules(grade.NonCashBenefits)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region Implementation of IRule<CashBenefit>

        public ObjectRules<NonCashBenefit> Rules
        {
            get { return new NoneCashBenefitsRules(); }
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Second, selectedSubRowId);

            PrePublish();

            if (GetMasterRecordValue(MasterRecordOrder.First) != 0)
            {
                return PartialView("Index");
            }
            return ErrorPartialMessage(Resources.Areas.OrgChart.Entities.Grade.GradeRules.NoGradeSelectedMessage);
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new NonCashBenefit()
                );
        }

        [HttpPost]
        public JsonResult Save(NonCashBenefit nonCashBenefit)
        {
            PrePublish();

            Grade grade = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First));

            if (nonCashBenefit.IsTransient())
            {
                grade.AddNonCashBenefit(nonCashBenefit);
            }
            else
            {
                NonCashBenefit original = grade.NonCashBenefits.Single(o => o.Id == nonCashBenefit.Id);
                nonCashBenefit.Grade = original.Grade;
                this.UpdateValueObject(nonCashBenefit, original);
            }

            if ((Rules.GetBrokenRules(nonCashBenefit).Count == 0) && (TryValidateModel(nonCashBenefit)))
            {
                Service.Update(grade);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(nonCashBenefit));
                grade.NonCashBenefits.Remove(nonCashBenefit);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", nonCashBenefit)
                                });
            }

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", nonCashBenefit)
                            });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Grade grade = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First));
            NonCashBenefit nonCashBenefit = grade.NonCashBenefits.SingleOrDefault(o => o.Id == id);

            try
            {
                grade.NonCashBenefits.Remove(nonCashBenefit);
                Service.Update(grade);
                SetMasterRecordValue(MasterRecordOrder.Second, 0);
                PrePublish();

                return RedirectToAction("Index", "Grade", new {selectedTabOrder = 4});
            }
            catch (Exception)
            {
                return ErrorPartialMessage(Resources.Shared.Messages.General.GeneralErrorOccurred);
            }
        }

        [HttpPost]
        public JsonResult JsonEdit(int id)
        {
            Grade grade = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First));
            NonCashBenefit nonCashBenefit = grade.NonCashBenefits.SingleOrDefault(c => c.Id == id);

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("Edit", nonCashBenefit)
                            });
        }

        #endregion
    }
}