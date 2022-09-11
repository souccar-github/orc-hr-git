#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Utilities;
using Infrastructure.Validation;
using HRIS.Domain.PMS.Entities;
using HRIS.Domain.PMS.ValueObjects.Implementation.JobDescription;
using UI.Areas.PMSComprehensive.Controllers.EntitiesRoots;
using UI.Extensions;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.PMSComprehensive.ValueObjects;


#endregion

namespace UI.Areas.PMSComprehensive.Controllers.ValueObjects
{
    public class JobDescriptionSectionItemController : AppraisalAggregateController, IRule<JobDescriptionSectionItem>
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

        #region Job Description Section

        private JobDescriptionSection _jobDescriptionSection;

        public JobDescriptionSection SecondEntity
        {
            get
            {
                return _jobDescriptionSection ??
                       (_jobDescriptionSection =
                        FirstEntity.JobDescriptionSections.Single(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #region Job Description Section Item

        private JobDescriptionSectionItem _jobDescriptionSectionItem;

        public JobDescriptionSectionItem ThirdEntity
        {
            get
            {
                return _jobDescriptionSectionItem ??
                       (_jobDescriptionSectionItem =
                        SecondEntity.JobDescriptionSectionItems.SingleOrDefault(
                            k => k.Id == GetMasterRecordValue(MasterRecordOrder.Third)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<JobDescriptionSectionItem>

        public ObjectRules<JobDescriptionSectionItem> Rules
        {
            get { return new JobDescriptionSectionItemRules(); }
        }

        #endregion

        #region Overrides of AppraisalAggregateController

        public override void CleanUpModelState()
        {
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                SecondEntity.JobDescriptionSectionItems.Where(
                    i => i.Id == GetMasterRecordValue(MasterRecordOrder.Third));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return SecondEntity.JobDescriptionSectionItems.Count != 0
                       ? Rules.GetExpiredRules(SecondEntity.JobDescriptionSectionItems)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region Utilities

        private void GetAppraisalIdValue()
        {
            TempData["appraisalId"] = FirstEntity.Id;
        }

        #endregion

        #region Master Job Description Section Item

        public ActionResult MasterIndex(int id = 0)
        {
            if (id != 0)
            {
                SetMasterRecordValue(MasterRecordOrder.Third, id);
                CurrentlyInSecondLevel = id;
            }

            PrePublish();

            AddToPath(masterRecordOrder: MasterRecordOrder.Third, level: RibbonLevels.B, actionName: "MasterIndex",
                      stepId: GetMasterRecordValue(MasterRecordOrder.Third),
                      areaName: PMSComprehensiveAreaRegistration.GetAreaName);

            #region Get Data

            int pageNo = 1;
            if (GetMasterRecordValue(MasterRecordOrder.Third) != 0)
            {
                int masterRecordValue = GetMasterRecordValue(MasterRecordOrder.Third);

                int count =
                    SecondEntity.JobDescriptionSectionItems.Where(jobDescriptionSectionItem => (jobDescriptionSectionItem.Id >= masterRecordValue)).Count();

                pageNo = count / 5;

                if (count % 5 > 0)
                {
                    pageNo++;
                }
            }

            ViewData["jobDescriptionSectionItems"] = SecondEntity.JobDescriptionSectionItems;
            ViewData["JobDescriptionSectionTaskSelectedRow"] = GetMasterRecordValue(MasterRecordOrder.Third);
            ViewData["PageTo"] = pageNo;

            #endregion

            return View("MasterIndex");
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
            GetAppraisalIdValue();

            return PartialView("Edit", new JobDescriptionSectionItem());
        }

        public ActionResult Save(JobDescriptionSectionItem jobDescriptionSectionItem)
        {
            PrePublish();

            if (jobDescriptionSectionItem.IsTransient())
            {
                SecondEntity.AddItems(jobDescriptionSectionItem);
            }
            else
            {
                #region Retrieve Parent

                jobDescriptionSectionItem.JobDescriptionSection = ThirdEntity.JobDescriptionSection;

                #endregion

                this.UpdateValueObject(jobDescriptionSectionItem, ThirdEntity);

                this.StringDecode(ThirdEntity);
            }

            if ((Rules.GetBrokenRules(jobDescriptionSectionItem).Count == 0) && (TryValidateModel(jobDescriptionSectionItem)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(jobDescriptionSectionItem));

                SecondEntity.JobDescriptionSectionItems.Remove(jobDescriptionSectionItem);

                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("List", jobDescriptionSectionItem)
                });
            }

            SetMasterRecordValue(MasterRecordOrder.Third, jobDescriptionSectionItem.Id);

            PrePublish();

            GetAppraisalIdValue();

            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("List", jobDescriptionSectionItem)
            });
        }


        public ActionResult JsonEdit()
        {
            GetAppraisalIdValue();

            return PartialView("Edit", ThirdEntity);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                JobDescriptionSectionItem jobDescriptionSectionItem =
                    SecondEntity.JobDescriptionSectionItems.SingleOrDefault(i => i.Id == id);

                try
                {
                    SecondEntity.JobDescriptionSectionItems.Remove(jobDescriptionSectionItem);

                    Service.Update(FirstEntity);

                    PrePublish();

                    return RedirectToAction("MasterIndex", "Appraisal");
                }
                catch (Exception)
                {
                    return ErrorPartialMessage("Error During Delete ! Please try Again");
                }
            }

            return ErrorPartialMessage("Error During Delete ! Please try Again");
        }

        #endregion
    }
}