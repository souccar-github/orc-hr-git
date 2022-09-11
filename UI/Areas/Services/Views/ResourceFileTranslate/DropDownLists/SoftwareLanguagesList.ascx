<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div class="editor-label-required">
    <%: Html.Label(Resources.Areas.Services.ResourceFileTranslate.ResourceFileTranslateModel.SoftwareLanguageField)%>
</div>
<div class="editor-field">
    <%= Html.Telerik().ComboBox()
                        .Name("AutoCompleteSoftwareLanguagesComboBox")
                .AutoFill(true)
                .HtmlAttributes(new { style = string.Format("width:{0}px", 210) })
                                .DataBinding(binding => binding.Ajax().Select("GetSoftwareLanguages", "ResourceFileTranslate"))
                .Filterable(filtering => filtering.FilterMode(AutoCompleteFilterMode.Contains))
                .HighlightFirstMatch(true)
                        .ClientEvents(events => events.OnChange("softwareLanguagesValueChanged"))
    %>
</div>
