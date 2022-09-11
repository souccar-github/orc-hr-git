#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Personnel.ValueObjects;
using UI.Areas.Personnel.Controllers.EntitiesRoots;
using UI.Extensions;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.Personnel.ValueObjects;

#endregion

namespace UI.Areas.Personnel.Controllers.ValueObjects
{
    public class SkillController : EmployeeAggregateController, IRule<Skill>
    {        
        #region Parents Chain

        #region Employee

        private Employee _employee;

        public Employee FirstEntity
        {
            get
            {
                return _employee ??
                       (_employee = Service.LoadById(GetMasterRecordValue(MasterRecordOrder.First)));
            }
        }

        #endregion

        #region Skill

        private Skill _skill;

        public Skill SecondEntity
        {
            get
            {
                return _skill ??
                       (_skill =
                        FirstEntity.Skills.SingleOrDefault(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<Skill>

        public ObjectRules<Skill> Rules
        {
            get { return new SkillRules(); }
        }

        #endregion

        #region Overrides of EmployeeAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("Name.Name");
            ModelState.Remove("Level.Name");
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                FirstEntity.Skills.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FirstEntity.Skills != null
                       ? Rules.GetExpiredRules(FirstEntity.Skills)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Second, selectedSubRowId);

            PrePublish();

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new Skill());
        }

        public ActionResult Save(Skill skill)
        {
            PrePublish();

            #region Permission Check

            if (skill.IsTransient())
            {
                if (ViewData["CanCreate"] != null && !(bool)ViewData["CanCreate"])
                {
                    ErrorPartialMessage(Resources.Shared.Messages.General.NotAllowedToAddMessage);
                    return Json(new
                    {
                        Success = false,
                        PartialViewHtml = RenderPartialViewToString("Error")
                    });
                }
            }
            else
            {
                if (ViewData["CanUpdate"] != null && !(bool)ViewData["CanUpdate"])
                {
                    ErrorPartialMessage(Resources.Shared.Messages.General.NotAllowedToEditMessage);
                    return Json(new
                    {
                        Success = false,
                        PartialViewHtml = RenderPartialViewToString("Error")
                    });
                }
            }

            #endregion

            if (skill.IsTransient())
            {
                FirstEntity.AddSkill(skill);
            }
            else
            {
                #region Retrieve Lists

                skill.Employee = SecondEntity.Employee;

                #endregion

                this.UpdateValueObject(skill, SecondEntity);
                this.StringDecode(SecondEntity);
            }

            if ((Rules.GetBrokenRules(skill).Count == 0) && (TryValidateModel(skill)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(skill));

                FirstEntity.Skills.Remove(skill);

                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("List", skill)
                });
            }

            SetMasterRecordValue(MasterRecordOrder.Second, skill.Id);

            PrePublish();

            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("List", skill)
            });
        }


        public ActionResult JsonDelete(int id)
        {
            try
            {
                FirstEntity.Skills.Remove(SecondEntity);

                Service.Update(FirstEntity);

                PrePublish();

                return Json(new
                {
                    Success = true,
                    PartialViewHtml = RenderPartialViewToString("List", new Skill())
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
            if (ViewData["CanDelete"] != null && !(bool)ViewData["CanDelete"])
            {
                ErrorPartialMessage(Resources.Shared.Messages.General.NotAllowedToDeleteMessage);

                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("Error")
                });
            }

            try
            {
                Skill skill = FirstEntity.Skills.SingleOrDefault(c => c.Id == id);

                FirstEntity.Skills.Remove(skill);

                Service.Update(FirstEntity);

                PrePublish();

                return RedirectToAction("Index", "Employee");
            }
            catch (Exception)
            {
                return ErrorPartialMessage(Resources.Shared.Messages.General.ErrorDuringDelete);
            }
        }

        [HttpPost]
        public JsonResult JsonEdit()
        {
            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("Edit", SecondEntity)
            });
        }

        #endregion
    }
}