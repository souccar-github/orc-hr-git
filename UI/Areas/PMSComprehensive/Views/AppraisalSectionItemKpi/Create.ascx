<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.PMS.Entities.Template.AppraisalSectionItemKpi>" %>
<%  using (
        Ajax.BeginForm(Model != null && !Model.IsTransient() ? "JsonEdit" : "JsonCreate", "AppraisalSectionItemKpi",
                       new AjaxOptions
                           {
                               OnComplete = "JsonSaveItemKpi_OnComplete",
                               HttpMethod = "POST"
                           }))
    {%>
<table>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Value) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Description) %>
        </td>
    </tr>
</table>
<fieldset class="ParentFieldset">
    <table width="100%">
        <tr>
            <%: Html.HiddenFor(model => model.Id) %>
            <td style="width: 50%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Value) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().NumericTextBox().Name("Value").MinValue(0).MaxValue(100) %>
                </div>
            </td>
            <td style="width: 50%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Description) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextAreaFor(model => model.Description) %>
                </div>
            </td>
        </tr>
    </table>
</fieldset>
<table width="100%">
    <tr>
        <td style="width: 100%; vertical-align: top" align="right">
            <input type="submit" value="<%: Resources.Shared.Buttons.Function.Save %>" onclick="DisableSaveButton()" />
        </td>
    </tr>
</table>
<% } %>