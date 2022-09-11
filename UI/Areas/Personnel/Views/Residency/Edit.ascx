<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Personnel.ValueObjects.Residency>" %>
<%@ Import Namespace="UI.Areas.Personnel.Helpers" %>
<table>
    <tr>
        <td style="color: Red">
            <%: Html.ValidationMessageFor(model => model.No) %>
        </td>
    </tr>
    <tr>
        <td style="color: Red">
            <%: Html.ValidationMessageFor(model => model.FirstName) %>
        </td>
    </tr>
    <tr>
        <td style="color: Red">
            <%: Html.ValidationMessageFor(model => model.MiddleName) %>
        </td>
    </tr>
    <tr>
        <td style="color: Red">
            <%: Html.ValidationMessageFor(model => model.LastName) %>
        </td>
    </tr>
    <tr>
        <td style="color: Red">
            <%: Html.ValidationMessageFor(model => model.MotherName) %>
        </td>
    </tr>
    <tr>
        <td style="color: Red">
            <%: Html.ValidationMessageFor(model => model.Nationality) %>
        </td>
    </tr>
    <tr>
        <td style="color: Red">
            <%: Html.ValidationMessageFor(model => model.Type) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.IssuanceDate) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.ExpiryDate) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Tel) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Address) %>
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
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label">
                    <%: Html.HiddenFor(model => model.Id) %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.No) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.No)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.FirstName) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.FirstName) %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.MiddleName) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.MiddleName) %>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.LastName) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.LastName) %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.MotherName) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.MotherName) %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Nationality) %>
                </div>
                <div class="editor-field-required">
                    <%: Html.Telerik().DropDownListFor(model => model.Nationality.Id)
                                     
                                       .BindTo(PersonnelDropDownListHelpers.ListOfNationalities)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                      
                    %>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Type) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.Type.Id)
                                     
                                       .BindTo(PersonnelDropDownListHelpers.ListOfResidencyTypes)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                      
                    %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.IssuanceDate) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DatePickerFor(model => model.IssuanceDate).Value(Model.IssuanceDate.Date).Min(DateTime.MinValue)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.ExpiryDate) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DatePickerFor(model => model.ExpiryDate).Value(Model.ExpiryDate.Date).Min(DateTime.MinValue)%>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.Tel) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.Tel) %>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.Address) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextAreaFor(model => model.Address)%>
                </div>
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
