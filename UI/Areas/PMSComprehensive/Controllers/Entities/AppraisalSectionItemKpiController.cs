using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.PMS.Entities;
using HRIS.Domain.PMS.Entities.Template;
using HRIS.Domain.PMS.RootEntities;
using Infrastructure.Validation;
using Telerik.Web.Mvc;
using UI.Areas.PMSComprehensive.Controllers.EntitiesRoots;
using UI.Areas.PMSComprehensive.DTO.ViewModels;
using UI.Extensions;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.PMSComprehensive.Entities;

namespace UI.Areas.PMSComprehensive.Controllers.Entities
{
    public class AppraisalSectionItemKpiController : AppraisalSectionAggregateController, IRule<AppraisalSectionItemKpi>
    {
        #region Parents Chain

        #region Appraisal Section

        private AppraisalSection _appraisalSection;

        public AppraisalSection FirstEntity
        {
            get
            {
                return _appraisalSection ??
                       (_appraisalSection = Repository.GetById(GetMasterRecordValue(MasterRecordOrder.First)));
            }
        }

        #endregion

        #region Appraisal Section Item

        private AppraisalSectionItem _appraisalSectionItem;

        public AppraisalSectionItem SecondEntity
        {
            get
            {
                return _appraisalSectionItem ??
                       (_appraisalSectionItem =
                        FirstEntity.Items.SingleOrDefault(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #region Appraisal Section Item Kpi

        private AppraisalSectionItemKpi _appraisalSectionItemKpi;

        public AppraisalSectionItemKpi ThirdEntity
        {
            get
            {
                return _appraisalSectionItemKpi ??
                       (_appraisalSectionItemKpi =
                        SecondEntity.Kpis.SingleOrDefault(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<AppraisalSectionItemKpi>

        public ObjectRules<AppraisalSectionItemKpi> Rules
        {
            get { return new AppraisalSectionItemKpiRules(); }
        }

        #endregion

        //
        // GET: /PMSComprehensive/AppraisalSectionItemKpi/

        public ActionResult Index(int sectionItemId)
        {

            PrePublish();

            SetMasterRecordValue(MasterRecordOrder.Second, sectionItemId);

            #region Security

            if (ViewData["CanRead"] != null && !(bool)ViewData["CanRead"])
            {
                return ErrorPartialMessage(Resources.Shared.Messages.General.CanReadMessage);
            }

            #endregion

            #region Get Data

            var appraisalSectionItemKpis = SecondEntity.Kpis;
            ViewData["appraisalSectionItemKpis"] = appraisalSectionItemKpis;
            ViewData["SelectedRow"] = GetMasterRecordValue(MasterRecordOrder.Second);

            #endregion

            return Json(new { PartialViewHtml = RenderPartialViewToString("AppraisalSectionItemKpiGrid") });
        }

        [GridAction]
        public ActionResult AjaxIndex()
        {
            var appraisalSectionItemKpis = SecondEntity.Kpis;
            var appraisalSectionItemKpisDTO = appraisalSectionItemKpis.Select(AppraisalSectionItemKpiViewModel.Create).ToList();
            ViewData["appraisalSectionItemKpis"] = appraisalSectionItemKpisDTO;

            return View("AppraisalSectionItemKpiGrid", new GridModel<AppraisalSectionItemKpiViewModel> { Data = appraisalSectionItemKpisDTO, Total = appraisalSectionItemKpisDTO.Count });
        }

        public ActionResult Create()
        {
            return Json(new
            {
                PartialViewHtml = RenderPartialViewToString("Create")
            });
        }

        public ActionResult JsonCreate(AppraisalSectionItemKpi appraisalSectionItemKpi)
        {

            PrePublish();

            #region Permission Check
            if (ViewData["CanCreate"] != null && !(bool)ViewData["CanCreate"])
            {
                ErrorPartialMessage(Resources.Shared.Messages.General.CanCreateMessage);
                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("Error")
                });
            }
            #endregion
            SecondEntity.AddKpi(appraisalSectionItemKpi);

            if ((Rules.GetBrokenRules(appraisalSectionItemKpi).Count == 0))
            {

                this.StringDecode(appraisalSectionItemKpi);
                try
                {
                    Repository.Update(FirstEntity);
                }
                catch (Exception)
                {
                }
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(appraisalSectionItemKpi));

                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("Create")
                });

            }

            SetMasterRecordValue(MasterRecordOrder.Third, appraisalSectionItemKpi.Id);

            PrePublish();
            ViewData["PageTo"] = 1;

            return Json(new
            {
                Success = true,
            });
        }


        public ActionResult Edit(int id)
        {
            var sectionItem = SecondEntity.Kpis.Single(x => x.Id == id);
            return Json(new
            {
                PartialViewHtml = RenderPartialViewToString("Create", sectionItem)
            });
        }

        public ActionResult JsonEdit(AppraisalSectionItemKpi appraisalSectionItemKpi)
        {
            PrePublish();

            #region Permission Check
            if (ViewData["CanUpdate"] != null && !(bool)ViewData["CanUpdate"])
            {
                ErrorPartialMessage(Resources.Shared.Messages.General.CanUpdateMessage);
                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("Error")
                });
            }
            #endregion
            appraisalSectionItemKpi.Item = SecondEntity;
            var original = SecondEntity.Kpis.Single(item => item.Id == appraisalSectionItemKpi.Id);
            this.UpdateValueObject(appraisalSectionItemKpi, original);

            if ((Rules.GetBrokenRules(appraisalSectionItemKpi).Count == 0))
            {
                this.StringDecode(appraisalSectionItemKpi);
                try
                {
                    Repository.Update(FirstEntity);
                }
                catch (Exception)
                {
                }
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(appraisalSectionItemKpi));

                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("Create", appraisalSectionItemKpi)
                });

            }

            PrePublish();

            ViewData["PageTo"] = 1;

            return Json(new
            {
                Success = true,
            });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult Delete(int id)
        {
            PrePublish();

            if (ViewData["CanDelete"] != null && !(bool)ViewData["CanDelete"])
            {
                var oldItems = SecondEntity.Kpis.Select(AppraisalSectionItemKpiViewModel.Create).ToList();

                return View("AppraisalSectionItemKpiGrid", new GridModel<AppraisalSectionItemKpiViewModel>() { Data = oldItems, Total = oldItems.Count });
            }
            var appraisalSectionItemKpi = SecondEntity.Kpis.Single(kpi => kpi.Id == id);
            SecondEntity.Kpis.Remove(appraisalSectionItemKpi);
            if (TryUpdateModel(appraisalSectionItemKpi))
            {
                Repository.Update(FirstEntity);
            }
            var appraisalSectionItemsDTO = SecondEntity.Kpis.Select(AppraisalSectionItemKpiViewModel.Create).ToList();

            return View("AppraisalSectionItemKpiGrid", new GridModel<AppraisalSectionItemKpiViewModel>() { Data = appraisalSectionItemsDTO, Total = appraisalSectionItemsDTO.Count });
        }

    }
}
