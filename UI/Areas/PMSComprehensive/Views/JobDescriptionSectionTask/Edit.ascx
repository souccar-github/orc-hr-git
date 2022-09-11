<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.PMS.ValueObjects.Implementation.JobDescription.JobDescriptionSectionTask>" %>
<table>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.RoleName) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.JobTask) %>
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
                <%: Html.HiddenFor(model => model.RoleName) %>
                <%: Html.HiddenFor(model => model.JobTask) %>
                <%: Html.HiddenFor(model => model.Comment) %>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.RoleName) %>
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.RoleName)%>
                </div>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.JobTask) %>
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.JobTask)%>
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
            </td>
            <td style="width: 33.3%; vertical-align: top">
                <div class="editor-label">
                    <%:Html.LabelFor(model => model.Comment)%>
                </div>
                <div class="editor-field">
                    <%:Html.TextAreaFor(model => model.Comment)%>
                </div>
                <%--<div class="display-label">
                    <%:Html.LabelFor(model => model.Description)%>
                </div>
                <div class="display-field">
                    <%:Html.TextAreaFor(model => model.Description, new ReadOnlyTextBox(true, "MultiLine"))%>
                </div>--%>
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
