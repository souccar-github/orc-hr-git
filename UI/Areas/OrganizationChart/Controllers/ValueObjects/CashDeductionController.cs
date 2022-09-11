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
    public class CashDeductionController : GradeAggregateController, IRule<CashDeduction>
    {
        #region Overrides of GradeAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("Type.Name");
            ModelState.Remove("Status.Name");
            ModelState.Remove("Occurrence.Name");
        }

        public override void FillList()
        {
            Grade grade = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First));
            if (grade != null)
                ViewData["ValueObjectsList"] =
                    grade.CashDeductions.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            Grade grade = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First));
            return grade.CashDeductions != null
                       ? Rules.GetExpiredRules(grade.CashDeductions)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region IRule<CashDeduction> Members

        public ObjectRules<CashDeduction> Rules
        {
            get { return new CashDeductionRules(); }
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
            return PartialView("Edit", new CashDeduction
                                           {
                                               ActiveDate = DateTime.Today.Date,
                                               InactiveDate = DateTime.Today.Date
                                           });
        }

        [HttpPost]
        public JsonResult Save(CashDeduction cashDeduction)
        {
            PrePublish();

            Grade grade = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First));

            if (cashDeduction.IsTransient())
            {
                grade.AddCashDeduction(cashDeduction);
            }
            else
            {
                CashDeduction original = grade.CashDeductions.Single(c => c.Id == cashDeduction.Id);

                cashDeduction.Grade = original.Grade;

                this.UpdateValueObject(cashDeduction, original);
            }

            if ((Rules.GetBrokenRules(cashDeduction).Count == 0) && (TryValidateModel(cashDeduction)))
            {
                Service.Update(grade);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(cashDeduction));

                grade.CashDeductions.Remove(cashDeduction);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", cashDeduction)
                                });
            }

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", cashDeduction)
                            });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Grade grade = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First));
            CashDeduction cashDeduction = grade.CashDeductions.SingleOrDefault(c => c.Id == id);

            try
            {
                grade.CashDeductions.Remove(cashDeduction);

                Service.Update(grade);

                SetMasterRecordValue(MasterRecordOrder.Second, 0);
                PrePublish();

                return RedirectToAction("Index", "Grade", new {selectedTabOrder = 1});
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
            CashDeduction cashDeduction = grade.CashDeductions.SingleOrDefault(c => c.Id == id);

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("Edit", cashDeduction)
                            });
        }

        #endregion
    }
}