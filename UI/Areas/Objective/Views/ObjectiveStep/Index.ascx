<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%:Resources.Areas.Objective.ValueObjects.ObjectiveStep.ObjectiveStepModel.ActionPlan %></legend>
    <div id="ValueObjectsList">
        <% Html.RenderPartial("ExpiredRules"); %>
        <% Html.RenderPartial("List"); %>
    </div>
</fieldset>
