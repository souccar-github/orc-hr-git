<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.PMS.Entities.AppraisalSectionItem>" %>
<%@ Import Namespace="UI.Areas.PMSComprehensive.Helpers" %>
  <%  using (
        Ajax.BeginForm(Model != null && !Model.IsTransient() ? "JsonEdit" : "JsonCreate", "AppraisalSectionItem",
                       new AjaxOptions
                           {
                               OnComplete = "JsonSaveItem_OnComplete",
                               HttpMethod = "POST"
                           }))
    {%>

<table>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Name) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Weight) %>
        </td>
    </tr>
</table>
<fieldset class="ParentFieldset">
    <legend>Appraisal Section Item</legend>
    <table width="100%">
        <tr>
            <%: Html.HiddenFor(model => model.Id) %>
            <td style="width: 33%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Name) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model=>model.Name)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Weight) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().NumericTextBox().Name("Weight").MinValue(0).MaxValue(float.MaxValue)%>
                </div>
            </td>
            <td style="width: 33%; vertical-align: top">
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.Description) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextAreaFor(model => model.Description)%>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <input type="submit" value="<%:Resources.Shared.Buttons.Function.Save %>" />
            </td>
        </tr>
    </table>
</fieldset>
<% } %>
