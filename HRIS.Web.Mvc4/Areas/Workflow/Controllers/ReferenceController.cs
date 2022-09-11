using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Extensions;
using HRIS.Domain.Workflow;
using Project.Web.Mvc4.Models;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Domain.Workflow.Enums;
using HRIS.Domain.TaskManagement.RootEntities;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Souccar.Domain.Security;
using Souccar.Core.Extensions;
using HRIS.Domain.Global.Enums;

namespace Project.Web.Mvc4.Areas.Workflow.Controllers
{
    public class ReferenceController : Controller
    {
        [HttpPost]
        public ActionResult FilterUser(User User, RequestInformation requestInformation)
        {
            Type typeOfClass = typeof(User);
            var users = ServiceFactory.ORMService.All<User>().ToList();
            var user = UserExtensions.CurrentUser;
            var employeesCanViewThem = UserExtensions.GetChildrenAsEmployess(user);
            var result = users.Select(x => new { Id = x.Id, Name = x.NameForDropdown }).ToList();
            if (!UserExtensions.IsCurrentUserFullAuthorized(typeOfClass.FullName))
            {
                result = users.Where(x => employeesCanViewThem.Any(y => y.User().Id == x.Id)).Select(x => new { Id = x.Id, Name = x.NameForDropdown }).ToList();
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult ReadQuarter(string typeName, RequestInformation requestInformation)
        {

            var result = new List<Dictionary<string, object>>();
            var type = typeName.ToType();
            var values = Enum.GetValues(type);

            foreach (var value in values)
            {
                if ((Quarter)value == Quarter.Nothing)
                    continue;
                var data = new Dictionary<string, object>();
                var name = ServiceFactory.LocalizationService.GetResource(type.FullName + "." + value.ToString());
                data["Name"] = !string.IsNullOrEmpty(name) ? name : value.ToString().ToCapitalLetters();
                data["Id"] = (int)value;
                result.Add(data);

            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReadSemiAnnual(string typeName, RequestInformation requestInformation)
        {
            var result = new List<Dictionary<string, object>>();
            var type = typeName.ToType();
            var values = Enum.GetValues(type);

            foreach (var value in values)
            {
                if ((SemiAnnual)value == SemiAnnual.Nothing)
                    continue;
                var data = new Dictionary<string, object>();
                var name = ServiceFactory.LocalizationService.GetResource(type.FullName + "." + value.ToString());
                data["Name"] = !string.IsNullOrEmpty(name) ? name : value.ToString().ToCapitalLetters();
                data["Id"] = (int)value;
                result.Add(data);
            }

            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadMonth(string typeName, RequestInformation requestInformation)
        {
            var result = new List<Dictionary<string, object>>();
            var type = typeName.ToType();
            var values = Enum.GetValues(type);

            foreach (var value in values)
            {
                if ((Month)value == Month.Nothing)
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
