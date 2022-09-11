<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Personnel.ValueObjects.Dependent>" %>
<%@ Import Namespace="UI.Areas.Personnel.Helpers" %>
<table>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Id) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.FirstName) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.LastName) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.PlaceOfBirth)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.DateOfBirth) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Nationality) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.ContactNumber) %>
        </td>
    </tr>
</table>
<fieldset class="ParentFieldset">
    <table width="100%">
        <tr>
            <td style="width: 33%; vertical-align: top">
                <%: Html.HiddenFor(model => model.Id) %>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.FirstName) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.FirstName) %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.LastName) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.LastName) %>
                </div>
            </td>
            <td style="width: 33%; vertical-align: top">
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
                    <%: Html.Telerik().DatePickerFor(model => model.DateOfBirth).Value(Model.DateOfBirth.Date).Min(DateTime.MinValue)%>
                </div>
            </td>
            <td style="width: 33%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Nationality) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.Nationality.Id)
                                       .BindTo(PersonnelDropDownListHelpers.ListOfNationalities)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                    %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.ContactNumber) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.ContactNumber) %>
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
