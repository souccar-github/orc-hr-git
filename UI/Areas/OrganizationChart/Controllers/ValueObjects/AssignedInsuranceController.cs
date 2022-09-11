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
    public class AssignedInsuranceController : NodeAggregateController, IRule<AssignedInsurance>
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
            ModelState.Remove("InsuranceCompany.Name");
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                PositionGrade.Insurances.Where(a => a.Id == GetMasterRecordValue(MasterRecordOrder.Third));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            if (PositionGrade != null)
                return Position != null
                           ? Rules.GetExpiredRules(PositionGrade.Insurances)
                           : new List<BrokenBusinessRule>();

            return new List<BrokenBusinessRule>();
        }

        #endregion

        #region Implementation of IRule<AssignedInsurance>

        public ObjectRules<AssignedInsurance> Rules
        {
            get { return new AssignedInsuranceRules(); }
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Third, selectedSubRowId);

            PrePublish();

            SaveTabIndex(4);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new AssignedInsurance
                                           {
                                               ActiveDate = DateTime.Today.Date,
                                               ExpiryDate = DateTime.Today.Date
                                           });
        }

        [HttpPost]
        public JsonResult Save(AssignedInsurance assignedInsurance)
        {
            PrePublish();

            if (assignedInsurance.IsTransient())
            {
                PositionGrade.AddInsurance(assignedInsurance);
            }
            else
            {
                AssignedInsurance original = getAssignedInsurance(assignedInsurance.Id);

                assignedInsurance.Grade = PositionGrade;

                this.UpdateValueObject(assignedInsurance, original);
            }

            if ((Rules.GetBrokenRules(assignedInsurance).Count == 0) && (TryValidateModel(assignedInsurance)))
            {
                PositionService.Update(Position);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(assignedInsurance));

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", assignedInsurance)
                                });
            }

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", assignedInsurance)
                            });
        }

        [HttpDelete]
        public ActionResult JsonDelete()
        {
            var assignedInsurance = getAssignedInsurance(GetMasterRecordValue(MasterRecordOrder.Third));

            try
            {
                PositionGrade.Insurances.Remove(assignedInsurance);

                PositionService.Update(Position);

                PrePublish();

                return Json(new
                                {
                                    Success = true,
                                    PartialViewHtml = RenderPartialViewToString("List", new AssignedInsurance())
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
            var assignedInsurance = getAssignedInsurance(id);

            try
            {
                PositionGrade.Insurances.Remove(assignedInsurance);

                PositionService.Update(Position);

                PrePublish();

                return RedirectToAction("Index", "Position", new { selectedTabOrder = 4 });
            }
            catch (Exception)
            {
                return ErrorPartialMessage(Resources.Shared.Messages.General.GeneralErrorOccurred);
            }
        }

        [HttpPost]
        public JsonResult JsonEdit()
        {
            var assignedInsurance = getAssignedInsurance(GetMasterRecordValue(MasterRecordOrder.Third));

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("Edit", assignedInsurance)
                            });
        }

        private AssignedInsurance getAssignedInsurance(int id)
        {
            return PositionGrade.Insurances.SingleOrDefault(c => c.Id == id);
        }

        #endregion
    }
}