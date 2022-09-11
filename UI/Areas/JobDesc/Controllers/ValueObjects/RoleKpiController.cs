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
    public class RoleKpiController : JobDescAggregateController, IRule<RoleKpi>
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
                       (_role = FirstEntity.Roles.Single(r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #region Role KPI

        private RoleKpi _roleKpi;

        public RoleKpi ThirdEntity
        {
            get
            {
                return _roleKpi ??
                       (_roleKpi =
                        SecondEntity.RoleKpis.SingleOrDefault(k => k.Id == GetMasterRecordValue(MasterRecordOrder.Third)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<RoleKpi>

        public ObjectRules<RoleKpi> Rules
        {
            get { return new RoleKpiRules(); }
        }

        #endregion

        #region Overrides of JobDescAggregateController

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                SecondEntity.RoleKpis.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Third));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return SecondEntity.RoleKpis.Count != 0
                       ? Rules.GetExpiredRules(SecondEntity.RoleKpis)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Third, selectedSubRowId);

            PrePublish();

            SaveTabIndexSecondLevel(1);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new RoleKpi());
        }

        public ActionResult Save(RoleKpi roleKpi)
        {
            PrePublish();

            if (roleKpi.IsTransient())
            {
                SecondEntity.AddKpi(roleKpi);
            }
            else
            {
                #region Retrieve Lists

                roleKpi.Role = ThirdEntity.Role;

                #endregion

                this.UpdateValueObject(roleKpi, ThirdEntity);

                this.StringDecode(ThirdEntity);
            }

            if ((Rules.GetBrokenRules(roleKpi).Count == 0) && (TryValidateModel(roleKpi)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(roleKpi));

                SecondEntity.RoleKpis.Remove(roleKpi);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", roleKpi)
                                });
            }

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", roleKpi)
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

        [HttpDelete]
        public ActionResult JsonDelete()
        {
            try
            {
                SecondEntity.RoleKpis.Remove(ThirdEntity);

                Service.Update(FirstEntity);

                PrePublish();

                return Json(new
                                {
                                    Success = true,
                                    PartialViewHtml = RenderPartialViewToString("List", new RoleKpi())
                                });
            }
            catch (Exception)
            {
                return ErrorPartialMessage(Resources.Shared.Messages.General.ErrorDuringDelete);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                RoleKpi roleKpi = SecondEntity.RoleKpis.SingleOrDefault(k => k.Id == id);

                try
                {
                    SecondEntity.RoleKpis.Remove(roleKpi);

                    Service.Update(FirstEntity);

                    PrePublish();

                    return RedirectToAction("MasterIndex", "Role");
                }
                catch (Exception)
                {
                    return ErrorPartialMessage(Resources.Shared.Messages.General.ErrorDuringDelete);
                }
            }

            return ErrorPartialMessage(Resources.Shared.Messages.General.ErrorDuringDelete);
        }

        #endregion
    }
}