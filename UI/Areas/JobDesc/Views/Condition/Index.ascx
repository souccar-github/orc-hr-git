<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%: Resources.Areas.JobDesc.ValueObjects.WorkingCondition.WorkingConditionModel.ConditionTitle %></legend>
    
    <div id="ValueObjectsList"> 
        <% Html.RenderPartial("List"); %>
    </div>
</fieldset>
