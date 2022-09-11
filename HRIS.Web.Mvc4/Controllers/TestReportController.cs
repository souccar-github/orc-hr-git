//using FastReport;
//using FastReport.Web;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace Project.Web.Mvc4.Controllers
//{
//    public class TestReportController : Controller
//    {
//        private WebReport webReport = new WebReport();
//        //
//        // GET: /TestReport/

//        public ActionResult Index()
//        {
//            SetReport();

//            webReport.Width = 750;
//            webReport.Height = 800;
//            webReport.ToolbarIconsStyle = ToolbarIconsStyle.Black;

//            ViewBag.WebReport = webReport;
//            return View();
//        }

//        private void SetReport()
//        {
//            //string report_path = @"d:\test.frx";
//            //System.Data.DataSet dataSet = new System.Data.DataSet();
//            //dataSet.ReadXml(@"d:\nwind.xml");
//            //webReport.Report.RegisterData(dataSet, "NorthWind");
//            webReport.Report.Load(@"d:\TestReport.frx");
//            webReport.CurrentTab.Name = "Simple List";
//            //// tab 2
//            //Report report2 = new Report();
//            //report2.RegisterData(dataSet, "NorthWind");
//            //report2.Load(report_path + "Labels.frx");
//            //webReport.AddTab(report2, "Labels");
//            //// tab 3
//            //Report report3 = new Report();
//            //report3.RegisterData(dataSet, "NorthWind");
//            //report3.Load(report_path + "Master-Detail.frx");
//            //webReport.AddTab(report3, "Master-Detail");
//        }

//    }
//}

