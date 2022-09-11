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
    public class JExperienceController : JobDescAggregateController, IRule<JExperience>
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

        #region JExperience

        private JExperience _experience;

        public JExperience ThirdEntity
        {
            get
            {
                return _experience ??
                       (_experience =
                        SecondEntity.Experiences.SingleOrDefault(c => c.Id == GetMasterRecordValue(MasterRecordOrder.Third)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<JExperience>

        public ObjectRules<JExperience> Rules
        {
            get { return new JobExperienceRules(); }
        }

        #endregion

        #region Overrides of JobDescAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("CareerLevel.Name");
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                SecondEntity.Experiences.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Third));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return SecondEntity.Experiences.Count != 0
                       ? Rules.GetExpiredRules(SecondEntity.Experiences)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Third, selectedSubRowId);

            PrePublish();

            SaveTabIndexSecondLevel(3);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new JExperience());
        }

        [HttpPost]
        public ActionResult Save(JExperience jExperience)
        {
            PrePublish();

            if (jExperience.IsTransient())
            {
                SecondEntity.AddExperience(jExperience);
            }
            else
            {
                jExperience.Specification = ThirdEntity.Specification;

                this.UpdateValueObject(jExperience, ThirdEntity);
                this.StringDecode(ThirdEntity);
            }

            if ((Rules.GetBrokenRules(jExperience).Count == 0) && (TryValidateModel(jExperience)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(jExperience));

                SecondEntity.Experiences.Remove(jExperience);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", jExperience)
                                });
            }

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", jExperience)
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
                SecondEntity.Experiences.Remove(ThirdEntity);

                Service.Update(FirstEntity);

                PrePublish();

                return Json(new
                                {
                                    Success = true,
                                    PartialViewHtml = RenderPartialViewToString("List", new JExperience())
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
                JExperience experience = SecondEntity.Experiences.Single(c => c.Id == id);

                SecondEntity.Experiences.Remove(experience);

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