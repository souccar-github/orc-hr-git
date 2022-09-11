<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Services/Views/Shared/Services.master"
    Inherits="System.Web.Mvc.ViewPage<UI.Areas.Services.DTO.ViewModels.DelegationViewModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divDelegationService" class="EntityAdd">
        <% Html.RenderPartial("DelegationService", Model); %>
    </div>
</asp:Content>
