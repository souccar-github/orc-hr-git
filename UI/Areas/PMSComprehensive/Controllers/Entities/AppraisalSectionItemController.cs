using System;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.PMS.Entities;
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
    public class AppraisalSectionItemController : AppraisalSectionAggregateController, IRule<AppraisalSectionItem>
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

        #endregion

        #region Implementation of IRule<AppraisalSectionItem>

        public ObjectRules<AppraisalSectionItem> Rules
        {
            get { return new AppraisalSectionItemRules(); }
        }

        #endregion

        public ActionResult Index(int sectionId)
        {

            PrePublish();

            SetMasterRecordValue(MasterRecordOrder.First, sectionId);

            #region Security

            if (ViewData["CanRead"] != null && !(bool)ViewData["CanRead"])
            {
                return ErrorPartialMessage(Resources.Shared.Messages.General.CanReadMessage);
            }

            #endregion

            #region Get Data

            var appraisalSectionItems = FirstEntity.Items;
            var appraisalSectionItemsDTO = appraisalSectionItems.Select(AppraisalSectionItemViewModel.Create).ToList();
            ViewData["AppraisalSectionItems"] = appraisalSectionItemsDTO;
            ViewData["SelectedRow"] = GetMasterRecordValue(MasterRecordOrder.First);

            #endregion

            return Json(new { PartialViewHtml = RenderPartialViewToString("AppraisalSectionItemGrid") });
        }

        [GridAction]
        public ActionResult AjaxIndex()
        {
            var appraisalSectionItems = FirstEntity.Items;
            var appraisalSectionItemsDTO = appraisalSectionItems.Select(AppraisalSectionItemViewModel.Create).ToList();
            ViewData["AppraisalSectionItems"] = appraisalSectionItemsDTO;

            return View("AppraisalSectionItemGrid", new GridModel { Data = appraisalSectionItemsDTO, Total = appraisalSectionItems.Count });
        }

        public ActionResult Create()
        {
            return Json(new
            {
                PartialViewHtml = RenderPartialViewToString("Create")
            });
        }

        public ActionResult JsonCreate(AppraisalSectionItem appraisalSectionItem)
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
            FirstEntity.AddSectionItem(appraisalSectionItem);

            if ((Rules.GetBrokenRules(appraisalSectionItem).Count == 0))
            {

                this.StringDecode(appraisalSectionItem);
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
                ModelState.AddModelErrors(Rules.GetBrokenRules(appraisalSectionItem));

                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("Create")
                });

            }

            SetMasterRecordValue(MasterRecordOrder.Second, appraisalSectionItem.Id);

            PrePublish();

            ViewData["PageTo"] = 1;

            return Json(new
            {
                Success = true,
            });
        }

        public ActionResult Edit(int id)
        {
            var sectionItem = FirstEntity.Items.Single(x => x.Id == id);
            return Json(new
            {
                PartialViewHtml = RenderPartialViewToString("Create", sectionItem)
            });
        }

        public ActionResult JsonEdit(AppraisalSectionItem appraisalSectionItem)
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
            appraisalSectionItem.Section = FirstEntity;
            var original = FirstEntity.Items.Single(item => item.Id == appraisalSectionItem.Id);
            this.UpdateValueObject(appraisalSectionItem, original);

            if ((Rules.GetBrokenRules(appraisalSectionItem).Count == 0))
            {
                this.StringDecode(appraisalSectionItem);
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
                ModelState.AddModelErrors(Rules.GetBrokenRules(appraisalSectionItem));

                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("Create", appraisalSectionItem)
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
                var oldItems = FirstEntity.Items.Select(AppraisalSectionItemViewModel.Create).ToList();

                return View("AppraisalSectionItemGrid", new GridModel<AppraisalSectionItemViewModel>() { Data = oldItems, Total = oldItems.Count });
            }
            var appraisalSectionItemKpi = FirstEntity.Items.Single(kpi => kpi.Id == id);
            FirstEntity.Items.Remove(appraisalSectionItemKpi);
            if (TryUpdateModel(appraisalSectionItemKpi))
            {
                Repository.Update(FirstEntity);
            }
            var appraisalSectionItemsDTO = FirstEntity.Items.Select(AppraisalSectionItemViewModel.Create).ToList();

            return View("AppraisalSectionItemGrid", new GridModel<AppraisalSectionItemViewModel> { Data = appraisalSectionItemsDTO, Total = appraisalSectionItemsDTO.Count });
        }
    }
}
;