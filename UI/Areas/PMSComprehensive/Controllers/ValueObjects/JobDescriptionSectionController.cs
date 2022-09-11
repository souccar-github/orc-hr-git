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
    public class JobDescriptionSectionController : AppraisalAggregateController, IRule<JobDescriptionSection>
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
                        FirstEntity.JobDescriptionSections.SingleOrDefault(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<JobDescriptionSection>

        public ObjectRules<JobDescriptionSection> Rules
        {
            get { return new JobDescriptionSectionRules(); }
        }

        #endregion

        #region Overrides of AppraisalAggregateController

        public override void CleanUpModelState()
        {
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                FirstEntity.JobDescriptionSections.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FirstEntity.JobDescriptionSections.Count != 0
                       ? Rules.GetExpiredRules(FirstEntity.JobDescriptionSections)
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

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Second, selectedSubRowId);

            CurrentlyInSecondLevel = selectedSubRowId;

            SaveTabIndex(1);

            PrePublish();

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            GetAppraisalIdValue();

            return PartialView("Edit", new JobDescriptionSection { Appraisal = FirstEntity });
        }

        public ActionResult Save(JobDescriptionSection jobDescriptionSection)
        {
            PrePublish();

            if (jobDescriptionSection.IsTransient())
            {
                FirstEntity.AddJobDescriptionSection(jobDescriptionSection);
            }
            else
            {
                #region Retrieve Direct Parent

                jobDescriptionSection.Appraisal = SecondEntity.Appraisal;

                #endregion

                this.UpdateValueObject(jobDescriptionSection, SecondEntity);

                this.StringDecode(SecondEntity);
            }

            if ((Rules.GetBrokenRules(jobDescriptionSection).Count == 0) && (TryValidateModel(jobDescriptionSection)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(jobDescriptionSection));

                FirstEntity.JobDescriptionSections.Remove(jobDescriptionSection);

                GetAppraisalIdValue();

                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("List", jobDescriptionSection)
                });
            }

            SetMasterRecordValue(MasterRecordOrder.Second, jobDescriptionSection.Id);

            PrePublish();

            GetAppraisalIdValue();

            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("List", jobDescriptionSection)
            });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                JobDescriptionSection jobDescriptionSection = FirstEntity.JobDescriptionSections.SingleOrDefault(c => c.Id == id);

                try
                {
                    FirstEntity.JobDescriptionSections.Remove(jobDescriptionSection);

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