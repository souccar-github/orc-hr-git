<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div id="Error">
    <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/65.png") %>" onclick="GetIndexes()"
        title='Indexes' alt="Indexes" height="36" width="36" />
    <%: TempData["Error"]%>
</div>
