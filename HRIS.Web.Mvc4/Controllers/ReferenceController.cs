using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FastReport.Utils;
using Project.Web.Mvc4.Factories;
using Project.Web.Mvc4.Models;
using Souccar.Domain.DomainModel;

using Souccar.Core.Extensions;
using Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.PayrollSystem.RootEntities;
using HRIS.Domain.PayrollSystem.Enums;

namespace Project.Web.Mvc4.Controllers
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class ReferenceController : Controller
    {
   

        public ActionResult ReadToList(string typeName, RequestInformation requestInformation)
        {
            var type = typeName.ToType();
            if (type == null)
                return Json(null, JsonRequestBehavior.AllowGet);
           
            var data = CrudController.GetAll(type);
            var result = new List<DropdownItemViewModel>();
            if (type.GetProperties().Any(x => x.Name == "NameForDropdown"))
            {
                result = GetResult("NameForDropdown", data);
            }

            else if (type.GetProperties().Any(x => x.Name == "Name"))
            {
                result = GetResult("Name", data);
            }

            else if (type.GetProperties().Any(x => x.Name == "Title"))
            {
                result = GetResult("Title", data);
            }
            else if (type.GetProperties().Any(x => x.Name == "Id"))
            {
                result = GetResult("Id", data);
            }


            result = result.OrderBy(x => x.Name).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReferenceInfo(string typeName, int id, RequestInformation requestInformation)
        {
            var type = typeName.ToType();

            var item = type.GetById(id);
            return Json(new { gridModel = GridViewModelFactory.Create(typeName.ToType(), requestInformation), item = item.ToDynamicObj() }, JsonRequestBehavior.AllowGet);

        }
     
    

        public List<DropdownItemViewModel> GetResult(string propName, IQueryable<Entity> data)
        {
            var result = new List<DropdownItemViewModel>();
            foreach (var entity in data)
            {
                var temp = new DropdownItemViewModel()
                {
                    Id = entity.Id,
                    Name = Convert.ToString(entity.GetPropertyValue(propName))

                };
                temp.Name = temp.Name.Trim();
                result.Add(temp);
            }
            return result;
        }

        public ActionResult ReadUsers(RequestInformation requestInformation)
        {
            var result = ServiceFactory.ORMService.All<Souccar.Domain.Security.User>().Select(x => new { Id = x.Id, Name = x.FullName }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);

        }

    }
}
