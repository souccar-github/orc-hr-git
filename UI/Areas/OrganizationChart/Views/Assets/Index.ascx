<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend">
        <%: Resources.Areas.OrgChart.Entities.Grade.GradeModel.AssetTitle %></legend>
    <div id="ValueObjectsList">
        <% Html.RenderPartial("List"); %>
    </div>
</fieldset>
