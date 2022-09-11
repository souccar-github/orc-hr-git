<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="Infrastructure.Validation" %>
<div style="color: Maroon; font-size: smaller;">
    <%
        if (ViewData["ExpiredRules"] != null)
        {
            foreach (BrokenBusinessRule brokenBusinessRule in (IList<BrokenBusinessRule>)ViewData["ExpiredRules"])
            {%>
    <%BrokenBusinessRule rule = brokenBusinessRule;%><%:Html.DisplayTextFor(model => rule.Rule)%>
    <br />
    <%
            }
        }%>
    <br />
</div>
