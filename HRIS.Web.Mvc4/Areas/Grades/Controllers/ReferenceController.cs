using HRIS.Domain.Personnel.Indexes;
using  Project.Web.Mvc4.Models;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Web.Mvc4.Areas.Grades.Controllers
{
    public class ReferenceController : Controller
    {
        //
        // GET: /Grade/Reference/

        public ActionResult ReadMajorType(string typeName, RequestInformation requestInformation)
        {
            var temp = ServiceFactory.ORMService.All<MajorType>();
            var result = temp.Select(x => new { Id = x.Id, Name = x.Name }).ToList();
            //result.Add(new { Id = 0, Name = "All" });
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReadMajor(string typeName, RequestInformation requestInformation)
        {
            var temp = ServiceFactory.ORMService.All<Major>();
            var result = temp.Select(x => new { Id = x.Id, Name = x.Name }).ToList();
            //result.Add(new { Id = 0, Name = "All" });
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
    }
}
