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
    public class AssignedCashDeductionController : NodeAggregateController, IRule<AssignedCashDeduction>
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
            ModelState.Remove("Type.Name");
            ModelState.Remove("Status.Name");
            ModelState.Remove("Occurrence.Name");
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                PositionGrade.CashDeductions.Where(a => a.Id == GetMasterRecordValue(MasterRecordOrder.Third));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            if (PositionGrade != null)
                return Position != null
                           ? Rules.GetExpiredRules(PositionGrade.CashDeductions)
                           : new List<BrokenBusinessRule>();

            return new List<BrokenBusinessRule>();
        }

        #endregion

        #region Implementation of IRule<AssignedCashDeduction>

        public ObjectRules<AssignedCashDeduction> Rules
        {
            get { return new AssignedCashDeductionRules(); }
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Third, selectedSubRowId);

            PrePublish();

            SaveTabIndex(3);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new AssignedCashDeduction
                                           {
                                               ActiveDate = DateTime.Today.Date,
                                               InactiveDate = DateTime.Today.Date
                                           });
        }

        [HttpPost]
        public JsonResult Save(AssignedCashDeduction assignedCashDeduction)
        {
            PrePublish();

            if (assignedCashDeduction.IsTransient())
            {
                PositionGrade.AddCashDeduction(assignedCashDeduction);
            }
            else
            {
                AssignedCashDeduction original = getAssignedCashDeduction(assignedCashDeduction.Id); ;

                assignedCashDeduction.Grade = PositionGrade;

                this.UpdateValueObject(assignedCashDeduction, original);
            }

            if ((Rules.GetBrokenRules(assignedCashDeduction).Count == 0) && (TryValidateModel(assignedCashDeduction)))
            {
                PositionService.Update(Position);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(assignedCashDeduction));

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", assignedCashDeduction)
                                });
            }

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", assignedCashDeduction)
                            });
        }

        [HttpDelete]
        public ActionResult JsonDelete(int id)
        {
            var assignedCashDeduction = getAssignedCashDeduction(GetMasterRecordValue(MasterRecordOrder.Third));

            try
            {
                PositionGrade.CashDeductions.Remove(assignedCashDeduction);

                PositionService.Update(Position);

                PrePublish();

                return Json(new
                                {
                                    Success = true,
                                    PartialViewHtml = RenderPartialViewToString("List", new AssignedCashDeduction())
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
            var assignedCashDeduction = getAssignedCashDeduction(id);

            try
            {
                PositionGrade.CashDeductions.Remove(assignedCashDeduction);

                PositionService.Update(Position);

                PrePublish();

                return RedirectToAction("Index", "Position", new { selectedTabOrder = 3 });
            }
            catch (Exception)
            {
                return ErrorPartialMessage(Resources.Shared.Messages.General.GeneralErrorOccurred);
            }
        }

        [HttpPost]
        public JsonResult JsonEdit(int id)
        {
            var assignedCashDeduction = getAssignedCashDeduction(GetMasterRecordValue(MasterRecordOrder.Third));

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("Edit", assignedCashDeduction)
                            });
        }

        private AssignedCashDeduction getAssignedCashDeduction(int id)
        {
            return PositionGrade.CashDeductions.SingleOrDefault(c => c.Id == id);
        }

        #endregion
    }
}