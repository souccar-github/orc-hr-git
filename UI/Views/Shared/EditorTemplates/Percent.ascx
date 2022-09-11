<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<float?>" %>
<%= Html.Telerik().PercentTextBoxFor(m => m)
        .InputHtmlAttributes(new { style = "width:100%" })
%>
