<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<% Html.DevExpress().ReportViewer(settings =>
    {
        settings.Name = "reportViewer1";
        settings.Report = (Reporting.JobDesc.TestReporting)ViewData["Report"];
        settings.CallbackRouteValues = new { Controller = "EmployeeReport", Action = "ReportViewerPartial"};
        settings.ExportRouteValues = new { Controller = "EmployeeReport", Action = "ExportReportViewer"};
    }).GetHtml();%>