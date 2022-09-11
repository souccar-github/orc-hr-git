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
    public class ComputerSkillController : JobDescAggregateController, IRule<ComputerSkill>
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

        #region Computer Skill

        private ComputerSkill _computerSkill;

        public ComputerSkill ThirdEntity
        {
            get
            {
                return _computerSkill ??
                       (_computerSkill =
                        SecondEntity.ComputerSkills.SingleOrDefault(c => c.Id == GetMasterRecordValue(MasterRecordOrder.Third)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<ComputerSkill>

        public ObjectRules<ComputerSkill> Rules
        {
            get { return new ComputerSkillRules(); }
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
                SecondEntity.ComputerSkills.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Third));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return SecondEntity.ComputerSkills.Count != 0
                       ? Rules.GetExpiredRules(SecondEntity.ComputerSkills)
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
            return PartialView("Edit", new ComputerSkill());
        }

        [HttpPost]
        public ActionResult Save(ComputerSkill computerSkill)
        {
            PrePublish();

            if (computerSkill.IsTransient())
            {
                SecondEntity.AddComputerSkill(computerSkill);
            }
            else
            {
                computerSkill.Specification = ThirdEntity.Specification;

                this.UpdateValueObject(computerSkill, ThirdEntity);

                this.StringDecode(ThirdEntity);
            }

            if ((Rules.GetBrokenRules(computerSkill).Count == 0) && (TryValidateModel(computerSkill)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(computerSkill));

                SecondEntity.ComputerSkills.Remove(computerSkill);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", computerSkill)
                                });
            }

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", computerSkill)
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
                SecondEntity.ComputerSkills.Remove(ThirdEntity);

                Service.Update(FirstEntity);

                PrePublish();

                return Json(new
                                {
                                    Success = true,
                                    PartialViewHtml = RenderPartialViewToString("List", new ComputerSkill())
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
                ComputerSkill computerSkill = SecondEntity.ComputerSkills.Single(c => c.Id == id);

                SecondEntity.ComputerSkills.Remove(computerSkill);

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