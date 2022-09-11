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
    public class TemplateCustomizedSectionController : AppraisalTemplateAggregateController, IRule<TemplateCustomizedSection>
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
                       (_templateCustomizedSection =
                        FirstEntity.Sections.SingleOrDefault(r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<TemplateCustomizedSection>

        public ObjectRules<TemplateCustomizedSection> Rules
        {
            get { return new TemplateCustomizedSectionRules(); }
        }

        #endregion

        #region Overrides of AppraisalTemplateAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("AppraisalSectionEvaluator.Evaluator");
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                FirstEntity.Sections.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FirstEntity.Sections.Count != 0
                       ? Rules.GetExpiredRules(FirstEntity.Sections)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region Utilities

        public ActionResult ClearSelection()
        {
            SetMasterRecordValue(MasterRecordOrder.Second, 0);

            return RedirectToAction("MasterIndex");
        }

        #endregion
         
        #region Master TemplateCustomizedSection

        public ActionResult MasterIndex(int id = 0)
        {
            if (id != 0)
            {
                SetMasterRecordValue(MasterRecordOrder.Second, id);
                CurrentlyInSecondLevel = id;
            }

            PrePublish();

            AddToPath(masterRecordOrder: MasterRecordOrder.Second, level: RibbonLevels.B, actionName: "MasterIndex",
            stepId: GetMasterRecordValue(MasterRecordOrder.Second), areaName: PMSComprehensiveAreaRegistration.GetAreaName, nodeName: Resources.Areas.PMS.Views.Shared.Navigator.CustomizedSection);

            #region Get Data

            int pageNo = 1;
            if (GetMasterRecordValue(MasterRecordOrder.Second) != 0)
            {
                int masterRecordValue = GetMasterRecordValue(MasterRecordOrder.Second);

                int count = FirstEntity.Sections.Where(sec => (sec.Id >= masterRecordValue)).Count();

                pageNo = count / 5;

                if (count % 5 > 0)
                {
                    pageNo++;
                }
            }

            ViewData["templateCustomizedSections"] = FirstEntity.Sections;
            ViewData["SelectedRow"] = GetMasterRecordValue(MasterRecordOrder.Second);
            ViewData["SectionItemsSelectedRow"] = GetMasterRecordValue(MasterRecordOrder.Third);
            ViewData["PageTo"] = pageNo;

            #endregion

            return View("MasterIndex");
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Second, selectedSubRowId);

            CurrentlyInSecondLevel = selectedSubRowId;

            SaveTabIndex(0);

            PrePublish();

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new TemplateCustomizedSection());
        }

        public ActionResult Save(TemplateCustomizedSection templateCustomizedSection)
        {
            PrePublish();

            if (templateCustomizedSection.IsTransient())
            {
                FirstEntity.AddSection(templateCustomizedSection);
            }
            else
            {
                #region Retrieve Parent

                templateCustomizedSection.AppraisalTemplate = SecondEntity.AppraisalTemplate;

                #endregion

                this.UpdateValueObject(templateCustomizedSection, SecondEntity);

                this.StringDecode(SecondEntity);
            }

            //FirstEntity.CustomizedSectionsWeight = SecondEntity.AppraisalTemplate.Sections.Sum((x => x.Weight));

            
            if ((Rules.GetBrokenRules(templateCustomizedSection).Count == 0) && (TryValidateModel(templateCustomizedSection)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(templateCustomizedSection));

                FirstEntity.Sections.Remove(templateCustomizedSection);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", templateCustomizedSection)
                                });
            }

            SetMasterRecordValue(MasterRecordOrder.Second, templateCustomizedSection.Id);

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", templateCustomizedSection)
                            });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                TemplateCustomizedSection templateCustomizedSection = FirstEntity.Sections.SingleOrDefault(c => c.Id == id);

                try
                {
                    FirstEntity.Sections.Remove(templateCustomizedSection);

                    //FirstEntity.CustomizedSectionsWeight = FirstEntity.Sections.Sum((x => x.Weight));

                    Service.Update(FirstEntity);

                    PrePublish();

                    SetMasterRecordValue(MasterRecordOrder.Second, 0);

                    return RedirectToAction("Index", "AppraisalTemplate");
                }
                catch (Exception)
                {
                    return ErrorPartialMessage(Resources.Shared.Messages.General.ErrorDuringDelete);
                }
            }

            return ErrorPartialMessage(Resources.Shared.Messages.General.ErrorDuringDelete);
        }

        //[HttpPost]
        public ActionResult JsonEdit()
        {
            return PartialView("Edit", SecondEntity);
        }

        #endregion
    }
}