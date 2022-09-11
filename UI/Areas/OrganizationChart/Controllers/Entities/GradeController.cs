#region

using System;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.OrgChart.Entities;
using Telerik.Web.Mvc;
using UI.Areas.OrganizationChart.Controllers.EntitiesRoots;
using UI.Areas.OrganizationChart.Helpers;
using UI.Helpers.Cache;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.OrganizationChart.Entities;

#endregion

namespace UI.Areas.OrganizationChart.Controllers.Entities
{
    public class GradeController : GradeAggregateController, IRule<Grade>
    {
        #region IRule<Grade> Members

        public ObjectRules<Grade> Rules
        {
            get { return new GradeRules(); }
        }

        #endregion

        #region Overrides of GradeAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("Level.Name");
            ModelState.Remove("Level.Order");
            ModelState.Remove("Name.Name");
            ModelState.Remove("Step.Name");
        }

        #endregion

        #region Utilities

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
                return ErrorPartialMessage(Resources.Shared.Messages.General.CanReadMessage);
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

                if (selectedTabOrder > 0)
                {
                    SaveTabIndex(selectedTabOrder);
                }
            }

            AddToPath(MasterRecordOrder.First, RibbonLevels.B, areaName: OrganizationChartAreaRegistration.GetAreaName,
            nodeName: Resources.Areas.OrgChart.Views.Shared.Navigator.Grade);

            #endregion

            #region Get Data

            IQueryable<Grade> grades = Service.GetAll();

            int pageNo = 1;

            if (GetMasterRecordValue(MasterRecordOrder.First) != 0)
            {
                int masterRecordValue = GetMasterRecordValue(MasterRecordOrder.First);

                int count = grades.Where(organizations => (organizations.Id >= masterRecordValue)).Count();

                pageNo = count / 5;

                if (count % 5 > 0)
                {
                    pageNo++;
                }

                pageNo = pageNo == 0 ? 1 : pageNo;
            }

            ViewData["grades"] = grades;
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
            }

            CurrentlyInSecondLevel = GetMasterRecordValue(MasterRecordOrder.First);

            Grade grade = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First));

            return PartialView("BasicInfo", grade);
        }

        #endregion

        #region Create

        public ActionResult Insert()
        {
            LoadStepsList();

            AddToPath(MasterRecordOrder.First, RibbonLevels.B,
                      areaName: OrganizationChartAreaRegistration.GetAreaName,
                      nodeName: Resources.Areas.OrgChart.Views.Shared.Navigator.Grade);

            return View("Insert", new Grade());
        }


        [HttpPost]
        public ActionResult JsonInsert(Grade grade)
        {
            PrePublish();

            if (ViewData["CanCreate"] != null && !(bool)ViewData["CanCreate"])
            {
                ErrorPartialMessage(Resources.Shared.Messages.General.CanCreateMessage);
                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("Error")
                                });
            }

            if ((Rules.GetBrokenRules(grade).Count == 0) && (TryValidateModel(grade)))
            {
                try
                {
                    Service.Update(grade);
                }
                catch (Exception)
                {
                }
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(grade));

                return View("Insert", grade);
            }

            SetMasterRecordValue(MasterRecordOrder.First, grade.Id);

            PrePublish();

            CacheProvider.ForceUpdate(OrganizationChartCacheKeys.Grade.ToString());

            return RedirectToAction("Index", new { id = grade.Id });
        }

        #endregion

        #region Update

        public ActionResult Edit(int id)
        {
            Grade grade = Service.GetById(id);

            return PartialView("Edit", grade);
        }

        [HttpPost]
        public ActionResult JsonEdit(Grade grade)
        {
            PrePublish();

            if (ViewData["CanUpdate"] != null && !(bool)ViewData["CanUpdate"])
            {
                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = ErrorPartialMessage(Resources.Shared.Messages.General.CanUpdateMessage)
                                });
            }

            if ((Rules.GetBrokenRules(grade).Count == 0) && (TryValidateModel(grade)))
            {
                Service.Update(grade);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(grade));

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("Create", grade)
                                });
            }

            SetMasterRecordValue(MasterRecordOrder.First, grade.Id);

            PrePublish();

            CacheProvider.ForceUpdate(OrganizationChartCacheKeys.Grade.ToString());

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("BasicInfo", grade)
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

            Grade grade = Service.LoadById(id);

            if (TryUpdateModel(grade))
            {
                Service.Delete(grade);
            }

            SetMasterRecordValue(MasterRecordOrder.First, 0);

            return RedirectToAction("Index");
        }

        #endregion

        #endregion
    }
}