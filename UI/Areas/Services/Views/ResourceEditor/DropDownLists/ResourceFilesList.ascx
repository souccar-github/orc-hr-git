<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div class="editor-label-required">
    <%: Html.Label(Resources.Areas.Services.ResourceEditor.ResourceEditorModel.ResourceFileField)%>
</div>
<div class="editor-field">
    <%= Html.Telerik().ComboBox()
                .Name("AutoCompleteRsourceFileNamesComboBox")
                .AutoFill(true)
                .HtmlAttributes(new { style = string.Format("width:{0}px", 210) })
                .Filterable(filtering => filtering.FilterMode(AutoCompleteFilterMode.Contains))
                .HighlightFirstMatch(true)
                .ClientEvents(events => events.OnChange("rsourceFileNamesValueChanged"))
    %>
</div>
