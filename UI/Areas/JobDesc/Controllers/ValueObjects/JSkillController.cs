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
    public class JSkillController : JobDescAggregateController, IRule<JSkill>
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

        #region JSkill

        private JSkill _skill;

        public JSkill ThirdEntity
        {
            get
            {
                return _skill ??
                       (_skill = SecondEntity.Skills.SingleOrDefault(c => c.Id == GetMasterRecordValue(MasterRecordOrder.Third)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<JSkill>

        public ObjectRules<JSkill> Rules
        {
            get { return new JSkillRules(); }
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
                SecondEntity.Skills.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Third));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return SecondEntity.Skills.Count != 0
                       ? Rules.GetExpiredRules(SecondEntity.Skills)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Third, selectedRowId);

            PrePublish();

            SaveTabIndexSecondLevel(6);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new JSkill());
        }

        public ActionResult Save(JSkill jSkill)
        {
            PrePublish();

            if (jSkill.IsTransient())
            {
                SecondEntity.AddSkill(jSkill);
            }
            else
            {
                #region Retrieve Lists

                jSkill.Specification = ThirdEntity.Specification;

                #endregion

                this.UpdateValueObject(jSkill, ThirdEntity);
                this.StringDecode(ThirdEntity);
            }

            if ((Rules.GetBrokenRules(jSkill).Count == 0) && (TryValidateModel(jSkill)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(jSkill));

                SecondEntity.Skills.Remove(jSkill);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", jSkill)
                                });
            }

            PrePublish();

            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", jSkill)
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
                SecondEntity.Skills.Remove(ThirdEntity);

                Service.Update(FirstEntity);

                PrePublish();

                return Json(new
                                {
                                    Success = true,
                                    PartialViewHtml = RenderPartialViewToString("List", new JSkill())
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
                JSkill skill = SecondEntity.Skills.Single(c => c.Id == id);

                SecondEntity.Skills.Remove(skill);

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