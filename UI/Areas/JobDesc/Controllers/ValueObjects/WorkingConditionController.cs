#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.JobDesc.Entities;
using HRIS.Domain.JobDesc.ValueObjects;
using Infrastructure.Validation;
using UI.Areas.JobDesc.Controllers.EntitiesRoots;
using UI.Extensions;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.JobDesc.ValueObjects;

#endregion

namespace UI.Areas.JobDesc.Controllers.ValueObjects
{
    public class WorkingConditionController : JobDescAggregateController, IRule<WorkingCondition>
    {
        #region Parents Chain

        #region JobDescription

        private JobDescription _jobDescription;

        public JobDescription FirstEntity
        {
            get
            {
                return _jobDescription ??
                       (_jobDescription = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First)));
            }
        }

        #endregion

        #region Specification

        private Specification _specification;

        public Specification SecondEntity
        {
            get { return _specification ?? (_specification = FirstEntity.Specification.First()); }
        }

        #endregion

        #region Working Condition

        private WorkingCondition _workingCondition;

        public WorkingCondition ThirdEntity
        {
            get
            {
                return _workingCondition ??
                       (_workingCondition =
                        SecondEntity.WorkingConditions.SingleOrDefault(c => c.Id == GetMasterRecordValue(MasterRecordOrder.Third)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<WorkingCondition>

        public ObjectRules<WorkingCondition> Rules
        {
            get { return new WorkingConditionRules(); }
        }

        #endregion

        #region Overrides of JobDescAggregateController

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                SecondEntity.WorkingConditions.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Third));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return SecondEntity.WorkingConditions.Count != 0
                       ? Rules.GetExpiredRules(SecondEntity.WorkingConditions)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Third, selectedSubRowId);

            PrePublish();

            SaveTabIndexSecondLevel(7);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new WorkingCondition());
        }

        [HttpPost]
        public ActionResult Save(WorkingCondition workingCondition)
        {
            PrePublish();
 
            if (workingCondition.IsTransient())
            {
                SecondEntity.AddWorkingCondition(workingCondition);
            }
            else
            {
                #region Retrieve Lists 

                workingCondition.Specification = ThirdEntity.Specification;
                workingCondition.Conditions = ThirdEntity.Conditions;

                #endregion

                this.UpdateValueObject(workingCondition, ThirdEntity);
                this.StringDecode(ThirdEntity);
            }

            if ((Rules.GetBrokenRules(workingCondition).Count == 0) && (TryValidateModel(workingCondition)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(workingCondition));

                SecondEntity.WorkingConditions.Remove(workingCondition);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", workingCondition)
                                });
            }

            SetMasterRecordValue(MasterRecordOrder.Third, workingCondition.Id);

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", workingCondition)
                            });
        }

        [HttpPost]
        public JsonResult JsonEdit()
        {
            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("Edit", ThirdEntity)
                            });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                WorkingCondition workingCondition = SecondEntity.WorkingConditions.Single(c => c.Id == id);

                SecondEntity.WorkingConditions.Remove(workingCondition);

                Service.Update(FirstEntity);

                PrePublish();

                return RedirectToAction("Index", "Specification");
            }
            catch (Exception)
            {
                return ErrorPartialMessage(Resources.Shared.Messages.General.ErrorDuringDelete);
            }
        }

        #endregion
    }
}