<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.PMS.ValueObjects.Implementation.JobDescription.JobDescriptionSectionItemKpi>" %>

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
            <td style="width: 50%; vertical-align: top">
                <%: Html.HiddenFor(model => model.Id) %>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Value) %>
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.Value)%>
                </div>
            </td>
            <td style="width: 50%; vertical-align: top">
                <div class="editor-label">
                    <%:Html.LabelFor(model => model.Description)%>
                </div>
                <div class="editor-field">
                    <%:Html.TextAreaFor(model => model.Description)%>
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
