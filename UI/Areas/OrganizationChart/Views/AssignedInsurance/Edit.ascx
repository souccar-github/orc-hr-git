<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.OrgChart.ValueObjects.AssignedGrade.AssignedInsurance>" %>
<%@ Import Namespace="UI.Areas.OrganizationChart.Helpers" %>
<table border="0" cellpadding="0" cellspacing="0" style="margin-left: 1px">
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.InsuranceNo) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Type) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.InsuranceCompany) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.CompanyAddress)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.ActiveDate) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.ExpiryDate) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.InsuranceCoverageRatio) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.RepresentativeContact) %>
        </td>
    </tr>
</table>
<br />
<fieldset class="ParentFieldset">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="width: 25%; vertical-align: top">
                <%: Html.HiddenFor(model => model.Id) %>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.InsuranceNo) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.InsuranceNo)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Type) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.Type.Id)
                                      .BindTo(DropDownListHelpers.ListOfInsuranceTypes)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                    %>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.InsuranceCompany) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.InsuranceCompany.Id)
                                      
                                      .BindTo(DropDownListHelpers.ListOfInsuranceCompanies)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                    %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.CompanyAddress)%>
                </div>
                <div class="editor-field">
                    <%: Html.TextAreaFor(model => model.CompanyAddress)%>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.ActiveDate)%>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DatePickerFor(model => model.ActiveDate).Value(Model.ActiveDate.Date).Min(DateTime.MinValue)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.ExpiryDate)%>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DatePickerFor(model => model.ExpiryDate).Value(Model.ExpiryDate.Date).Min(DateTime.MinValue)%>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.InsuranceCoverageRatio)%>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().PercentTextBox().Name("InsuranceCoverageRatio").MinValue(0).MaxValue(100)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.RepresentativeContact)%>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.RepresentativeContact)%>
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
