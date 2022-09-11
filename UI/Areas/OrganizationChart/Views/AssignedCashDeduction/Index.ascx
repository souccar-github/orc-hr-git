<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend">
        <%: Resources.Areas.OrgChart.ValueObjects.AssignedGrade.PositionGrade.PositionGradeModel.CashDeductionsTitle %></legend>
    <div id="ValueObjectsList">
        <% Html.RenderPartial("List"); %>
    </div>
</fieldset>
