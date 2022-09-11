using HRIS.Domain.Global.Constant;
using Project.Web.Mvc4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Web.Mvc4.Areas.Audit.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Audit/Home/

        public ActionResult Index(RequestInformation.Navigation.Step moduleInfo)
        {
            if (TempData["Module"] == null)
                return RedirectToAction("Welcome", "Module", new { area = "", id = ModulesNames.Audit });
            return View();
        }

    }
}
