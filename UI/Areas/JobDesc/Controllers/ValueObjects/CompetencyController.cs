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
    public class CompetencyController : JobDescAggregateController, IRule<Competency>
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

        #region Competency

        private Competency _competency;

        public Competency ThirdEntity
        {
            get
            {
                return _competency ??
                       (_competency =
                        SecondEntity.Competencies.SingleOrDefault(c => c.Id == GetMasterRecordValue(MasterRecordOrder.Third)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<Competency>

        public ObjectRules<Competency> Rules
        {
            get { return new CompetencyRules(); }
        }

        #endregion

        #region Overrides of JobDescAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("Level.Name");
            ModelState.Remove("Type.Name");
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                SecondEntity.Competencies.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Third));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return SecondEntity.Competencies.Count != 0
                       ? Rules.GetExpiredRules(SecondEntity.Competencies)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Third, selectedRowId);

            PrePublish();

            SaveTabIndexSecondLevel(0);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new Competency());
        }

        public ActionResult Save(Competency competency)
        {
            PrePublish();

            if (competency.IsTransient())
            {
                SecondEntity.AddCompetency(competency);
            }
            else
            {
                #region Retrieve Lists

                competency.Specification = ThirdEntity.Specification;

                #endregion

                this.UpdateValueObject(competency, ThirdEntity);
                this.StringDecode(ThirdEntity);
            }

            if ((Rules.GetBrokenRules(competency).Count == 0) && (TryValidateModel(competency)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(competency));

                SecondEntity.Competencies.Remove(competency);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", competency)
                                });
            }

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", competency)
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
                SecondEntity.Competencies.Remove(ThirdEntity);

                Service.Update(FirstEntity);

                PrePublish();

                return Json(new
                                {
                                    Success = true,
                                    PartialViewHtml = RenderPartialViewToString("List", new Competency())
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
                Competency competency = SecondEntity.Competencies.Single(c => c.Id == id);

                SecondEntity.Competencies.Remove(competency);

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