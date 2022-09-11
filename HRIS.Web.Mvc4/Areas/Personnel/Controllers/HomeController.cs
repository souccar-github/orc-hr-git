using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using  Project.Web.Mvc4.Models;
using HRIS.Domain.Global.Constant;

namespace Project.Web.Mvc4.Areas.Personnel.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(RequestInformation.Navigation.Step moduleInfo)
        {
            if (TempData["Module"] == null)
                return RedirectToAction("Welcome", "Module", new { area = "", id = ModulesNames.Personnel });

            return View();
        }
    }
}
