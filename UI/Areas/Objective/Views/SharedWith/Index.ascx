<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%:Resources.Areas.Objective.ValueObjects.SharedWith.SharedWithModel.SharedWith %></legend>
    <div id="ValueObjectsList">
        <%
            Html.RenderPartial("List");%>
    </div>   
</fieldset>
