using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.Global.Constant;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            if (TempData["Module"] == null)
                return RedirectToAction("Welcome", "Module", new { area = "", id = ModulesNames.EmployeeRelationServices });


            return View();
        }

    }
}
