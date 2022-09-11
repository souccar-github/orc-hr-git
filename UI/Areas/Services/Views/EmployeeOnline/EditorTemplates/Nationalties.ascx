<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<%= Html.Telerik().DropDownList()
            .Name("Nationalties")
                .BindTo(new SelectList((IEnumerable)ViewData["nationalties"], "Id", "Name"))
%>