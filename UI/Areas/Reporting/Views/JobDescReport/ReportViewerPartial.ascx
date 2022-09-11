<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<% Html.DevExpress().ReportViewer(settings =>
    {
        settings.Name = "reportViewer1";
        settings.Report = (Reporting.JobDesc.JobDescTemplate)ViewData["Report"];
        settings.CallbackRouteValues = new { Controller = "JobDescReport", Action = "ReportViewerPartial", positionId = ViewData["positionId"] };
        settings.ExportRouteValues = new { Controller = "JobDescReport", Action = "ExportReportViewer", positionId = ViewData["positionId"] };
    }).GetHtml();%>