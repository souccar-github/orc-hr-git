<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div id="ErrorDiv" style="color: Red;">
    <%: TempData["Error"]%>
    <br />
    <br />
</div>
