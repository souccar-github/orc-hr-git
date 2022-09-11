#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Utilities;
using Infrastructure.Validation;
using HRIS.Domain.PMS.Entities;
using HRIS.Domain.PMS.ValueObjects;
using UI.Areas.PMSComprehensive.Controllers.EntitiesRoots;
using UI.Extensions;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.PMSComprehensive.ValueObjects;

#endregion

namespace UI.Areas.PMSComprehensive.Controllers.ValueObjects 
{
    public class TemplateItemKpiController : AppraisalTemplateAggregateController, IRule<TemplateItemKpi>
    {
        #region Parents Chain

        #region AppraisalTemplate

        private AppraisalTemplate _appraisalTemplate;

        public AppraisalTemplate FirstEntity
        {
            get
            {
                return _appraisalTemplate ??
                       (_appraisalTemplate = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First)));
            }
        }

        #endregion

        #region TemplateCustomizedSection

        private TemplateCustomizedSection _templateCustomizedSection;

        public TemplateCustomizedSection SecondEntity
        {
            get
            {
                return _templateCustomizedSection ??
                       (_templateCustomizedSection = FirstEntity.Sections.SingleOrDefault(r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #region TemplateSectionItem

        private TemplateSectionItem _templateSectionItem;

        public TemplateSectionItem ThirdEntity
        {
            get
            {
                return _templateSectionItem ??
                       (_templateSectionItem =
                        SecondEntity.SectionItems.SingleOrDefault(k => k.Id == GetMasterRecordValue(MasterRecordOrder.Third)));
            }
        }

        #endregion

        #region TemplateItemKpi

        private TemplateItemKpi _templateItemKpi;

        public TemplateItemKpi FourthEntity
        {
            get
            {
                return _templateItemKpi ??
                       (_templateItemKpi =
                        ThirdEntity.Kpis.SingleOrDefault(k => k.Id == GetMasterRecordValue(MasterRecordOrder.Fourth)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<TemplateItemKpi>

        public ObjectRules<TemplateItemKpi> Rules
        {
            get { return new TemplateItemKpiRules(); }
        }

        #endregion

        #region Overrides of AppraisalTemplateAggregateController

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                ThirdEntity.Kpis.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Fourth));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return ThirdEntity.Kpis.Count != 0
                       ? Rules.GetExpiredRules(ThirdEntity.Kpis)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Fourth, selectedSubRowId);

            PrePublish();

            SaveTabIndexSecondLevel(0);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new TemplateItemKpi());
        }

        [HttpPost]
        public ActionResult Save(TemplateItemKpi templateItemKpi)
        {
            PrePublish();

            if (templateItemKpi.IsTransient())
            {
                ThirdEntity.AddKpi(templateItemKpi);
            }
            else
            {
                templateItemKpi.TemplateSectionItem = FourthEntity.TemplateSectionItem;

                this.UpdateValueObject(templateItemKpi, FourthEntity);
                this.StringDecode(FourthEntity);
            }

            if ((Rules.GetBrokenRules(templateItemKpi).Count == 0) && (TryValidateModel(templateItemKpi)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(templateItemKpi));

                ThirdEntity.Kpis.Remove(templateItemKpi);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", templateItemKpi)
                                });
            }
            
            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", templateItemKpi)
                            });
        }

        //[HttpPost]
        public ActionResult JsonEdit()
        {
            return PartialView("Edit", FourthEntity);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                TemplateItemKpi templateItemKpi = ThirdEntity.Kpis.Single(k => k.Id == id);

                ThirdEntity.Kpis.Remove(templateItemKpi);

                Service.Update(FirstEntity);

                PrePublish();

                return RedirectToAction("MasterIndex", "TemplateCustomizedSection");
            }
            catch (Exception)
            {
                return ErrorPartialMessage(Resources.Shared.Messages.General.ErrorDuringDelete);
            }
        }

        #endregion
    }
}