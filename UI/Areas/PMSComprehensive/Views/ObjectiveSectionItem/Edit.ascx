<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.PMS.ValueObjects.Implementation.Objective.ObjectiveSectionItem>" %>
<table>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Rate) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Weight) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Comment) %>
        </td>
    </tr>
</table>
<fieldset class="ParentFieldset">
    <table width="100%">
        <tr>
            <%: Html.HiddenFor(model => model.Id) %>
            <td style="width: 33%; vertical-align: top">
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Name) %>
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.Name)%>
                </div>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.IsShared)%>
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.IsShared)%>
                </div>
            </td>
            <td style="width: 33%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Weight) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().NumericTextBox().Name("Weight").MinValue(0).MaxValue(float.MaxValue)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Rate) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().NumericTextBox().Name("Rate").MinValue(0).MaxValue(float.MaxValue)%>
                </div>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.SharedWithPercentage)%>
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.SharedWithPercentage)%>
                </div>
            </td>
            <td style="width: 33%; vertical-align: top">
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.Comment) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextAreaFor(model => model.Comment)%>
                </div>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Description) %>
                </div>
                <div class="display-field">
                    <%: Html.TextAreaFor(model => model.Description, new { @readonly = true, @class = "MultiLine", @disapled = true })%>
                </div>
            </td>
            <td style="width: 33%; vertical-align: top">
            </td>
        </tr>
    </table>
</fieldset>
<table width="100%">
    <tr>
        <td style="width: 100%; vertical-align: top" align="right">
            <input type="submit" value="Save" onclick="DisableSaveButton()" />
        </td>
    </tr>
</table>
