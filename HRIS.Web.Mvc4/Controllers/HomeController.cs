
using System;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Project.Web.Mvc4.CopyProtection;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Extensions;
using Souccar.Domain.Security;
using Project.Web.Mvc4.ProjectModels;
using NHibernateDBGenerator.NHibernate.Helpers;
using NHibernateDBGenerator.NHibernate.Enumerations;

namespace Project.Web.Mvc4.Controllers
{
    public class HomeController : Controller
    {
        [Authorize()]
        public ActionResult Index()
        {

            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            if (isActionExists("Index", "HomeController", "dashboard"))
            {
                //if (item.Name=="ReportGenerator")
                //    return RedirectToAction("Index", "ReportBuilder", new { area = item.Name });
                return RedirectToAction("Index", "Home", new { area = "dashboard" });
            }

            return View();
        }



        public ActionResult ArLanguage()
        {
            CurrentLocale.Language = Locale.Rtl;
            //CurrentLocale.Language = Locale.ar_SY;
            if (Request.UrlReferrer != null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }

            return Index();
        }

        public ActionResult EnLanguage()
        {
            //CurrentLocale.Language = Locale.en_US;
            CurrentLocale.Language = Locale.Ltr;
            if (Request.UrlReferrer != null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }

            return Index();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult GetCurrentDesign( ) {

            return PartialView(BuildNavigation.CurrentNavigation.BuildTabDesign());

        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Settings()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public ActionResult Settings(ThemingType themingType)
        {            
            DirectoryInfo directoryInfo = new DirectoryInfo(Server.MapPath("~/Content/images/theme-" + UserExtensions.CurrentUserTheming));
            if (!directoryInfo.Exists)
            {
                HttpCookie MyCookie = new HttpCookie("IsThemingSupported");
                MyCookie.Value = false.ToString();
                Response.Cookies.Add(MyCookie);
                return View();
            }
            else
            {
                var user = UserExtensions.CurrentUser;
                if (user != null)
                {
                    user.ThemingType = themingType;
                    user.Save();
                }
                HttpCookie MyCookie = new HttpCookie("Theming");
                MyCookie.Value = themingType.ToString().ToLower();
                Response.Cookies.Add(MyCookie);
                MyCookie = new HttpCookie("IsThemingSupported");
                MyCookie.Value = true.ToString();
                Response.Cookies.Add(MyCookie);
                return View();
            }
            
        }
        public ActionResult Tooltip(string module)
        {
            var tooltip = new Tooltip();
            switch (module)
            {
                case "Personnel":
                    tooltip.Title = "Nuha Shweihna";
                    tooltip.ImgPath = Url.Content("~/Content/images/maestro/nuha.png");
                    tooltip.Role = "HR Manager";
                    break;
                case "PerformanceAppraisal":
                    tooltip.Title = "Section Template";
                    tooltip.ImgPath = Url.Content("~/Content/images/maestro/section.png");
                    tooltip.Role = "Appraisal Template";
                    break;
                case "OrganizationChart":
                    tooltip.Title = "Organization  Chart";
                    tooltip.ImgPath = Url.Content("~/Content/images/maestro/organization.png");
                    tooltip.Role = "Hierarchy";
                    break;
                case "JobDescription":
                    tooltip.Title = "Software Engineer";
                    tooltip.ImgPath = Url.Content("~/Content/images/maestro/job.png");
                    tooltip.Role = "Developer";
                    break;
            }

            return PartialView("TooltipPartial", tooltip);
        }

        public bool isActionExists(string actionName, string controllerName, string areaName)
        {
            var types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            var listType = types.Where(t => t.Name == controllerName).ToList();
            var type = listType.Where(x => x.Namespace.Contains(areaName)).SingleOrDefault();
            if (type != null && type.GetMethod(actionName) != null)
                return true;
            return false;
        }
        public ActionResult UpdateDb()
        {
            try
            {
                FluentSessionProvider.GenerateSchema(GenerateSchemaMode.Update);
            }
            catch (Exception ex)
            {
                return Content("DB Update Failed ! " + ex.Message);
            }
            return Content("DB Updated !");
        }
    }





}

