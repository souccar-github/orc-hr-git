using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Reporting.JobDesc;
namespace UI.Areas.Reporting.Controllers
{
    public class JobDescReportController : Controller
    {
        public ActionResult Index(int positionId)
        {
            ViewData["positionId"] = positionId;
            return View();
        }

        public ActionResult ReportViewerPartial(int positionId)
        {
            var langauge = Session["CurrentUICulture"].ToString();
            var culture = new CultureInfo(langauge);
            var report = new JobDescTemplate(positionId, culture);
            ViewData["Report"] = report;
            return PartialView("ReportViewerPartial");
        }

        public ActionResult ExportReportViewer(int positionId)
        {
            
           // var culture = System.Threading.Thread.CurrentThread.CurrentUICulture.ToString();
           // var culture = Session["CurrentUICulture"].ToString();
            var report = new JobDescTemplate(positionId);
    
            return DevExpress.Web.Mvc.ReportViewerExtension.ExportTo(report);
        }
    }
}
