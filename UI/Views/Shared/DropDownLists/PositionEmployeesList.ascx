<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="UI.Helpers" %>
<fieldset style="background-color: blueviolet; height: auto; width: 30px">
    <%:Html.Telerik().ComboBox()
                                  .Name("positionEmployees")
                                  .AutoFill(true)
                                  .BindTo(DropDownListHelpers.ListOfSelectedPositionEmployees(0))
                                  .HtmlAttributes(new {style = string.Format("width:{0}px", 300)})
                                  .Filterable(filtering => filtering.FilterMode(AutoCompleteFilterMode.Contains))
                                  .HighlightFirstMatch(true)
                                  .SelectedIndex(-1)
                                  .ClientEvents(events => events.OnClose("positionEmployeesEvent"))
    %>
</fieldset>
