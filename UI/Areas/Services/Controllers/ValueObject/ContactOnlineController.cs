#region

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infrastructure.Validation;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Personnel.ValueObjects;
using Repository.UnitOfWork;
using Service;
using Telerik.Web.Mvc;
using UI.Areas.Personnel.Controllers.EntitiesRoots;
using UI.Areas.Personnel.Helpers;
using UI.Areas.Services.Controllers.EntitiesRoots;
using UI.Areas.Services.DTO.ViewModels;
using UI.Controllers;
using UI.Extensions;
using UI.Filters;
using UI.Helpers.Cache;
using UI.Helpers.Model;
using UI.Utilities;
using Validation.Personnel.ValueObjects;

#endregion

namespace UI.Areas.Services.Controllers.ValueObject
{
    public class ContactOnlineController : EmployeeOnlineController
    {

        

        #region // Parents Chain //

        #region // Employee //

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

        #endregion

        #region // IRule<Contact> Members //

        public ObjectRules<Contact> Rules
        {
            get { return new ContactRules(); }
        }

        #endregion

        #region // CRUD //

       

        [GridAction]
        public ActionResult ReadContacts(int employeeId)
        {
            SetMasterRecordValue(MasterRecordOrder.First, employeeId);

            return View("Index", new GridModel(GetContactsList()));
        }
        [HttpPost]
        [GridAction]
        public ActionResult InsertContact()
        {
            var contact = new Contact();
            //var contact1 = FirstEntity.Contact.Count;

            if (TryUpdateModel(contact))
            {
                FirstEntity.AddContact(contact);
                Service.Update(FirstEntity);
                
                CacheProvider.ForceUpdate(PersonnelCacheKeys.Contact.ToString());
            }

            return View("Index", new GridModel(GetContactsList()));
        }

        [HttpPost]
        [GridAction]
        public ActionResult UpdateContact(int id)
        {
            var contact = FirstEntity.Contact.Where(x => x.Id == id).Single();

            if (TryUpdateModel(contact))
            {
                contact.Employee = FirstEntity;
                Service.Update(FirstEntity);
                
                CacheProvider.ForceUpdate(PersonnelCacheKeys.Contact.ToString());
            }

            return View("Index", new GridModel(GetContactsList()));
        }

        [HttpPost]
        [GridAction]
        public ActionResult DeleteContact(int id)
        {
            var contact = FirstEntity.Contact.Where(x => x.Id == id).Single();

            if (TryUpdateModel(contact))
            {
                FirstEntity.Contact.Remove(contact);
                Service.Update(FirstEntity);
                CacheProvider.ForceUpdate(PersonnelCacheKeys.Contact.ToString());
            }

            return View("Index", new GridModel(GetContactsList()));
        }

       
        #endregion

        #region // Tools //

        private IEnumerable<Contact> GetContactsList()
        {
            IEnumerable<Contact> contacts = FirstEntity.Contact;

            return contacts.Select(item => new Contact
                                               {
                                                   Id = item.Id, FirstContact = item.FirstContact, SecondContact = item.SecondContact, Fax = item.Fax, PrimaryEMail = item.PrimaryEMail, SecondaryEMail = item.SecondaryEMail, Address = item.Address, Facebook = item.Facebook, POBox = item.POBox, Twitter = item.Twitter, WebSite = item.WebSite
                                               }).ToList();
        }

       
       

        #endregion

    }
}