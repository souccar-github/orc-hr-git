<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<UI.Models.ChangePasswordModel>" %>

<asp:Content ID="changePasswordTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <%:Resources.Views.Account.Account.ChangePasswordTitle %>
</asp:Content>
<asp:Content ID="changePasswordContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        <%:Resources.Views.Account.Account.ChangePasswordTitle %></h2>
    <p>
        <%:Resources.Views.Account.Account.FormToChangePasswordTitle %>
    </p>
    <p>
        <%:String.Format(Resources.Views.Account.Account.PasswordsLength, ViewData["PasswordLength"])%>
    </p>
    <% using (Html.BeginForm())
       { %>
    <%: Html.ValidationSummary(true, Resources.Views.Account.Account.ChangePasswordUnsuccessfulMessage)%>
    <div>
        <fieldset>
            <legend>
                <%:Resources.Views.Account.Account.AccountInformationTitle %></legend>
            <div class="editor-label">
                <%: Html.LabelFor(m => m.OldPassword) %>
            </div>
            <div class="editor-field">
                <%: Html.PasswordFor(m => m.OldPassword) %>
                <%: Html.ValidationMessageFor(m => m.OldPassword) %>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(m => m.NewPassword) %>
            </div>
            <div class="editor-field">
                <%: Html.PasswordFor(m => m.NewPassword) %>
                <%: Html.ValidationMessageFor(m => m.NewPassword) %>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(m => m.ConfirmPassword) %>
            </div>
            <div class="editor-field">
                <%: Html.PasswordFor(m => m.ConfirmPassword) %>
                <%: Html.ValidationMessageFor(m => m.ConfirmPassword) %>
            </div>
            <p>
                <input type="submit" value="<%:Resources.Shared.Buttons.Function.ChangePassword %>" />
            </p>
        </fieldset>
    </div>
    <% } %>
</asp:Content>
