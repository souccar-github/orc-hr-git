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
    public class PassportController : EmployeeAggregateController, IRule<Passport>
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

        private Passport _passport;

        public Passport SecondEntity
        {
            get
            {
                return _passport ??
                       (_passport =
                        FirstEntity.Passports.SingleOrDefault(
                            r => r.Id == GetMasterRecordValue(MasterRecordOrder.Second)));
            }
        }

        #endregion

        #endregion

        #region Implementation of IRule<Passport>

        public ObjectRules<Passport> Rules
        {
            get { return new PassportRules(); }
        }

        #endregion

        #region Overrides of EmployeeAggregateController

        public override void CleanUpModelState()
        {
            ModelState.Remove("PlaceOfIssuance.Name");
        }


        public override void FillList()
        {

            ViewData["ValueObjectsList"] = FirstEntity.Passports.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second)).OrderBy(o => o.IssuanceDate);
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FirstEntity.Passports != null
                        ? Rules.GetExpiredRules(FirstEntity.Passports)
                        : new List<BrokenBusinessRule>();

        }
        #endregion


        #region CRUD


        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Second, selectedSubRowId);

            PrePublish();

            SaveTabIndex(2);

            return PartialView("Index");
        }



        public PartialViewResult Load()
        {
            return PartialView("Edit", new Passport());
        }


        [HttpPost]
        public JsonResult Save(Passport passport)
        {
            PrePublish();

            #region Permission Check

            if (passport.IsTransient())
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


            if (passport.IsTransient())
            {
                FirstEntity.AddPassport(passport);
            }
            else
            {
                #region Retrieve Lists

                passport.Employee = SecondEntity.Employee;

                #endregion


                this.UpdateValueObject(passport, SecondEntity);
            }

            if ((Rules.GetBrokenRules(passport).Count == 0) && (TryValidateModel(passport)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(passport));

                FirstEntity.Passports.Remove(passport);

                return Json(new
                                {
                                    Success = false,
                                    PartialViewHtml = RenderPartialViewToString("List", passport)
                                });
            }
            SetMasterRecordValue(MasterRecordOrder.Second, passport.Id);

            PrePublish();
            return Json(new
                            {
                                Success = true,
                                PartialViewHtml = RenderPartialViewToString("List", passport)
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
                Passport passport = FirstEntity.Passports.SingleOrDefault(c => c.Id == id);

                FirstEntity.Passports.Remove(passport);

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