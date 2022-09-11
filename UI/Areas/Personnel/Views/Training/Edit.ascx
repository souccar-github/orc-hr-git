<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Personnel.ValueObjects.Training>" %>
<%@ Import Namespace="UI.Areas.Personnel.Helpers" %>
<table>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.CourseName) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.CourseDuration) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.CertificateIssuanceDate)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Status)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.TrainingCenter) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.TrainingCenterLocation) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Notes) %>
        </td>
    </tr>
</table>
<br />
<fieldset class="ParentFieldset">
    <table width="100%">
        <tr>
            <td style="width: 27%; vertical-align: top">
                <%: Html.HiddenFor(model => model.Id) %>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.CourseName) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.CourseName) %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.CourseDuration) %>
                    / Hours
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().IntegerTextBox().Name("CourseDuration").MinValue(1).MaxValue(900)%>
                </div>
            </td>
            <td style="width: 23%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.CertificateIssuanceDate)%>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DatePicker().Name("CertificateIssuanceDate").Value(Model.CertificateIssuanceDate.Date).Min(DateTime.MinValue)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Status) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.Status.Id)
                                      
                                      .BindTo(PersonnelDropDownListHelpers.ListOfStatuses)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                    %>
                </div>
            </td>
            <td style="width: 27%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.TrainingCenter) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.TrainingCenter) %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.TrainingCenterLocation) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextAreaFor(model => model.TrainingCenterLocation) %>
                </div>
            </td>
            <td style="width: 23%; vertical-align: top">
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
