using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.Global.Constant;
using  Project.Web.Mvc4.Models;

namespace Project.Web.Mvc4.Areas.JobDescription.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(RequestInformation.Navigation.Step moduleInfo)
        {
            if (TempData["Module"] == null)
                return RedirectToAction("Welcome", "Module", new { area = "", id = ModulesNames.JobDescription });

            return View();
        }
    }
}
