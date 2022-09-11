#region

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Infrastructure.Validation;
using UI.Areas.PMSComprehensive.Controllers.EntitiesRoots;
using UI.Extensions;
using UI.Helpers.Model;
using UI.Utilities;

#endregion

namespace UI.Areas.PMSComprehensive.Controllers.ValueObjects
{
    public class JobDescriptionSectionTaskController : AppraisalAggregateController, IRule<JobDescriptionSectionTask>
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

        #endregion

        #region Implementation of IRule<JobDescriptionSectionTask>

        public ObjectRules<JobDescriptionSectionTask> Rules
        {
            get { return new JobDescriptionSectionTaskRules(); }
        }

        #endregion

        #region Overrides of AppraisalAggregateController

        public override void CleanUpModelState()
        {
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                ThirdEntity.JobDescriptionSectionTasks.Where(
                    i => i.Id == GetMasterRecordValue(MasterRecordOrder.Fourth));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return ThirdEntity.JobDescriptionSectionTasks.Count != 0
                       ? Rules.GetExpiredRules(ThirdEntity.JobDescriptionSectionTasks)
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
            SetMasterRecordValue(MasterRecordOrder.Fourth, selectedSubRowId);

            PrePublish();

            SaveTabIndexSecondLevel(0);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            GetAppraisalIdValue();

            return PartialView("Edit", new JobDescriptionSectionTask());
        }

        public ActionResult Save(JobDescriptionSectionTask jobDescriptionSectionTask)
        {
            PrePublish();

            if (jobDescriptionSectionTask.IsTransient())
            {
                ThirdEntity.AddItems(jobDescriptionSectionTask);
            }
            else
            {
                #region Retrieve Parent

                jobDescriptionSectionTask.JobDescriptionSectionItem = FourthEntity.JobDescriptionSectionItem;

                #endregion

                this.UpdateValueObject(jobDescriptionSectionTask, FourthEntity);

                this.StringDecode(FourthEntity);
            }

            if ((Rules.GetBrokenRules(jobDescriptionSectionTask).Count == 0) &&
                (TryValidateModel(jobDescriptionSectionTask)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(jobDescriptionSectionTask));

                ThirdEntity.JobDescriptionSectionTasks.Remove(jobDescriptionSectionTask);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", jobDescriptionSectionTask)
                                });
            }

            SetMasterRecordValue(MasterRecordOrder.Fourth, jobDescriptionSectionTask.Id);

            PrePublish();

            GetAppraisalIdValue();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", jobDescriptionSectionTask)
                            });
        }


        public ActionResult JsonEdit()
        {
            GetAppraisalIdValue();

            return PartialView("Edit", FourthEntity);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                JobDescriptionSectionTask jobDescriptionSectionTask =
                    ThirdEntity.JobDescriptionSectionTasks.SingleOrDefault(i => i.Id == id);

                try
                {
                    ThirdEntity.JobDescriptionSectionTasks.Remove(jobDescriptionSectionTask);

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