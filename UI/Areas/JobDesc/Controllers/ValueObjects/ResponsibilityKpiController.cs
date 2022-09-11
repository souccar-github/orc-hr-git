#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.JobDesc.Entities;
using HRIS.Domain.JobDesc.ValueObjects;
using UI.Areas.JobDesc.Controllers.EntitiesRoots;
using UI.Extensions;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.JobDesc.ValueObjects;

#endregion

namespace UI.Areas.JobDesc.Controllers.ValueObjects
{
    public class ResponsibilityKpiController : JobDescAggregateController, IRule<ResponsibilityKpi>
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

        #region Role

        private Role _role;

        public Role SecondEntity
        {
            get
            {
                return _role ??
                       (_role = FirstEntity.Roles.SingleOrDefault(r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #region Responsibility

        private Responsibility _responsibility;

        public Responsibility ThirdEntity
        {
            get
            {
                return _responsibility ??
                       (_responsibility =
                        SecondEntity.Responsibilities.SingleOrDefault(k => k.Id == GetMasterRecordValue(MasterRecordOrder.Third)));
            }
        }

        #endregion

        #region Responsibility Kpi

        private ResponsibilityKpi _responsibilityKpi;

        public ResponsibilityKpi FourthEntity
        {
            get
            {
                return _responsibilityKpi ??
                       (_responsibilityKpi =
                        ThirdEntity.ResponsibilityKpis.SingleOrDefault(k => k.Id == GetMasterRecordValue(MasterRecordOrder.Fourth)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<ResponsibilityKpi>

        public ObjectRules<ResponsibilityKpi> Rules
        {
            get { return new ResponsibilityKpiRules(); }
        }

        #endregion

        #region Overrides of JobDescAggregateController

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                ThirdEntity.ResponsibilityKpis.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Fourth));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return ThirdEntity.ResponsibilityKpis.Count != 0
                       ? Rules.GetExpiredRules(ThirdEntity.ResponsibilityKpis)
                       : new List<BrokenBusinessRule>();
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
            return PartialView("Edit", new ResponsibilityKpi());
        }

        [HttpPost]
        public ActionResult Save(ResponsibilityKpi responsibilityKpi)
        {
            PrePublish();

            if (responsibilityKpi.IsTransient())
            {
                ThirdEntity.AddKpi(responsibilityKpi);
            }
            else
            {
                responsibilityKpi.Responsibility = FourthEntity.Responsibility;

                this.UpdateValueObject(responsibilityKpi, FourthEntity);
                this.StringDecode(FourthEntity);
            }

            if ((Rules.GetBrokenRules(responsibilityKpi).Count == 0) && (TryValidateModel(responsibilityKpi)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(responsibilityKpi));

                ThirdEntity.ResponsibilityKpis.Remove(responsibilityKpi);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", responsibilityKpi)
                                });
            }
            
            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", responsibilityKpi)
                            });
        }

        [HttpPost]
        public JsonResult JsonEdit()
        {
            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("Edit", FourthEntity)
                            });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                ResponsibilityKpi responsibilityKpi = ThirdEntity.ResponsibilityKpis.Single(k => k.Id == id);

                ThirdEntity.ResponsibilityKpis.Remove(responsibilityKpi);

                Service.Update(FirstEntity);

                PrePublish();

                return RedirectToAction("MasterIndex", "Role");
            }
            catch (Exception)
            {
                return ErrorPartialMessage(Resources.Shared.Messages.General.ErrorDuringDelete);
            }
        }

        #endregion
    }
}