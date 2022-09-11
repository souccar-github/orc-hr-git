using HRIS.Domain.Personnel.RootEntities;
using Project.Web.Mvc4.Models;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Souccar.Core.Extensions;
using Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.Global.Enums;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using HRIS.Domain.EmployeeRelationServices.RootEntities;

namespace Project.Web.Mvc4.Areas.Personnel.Controllers
{
    public class ReferenceController : Controller
    {
        //
        // GET: /Personnel/Reference/

        public ActionResult ReadSpouseForChild(string typeName, RequestInformation requestInformation)
        {
            var emp = ServiceFactory.ORMService.GetById<Employee>(requestInformation.NavigationInfo.Previous[0].RowId);
            var result = emp.Spouse.Select(x => new { Id = x.Id, Name = string.Format("{0} {1}", x.FirstName, x.LastName) }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult FilterEmployee(Employee Employee, RequestInformation requestInformation)
        {
            Type typeOfClass = typeof(Employee);
            var employees = ServiceFactory.ORMService.All<Employee>().ToList();
            var user = UserExtensions.CurrentUser;
            var employeesCanViewThem = UserExtensions.GetChildrenAsEmployess(user);
            var result = employees.Select(x => new { Id = x.Id, Name = x.NameForDropdown }).ToList();
            if (!UserExtensions.IsCurrentUserFullAuthorized(typeOfClass.FullName))
            {
                result = employeesCanViewThem.Select(x => new { Id = x.Id, Name = x.NameForDropdown }).ToList();
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReadPeriodWithoutCustom(string typeName, RequestInformation requestInformation)
        {
            var result = new List<Dictionary<string, object>>();
            var type = typeName.ToType();
            var values = Enum.GetValues(type);
            foreach (var value in values)
            {
                if((Period)value==Period.Custom)
                    continue;
                var data = new Dictionary<string, object>();
                var name = ServiceFactory.LocalizationService.GetResource(type.FullName + "." + value.ToString());
                data["Name"] = !string.IsNullOrEmpty(name) ? name : value.ToString().ToCapitalLetters();
                data["Id"] = (int)value;
                result.Add(data);
            }
            
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

    }
}
