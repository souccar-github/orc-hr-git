using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.Navigation;
using  Project.Web.Mvc4.ProjectModels;

namespace Project.Web.Mvc4.Controllers
{
    public class ModuleController : Controller
    {
        public ActionResult Welcome(string id)
        {
            RequestInformation.Navigation.Step item = new RequestInformation.Navigation.Step();
            item.Name = id;
            try
                {
                item.ImageClass = BuildNavigation.GetModule(item.Name).ImageClass;
            }
            catch (Exception e)
            {
              //  return Json(new { Data = data, Errors = new { Exception = GlobalResource.ExceptionMessage } });
                //return Json(new { Data = data, Errors = new { Exception = e.Message } });
            }
            item.SmallImageClass = BuildNavigation.GetModule(item.Name).SmallImageClass;
            item.Title = BuildNavigation.GetModule(item.Name).Title;
            item.IsDetailHide = BuildNavigation.GetModule(item.Name).IsDetailHidden;
            TempData["Module"] = item;

          


            if (isActionExists("Index", "HomeController", item.Name))
            {
                //if (item.Name=="ReportGenerator")
                //    return RedirectToAction("Index", "ReportBuilder", new { area = item.Name });
                return RedirectToAction("Index", "Home", new { area = item.Name });
            }
            if (isViewExists(id))
                return View(id);
            else
                return View();
        }

        public bool isActionExists(string actionName, string controllerName, string areaName)
        {
            var types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            var listType = types.Where( t => t.Name == controllerName ).ToList();
            var type = listType.Where(x => x.Namespace.Contains("."+areaName+".")).SingleOrDefault();
            if (type != null && type.GetMethod(actionName) != null)
                return true;
            return false;
        }

        private bool isViewExists(string name)
        {
            ViewEngineResult result = ViewEngines.Engines.FindView(ControllerContext, name, null);
            return (result.View != null);
        }
    }
}
