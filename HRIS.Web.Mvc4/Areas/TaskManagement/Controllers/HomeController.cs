using HRIS.Domain.Global.Constant;
using  Project.Web.Mvc4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Web.Mvc4.Areas.TaskManagement.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(RequestInformation.Navigation.Step moduleInfo)
        {
            if (TempData["Module"] == null)
                return RedirectToAction("Welcome", "Module", new { area = "", id = ModulesNames.TaskManagement });

            return View();
        }

    }
}
