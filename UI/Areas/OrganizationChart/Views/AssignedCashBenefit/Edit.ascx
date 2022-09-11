<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.OrgChart.ValueObjects.AssignedGrade.AssignedCashBenefit>" %>
<%@ Import Namespace="UI.Areas.OrganizationChart.Helpers" %>
<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Type) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Occurrence) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Status) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.EmployeePaymentAmount) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.CompanyPaymentAmount) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.CompanyPaymentRatio) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.CompanyDeductionRatio) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.ActiveDate) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.InactiveDate) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Description) %>
        </td>
    </tr>
</table>
<fieldset class="ParentFieldset">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="width: 33.3%; vertical-align: top">
                <div class="editor-label">
                    <%: Html.HiddenFor(model => model.Id) %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Type) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.Type.Id)
                                      .BindTo(DropDownListHelpers.ListOfCashBenefitTypes)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                    %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Occurrence) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.Occurrence.Id)
                                      .BindTo(DropDownListHelpers.ListOfTimeIntervals)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                    %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Status) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.Status.Id)
                                      .BindTo(UI.Areas.Personnel.Helpers.PersonnelDropDownListHelpers.ListOfStatuses)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                    %>
                </div>
            </td>
            <td style="width: 33.3%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.EmployeePaymentAmount) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().NumericTextBox().Name("EmployeePaymentAmount").MinValue(0).MaxValue(float.MaxValue)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.CompanyPaymentAmount) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().NumericTextBox().Name("CompanyPaymentAmount").MinValue(0).MaxValue(float.MaxValue)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.CompanyPaymentRatio) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().PercentTextBox().Name("CompanyPaymentRatio").MinValue(0).MaxValue(int.MaxValue)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.CompanyDeductionRatio) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().PercentTextBox().Name("CompanyDeductionRatio").MinValue(1).MaxValue(100)%>
                </div>
            </td>
            <td style="width: 33.3%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.ActiveDate) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DatePickerFor(model => model.ActiveDate).Value(Model.ActiveDate.Date).Min(DateTime.MinValue)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.InactiveDate) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DatePickerFor(model => model.InactiveDate).Value(Model.InactiveDate.Date).Min(DateTime.MinValue)%>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.Description) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextAreaFor(model => model.Description) %>
                </div>
            </td>
        </tr>
    </table>
</fieldset>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td style="width: 100%; vertical-align: top" align="right">
            <input type="image" value="Save" src="<%= Url.Content("~/Content/Ribbon/Icons/48/37.png") %>"
                title=<%: Resources.Shared.Buttons.Function.Save %> alt=<%: Resources.Shared.Buttons.Function.Save %> height="24" width="24" align="middle" />
        </td>
    </tr>
</table>
