<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<UI.Models.RegisterModel>" %>

<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <%:Resources.Views.Account.Account.RegisterTitle %>
</asp:Content>
<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        <%:Resources.Views.Account.Account.CreateAccountTitle %></h2>
    <p>
        <%:Resources.Views.Account.Account.FormToCreateAccountTitle %>
    </p>
    <p>
        <%:String.Format(Resources.Views.Account.Account.PasswordsLength, ViewData["PasswordLength"])%>
    </p>
    <% using (Html.BeginForm())
       { %>
    <%: Html.ValidationSummary(true, Resources.Views.Account.Account.AccountCreationUnsuccessfulMessage)%>
    <div>
        <fieldset>
            <legend>
                <%:Resources.Views.Account.Account.AccountInformationTitle%></legend>
            <div class="editor-label">
                <%: Html.LabelFor(m => m.UserName) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(m => m.UserName) %>
                <%: Html.ValidationMessageFor(m => m.UserName) %>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(m => m.Email) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(m => m.Email) %>
                <%: Html.ValidationMessageFor(m => m.Email) %>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(m => m.Password) %>
            </div>
            <div class="editor-field">
                <%: Html.PasswordFor(m => m.Password) %>
                <%: Html.ValidationMessageFor(m => m.Password) %>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(m => m.ConfirmPassword) %>
            </div>
            <div class="editor-field">
                <%: Html.PasswordFor(m => m.ConfirmPassword) %>
                <%: Html.ValidationMessageFor(m => m.ConfirmPassword) %>
            </div>
            <p>
                <input type="submit" value="<%:Resources.Shared.Buttons.Function.Register %>" />
            </p>
        </fieldset>
    </div>
    <% } %>
</asp:Content>
