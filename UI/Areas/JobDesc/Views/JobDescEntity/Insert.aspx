<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/JobDesc/Views/Shared/JobDesc.master"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   <% Html.RenderPartial("Create", Model); %>
</asp:Content>
