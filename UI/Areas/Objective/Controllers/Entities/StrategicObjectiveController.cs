#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.Objectives.RootEntities;
using Infrastructure.Validation;
using HRIS.Domain.Objectives.Entities;
using Service;
using Telerik.Web.Mvc;
using UI.Areas.Objective.Controllers.EntitiesRoots;
using UI.Areas.Objective.Helpers;
using UI.Extensions;
using UI.Helpers.Cache;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.Objective.Entities;

#endregion

namespace UI.Areas.Objective.Controllers.Entities
{
    public class StrategicObjectiveController : StrategicObjectiveAggregateController,
                                                     IRule<StrategicObjective>
    {
        #region IRule<OrganizationalObjective> Members

        public ObjectRules<StrategicObjective> Rules
        {
            get { return new StrategicObjectiveRules(); }
        }

        #endregion

        #region Overrides of OrganizationalObjectiveAggregateController

        public override void CleanUpModelState()
        {
            //ModelState.Remove("JobRole.Name");
            //ModelState.Remove("JobTitle.Name");
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
            }

            AddToPath(MasterRecordOrder.First, RibbonLevels.A, stepId: id,
                      areaName: ObjectiveAreaRegistration.GetAreaName, nodeName: Resources.Areas.Objective.Views.Shared.Navigator.OrganiozationalObjective);

            #endregion

            #region Get Data

            IQueryable<StrategicObjective> organizationalObjectives = Service.GetAll();

            int pageNo = 1;
            if (GetMasterRecordValue(MasterRecordOrder.First) != 0)
            {
                int masterRecordValue = GetMasterRecordValue(MasterRecordOrder.First);

                int count =
                    organizationalObjectives.Where(
                        organizationalObjective => (organizationalObjective.Id >= masterRecordValue)).Count();

                pageNo = count / 5;

                if (count % 5 > 0)
                {
                    pageNo++;
                }

                pageNo = pageNo == 0 ? 1 : pageNo;
            }

            ViewData["organizationalObjectives"] = organizationalObjectives;
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

            StrategicObjective organizationalObjective =
                Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First));

            return PartialView("BasicInfo", organizationalObjective);
        }

        #endregion

        #region Create

        public ActionResult Insert()
        {
            LoadStepsList();

            AddToPath(MasterRecordOrder.First, RibbonLevels.Root, areaName: ObjectiveAreaRegistration.GetAreaName,
                      nodeName: Resources.Areas.Objective.Entities.OrganizationalObjective.OrganizationalObjectiveModel.OrganizationalObjective);

            return View("Insert", new StrategicObjective());
        }

        [HttpPost]
        public ActionResult JsonInsert(StrategicObjective organizationalObjective)
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

            if ((Rules.GetBrokenRules(organizationalObjective).Count == 0) &&
                (TryValidateModel(organizationalObjective)))
            {
                this.StringDecode(organizationalObjective);
                organizationalObjective.Code.Generate();
                try
                {
                    Service.Update(organizationalObjective);
                }
                catch (Exception)
                {
                }
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(organizationalObjective));

                return View("Insert", organizationalObjective);
            }

            SetMasterRecordValue(MasterRecordOrder.First, organizationalObjective.Id);

            PrePublish();

            CacheProvider.ForceUpdate(ObjectiveCacheKeys.OrganizationalObjective.ToString());

            return RedirectToAction("Index", new { id = organizationalObjective.Id });
        }

        #endregion

        #region Update

        public ActionResult Edit(int id)
        {
            StrategicObjective organizationalObjective = Service.LoadById(id);

            return PartialView("Edit", organizationalObjective);
        }

        [HttpPost]
        public ActionResult JsonEdit(StrategicObjective organizationalObjective)
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

            if ((Rules.GetBrokenRules(organizationalObjective).Count == 0) &&
                (TryValidateModel(organizationalObjective)))
            {
                Service.Update(organizationalObjective);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(organizationalObjective));

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("Edit", organizationalObjective)
                                });
            }

            SetMasterRecordValue(MasterRecordOrder.First, organizationalObjective.Id);

            PrePublish();

            CacheProvider.ForceUpdate(ObjectiveCacheKeys.OrganizationalObjective.ToString());

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("BasicInfo", organizationalObjective)
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

            StrategicObjective organizationalObjective = Service.LoadById(id);

            if (TryUpdateModel(organizationalObjective))
            {
                Service.Delete(organizationalObjective);
            }

            SetMasterRecordValue(MasterRecordOrder.First, 0);

            return RedirectToAction("Index");
        }

        #endregion

        #endregion

        #region Back To Master

        public ActionResult GoToOrganizationalObjective(int id = 0, int selectedTabOrder = 0, bool ribbon = false,
                                                        bool ribbonSubEntity = false)
        {
            if (selectedTabOrder > 0)
            {
                SaveTabIndex(selectedTabOrder == 11 ? 0 : selectedTabOrder);
            }

            return RedirectToAction("Index", "StrategicObjective", new { id, ribbon, ribbonSubEntity });
        }

        #endregion
    }
}