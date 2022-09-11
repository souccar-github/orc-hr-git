using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.Global.Constant;
using  Project.Web.Mvc4.Models;

namespace Project.Web.Mvc4.Areas.OrganizationChart.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /OrganizationChart/Home/

        public ActionResult Index(RequestInformation.Navigation.Step moduleInfo)
        {
            if (TempData["Module"] == null)
                return RedirectToAction("Welcome", "Module", new { area = "", id = ModulesNames.OrganizationChart });
            return View();
        }

    }
}
