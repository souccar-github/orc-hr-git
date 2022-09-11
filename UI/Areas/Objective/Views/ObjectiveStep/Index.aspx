<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<fieldset class="ParentFieldset">
    <legend class="ParentLegend">Objective Step</legend>
    <div id="ValueObjectsList">
        <% Html.RenderPartial("List"); %>
    </div>
</fieldset>
