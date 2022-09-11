<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div style="color: Red;">
    <% if (TempData["GlobalError"] != null)
       { %>
    <%: TempData["GlobalError"].ToString()%>
    <% } %>
    
    
    </div>
