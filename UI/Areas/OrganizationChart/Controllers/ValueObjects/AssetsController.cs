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
    public class AssetsController : GradeAggregateController, IRule<Asset>
    {
        #region Overrides of GradeAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("Status.Name");
            ModelState.Remove("Type.Name");
            ModelState.Remove("CurrencyType.Name");
            ModelState.Remove("Per.Name");
        }

        public override void FillList()
        {
            Grade grade = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First));
            if (grade != null)
                ViewData["ValueObjectsList"] =
                    grade.Assets.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            Grade grade = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First));
            return grade != null
                       ? Rules.GetExpiredRules(grade.Assets)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region Implementation of IRule<Assets>

        public ObjectRules<Asset> Rules
        {
            get { return new AssetRules(); }
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Second, selectedSubRowId);

            PrePublish();

            return GetMasterRecordValue(MasterRecordOrder.First) != 0
                       ? PartialView("Index")
                       : ErrorPartialMessage(Resources.Areas.OrgChart.Entities.Grade.GradeRules.NoGradeSelectedMessage);
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new Asset
                                           {
                                               PurchaseDate = DateTime.Today.Date,
                                               ExpiryDate = DateTime.Today.Date
                                           });
        }

        [HttpPost]
        public JsonResult Save(Asset assets)
        {
            PrePublish();

            Grade grade = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First));

            if (assets.IsTransient())
            {
                grade.AddAsset(assets);
            }
            else
            {
                Asset original = grade.Assets.Single(c => c.Id == assets.Id);

                assets.Grade = original.Grade;

                this.UpdateValueObject(assets, original);
            }

            if ((Rules.GetBrokenRules(assets).Count == 0) && (TryValidateModel(assets)))
            {
                Service.Update(grade);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(assets));

                grade.Assets.Remove(assets);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", assets)
                                });
            }

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", assets)
                            });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Grade grade = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First));
            Asset asset = grade.Assets.SingleOrDefault(c => c.Id == id);

            try
            {
                grade.Assets.Remove(asset);

                Service.Update(grade);

                SetMasterRecordValue(MasterRecordOrder.Second, 0);
                PrePublish();

                return RedirectToAction("Index", "Grade", new {selectedTabOrder = 2});
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
            Asset asset = grade.Assets.SingleOrDefault(c => c.Id == id);
            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("Edit", asset)
                            });
        }

        #endregion
    }
}