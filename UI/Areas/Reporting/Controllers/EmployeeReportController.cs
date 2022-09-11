using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Personnel.Entities;
using Reporting.JobDesc;
namespace UI.Areas.Reporting.Controllers
{
    public class EmployeeReportController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReportViewerPartial()
        {
            var report = new TestReporting();
            ViewData["Report"] = report;
            return PartialView("ReportViewerPartial");
        }
        public ActionResult ExportReportViewer()
        {
            var report = new TestReporting();
            return DevExpress.Web.Mvc.ReportViewerExtension.ExportTo(report);
        }
    }
}
