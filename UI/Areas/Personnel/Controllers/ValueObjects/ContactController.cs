#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.Personnel.ValueObjects;
using UI.Areas.Personnel.Controllers.EntitiesRoots;
using UI.Extensions;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.Personnel.ValueObjects;
using HRIS.Domain.Personnel.Entities;

#endregion

namespace UI.Areas.Personnel.Controllers.ValueObjects
{
    public class ContactController : EmployeeAggregateController, IRule<Contact>
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

        #region Contact

        private Contact _contact;

        public Contact SecondEntity
        {
            get
            {
                return _contact ??
                       (_contact =
                        FirstEntity.Contact.SingleOrDefault());
            }
        }

        #endregion

        #endregion

        #region IRule<Contact> Members

        public ObjectRules<Contact> Rules
        {
            get { return new ContactRules(); }
        }

        #endregion

        #region Overrides of EmployeeAggregateController

        public override void FillList()
        {
            ViewData["ValueObjectsList"] =
                FirstEntity.Contact.Where(i => i.Id == GetMasterRecordValue(MasterRecordOrder.Second));
        }

        public override List<BrokenBusinessRule> GetExpiredRules()
        {
            return FirstEntity.Contact != null
                       ? Rules.GetExpiredRules(FirstEntity.Contact)
                       : new List<BrokenBusinessRule>();
        }

        #endregion

        #region CRUD

        public ActionResult Index(int selectedSubRowId = 0)
        {
            SetMasterRecordValue(MasterRecordOrder.Second, selectedSubRowId);

            PrePublish();

            SaveTabIndex(1);

            return PartialView("Index");
        }

        public PartialViewResult Load()
        {
            return PartialView("Edit", new Contact());
        }

        public JsonResult Save(Contact contact)
        {
            PrePublish();

            #region Permission Check

            if (contact.IsTransient())
            {
                if (ViewData["CanCreate"] != null && !(bool)ViewData["CanCreate"])
                {
                    ErrorPartialMessage("You Are Not Allowed To Add !!");
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
                    ErrorPartialMessage("You Are Not Allowed To Edit !!");
                    return Json(new
                    {
                        Success = false,
                        PartialViewHtml = RenderPartialViewToString("Error")
                    });
                }
            }

            #endregion

            if (contact.IsTransient())
            {
                FirstEntity.AddContact(contact);
            }
            else
            {
                #region Retrieve Lists

                contact.Employee = SecondEntity.Employee;

                #endregion

                this.UpdateValueObject(contact, SecondEntity);
            }

            if ((Rules.GetBrokenRules(contact).Count == 0) && (TryValidateModel(contact)))
            {
                Service.Update(FirstEntity);
            }
            else
            {
                ModelState.AddModelErrors(Rules.GetBrokenRules(contact));

                FirstEntity.Contact.Remove(contact);

                return Json(new
                {
                    Success = false,
                    PartialViewHtml = RenderPartialViewToString("List", contact)
                });
            }

            SetMasterRecordValue(MasterRecordOrder.Second, contact.Id);

            PrePublish();

            return Json(new
            {
                Success = true,
                PartialViewHtml = RenderPartialViewToString("List", contact)
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
                Contact contact = FirstEntity.Contact.SingleOrDefault();

                FirstEntity.Contact.Remove(contact);

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