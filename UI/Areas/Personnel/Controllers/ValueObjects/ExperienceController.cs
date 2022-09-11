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
    public class ExperienceController : EmployeeAggregateController, IRule<Experience>
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

        #region Passport

        private Experience _experience;

        public Experience SecondEntity
        {
            get
            {
                return _experience ??
                       (_experience =
                        FirstEntity.Experiences.SingleOrDefault(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #endregion

        #region IRule<Experience> Members

        public ObjectRules<Experience> Rules
        {
            get { return new ExperienceRules(); }
        }

        #endregion


        #region Overrides of EmployeeAggregateController
        public override void FillList()
        {
            ViewData["ValueObjectsList"] = FirstEntity.Experiences.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second)).OrderBy(o => o.StartDate);
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FirstEntity.Experiences != null
                        ? Rules.GetExpiredRules(FirstEntity.Experiences)
                        : new List<BrokenBusinessRule>();
        }
        #endregion


        #region CRUD


        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Second, selectedSubRowId);

            PrePublish();

            SaveTabIndex(11);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new Experience());
        }

        [HttpPost]
        public JsonResult Save(Experience experience)
        {
            PrePublish();

            #region Permission Check

            if (experience.IsTransient())
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
                    ErrorPartialMessage(Resources.Shared.Messages.General.NotAllowedToAddMessage);
                    return Json(new
                    {
                        Success = false,
                        PartialViewHtml = RenderPartialViewToString("Error")
                    });
                }
            }

            #endregion


            if (experience.IsTransient())
            {
                FirstEntity.AddExperience(experience);
            }
            else
            {
                #region Retrieve Lists

                experience.Employee = SecondEntity.Employee;

                #endregion


                this.UpdateValueObject(experience, SecondEntity);
            }

            if ((Rules.GetBrokenRules(experience).Count == 0)) //&& (TryValidateModel(experience)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(experience));

                FirstEntity.Experiences.Remove(experience);

                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("List", experience)
                });
            }
            SetMasterRecordValue(MasterRecordOrder.Second, experience.Id);

            PrePublish();
            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("List", experience)
            });
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
                Experience experience = FirstEntity.Experiences.SingleOrDefault(c => c.Id == id);

                FirstEntity.Experiences.Remove(experience);

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