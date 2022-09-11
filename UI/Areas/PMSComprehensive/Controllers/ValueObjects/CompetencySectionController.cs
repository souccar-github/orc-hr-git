#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Utilities;
using Infrastructure.Validation;
using HRIS.Domain.PMS.Entities;
using HRIS.Domain.PMS.ValueObjects.Implementation.Competency;
using Telerik.Web.Mvc;
using UI.Areas.PMSComprehensive.Controllers.EntitiesRoots;
using UI.Extensions;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.PMSComprehensive.ValueObjects;
using UI.Helpers.Cache;
using UI.Areas.PMSComprehensive.Helpers;

#endregion

namespace UI.Areas.PMSComprehensive.Controllers.ValueObjects
{
    public class CompetencySectionController : AppraisalAggregateController, IRule<CompetencySection>
    {
        #region Parents Chain

        #region Appraisal

        private Appraisal _appraisal;

        public Appraisal FirstEntity
        {
            get
            {
                return _appraisal ??
                       (_appraisal = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First)));
            }
        }

        #endregion

        #region Competency Section

        private CompetencySection _competencySection;

        public CompetencySection SecondEntity
        {
            get
            {
                return _competencySection ??
                       (_competencySection =
                        FirstEntity.CompetencySections.SingleOrDefault(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<CompetencySection>

        public ObjectRules<CompetencySection> Rules
        {
            get { return new CompetencySectionRules(); }
        }

        #endregion

        #region Overrides of AppraisalAggregateController

        public override void CleanUpModelState()
        {
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                FirstEntity.CompetencySections.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FirstEntity.CompetencySections.Count != 0
                       ? Rules.GetExpiredRules(FirstEntity.CompetencySections)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region Utilities

        public ActionResult ClearSelection()
        {
            SetMasterRecordValue(MasterRecordOrder.Second, 0);

            return RedirectToAction("MasterIndex");
        }

        private void GetAppraisalIdValue()
        {
            TempData["appraisalId"] = FirstEntity.Id;
        }

        #endregion

        #region Master

        //public ActionResult MasterIndex(int id = 0)
        //{
        //    if (id != 0)
        //    {
        //        SetMasterRecordValue(MasterRecordOrder.Second, id);
        //        CurrentlyInSecondLevel = id;
        //    }

        //    PrePublish();

        //    AddToPath(masterRecordOrder: MasterRecordOrder.Second, level: RibbonLevels.B, actionName: "MasterIndex",
        //              stepId: GetMasterRecordValue(MasterRecordOrder.Second),
        //              areaName: PMSComprehensiveAreaRegistration.GetAreaName);

        //    #region Get Data

        //    int pageNo = 1;
        //    if (GetMasterRecordValue(MasterRecordOrder.Second) != 0)
        //    {
        //        int masterRecordValue = GetMasterRecordValue(MasterRecordOrder.Second);

        //        int count =
        //            FirstEntity.CompetencySections.Where(competencySection => (competencySection.Id >= masterRecordValue)).Count();

        //        pageNo = count / 5;

        //        if (count % 5 > 0)
        //        {
        //            pageNo++;
        //        }
        //    }

        //    ViewData["competencySections"] = FirstEntity.CompetencySections;
        //    ViewData["SelectedRow"] = GetMasterRecordValue(MasterRecordOrder.Second);
        //    ViewData["PageTo"] = pageNo;

        //    #endregion

        //    return View("MasterIndex");
        //}

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
            GetAppraisalIdValue();

            return PartialView("Edit", new CompetencySection { Appraisal = FirstEntity });
        }

        public ActionResult Save(CompetencySection competencySection)
        {
            PrePublish();

            if (competencySection.IsTransient())
            {
                FirstEntity.AddCompetencySection(competencySection);
            }
            else
            {
                #region Retrieve Direct Parent

                competencySection.Appraisal = SecondEntity.Appraisal;

                #endregion

                this.UpdateValueObject(competencySection, SecondEntity);

                this.StringDecode(SecondEntity);
            }

            if ((Rules.GetBrokenRules(competencySection).Count == 0) && (TryValidateModel(competencySection)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(competencySection));

                FirstEntity.CompetencySections.Remove(competencySection);

                GetAppraisalIdValue();

                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("List", competencySection)
                });
            }

            SetMasterRecordValue(MasterRecordOrder.Second, competencySection.Id);

            PrePublish();

            GetAppraisalIdValue();

            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("List", competencySection)
            });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                CompetencySection competencySection = FirstEntity.CompetencySections.SingleOrDefault(c => c.Id == id);

                try
                {
                    FirstEntity.CompetencySections.Remove(competencySection);

                    Service.Update(FirstEntity);

                    PrePublish();

                    SetMasterRecordValue(MasterRecordOrder.Second, 0);

                    return RedirectToAction("Index", "Appraisal");
                }
                catch (Exception)
                {
                    return ErrorPartialMessage("Error During Delete ! Please try Again");
                }
            }

            return ErrorPartialMessage("Error During Delete ! Please try Again");
        }

        public ActionResult JsonEdit()
        {
            GetAppraisalIdValue();

            return PartialView("Edit", SecondEntity);
        }

        #endregion
    }
}