#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.PMS.Entities.Objective;
using HRIS.Domain.PMS.RootEntities;
using Infrastructure.Validation;
using UI.Areas.PMSComprehensive.Controllers.EntitiesRoots;
using UI.Extensions;
using UI.Helpers.Model;
using UI.Utilities;

#endregion

namespace UI.Areas.PMSComprehensive.Controllers.ValueObjects
{
    public class ObjectiveSectionItemController : AppraisalAggregateController, IRule<ObjectiveSectionItem>
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

        #region ObjectiveSectionItem

        private ObjectiveSection _objectiveSection;

        public ObjectiveSection SecondEntity
        {
            get
            {
                return _objectiveSection ??
                       (_objectiveSection =
                        FirstEntity.ObjectiveSections.Single(r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #region ObjectiveSectionItem

        private ObjectiveSectionItem _objectiveSectionItem;

        public ObjectiveSectionItem ThirdEntity
        {
            get
            {
                return _objectiveSectionItem ??
                       (_objectiveSectionItem =
                        SecondEntity.Items.SingleOrDefault(
                            k => k.Id == GetMasterRecordValue(MasterRecordOrder.Third)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<ObjectiveSectionItem>

        public ObjectRules<ObjectiveSectionItem> Rules
        {
            get { throw new NotImplementedException();}
        }

        #endregion

        #region Overrides of AppraisalAggregateController

        public override void CleanUpModelState()
        {
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                SecondEntity.Items.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Third));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return SecondEntity.Items.Count != 0
                       ? Rules.GetExpiredRules(SecondEntity.Items)
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
            return PartialView("Edit", new ObjectiveSectionItem());
        }

        public ActionResult Save(ObjectiveSectionItem objectiveSectionItem)
        {
            PrePublish();

            if (objectiveSectionItem.IsTransient())
            {
                SecondEntity.AddItems(objectiveSectionItem);
            }
            else
            {
                #region Retrieve Parent

                objectiveSectionItem.Section = ThirdEntity.Section;

                #endregion

                this.UpdateValueObject(objectiveSectionItem, ThirdEntity);

                this.StringDecode(ThirdEntity);
            }

            if ((Rules.GetBrokenRules(objectiveSectionItem).Count == 0) && (TryValidateModel(objectiveSectionItem)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(objectiveSectionItem));

                SecondEntity.Items.Remove(objectiveSectionItem);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", objectiveSectionItem)
                                });
            }

            SetMasterRecordValue(MasterRecordOrder.Third, objectiveSectionItem.Id);

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", objectiveSectionItem)
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
                ObjectiveSectionItem objectiveSectionItem =
                    SecondEntity.Items.SingleOrDefault(i => i.Id == id);

                try
                {
                    SecondEntity.Items.Remove(objectiveSectionItem);

                    Service.Update(FirstEntity);

                    PrePublish();

                    return RedirectToAction("MasterIndex", "ObjectiveSection");
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