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
    public class ResponsibilityController : JobDescAggregateController, IRule<Responsibility>
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

        #region Responsibility

        private Responsibility _responsibility;

        public Responsibility ThirdEntity
        {
            get
            {
                return _responsibility ??
                       (_responsibility =
                        SecondEntity.Responsibilities.SingleOrDefault(
                            k => k.Id == GetMasterRecordValue(MasterRecordOrder.Third)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<Responsibility>

        public ObjectRules<Responsibility> Rules
        {
            get { return new ResponsibilityRules(); }
        }

        #endregion

        #region Overrides of JobDescAggregateController

        public override void CleanUpModelState()
        {
         //   ModelState.Remove("JobRole.Name");
            ModelState.Remove("JobTitle.Name");
            ModelState.Remove("Priority.Name");
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                SecondEntity.Responsibilities.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Third));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return SecondEntity.Responsibilities.Count != 0
                       ? Rules.GetExpiredRules(SecondEntity.Responsibilities)
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
            return PartialView("Edit", new Responsibility());
        }

        public ActionResult Save(Responsibility responsibility)
        {
            PrePublish();

            if (responsibility.IsTransient())
            {
                SecondEntity.AddResponsibility(responsibility);
            }
            else
            {
                #region Retrieve Lists

                responsibility.Role = ThirdEntity.Role;
                responsibility.ResponsibilityKpis = ThirdEntity.ResponsibilityKpis;

                #endregion

                this.UpdateValueObject(responsibility, ThirdEntity);

                this.StringDecode(ThirdEntity);
            }

            if ((Rules.GetBrokenRules(responsibility).Count == 0) && (TryValidateModel(responsibility)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(responsibility));

                SecondEntity.Responsibilities.Remove(responsibility);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", responsibility)
                                });
            }

            SetMasterRecordValue(MasterRecordOrder.Third, responsibility.Id);

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", responsibility)
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
            if (id != 0)
            {
                Responsibility responsibility = SecondEntity.Responsibilities.SingleOrDefault(i => i.Id == id);

                try
                {
                    SecondEntity.Responsibilities.Remove(responsibility);

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