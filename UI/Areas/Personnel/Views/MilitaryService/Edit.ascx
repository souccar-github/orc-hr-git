<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Personnel.ValueObjects.MilitaryService>" %>
<%@ Import Namespace="UI.Areas.Personnel.Helpers" %>
<table>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Id)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.MilitiryServiceNo)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.ServiceStartDate)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Months)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Days)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Notes)%>
        </td>
    </tr>
</table>
<fieldset class="ParentFieldset">
    <table width="100%">
        <tr>
            <td style="width: 20%; vertical-align: top">
                <%: Html.HiddenFor(model => model.Id) %>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.MilitiryServiceNo) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.MilitiryServiceNo)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.ServiceStartDate) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DatePickerFor(model => model.ServiceStartDate).Value(Model.ServiceStartDate.Date).Min(DateTime.MinValue)%>
                </div>
            </td>
            <td style="width: 20%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.Label("Duration M/D") %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Months) %>
                </div>
                <div class="editor-field" style="width: 10">
                    <%: Html.Telerik().IntegerTextBox().Name("Months").MinValue(1).MaxValue(50).EmptyMessage("Enter Value") %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Days) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().IntegerTextBox().Name("Days").MinValue(0).MaxValue(31)%>
                </div>
            </td>
            <td style="width: 20%; vertical-align: top">
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
