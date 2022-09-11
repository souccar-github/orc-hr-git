#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Utilities;
using Infrastructure.Validation;
using HRIS.Domain.PMS.Entities;
using UI.Areas.PMSComprehensive.Controllers.EntitiesRoots;
using UI.Extensions;
using UI.Helpers.Model;
using HRIS.Domain.PMS.ValueObjects.Implementation.Objective;
using UI.Utilities;
using Validation.PMSComprehensive.ValueObjects;
//using Resources.Areas.PMS.ValueObjects.ObjectiveSection;

#endregion

namespace UI.Areas.PMSComprehensive.Controllers.ValueObjects
{
    public class ObjectiveSectionController : AppraisalAggregateController, IRule<ObjectiveSection>
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

        #region ObjectiveSection

        private ObjectiveSection _objectiveSection;

        public ObjectiveSection SecondEntity
        {
            get
            {
                return _objectiveSection ??
                       (_objectiveSection =
                        FirstEntity.ObjectiveSections.SingleOrDefault(r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<ObjectiveSection>

        public ObjectRules<ObjectiveSection> Rules
        {
            get { return new ObjectiveSectionRules(); }
        }

        #endregion

        #region Overrides of AppraisalAggregateController

        public override void CleanUpModelState()
        {
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                FirstEntity.ObjectiveSections.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FirstEntity.ObjectiveSections.Count != 0
                       ? Rules.GetExpiredRules(FirstEntity.ObjectiveSections)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Second, selectedSubRowId);

            CurrentlyInSecondLevel = selectedSubRowId;

            SaveTabIndex(2);

            PrePublish();

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new ObjectiveSection());
        }

        public ActionResult Save(ObjectiveSection objectiveSection)
        {
            PrePublish();

            if (objectiveSection.IsTransient())
            {
                FirstEntity.AddObjectiveSection(objectiveSection);
            }
            else
            {
                #region Retrieve Parent

                objectiveSection.Appraisal = SecondEntity.Appraisal;

                #endregion

                this.UpdateValueObject(objectiveSection, SecondEntity);

                this.StringDecode(SecondEntity);
            }

            if ((Rules.GetBrokenRules(objectiveSection).Count == 0) && (TryValidateModel(objectiveSection)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(objectiveSection));

                FirstEntity.ObjectiveSections.Remove(objectiveSection);

                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("List", objectiveSection)
                });
            }

            SetMasterRecordValue(MasterRecordOrder.Second, objectiveSection.Id);

            PrePublish();

            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("List", objectiveSection)
            });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                ObjectiveSection objectiveSection = FirstEntity.ObjectiveSections.SingleOrDefault(c => c.Id == id);

                try
                {
                    FirstEntity.ObjectiveSections.Remove(objectiveSection);

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

        //[HttpPost]
        public ActionResult JsonEdit()
        {
            return PartialView("Edit", SecondEntity);
        }

        #endregion
    }
}