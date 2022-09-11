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
    public class AssignedNonCashBenefitController : NodeAggregateController, IRule<AssignedNonCashBenefit>
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
                PositionGrade.NonCashBenefits.Where(a => a.Id == GetMasterRecordValue(MasterRecordOrder.Third));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            if (PositionGrade != null)
                return Position != null
                           ? Rules.GetExpiredRules(PositionGrade.NonCashBenefits)
                           : new List<BrokenBusinessRule>();

            return new List<BrokenBusinessRule>();
        }

        #endregion

        #region Implementation of IRule<AssignedNonCashBenefit>

        public ObjectRules<AssignedNonCashBenefit> Rules
        {
            get { return new AssignedNonCashBenefitRules(); }
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Third, selectedSubRowId);

            PrePublish();

            SaveTabIndex(5);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new AssignedNonCashBenefit
                                           {
                                               ActiveDate = DateTime.Today.Date,
                                               InactiveDate = DateTime.Today.Date
                                           });
        }

        [HttpPost]
        public JsonResult Save(AssignedNonCashBenefit assignedNonCashBenefit)
        {
            PrePublish();

            if (assignedNonCashBenefit.IsTransient())
            {
                PositionGrade.AddNonCashBenefit(assignedNonCashBenefit);
            }
            else
            {
                AssignedNonCashBenefit original = getAssignedNonCashBenefit(assignedNonCashBenefit.Id);

                assignedNonCashBenefit.Grade = PositionGrade;

                this.UpdateValueObject(assignedNonCashBenefit, original);
            }

            if ((Rules.GetBrokenRules(assignedNonCashBenefit).Count == 0) && (TryValidateModel(assignedNonCashBenefit)))
            {
                PositionService.Update(Position);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(assignedNonCashBenefit));

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", assignedNonCashBenefit)
                                });
            }

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", assignedNonCashBenefit)
                            });
        }

        [HttpDelete]
        public ActionResult JsonDelete()
        {
            var assignedNonCashBenefit = getAssignedNonCashBenefit(GetMasterRecordValue(MasterRecordOrder.Third));

            try
            {
                PositionGrade.NonCashBenefits.Remove(assignedNonCashBenefit);

                PositionService.Update(Position);

                PrePublish();

                return Json(new
                                {
                                    Success = true,
                                    PartialViewHtml = RenderPartialViewToString("List", new AssignedNonCashBenefit())
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
            var assignedNonCashBenefit = getAssignedNonCashBenefit(id);

            try
            {
                PositionGrade.NonCashBenefits.Remove(assignedNonCashBenefit);

                PositionService.Update(Position);

                PrePublish();

                return RedirectToAction("Index", "Position", new { selectedTabOrder = 5 });
            }
            catch (Exception)
            {
                return ErrorPartialMessage(Resources.Shared.Messages.General.GeneralErrorOccurred);
            }
        }

        [HttpPost]
        public JsonResult JsonEdit()
        {
            var assignedNonCashBenefit = getAssignedNonCashBenefit(GetMasterRecordValue(MasterRecordOrder.Third));

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("Edit", assignedNonCashBenefit)
                            });
        }

        private AssignedNonCashBenefit getAssignedNonCashBenefit(int id)
        {
            return PositionGrade.NonCashBenefits.SingleOrDefault(c => c.Id == id);
        }

        #endregion
    }
}