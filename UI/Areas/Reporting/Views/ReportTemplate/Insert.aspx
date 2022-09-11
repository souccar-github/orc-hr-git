<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Reporting/Views/Shared/ReportingViewers.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="EntityAdd">
        <% Html.RenderPartial("Create", Model); %>
    </div>
</asp:Content>
