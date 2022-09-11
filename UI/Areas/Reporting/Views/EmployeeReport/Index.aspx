<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Reporting/Views/Shared/ReportingViewers.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.DevExpress().ReportToolbar(settings =>
                                                            {
                                                                settings.Name = "ReportToolbar";
                                                                settings.ReportViewerName = "reportViewer1";
                                                            }
                            ).GetHtml() %>
    <% Html.RenderPartial("ReportViewerPartial"); %>
</asp:Content>
