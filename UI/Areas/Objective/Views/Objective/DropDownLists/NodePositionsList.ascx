<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="UI.Helpers" %>
<fieldset style="background-color: Yellow; height: auto; width: 300px">
    <%:Html.Telerik().ComboBox()
                                  .Name("nodePositions")
                                  .AutoFill(true)
                                  .BindTo(DropDownListHelpers.ListOfSelectedNodePosition(0))
                                  .HtmlAttributes(new {style = string.Format("width:{0}px", 300)})
                                  .Filterable(filtering => filtering.FilterMode(AutoCompleteFilterMode.Contains))
                                  .HighlightFirstMatch(true)
                                  .SelectedIndex(-1)
                                  .ClientEvents(events => events.OnClose("nodePositionsEvent"))
    %>
</fieldset>
