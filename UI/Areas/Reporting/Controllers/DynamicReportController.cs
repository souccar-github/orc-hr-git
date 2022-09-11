using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Reporting.DynamicReports;
using UI.Areas.Reporting.Controllers.EntitiesRoots;

namespace UI.Areas.Reporting.Controllers
{
    public class DynamicReportController : ReportAggregateController
    {
        public ActionResult Index(int reportId)
        {
            ViewData["reportId"] = reportId;
            return View();
        }

        public ActionResult ReportViewerPartial(int reportId)
        {
            var report = Service.GetById(reportId);
            var dynamicReport = new DynamicReport(System.Threading.Thread.CurrentThread.CurrentUICulture, report);
            ViewData["Report"] = dynamicReport;
            return PartialView("ReportViewerPartial");
        }

        public ActionResult ExportReportViewer(int reportId)
        {
            var report = Service.GetById(reportId);
            var dynamicReport = new DynamicReport(System.Threading.Thread.CurrentThread.CurrentUICulture, report);
            return DevExpress.Web.Mvc.ReportViewerExtension.ExportTo(dynamicReport);
        }
    }
}
