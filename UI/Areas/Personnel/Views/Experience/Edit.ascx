<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Personnel.ValueObjects.Experience>" %>
<%@ Import Namespace="UI.Areas.Personnel.Helpers" %>
<table>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.JobTitle) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Industry) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.StartDate)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.EndDate)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.LeaveReason) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.ReferenceFullName) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.ReferenceJobTitle) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.ReferenceContact) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Notes) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.CompanyWebSite) %>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.ReferenceEMail) %>
        </td>
    </tr>
</table>
<fieldset class="ParentFieldset">
    <table width="100%">
        <tr>
            <td style="width: 25%; vertical-align: top">
                <%: Html.HiddenFor(model => model.Id) %>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.JobTitle) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.JobTitle) %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.CompanyName) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.CompanyName) %>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.CompanyLocation) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.CompanyLocation) %>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.CompanyWebSite) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.CompanyWebSite) %>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Industry) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.Industry) %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.StartDate) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DatePicker().Name("StartDate").Value(Model.StartDate.Date).Min(DateTime.MinValue)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.EndDate) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DatePicker().Name("EndDate").Value(Model.EndDate.Date).Min(DateTime.MinValue)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.LeaveReason) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.LeaveReason) %>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.ReferenceFullName) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.ReferenceFullName) %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.ReferenceJobTitle) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.ReferenceJobTitle) %>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.ReferenceContact) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.ReferenceContact) %>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.ReferenceEMail) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.ReferenceEMail) %>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.Notes) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextAreaFor(model => model.Notes) %>
                </div>
            </td>
        </tr>
    </table>
</fieldset>
<table>
    <tr>
        <td>
            <input type="submit" value="<%: Resources.Shared.Buttons.Function.Save %>" onclick=" DisableSaveButton(); " />
        </td>
    </tr>
</table>
