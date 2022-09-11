<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.PMS.ValueObjects.Implementation.JobDescription.JobDescriptionSectionItem>" %>

<table>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.JobTitle) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Weigth) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Rate) %>
        </td>
    </tr>
</table>
<fieldset class="ParentFieldset">
    <table width="100%">
        <tr>
            <td style="width: 33.3%; vertical-align: top">
                <%: Html.HiddenFor(model => model.Id) %>
                <%: Html.HiddenFor(model => model.JobTitle) %>

                <div class="display-label">
                    <%: Html.LabelFor(model => model.JobTitle) %>
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.JobTitle)%>
                </div>
            </td>
            <td style="width: 33.3%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Weigth) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().PercentTextBox().Name("Weigth").MinValue(0).MaxValue(100)%>
                </div>
            </td>
            <td style="width: 33.3%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Rate) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().PercentTextBox().Name("Rate").MinValue(0).MaxValue(100)%>
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
