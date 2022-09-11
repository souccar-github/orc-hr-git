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
    public class SpouseController : EmployeeAggregateController, IRule<Spouse>
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

        #region Spouse

        private Spouse _spouse;

        public Spouse SecondEntity
        {
            get
            {
                return _spouse ??
                       (_spouse =
                        FirstEntity.Spouse.SingleOrDefault());
            }
        }

        #endregion

        #endregion

        #region IRule<Spouse> Members

        public ObjectRules<Spouse> Rules
        {
            get { return new SpouseRules(); }
        }

        #endregion

        #region Overrides of EmployeeAggregateController

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                FirstEntity.Spouse.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override void CleanUpModelState()
        {
            ModelState.Remove("Nationality.Name");
            ModelState.Remove("PlaceOfBirth.Name");
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FirstEntity.Spouse != null
                        ? Rules.GetExpiredRules(FirstEntity.Spouse)
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
            return PartialView("Edit", new Spouse());
        }

        public JsonResult Save(Spouse spouse)
        {
            PrePublish();

            #region Permission Check

            if (spouse.IsTransient())
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

            #region Dates

            if (spouse.ResidencyExpiryDate == DateTime.MinValue && string.IsNullOrEmpty(spouse.ResidencyNo))
            {
                spouse.ResidencyExpiryDate = new DateTime(1800, 1, 1);

                ModelState.Remove("ResidencyExpiryDate");
            }

            if (spouse.PassportExpiryDate == DateTime.MinValue && string.IsNullOrEmpty(spouse.PassportNo))
            {
                spouse.PassportExpiryDate = new DateTime(1800, 1, 1);

                ModelState.Remove("PassportExpiryDate");
            }

            #endregion

            if (spouse.IsTransient())
            {
                FirstEntity.AddSpouse(spouse);
            }
            else
            {
                #region Retrieve Lists

                spouse.Employee = SecondEntity.Employee;

                #endregion

                this.UpdateValueObject(spouse, SecondEntity);
            }

            if ((Rules.GetBrokenRules(spouse).Count == 0) && (TryValidateModel(spouse)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(spouse));

                FirstEntity.Spouse.Remove(spouse);

                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("List", spouse)
                });
            }

            SetMasterRecordValue(MasterRecordOrder.Second, spouse.Id);

            PrePublish();

            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("List", spouse)
            });
        }

        [HttpPost]
        public ActionResult Delete()
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
                Spouse spouse = FirstEntity.Spouse.SingleOrDefault();

                FirstEntity.Spouse.Remove(spouse);

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