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
    public class JLanguageController : JobDescAggregateController, IRule<JLanguage>
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

        #region JLanguage

        private JLanguage _language;

        public JLanguage ThirdEntity
        {
            get
            {
                return _language ??
                       (_language =
                        SecondEntity.Languages.SingleOrDefault(c => c.Id == GetMasterRecordValue(MasterRecordOrder.Third)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<JLangauge>

        public ObjectRules<JLanguage> Rules
        {
            get { return new JLangaugeRules(); }
        }

        #endregion

        #region Overrides of JobDescAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("Name.Name");
            ModelState.Remove("Listening.Name");
            ModelState.Remove("Reading.Name");
            ModelState.Remove("Speaking.Name");
            ModelState.Remove("Writing.Name");
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                SecondEntity.Languages.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Third));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return SecondEntity.Languages.Count != 0
                       ? Rules.GetExpiredRules(SecondEntity.Languages)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Third, selectedRowId);

            PrePublish();

            SaveTabIndexSecondLevel(5);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new JLanguage());
        }

        public ActionResult Save(JLanguage langauge)
        {
            PrePublish();

            if (langauge.IsTransient())
            {
                SecondEntity.AddLanguage(langauge);
            }
            else
            {
                #region Retrieve Lists

                langauge.Specification = ThirdEntity.Specification;

                #endregion

                this.UpdateValueObject(langauge, ThirdEntity);
                this.StringDecode(ThirdEntity);
            }

            if ((Rules.GetBrokenRules(langauge).Count == 0) && (TryValidateModel(langauge)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(langauge));

                SecondEntity.Languages.Remove(langauge);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", langauge)
                                });
            }

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", langauge)
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
                SecondEntity.Languages.Remove(ThirdEntity);

                Service.Update(FirstEntity);

                PrePublish();

                return Json(new
                                {
                                    Success = true,
                                    PartialViewHtml = RenderPartialViewToString("List", new JLanguage())
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
            try
            {
                JLanguage jLanguage = SecondEntity.Languages.Single(c => c.Id == id);

                SecondEntity.Languages.Remove(jLanguage);

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