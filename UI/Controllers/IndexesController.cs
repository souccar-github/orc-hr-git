#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Souccar.Core;
using Souccar.Domain.DomainModel;
using Telerik.Web.Mvc;
using Infrastructure.Entities;
using Service;
using UI.Helpers.Cache;
using Infrastructure.Validation;
using UI.Helpers.Localization;
using UI.Helpers.Model;
using UI.Models;

#endregion

namespace UI.Controllers
{
    public class IndexesController<T> : LocalizationController where T : IndexEntity, IAggregateRoot, new()
    {
       
        protected EntityServiceBase<T> Service;

        public IndexesController()
        {
            Service = new EntityService<T>();
        }

        public void SetGlobalErrorMessage(string errorMessage)
        {
            TempData["GlobalError"] = errorMessage;
        }

       

        public ActionResult Index()
        {
            return View();
        }

        


        [GridAction]
        public ActionResult AjaxGridSelect()
        {
            
            return View(new GridModel(Service.GetAll()));
        }

        [HttpPost]
        [GridAction]
        public ActionResult AjaxGridInsert()
        {
            try
            {
                var tObject = new T();
                if (TryUpdateModel(tObject))
                {
                    if (Service.GetAll().Any(t => t.Name == tObject.Name))
                    {
                        ModelState.AddModelError(DomainErrors.DupLicateValueError.ToString(), string.Format(Resources.Shared.Messages.General.DuplicateValueError, tObject.Name));
                        return new ErrorResult(string.Format(Resources.Shared.Messages.General.DuplicateValueError, tObject.Name), false);
                    }
                    Service.Update(tObject);
                    CacheProvider.ForceUpdate(tObject.GetType().Name);
                    return View("Index", new GridModel(Service.GetAll()));
                }
                else
                {
                    return new ErrorResult("", true);
                }

            }
            catch (Exception ex)
            {
                // handel error on the Repository layer
                return new ErrorResult(ex.Message, false);

            }


        }

        [HttpPost]
        [GridAction]
        public ActionResult AjaxGridUpdate(string id)
        {
            try
            {
                var tObject = new T();
                if (TryUpdateModel(tObject))
                {
                    if (Service.GetAll().Any(t => t.Name == tObject.Name && t.Id != tObject.Id))
                    {

                        ModelState.AddModelError(DomainErrors.DupLicateValueError.ToString(), string.Format(Resources.Shared.Messages.General.DuplicateValueError, tObject.Name));
                        return new ErrorResult(string.Format(Resources.Shared.Messages.General.DuplicateValueError, tObject.Name), false);
                    }
                    
                    Service.Update(tObject);
                    CacheProvider.ForceUpdate(tObject.GetType().Name);
                    return View("Index", new GridModel(Service.GetAll()));
                }
                else
                {
                    return new ErrorResult("", true);
                }



            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message, false);
            }

        }

        [HttpPost]
        [GridAction]
        public ActionResult AjaxGridDelete(string id)
        {

            try
            {
                T tObject = Service.GetById(int.Parse(id));

                if (TryUpdateModel(tObject))
                {
                    Service.Delete(tObject);
                    CacheProvider.ForceUpdate(tObject.GetType().Name);
                    return View("Index", new GridModel(Service.GetAll()));
                }
                else
                {
                    return new ErrorResult("", true);
                }


            }
            catch (Exception ex)
            {
                ModelState.AddModelError(DomainErrors.ReferncesValueError.ToString(), string.Format(Resources.Shared.Messages.General.ReferncesValueError));
                return new ErrorResult(string.Format(Resources.Shared.Messages.General.ReferncesValueError), false);
       
               // return new ErrorResult(ex.Message, false);
            }

        }
    }
}