<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<% Html.DevExpress().ReportViewer(settings =>
    {
        settings.Name = "reportViewer1";
        settings.Report = (Reporting.DynamicReports.DynamicReport)ViewData["Report"];
        settings.CallbackRouteValues = new { Controller = "DynamicReport", Action = "ReportViewerPartial", reportId = ViewData["reportId"] };
        settings.ExportRouteValues = new { Controller = "DynamicReport", Action = "ExportReportViewer", reportId = ViewData["reportId"] };
    }).GetHtml();%>