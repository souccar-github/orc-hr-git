<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%: Resources.Areas.OrgChart.Entities.Grade.GradeModel.InsurancesTitle %></legend>
    <div id="ValueObjectsList">
        <% Html.RenderPartial("List"); %>
    </div>
</fieldset>
