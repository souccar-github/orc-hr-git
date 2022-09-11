<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Objective/Views/Shared/Objective.master"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="EntityAdd">
        <% Html.RenderPartial("Create", Model); %></div>
</asp:Content>
