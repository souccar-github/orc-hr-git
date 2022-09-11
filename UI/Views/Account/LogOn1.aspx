<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<UI.Models.LogOnModel>" %>

<asp:Content ContentPlaceHolderID="TitleContent" ID="loginTitle" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" ID="loginContent" runat="server">
    <script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>"
        type="text/javascript"></script>
    <% using (Html.BeginForm())
       { %>
    <%: Html.ValidationSummary(true, Resources.Views.Account.Account.LoginUnsuccessful)%>
    <div>
        <fieldset>
            <legend>
                <%:Resources.Views.Account.Account.AccountInformationTitle %></legend>          
            <table>
                <tr>
                    <td>
                        <div class="editor-label">
                            <%: Html.LabelFor(m => m.UserName) %>
                        </div>
                        <div class="editor-field">
                            <%: Html.TextBoxFor(m => m.UserName) %>
                            <%: Html.ValidationMessageFor(m => m.UserName) %>
                        </div>
                        <div class="editor-label">
                            <%: Html.LabelFor(m => m.Password) %>
                        </div>
                        <div class="editor-field">
                            <%: Html.PasswordFor(m => m.Password) %>
                            <%: Html.ValidationMessageFor(m => m.Password) %>
                        </div>
                        <div class="editor-label">
                            <%: Html.CheckBoxFor(m => m.RememberMe) %>
                            <%: Html.LabelFor(m => m.RememberMe) %>
                        </div>
                    </td>
                    <td>
                        <p>
                            <input type="submit" value="<%:Resources.Shared.Buttons.Function.Login %>" />
                        </p>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <% } %>
</asp:Content>
