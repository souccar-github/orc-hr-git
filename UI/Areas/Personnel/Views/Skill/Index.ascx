<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%: Resources.Areas.Personnel.Entities.Employee.EmployeeModel.Skills %></legend>
    <div id="ValueObjectsList">
        <% Html.RenderPartial("ExpiredRules"); %>
        <% Html.RenderPartial("List"); %>
    </div>
</fieldset>
