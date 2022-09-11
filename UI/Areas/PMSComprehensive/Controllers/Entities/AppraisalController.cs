#region

using System;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.PMS.RootEntities;
using Infrastructure.Validation;

using Telerik.Web.Mvc;
using UI.Areas.PMSComprehensive.Controllers.EntitiesRoots;
using UI.Areas.PMSComprehensive.Helpers;
using UI.Extensions;
using UI.Helpers.Cache;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.PMSComprehensive.Entities;
using DropDownListHelpers = UI.Helpers.DropDownListHelpers;

#endregion

namespace UI.Areas.PMSComprehensive.Controllers.Entities
{
    public class AppraisalController : AppraisalAggregateController, IRule<Appraisal>
    {

        #region IRule<Apprasial> Members

        public ObjectRules<Appraisal> Rules
        {
            get { return new AppraisalRules(); }
        }

        #endregion

        #region Overrides of ApprasialAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("Type.Name");
            ModelState.Remove("Period.Name");
        }

        #endregion

        #region CRUD

        #region Read

        [GridAction]
        public ActionResult Index(int id = 0, int selectedTabOrder = 0, bool ribbon = false,
                                  bool ribbonSubEntity = false)
        {
            PrePublish();

            #region Security

            if (ViewData["CanRead"] != null && !(bool)ViewData["CanRead"])
            {
                return ErrorPartialMessage("You Are Not Allowed To Browse This Module !!");
            }

            #endregion

            #region Manage Tab, Path, and MastersList

            if (ribbonSubEntity)
            {
                SetMasterRecordValue(MasterRecordOrder.Second, 0);
                CurrentlyInSecondLevel = 0;
            }

            if (ribbon)
            {
                ClearMasterRecords();
                SaveTabIndex(0);
            }
            else
            {
                if (id != 0)
                {
                    SetMasterRecordValue(MasterRecordOrder.First, id);
                }
            }


            AddToPath(MasterRecordOrder.First, RibbonLevels.Root, stepId: id,
                      areaName: PMSComprehensiveAreaRegistration.GetAreaName, nodeName: "Appraisal");

            #endregion

            #region Get Data


            var appraisals = Service.GetAll();

            int pageNo = 1;

            if (GetMasterRecordValue(MasterRecordOrder.First) != 0)
            {
                int masterRecordValue = GetMasterRecordValue(MasterRecordOrder.First);

                int count = appraisals.Where(appra => (appra.Id >= masterRecordValue)).Count();

                pageNo = count / 5;

                if (count % 5 > 0)
                {
                    pageNo++;
                }

                pageNo = pageNo == 0 ? 1 : pageNo;
            }

            ViewData["appraisal"] = appraisals;
            ViewData["SelectedRow"] = GetMasterRecordValue(MasterRecordOrder.First);
            ViewData["PageTo"] = pageNo;

            #endregion

            return View();
        }

        public ActionResult PartialMasterInfo(int selectedRowId = 0)
        {
            PrePublish();

            if (selectedRowId != 0)
            {
                SetMasterRecordValue(MasterRecordOrder.First, selectedRowId);
                CurrentlyInFirstLevel = true;
            }

            Appraisal appraisal =
                Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First));

            return PartialView("BasicInfo", appraisal);
        }

        #endregion

        #region Create

        public ActionResult Insert()
        {
            LoadStepsList();

            AddToPath(MasterRecordOrder.First, RibbonLevels.Root, areaName: PMSComprehensiveAreaRegistration.GetAreaName,
                      nodeName: "PMS Comprehensive");

            return View("Insert", new Appraisal());
        }

        [HttpPost]
        public ActionResult JsonInsert(Appraisal appraisal)
        {
            PrePublish();

            #region Permission Check
            if (ViewData["CanCreate"] != null && !(bool)ViewData["CanCreate"])
            {
                ErrorPartialMessage("You Are Not Allowed To Add !!");
                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("Error")
                });
            }
            #endregion

            if ((Rules.GetBrokenRules(appraisal).Count == 0))
            {

                this.StringDecode(appraisal);

                try
                {
                    Service.Update(appraisal);
                }
                catch (Exception)
                {
                }
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(appraisal));

                return View("Insert", appraisal);

            }

            SetMasterRecordValue(MasterRecordOrder.First, appraisal.Id);

            PrePublish();

            CacheProvider.ForceUpdate(PMSComprehensiveCacheKeys.Appraisal.ToString());

            return RedirectToAction("Index", new { id = appraisal.Id });

        }

        #endregion

        #region Update
        public ActionResult Edit(int id)
        {
            Appraisal appraisal = Service.LoadById(id);

            return PartialView("Edit", appraisal);
        }


        public ActionResult JsonEdit(Appraisal appraisal)
        {
            PrePublish();

            #region Permission Check

            if (ViewData["CanUpdate"] != null && !(bool)ViewData["CanUpdate"])
            {
                return Json(new
                {
                    Success = false,
                    PartialViewHtml = ErrorPartialMessage("You Are Not Allowed To Update !!")
                });
            }

            #endregion


            if ((Rules.GetBrokenRules(appraisal).Count == 0) && (TryValidateModel(appraisal)))
            {
                Service.Update(appraisal);
            }
            else
            {


                ModelState.AddModelErrors(Rules.GetBrokenRules(appraisal));

                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("Edit", appraisal)
                });
            }

            SetMasterRecordValue(MasterRecordOrder.First, appraisal.Id);

            PrePublish();

            CacheProvider.ForceUpdate(PMSComprehensiveCacheKeys.Appraisal.ToString());

            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("BasicInfo", appraisal)
            });
        } 
        #endregion

        #region Delete

        [HttpPost]
        public ActionResult Delete(int id)
        {
            PrePublish();

            if (ViewData["CanDelete"] != null && !(bool)ViewData["CanDelete"])
            {
                return RedirectToAction("Index");
            }

            Appraisal appraisal = Service.LoadById(id);

            if (TryUpdateModel(appraisal))
            {
                Service.Delete(appraisal);
            }

            SetMasterRecordValue(MasterRecordOrder.First, 0);

            return RedirectToAction("Index");
        }

        #endregion

        #endregion

        #region Back To Master

        public ActionResult GoToAppraisal(int id = 0, int selectedTabOrder = 0, bool ribbon = false,
                                        bool ribbonSubEntity = false)
        {
            if (selectedTabOrder > 0)
            {
                SaveTabIndex(selectedTabOrder == 11 ? 0 : selectedTabOrder);
            }

            CurrentlyInSecondLevel = 0;

            return RedirectToAction("Index", "Appraisal", new { id, ribbon, ribbonSubEntity });
        }

        #endregion

        #region Go To Details

        public ActionResult GoToProjectSection(int selectedTabOrder = 0)
        {
            SaveTabIndexSecondLevel(selectedTabOrder);

            if (GetMasterRecordValue(MasterRecordOrder.Second) == 0)
            {
                return RedirectToAction("Index", new { selectedTabOrder = 1 });
            }

            AddToPath(MasterRecordOrder.First, RibbonLevels.Root, stepId: GetMasterRecordValue(MasterRecordOrder.First),
                      areaName: PMSComprehensiveAreaRegistration.GetAreaName, nodeName: "ProjectSection",
                      actionName: "GoToProjectSection");

            return RedirectToAction("MasterIndex", "ProjectSection");
        }

        #endregion
    }
}
