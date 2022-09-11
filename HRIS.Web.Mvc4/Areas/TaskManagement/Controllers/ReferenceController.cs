using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Souccar.Infrastructure.Core;
using  Project.Web.Mvc4.Extensions;
using HRIS.Domain.Workflow;
using  Project.Web.Mvc4.Models;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Domain.Workflow.Enums;
using HRIS.Domain.TaskManagement.RootEntities;
using  Project.Web.Mvc4.Helpers.DomainExtensions;

namespace Project.Web.Mvc4.Areas.TaskManagement.Controllers
{
    public class ReferenceController : Controller
    {
        //
        // GET: /TaskManagement/Reference/

        public ActionResult ReadTask(string typeName, RequestInformation requestInformation)
        {
            var temp=  ServiceFactory.ORMService.All<Task>().Where(x => x.Employee == EmployeeExtensions.CurrentEmployee);
            var result = temp.Select(x => new { Id = x.Id, Name = x.Title }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
        

    }
}
