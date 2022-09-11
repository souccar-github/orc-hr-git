<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<fieldset>
    <div class="editor-label">
        Owner Name
    </div>
    <%:Html.Telerik().ComboBox()
                                  .Name("positionEmployees")
                                  .AutoFill(true)
                                  .BindTo(UI.Helpers.DropDownListHelpers.ListOfSelectedPositionEmployees(0))
                                  .HtmlAttributes(new {style = string.Format("width:{0}px", 300)})
                                  .Filterable(filtering => filtering.FilterMode(AutoCompleteFilterMode.Contains))
                                  .HighlightFirstMatch(true)
                                  .SelectedIndex(0)
                                  .ClientEvents(events => events.OnClose("positionEmployeesEvent"))
    %>
</fieldset>
