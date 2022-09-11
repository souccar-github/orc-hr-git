<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<fieldset class="ParentFieldset">
    <legend class="ParentLegend">Job Description Section Item Kpi</legend>
    <div id="ValueObjectsList">
        <% Html.RenderPartial("ExpiredRules"); %>
        <% Html.RenderPartial("List"); %>
    </div>
</fieldset>
