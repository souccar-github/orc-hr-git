<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<fieldset class="ParentFieldset">
    <legend class="ParentLegend">Competency Section Item</legend>
    <div id="ValueObjectsList">
        <% Html.RenderPartial("ExpiredRules"); %>
        <% Html.RenderPartial("List"); %>
    </div>
</fieldset>
