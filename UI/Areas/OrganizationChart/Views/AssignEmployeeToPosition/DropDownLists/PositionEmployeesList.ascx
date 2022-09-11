<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div class="editor-label-required">
    <%: Html.Label(Resources.Areas.OrgChart.ValueObjects.AssignEmployeeToPosition.AssignEmployeeToPositionModel.Employee)%>
</div>
<div class="editor-field">
    <%= Html.Telerik().ComboBox()
                .Name("AutoCompleteEmployeesComboBox")
                .AutoFill(true)
                .HtmlAttributes(new { style = string.Format("width:{0}px", 210) })
                .DataBinding(binding => binding.Ajax().Select("GetEmployees", "AssignEmployeeToPosition"))
                .Filterable(filtering => filtering.FilterMode(AutoCompleteFilterMode.Contains))
                .HighlightFirstMatch(true)
                .ClientEvents(events => events.OnChange("employeesValueChanged"))
    %>
</div>
