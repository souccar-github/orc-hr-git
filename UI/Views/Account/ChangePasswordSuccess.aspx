<%@Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="changePasswordTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <%:Resources.Views.Account.Account.ChangePasswordTitle %>
</asp:Content>

<asp:Content ID="changePasswordSuccessContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%:Resources.Views.Account.Account.ChangePasswordTitle %></h2>
    <p>
        <%:Resources.Views.Account.Account.ChangePasswordSuccessfulMessage%>
    </p>
</asp:Content>
