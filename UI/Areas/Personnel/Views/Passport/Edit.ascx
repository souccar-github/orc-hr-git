<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Personnel.ValueObjects.Passport>" %>
<table>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Number) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.FirstName)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.MiddleName)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.LastName)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.MotherName)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.PlaceOfIssuance)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.IssuanceDate)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.ExpiryDate)%>
        </td>
    </tr>
</table>
<br />
<fieldset class="ParentFieldset">
    <table width="100%">
        <tr>
            <td style="width: 25%; vertical-align: top">
                <%: Html.HiddenFor(model => model.Id) %>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Number)%>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.Number) %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.FirstName)%>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.FirstName)%>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.MiddleName)%>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.MiddleName)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.LastName)%>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.LastName)%>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.MotherName)%>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.MotherName)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.PlaceOfIssuance) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.PlaceOfIssuance.Id)
                                     
                                       .BindTo(UI.Areas.Personnel.Helpers.PersonnelDropDownListHelpers.ListOfCountries)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                      
                    %>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
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
