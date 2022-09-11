using System;
using System.Web.Mvc;
using FastReport.Web;
using Project.Web.Mvc4.Models;
using Souccar.Infrastructure.Core;
using Souccar.Domain.Reporting;
using Project.Web.Mvc4.Helpers;
using System.Web.UI.WebControls;
using System.Configuration;
using Microsoft.Reporting.WebForms;
using Project.Web.Mvc4.Areas.Reporting.Helpers;
using System.Windows.Forms;

namespace Project.Web.Mvc4.Controllers
{
    public class ReportingPreviewController : Controller
    {
        [HttpPost]
        public ActionResult Index(RequestInformation requestInformation=null)
        {  

            //FastReport
            var report = ServiceFactory.ORMService.GetById<ReportDefinition>(int.Parse(requestInformation.NavigationInfo.Previous[0].Name));
            string fastReport = "frx";
            if (report.FileName.Contains(fastReport))
            {
                var webReport = new WebReport();
                webReport.Report.Load(UploadHelper.UploadRootPath + "Souccar.Domain.Report.ReportDefinition/FileName/" + report.FileName);
                for (int i = 0; i < webReport.Report.Dictionary.Connections.Count; i++)
                {
                    webReport.Report.Dictionary.Connections[i].ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                }
                webReport.CurrentTab.Name = ServiceFactory.LocalizationService.GetResource(report.FileName);

                webReport.Width = Unit.Percentage(100);
                webReport.Height = Unit.Percentage(100);
                webReport.ToolbarIconsStyle = ToolbarIconsStyle.Black;

                ViewBag.WebReport = webReport;
                return PartialView("IndexForFastReport");
            }
            else
            {
                var height = Screen.PrimaryScreen.Bounds.Height-269;
                var reportViewer = new ReportViewer();
                reportViewer.ProcessingMode = ProcessingMode.Remote;
                var reportproject = ConfigurationManager.AppSettings["SSRS_ReportProject"];
                var serverurl = ConfigurationManager.AppSettings["SSRS_ServerURL"];
                reportViewer.ServerReport.ReportPath = reportproject + report.FileName;
                reportViewer.ServerReport.ReportServerUrl = new Uri(serverurl);
                reportViewer.Width = Unit.Percentage(100);
                reportViewer.Height = Unit.Pixel(height);
                reportViewer.CssClass = "ssrsReportStyle";
                reportViewer.ShowPrintButton = true;
                reportViewer.ServerReport.DisplayName = ServiceFactory.LocalizationService.GetResource(report.FileName) + "_" + DateTime.Now.ToString("dd-MM-yyyy HH mm ss");
                string userName = ConfigurationManager.AppSettings["SSRS_UserName"];
                string password = ConfigurationManager.AppSettings["SSRS_Password"];
                string domain = ConfigurationManager.AppSettings["SSRS_Domain"];
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(domain))
                {
                    reportViewer.ServerReport.ReportServerCredentials = new CustomReportCredentials(userName, password, domain);
                }
                ReportingHelper.PassReportParameter(reportViewer);
                ViewBag.ReportViewer = reportViewer;
                ViewBag.Height = height;
                return PartialView("IndexForSSRS");
            }
        }


        public class CustomReportCredentials : IReportServerCredentials
        {
            private string _UserName;
            private string _PassWord;
            private string _DomainName;

            public CustomReportCredentials(string UserName, string PassWord, string DomainName)
            {
                _UserName = UserName;
                _PassWord = PassWord;
                _DomainName = DomainName;
                NetworkCredentials = new System.Net.NetworkCredential(UserName, PassWord, DomainName);
            }
            public CustomReportCredentials(System.Net.NetworkCredential nc)
            {
                NetworkCredentials = nc;
            }
            public System.Security.Principal.WindowsIdentity ImpersonationUser
            {
                get { return null; }
            }

            public System.Net.ICredentials NetworkCredentials
            {
                get;
                set;
            }

            public bool GetFormsCredentials(out System.Net.Cookie authCookie, out string user,
             out string password, out string authority)
            {
                authCookie = null;
                user = password = authority = null;
                return false;
            }
        }
    }
}

