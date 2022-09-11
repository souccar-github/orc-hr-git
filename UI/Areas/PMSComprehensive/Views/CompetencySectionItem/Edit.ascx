<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.PMS.ValueObjects.Implementation.Competency.CompetencySectionItem>" %>
<%@ Import Namespace="UI.Helpers.Views" %>
<table>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Name) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Type) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Level) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Weight) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Rate) %>
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
            <td style="width: 33.3%; vertical-align: top">
                <%: Html.HiddenFor(model => model.Id) %>
                <%: Html.HiddenFor(model => model.Name) %>
                <%: Html.HiddenFor(model => model.Type) %>
                <%: Html.HiddenFor(model => model.Level) %>
                <%: Html.HiddenFor(model => model.Description) %>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Name) %>
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.Name)%>
                </div>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Type) %>
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.Type)%>
                </div>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Level) %>
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.Level)%>
                </div>
            </td>
            <td style="width: 33.3%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Weight) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().PercentTextBox().Name("Weight").MinValue(0).MaxValue(100)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Rate) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().PercentTextBox().Name("Rate").MinValue(0).MaxValue(100)%>
                </div>
                <div class="editor-label">
                    <%:Html.LabelFor(model => model.Comment)%>
                </div>
                <div class="editor-field">
                    <%:Html.TextAreaFor(model => model.Comment)%>
                </div>
            </td>
            <td style="width: 33.3%; vertical-align: top">
                <div class="display-label">
                    <%:Html.LabelFor(model => model.Description)%>
                </div>
                <div class="display-field">
                    <%:Html.TextAreaFor(model => model.Description, new ReadOnlyTextBox(true, "MultiLine"))%>
                </div>
            </td>
        </tr>
    </table>
</fieldset>
<table width="100%">
    <tr>
        <td style="width: 10%; vertical-align: top">
            <input type="submit" value="Save" onclick="DisableSaveButton()" />
        </td>
    </tr>
</table>
