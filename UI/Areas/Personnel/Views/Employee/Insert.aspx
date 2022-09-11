<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Personnel/Views/Shared/Personnel.master"
    Inherits="System.Web.Mvc.ViewPage<HRIS.Domain.Personnel.Entities.Employee>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="EntityAdd">
        <% Html.RenderPartial("EmployeeUserControlNew", Model); %></div>
</asp:Content>
