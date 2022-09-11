#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.PMS.Entities.Objective;
using HRIS.Domain.PMS.RootEntities;
using Infrastructure.Validation;
using Resources.Areas.PMS.ValueObjects.ObjectiveSectionItemKpi;
using UI.Areas.PMSComprehensive.Controllers.EntitiesRoots;
using UI.Utilities;

#endregion

namespace UI.Areas.PMSComprehensive.Controllers.ValueObjects
{
    public class ObjectiveSectionItemKpiController : AppraisalAggregateController, IRule<ObjectiveSectionItemKpi>
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
                        FirstEntity.ObjectiveSections.SingleOrDefault(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
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

        #region ObjectiveSectionItemKpi

        private ObjectiveSectionItemKpi _objectiveSectionItemKpi;

        public ObjectiveSectionItemKpi FourthEntity
        {
            get
            {
                return _objectiveSectionItemKpi ??
                       (_objectiveSectionItemKpi =
                        ThirdEntity.Kpis.SingleOrDefault(
                            k => k.Id == GetMasterRecordValue(MasterRecordOrder.Fourth)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<ObjectiveSectionItemKpi>

        public ObjectRules<ObjectiveSectionItemKpi> Rules
        {
            get { throw new NotImplementedException();}
        }

        #endregion

        #region Overrides of AppraisalTemplateAggregateController

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                ThirdEntity.Kpis.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Fourth));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return ThirdEntity.Kpis.Count != 0
                       ? Rules.GetExpiredRules(ThirdEntity.Kpis)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region CRUD

        //public ActionResult Index(int selectedSubRowId = 0)
        //{
        //    SetMasterRecordValue(MasterRecordOrder.Fourth, selectedSubRowId);

        //    PrePublish();

        //    SaveTabIndexSecondLevel(0);

        //    return PartialView("Index");
        //}

        //public PartialViewResult Load()
        //{
        //    return PartialView("Edit", new ObjectiveSectionItemKpi());
        //}

        public ActionResult ReadOnly()
        {
            PrePublish();

            if (ThirdEntity.Kpis.Count == 0)
            {
                return RedirectToAction("Index", "Appraisal");
            }

            IList<ObjectiveSectionItemKpi> objectiveSectionItemKpi = ThirdEntity.Kpis;

            if (objectiveSectionItemKpi.Count > 0)
            {
                ViewData["ObjectiveSectionItemKpis"] = objectiveSectionItemKpi.ToList();
            }

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("ReadOnly")
                            });
        }

        //[HttpPost]
        //public ActionResult Save(ObjectiveSectionItemKpi objectiveSectionItemKpi)
        //{
        //    PrePublish();

        //    if (objectiveSectionItemKpi.IsTransient())
        //    {
        //        ThirdEntity.AddKpi(objectiveSectionItemKpi);
        //    }
        //    else
        //    {
        //        objectiveSectionItemKpi.ObjectiveSectionItem = FourthEntity.ObjectiveSectionItem;

        //        this.UpdateValueObject(objectiveSectionItemKpi, FourthEntity);
        //        this.StringDecode(FourthEntity);
        //    }

        //    if ((Rules.GetBrokenRules(objectiveSectionItemKpi).Count == 0) && (TryValidateModel(objectiveSectionItemKpi)))
        //    {
        //        Service.Update(FirstEntity);
        //    }
        //    else
        //    {
        //        ModelState.AddModelErrors(Rules.GetBrokenRules(objectiveSectionItemKpi));

        //        ThirdEntity.ObjectiveSectionItemKpis.Remove(objectiveSectionItemKpi);

        //        return Json(new
        //                        {
        //                            Success = false,
        //                            PartialViewHtml = RenderPartialViewToString("List", objectiveSectionItemKpi)
        //                        });
        //    }

        //    PrePublish();

        //    return Json(new
        //                    {
        //                        Success = true,
        //                        PartialViewHtml = RenderPartialViewToString("List", objectiveSectionItemKpi)
        //                    });
        //}

        ////[HttpPost]
        //public ActionResult JsonEdit()
        //{
        //    return PartialView("Edit", FourthEntity);
        //}

        //[HttpPost]
        //public ActionResult Delete(int id)
        //{
        //    try
        //    {
        //        ObjectiveSectionItemKpi objectiveSectionItemKpi = ThirdEntity.ObjectiveSectionItemKpis.Single(k => k.Id == id);

        //        ThirdEntity.ObjectiveSectionItemKpis.Remove(objectiveSectionItemKpi);

        //        Service.Update(FirstEntity);

        //        PrePublish();

        //        return RedirectToAction("MasterIndex", "TemplateCustomizedSection");
        //    }
        //    catch (Exception)
        //    {
        //        return ErrorPartialMessage("Error During Delete ! Please try Again");
        //    }
        //}

        #endregion
    }
}