#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.OrgChart.ValueObjects;
using HRIS.Domain.OrgChart.ValueObjects.AssignedGrade;
using UI.Areas.OrganizationChart.Controllers.EntitiesRoots;
using UI.Extensions;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.OrganizationChart.ValueObjects;

#endregion

namespace UI.Areas.OrganizationChart.Controllers.ValueObjects
{
    public class AssignedCashBenefitController : NodeAggregateController, IRule<AssignedCashBenefit>
    {
        #region Parents Chain

        #region Position

        public Position Position
        {
            get
            {
                if (GetMasterRecordValue(MasterRecordOrder.Second) != 0)
                {
                    return PositionService.LoadById(GetMasterRecordValue(MasterRecordOrder.Second));
                }

                return null;
            }
        }

        #endregion

        #region PositionGrade

        public PositionGrade PositionGrade
        {
            get { return Position.Grades.Where(p => p.IsActive).Single(); }
        }

        #endregion

        #endregion

        #region Overrides of NodeAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("Status.Name");
            ModelState.Remove("Type.Name");
            ModelState.Remove("Occurrence.Name");
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                PositionGrade.CashBenefits.Where(a => a.Id == GetMasterRecordValue(MasterRecordOrder.Third));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            if (PositionGrade != null)
                return Position != null
                           ? Rules.GetExpiredRules(PositionGrade.CashBenefits)
                           : new List<BrokenBusinessRule>();

            return new List<BrokenBusinessRule>();
        }

        #endregion

        #region Implementation of IRule<AssignedCashBenefit>

        public ObjectRules<AssignedCashBenefit> Rules
        {
            get { return new AssignedCashBenefitRules(); }
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Third, selectedSubRowId);

            PrePublish();

            SaveTabIndex(2);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new AssignedCashBenefit
                                           {
                                               ActiveDate = DateTime.Today.Date,
                                               InactiveDate = DateTime.Today.Date
                                           });
        }

        [HttpPost]
        public JsonResult Save(AssignedCashBenefit assignedCashBenefit)
        {
            PrePublish();

            if (assignedCashBenefit.IsTransient())
            {
                PositionGrade.AddCashBenefit(assignedCashBenefit);
            }
            else
            {
                AssignedCashBenefit original = getAssignedCashBenefit(assignedCashBenefit.Id);

                assignedCashBenefit.Grade = PositionGrade;

                this.UpdateValueObject(assignedCashBenefit, original);
            }

            if ((Rules.GetBrokenRules(assignedCashBenefit).Count == 0) && (TryValidateModel(assignedCashBenefit)))
            {
                PositionService.Update(Position);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(assignedCashBenefit));

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", assignedCashBenefit)
                                });
            }

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", assignedCashBenefit)
                            });
        }

        [HttpDelete]
        public ActionResult JsonDelete()
        {
            var assignedCashBenefit = getAssignedCashBenefit(GetMasterRecordValue(MasterRecordOrder.Third));

            try
            {
                PositionGrade.CashBenefits.Remove(assignedCashBenefit);

                PositionService.Update(Position);

                PrePublish();

                return Json(new
                                {
                                    Success = true,
                                    PartialViewHtml = RenderPartialViewToString("List", new AssignedCashBenefit())
                                });
            }
            catch (Exception)
            {
                return ErrorPartialMessage(Resources.Shared.Messages.General.GeneralErrorOccurred);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var assignedCashBenefit = getAssignedCashBenefit(id);

            try
            {
                PositionGrade.CashBenefits.Remove(assignedCashBenefit);

                PositionService.Update(Position);

                PrePublish();

                return RedirectToAction("Index", "Position", new { selectedTabOrder = 2 });
            }
            catch (Exception)
            {
                return ErrorPartialMessage(Resources.Shared.Messages.General.GeneralErrorOccurred);
            }
        }

        [HttpPost]
        public JsonResult JsonEdit()
        {
            var assignedCashBenefit = getAssignedCashBenefit(GetMasterRecordValue(MasterRecordOrder.Third));

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("Edit", assignedCashBenefit)
                            });
        }

        private AssignedCashBenefit getAssignedCashBenefit(int id)
        {
            return PositionGrade.CashBenefits.SingleOrDefault(c => c.Id == id);
        }

        #endregion
    }
}