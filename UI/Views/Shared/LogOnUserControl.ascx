<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
    if (Request.IsAuthenticated)
    {
%>
<%:Resources.Shared.Messages.General.WelcomeBack %>
<b>
    <%:Html.Encode(Page.User.Identity.Name)%></b>! [<%:Html.ActionLink(Resources.Shared.Buttons.Function.LogOff, "LogOff", "Account", new { area = "" }, null)%>]
<%
    }
    else
    {
%>
[<%:Html.ActionLink(Resources.Shared.Buttons.Function.Login, "LogOn", "Account", new { area = "" }, null)%>]
<%
    }
%>
