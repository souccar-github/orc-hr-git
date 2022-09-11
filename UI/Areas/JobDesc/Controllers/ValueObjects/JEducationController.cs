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
    public class JEducationController : JobDescAggregateController, IRule<JEducation>
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

        #region JEducation

        private JEducation _education;

        public JEducation ThirdEntity
        {
            get
            {
                return _education ??
                       (_education =
                        SecondEntity.Educations.SingleOrDefault(c => c.Id == GetMasterRecordValue(MasterRecordOrder.Third)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<JEducation>

        public ObjectRules<JEducation> Rules
        {
            get { return new JEducationRules(); }
        }

        #endregion

        #region Overrides of JobDescAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("Major.Name");
            ModelState.Remove("Rank.Name");
            ModelState.Remove("Type.Name");
            ModelState.Remove("ScoreType.Name");
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                SecondEntity.Educations.Where(
                    i => i.Id == GetMasterRecordValue(MasterRecordOrder.Third));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return SecondEntity.Educations.Count != 0
                       ? Rules.GetExpiredRules(SecondEntity.Educations)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Third, selectedRowId);

            PrePublish();

            SaveTabIndexSecondLevel(2);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new JEducation());
        }

        public ActionResult Save(JEducation jEducation)
        {
            PrePublish();
   
            if (jEducation.IsTransient())
            {
                SecondEntity.AddEducation(jEducation);
            }
            else
            {
                #region Retrieve Lists

                jEducation.Specification = ThirdEntity.Specification;

                #endregion

                this.UpdateValueObject(jEducation, ThirdEntity);
                this.StringDecode(ThirdEntity);
            }

            if ((Rules.GetBrokenRules(jEducation).Count == 0) && (TryValidateModel(jEducation)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(jEducation));

                SecondEntity.Educations.Remove(jEducation);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", jEducation)
                                });
            }

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", jEducation)
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
                SecondEntity.Educations.Remove(ThirdEntity);

                Service.Update(FirstEntity);

                PrePublish();

                return Json(new
                                {
                                    Success = true,
                                    PartialViewHtml = RenderPartialViewToString("List", new JEducation())
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
                JEducation education = SecondEntity.Educations.Single(c => c.Id == id);

                SecondEntity.Educations.Remove(education);

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