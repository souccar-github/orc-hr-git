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
    public class CashBenefitController : GradeAggregateController, IRule<CashBenefit>
    {
        #region Overrides of GradeAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("Status.Name");
            ModelState.Remove("Type.Name");
            ModelState.Remove("Occurrence.Name");
        }

        public override void FillList()
        {
            Grade grade = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First));
            if (grade != null)
                ViewData["ValueObjectsList"] =
                    grade.CashBenefits.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            Grade grade = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First));
            return grade.CashBenefits != null
                       ? Rules.GetExpiredRules(grade.CashBenefits)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region Implementation of IRule<CashBenefit>

        public ObjectRules<CashBenefit> Rules
        {
            get { return new CashBenefitsRules(); }
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
            return PartialView("Edit", new CashBenefit());
        }

        [HttpPost]
        public JsonResult Save(CashBenefit casheBenefit)
        {
            PrePublish();

            Grade grade = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First));

            if (casheBenefit.IsTransient())
            {
                grade.AddCashBenefit(casheBenefit);
            }
            else
            {
                CashBenefit original = grade.CashBenefits.Single(c => c.Id == casheBenefit.Id);

                casheBenefit.Grade = original.Grade;

                this.UpdateValueObject(casheBenefit, original);
            }

            if ((Rules.GetBrokenRules(casheBenefit).Count == 0) && (TryValidateModel(casheBenefit)))
            {
                Service.Update(grade);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(casheBenefit));

                grade.CashBenefits.Remove(casheBenefit);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", casheBenefit)
                                });
            }

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", casheBenefit)
                            });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Grade grade = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First));
            CashBenefit cashBenefit = grade.CashBenefits.SingleOrDefault(c => c.Id == id);

            try
            {
                grade.CashBenefits.Remove(cashBenefit);

                Service.Update(grade);

                SetMasterRecordValue(MasterRecordOrder.Second, 0);
                PrePublish();

                return RedirectToAction("Index", "Grade", new { selectedTabOrder = 3 });
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
            CashBenefit cacheBenefit = grade.CashBenefits.SingleOrDefault(c => c.Id == id);
            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("Edit", cacheBenefit)
                            });
        }

        #endregion
    }
}