<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<ul id="myMenu" class="contextMenu">
    <li class="add"><a href="#add"><%:Resources.Areas.OrgChart.ValueObjects.Node.Buttons.AddSubNode %></a></li>
    <li class="edit"><a href="#edit"><%:Resources.Areas.OrgChart.ValueObjects.Node.Buttons.EditNode %></a></li>
    <li class="delete"><a href="#delete"><%:Resources.Areas.OrgChart.ValueObjects.Node.Buttons.DeleteNode %></a></li>
    <li class="edit"><a href="#addPos"><%:Resources.Areas.OrgChart.ValueObjects.Node.Buttons.AddEditPosition %></a></li>
    <li class="quit"><a href="#showPos"><%:Resources.Areas.OrgChart.ValueObjects.Node.Buttons.ShowPositions %></a></li>
</ul>
