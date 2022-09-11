
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
    public class TemplateSectionItemController : AppraisalTemplateAggregateController, IRule<TemplateSectionItem>
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
                       (_templateCustomizedSection = FirstEntity.Sections.Single(r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
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
                        SecondEntity.SectionItems.SingleOrDefault(
                            k => k.Id == GetMasterRecordValue(MasterRecordOrder.Third)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<TemplateSectionItem>

        public ObjectRules<TemplateSectionItem> Rules
        {
            get { return new TemplateSectionItemRules(); }
        }

        #endregion

        #region Overrides of AppraisalTemplateAggregateController

        public override void CleanUpModelState()
        {
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                SecondEntity.SectionItems.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Third));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return SecondEntity.SectionItems.Count != 0
                       ? Rules.GetExpiredRules(SecondEntity.SectionItems)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Third, selectedSubRowId);

            PrePublish();

            SaveTabIndexSecondLevel(0);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new TemplateSectionItem());
        }

        public ActionResult Save(TemplateSectionItem templateSectionItem)
        {
            PrePublish();

            if (templateSectionItem.IsTransient())
            {
                SecondEntity.AddItems(templateSectionItem);
            }
            else
            {
                #region Retrieve Parent

                templateSectionItem.TemplateCustomizedSection = ThirdEntity.TemplateCustomizedSection;

                #endregion

                this.UpdateValueObject(templateSectionItem, ThirdEntity);

                this.StringDecode(ThirdEntity);
            }

            //FirstEntity.CustomizedSectionsWeight = SecondEntity.AppraisalTemplate.Sections.Sum((x => x.Weight));
            //FirstEntity.CustomizedSectionsWeight += ThirdEntity.TemplateCustomizedSection.SectionItems.Sum((x => x.Weight));

            if ((Rules.GetBrokenRules(templateSectionItem).Count == 0) && (TryValidateModel(templateSectionItem)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(templateSectionItem));

                SecondEntity.SectionItems.Remove(templateSectionItem);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", templateSectionItem)
                                });
            }

            SetMasterRecordValue(MasterRecordOrder.Third, templateSectionItem.Id);

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", templateSectionItem)
                            });
        }

        //[HttpPost]
        public ActionResult JsonEdit()
        {
            return PartialView("Edit", ThirdEntity);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                TemplateSectionItem templateSectionItem = SecondEntity.SectionItems.SingleOrDefault(i => i.Id == id);

                try
                {
                    SecondEntity.SectionItems.Remove(templateSectionItem);

                    Service.Update(FirstEntity);

                    PrePublish();

                    return RedirectToAction("MasterIndex", "TemplateCustomizedSection");
                }
                catch (Exception)
                {
                    return ErrorPartialMessage(Resources.Shared.Messages.General.ErrorDuringDelete);
                }
            }

            return ErrorPartialMessage(Resources.Shared.Messages.General.ErrorDuringDelete);
        }

        #endregion
    }
}