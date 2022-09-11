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
    public class JobDescriptionSectionItemKpiController : AppraisalAggregateController, IRule<JobDescriptionSectionItemKpi>
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

        #region Job Description Section Task

        private JobDescriptionSectionTask _jobDescriptionSectionTask;

        public JobDescriptionSectionTask FourthEntity
        {
            get
            {
                return _jobDescriptionSectionTask ??
                       (_jobDescriptionSectionTask =
                        ThirdEntity.JobDescriptionSectionTasks.SingleOrDefault(
                            k => k.Id == GetMasterRecordValue(MasterRecordOrder.Fourth)));
            }
        }

        #endregion

        #region Job Description Section Item Kpi

        private JobDescriptionSectionItemKpi _jobDescriptionSectionItemKpi;

        public JobDescriptionSectionItemKpi FifthEntity
        {
            get
            {
                return _jobDescriptionSectionItemKpi ??
                       (_jobDescriptionSectionItemKpi =
                        FourthEntity.JobDescriptionSectionItemKpis.SingleOrDefault(
                            k => k.Id == GetMasterRecordValue(MasterRecordOrder.Fifth)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<JobDescriptionSectionItemKpi>

        public ObjectRules<JobDescriptionSectionItemKpi> Rules
        {
            get { return new JobDescriptionSectionItemKpiRules(); }
        }

        #endregion

        #region Overrides of AppraisalAggregateController

        public override void CleanUpModelState()
        {
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                FourthEntity.JobDescriptionSectionItemKpis.Where(
                    i => i.Id == GetMasterRecordValue(MasterRecordOrder.Fifth));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FourthEntity.JobDescriptionSectionItemKpis.Count != 0
                       ? Rules.GetExpiredRules(FourthEntity.JobDescriptionSectionItemKpis)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region Utilities

        private void GetAppraisalIdValue()
        {
            TempData["appraisalId"] = FirstEntity.Id;
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Fifth, selectedSubRowId);

            PrePublish();

            SaveTabIndexSecondLevel(0);

            return PartialView("Index");
        }

        public ActionResult ReadOnly()
        {
            PrePublish();

            if (FourthEntity.JobDescriptionSectionItemKpis.Count == 0)
            {
                return RedirectToAction("MasterIndex", "JobDescriptionSectionItem");
            }

            IList<JobDescriptionSectionItemKpi> jobDescriptionSectionItemKpis = FourthEntity.JobDescriptionSectionItemKpis;

            if (jobDescriptionSectionItemKpis.Count > 0)
            {
                ViewData["jobDescriptionSectionItemKpis"] = jobDescriptionSectionItemKpis.ToList();
            }

            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("ReadOnly")
            });
        }

        public PartialViewResult Load()
        {
            GetAppraisalIdValue();

            return PartialView("Edit", new JobDescriptionSectionItemKpi());
        }

        public ActionResult Save(JobDescriptionSectionItemKpi jobDescriptionSectionItemKpi)
        {
            PrePublish();

            if (jobDescriptionSectionItemKpi.IsTransient())
            {
                FourthEntity.AddKpi(jobDescriptionSectionItemKpi);
            }
            else
            {
                #region Retrieve Parent

                jobDescriptionSectionItemKpi.JobDescriptionSectionTask = FifthEntity.JobDescriptionSectionTask;

                #endregion

                this.UpdateValueObject(jobDescriptionSectionItemKpi, FifthEntity);

                this.StringDecode(FifthEntity);
            }

            if ((Rules.GetBrokenRules(jobDescriptionSectionItemKpi).Count == 0) && (TryValidateModel(jobDescriptionSectionItemKpi)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(jobDescriptionSectionItemKpi));

                FourthEntity.JobDescriptionSectionItemKpis.Remove(jobDescriptionSectionItemKpi);

                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("List", jobDescriptionSectionItemKpi)
                });
            }

            SetMasterRecordValue(MasterRecordOrder.Fifth, jobDescriptionSectionItemKpi.Id);

            PrePublish();

            GetAppraisalIdValue();

            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("List", jobDescriptionSectionItemKpi)
            });
        }


        public ActionResult JsonEdit()
        {
            GetAppraisalIdValue();

            return PartialView("Edit", FifthEntity);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                JobDescriptionSectionItemKpi jobDescriptionSectionItemKpi =
                    FourthEntity.JobDescriptionSectionItemKpis.SingleOrDefault(i => i.Id == id);

                try
                {
                    FourthEntity.JobDescriptionSectionItemKpis.Remove(jobDescriptionSectionItemKpi);

                    Service.Update(FirstEntity);

                    PrePublish();

                    return RedirectToAction("MasterIndex", "JobDescriptionSectionItem");
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