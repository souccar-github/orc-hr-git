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
    public class InsuranceController : GradeAggregateController, IRule<Insurance>
    {
        #region Overrides of GradeAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("Type.Name");
            ModelState.Remove("InsuranceCompany.Name");
        }

        public override void FillList()
        {
            Grade grade = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First));
            if (grade != null)
                ViewData["ValueObjectsList"] =
                    grade.Insurances.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First)).Insurances != null
                       ? Rules.GetExpiredRules(
                           Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First)).Insurances)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region IRule<Insurance> Members

        public ObjectRules<Insurance> Rules
        {
            get { return new InsuranceRules(); }
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Second, selectedSubRowId);

            PrePublish();

            SaveTabIndex(0);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new Insurance());
        }

        [HttpPost]
        public JsonResult Save(Insurance insurance)
        {
            PrePublish();

            Grade grade = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First));

            if (insurance.IsTransient())
            {
                grade.AddInsurance(insurance);
            }
            else
            {
                Insurance original = grade.Insurances.Single(c => c.Id == insurance.Id);

                insurance.Grade = original.Grade;

                this.UpdateValueObject(insurance, original);
            }

            if ((Rules.GetBrokenRules(insurance).Count == 0) && (TryValidateModel(insurance)))
            {
                Service.Update(grade);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(insurance));

                grade.Insurances.Remove(insurance);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", insurance)
                                });
            }

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", insurance)
                            });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Grade grade = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First));
            Insurance insurance = grade.Insurances.SingleOrDefault(c => c.Id == id);

            try
            {
                grade.Insurances.Remove(insurance);

                Service.Update(grade);

                SetMasterRecordValue(MasterRecordOrder.Second, 0);

                PrePublish();

                return RedirectToAction("Index", "Grade", new {selectedTabOrder = 0});
            }
            catch (Exception)
            {
                return ErrorPartialMessage(Resources.Shared.Messages.General.GeneralErrorOccurred);
            }
        }

        [HttpPost]
        public JsonResult JsonEdit()
        {
            Grade grade = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First));
            Insurance insurance =
                grade.Insurances.SingleOrDefault(c => c.Id == GetMasterRecordValue(MasterRecordOrder.Second));

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("Edit", insurance)
                            });
        }

        #endregion
    }
}