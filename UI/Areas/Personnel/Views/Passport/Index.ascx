<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Personnel.ValueObjects.Passport>" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%: Resources.Areas.Personnel.Entities.Employee.EmployeeModel.Passports %></legend>
    <div id="ValueObjectsList">
        <% Html.RenderPartial("ExpiredRules"); %>
        <% Html.RenderPartial("List"); %>
    </div>
</fieldset>
