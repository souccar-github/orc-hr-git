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
    public class AssignedAssetController : NodeAggregateController, IRule<AssignedAsset>
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
            ModelState.Remove("CurrencyType.Name");
            ModelState.Remove("Per.Name");
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                PositionGrade.Assets.Where(a => a.Id == GetMasterRecordValue(MasterRecordOrder.Third));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            if (PositionGrade != null)
                return Position != null
                           ? Rules.GetExpiredRules(PositionGrade.Assets)
                           : new List<BrokenBusinessRule>();

            return new List<BrokenBusinessRule>();
        }

        #endregion

        #region Implementation of IRule<AssignedAssets>

        public ObjectRules<AssignedAsset> Rules
        {
            get { return new AssigendAssetRules(); }
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Third, selectedSubRowId);

            PrePublish();

            SaveTabIndex(1);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new AssignedAsset
                                           {
                                               PurchaseDate = DateTime.Today.Date,
                                               ExpiryDate = DateTime.Today.Date
                                           });
        }

        [HttpPost]
        public JsonResult Save(AssignedAsset assignedAsset)
        {
            PrePublish();

            if (assignedAsset.IsTransient())
            {
                PositionGrade.AddAsset(assignedAsset);
            }
            else
            {
                AssignedAsset original = getAssignedAsset(assignedAsset.Id);

                assignedAsset.Grade = PositionGrade;

                this.UpdateValueObject(assignedAsset, original);
            }

            if ((Rules.GetBrokenRules(assignedAsset).Count == 0) && (TryValidateModel(assignedAsset)))
            {
                PositionService.Update(Position);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(assignedAsset));

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", assignedAsset)
                                });
            }

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", assignedAsset)
                            });
        }

        [HttpDelete]
        public ActionResult JsonDelete()
        {
            var assignedAsset = getAssignedAsset(GetMasterRecordValue(MasterRecordOrder.Third));

            try
            {
                PositionGrade.Assets.Remove(assignedAsset);

                PositionService.Update(Position);

                PrePublish();

                return Json(new
                                {
                                    Success = true,
                                    PartialViewHtml = RenderPartialViewToString("List", new AssignedAsset())
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
            var assignedAsset = getAssignedAsset(id);

            try
            {
                PositionGrade.Assets.Remove(assignedAsset);

                PositionService.Update(Position);

                PrePublish();

                return RedirectToAction("Index", "Position", new { selectedTabOrder = 1 });
            }
            catch (Exception)
            {
                return ErrorPartialMessage(Resources.Shared.Messages.General.GeneralErrorOccurred);
            }
        }

        [HttpPost]
        public JsonResult JsonEdit()
        {
            var assignedAsset = getAssignedAsset(GetMasterRecordValue(MasterRecordOrder.Third));

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("Edit", assignedAsset)
                            });
        }

        private AssignedAsset getAssignedAsset(int id)
        {
            return PositionGrade.Assets.SingleOrDefault(c => c.Id == id);
        }

        #endregion
    }
}