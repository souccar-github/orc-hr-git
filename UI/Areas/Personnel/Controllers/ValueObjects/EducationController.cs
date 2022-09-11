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
    public class EducationController : EmployeeAggregateController, IRule<Education>
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

        #region Education

        private Education _education;

        public Education SecondEntity
        {
            get
            {
                return _education ??
                       (_education =
                        FirstEntity.Educations.SingleOrDefault(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<Education>

        public ObjectRules<Education> Rules
        {
            get { return new EducationRules(); }
        }

        #endregion

        #region Overrides of EmployeeAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("Type.Name");
            ModelState.Remove("Country.Name");
            ModelState.Remove("Degree.Name");
            ModelState.Remove("Scale.Name");
            ModelState.Remove("Major.Name");
            ModelState.Remove("ScoreType.Name");
            ModelState.Remove("Rank.Name");
        }

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                FirstEntity.Educations.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FirstEntity.Educations != null
                       ? Rules.GetExpiredRules(FirstEntity.Educations)
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
            return PartialView("Edit", new Education());
        }

        public ActionResult Save(Education education)
        {
            PrePublish();

            #region Permission Check

            if (education.IsTransient())
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

            if (education.IsTransient())
            {
                FirstEntity.AddEducation(education);
            }
            else
            {
                #region Retrieve Lists

                education.Employee = SecondEntity.Employee;

                #endregion

                this.UpdateValueObject(education, SecondEntity);
                this.StringDecode(SecondEntity);
            }

            if ((Rules.GetBrokenRules(education).Count == 0) && (TryValidateModel(education)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(education));

                FirstEntity.Educations.Remove(education);

                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("List", education)
                });
            }

            SetMasterRecordValue(MasterRecordOrder.Second, education.Id);

            PrePublish();

            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("List", education)
            });
        }


        public ActionResult JsonDelete(int id)
        {
            try
            {
                FirstEntity.Educations.Remove(SecondEntity);

                Service.Update(FirstEntity);

                PrePublish();

                return Json(new
                {
                    Success = true,
                    PartialViewHtml = RenderPartialViewToString("List", new Education())
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
                Education education = FirstEntity.Educations.SingleOrDefault(c => c.Id == id);

                FirstEntity.Educations.Remove(education);

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