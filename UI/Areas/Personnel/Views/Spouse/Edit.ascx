<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Personnel.ValueObjects.Spouse>" %>
<%@ Import Namespace="UI.Areas.Personnel.Helpers" %>
<table>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Id)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.FirstName)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.LastName)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Nationality)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.PlaceOfBirth)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.DateOfBirth)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.ResidencyNo)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.ResidencyExpiryDate)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.PassportNo)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.PassportExpiryDate)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.FirstContactNumber)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.SecondContactNumber)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.EMail)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.WorkEMail)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.CompanyName)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.JobTitle)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.WorkAddress)%>
        </td>
    </tr>
</table>
<fieldset class="ParentFieldset">
    <table width="100%">
        <tr>
            <td style="width: 25%; vertical-align: top">
                <%: Html.HiddenFor(model => model.Id) %>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.FirstName) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.FirstName, new { TabIndex = 1 })%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.LastName) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.LastName, new { TabIndex = 2 })%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.PlaceOfBirth) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.PlaceOfBirth.Id)
                                      .BindTo(PersonnelDropDownListHelpers.ListOfCountries)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                    %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.DateOfBirth) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DatePickerFor(model => model.DateOfBirth).HtmlAttributes(new { TabIndex = 5 }).Value(Model.DateOfBirth.Date).Min(DateTime.MinValue)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Nationality) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.Nationality.Id)
                                      .BindTo(PersonnelDropDownListHelpers.ListOfNationalities)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                    %>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.ResidencyNo) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.ResidencyNo,new { TabIndex = 6 })%>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.ResidencyExpiryDate) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DatePickerFor(model => model.ResidencyExpiryDate).HtmlAttributes(new { TabIndex = 7 }).Value(Model.ResidencyExpiryDate.Date).Min(DateTime.MinValue)%>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.PassportNo) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.PassportNo, new { TabIndex = 8 })%>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.PassportExpiryDate) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DatePickerFor(model => model.PassportExpiryDate).HtmlAttributes(new { TabIndex = 9 }).Value(Model.PassportExpiryDate.Date).Min(DateTime.MinValue)%>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.FirstContactNumber) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.FirstContactNumber, new { TabIndex = 10 })%>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.SecondContactNumber) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.SecondContactNumber, new { TabIndex = 11 })%>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.EMail) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.EMail, new { TabIndex = 12 })%>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.WorkEMail) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.WorkEMail, new { TabIndex = 13 })%>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.CompanyName) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.CompanyName, new { TabIndex = 14 })%>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.JobTitle) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.JobTitle, new { TabIndex = 15 })%>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.WorkAddress) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextAreaFor(model => model.WorkAddress, new { TabIndex = 16 })%>
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
