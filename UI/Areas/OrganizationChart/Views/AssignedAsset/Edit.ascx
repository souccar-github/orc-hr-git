<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.OrgChart.ValueObjects.AssignedGrade.AssignedAsset>" %>
<%@ Import Namespace="UI.Areas.OrganizationChart.Helpers" %>
<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.SerialNo) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Name) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Type) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.CurrencyType) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Status) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.UnitCost) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.PurchaseDate) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.ExpiryDate) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.ProductLifeCycle) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.DepreciationAmount) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.DepreciationPeriod) %>
        </td>
    </tr>    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Per) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Provider) %>
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
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label">
                    <%: Html.HiddenFor(model => model.Id) %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.SerialNo) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.SerialNo)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Name) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.Name) %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Type) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.Type.Id)
                                      .BindTo(DropDownListHelpers.ListOfAssetType)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                    %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.CurrencyType) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.CurrencyType.Id)
                                      .BindTo(DropDownListHelpers.ListOfCurrencyType)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                    %>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Status) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.Status.Id)
                                      .BindTo(DropDownListHelpers.ListOfAssetStatus)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                    %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.UnitCost) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().NumericTextBox().Name("UnitCost").MinValue(0).MaxValue(Double.MaxValue)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.PurchaseDate) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DatePickerFor(model => model.PurchaseDate).Value(Model.PurchaseDate.Date).Min(DateTime.MinValue)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.ExpiryDate) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DatePickerFor(model => model.ExpiryDate).Value(Model.ExpiryDate.Date).Min(DateTime.MinValue)%>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.ProductLifeCycle) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.ProductLifeCycle) %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.DepreciationAmount) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().PercentTextBox().Name("DepreciationAmount").MinValue(0).MaxValue(100)%>
                </div>
                <table>
                    <tr>
                        <td>
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.DepreciationPeriod) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().NumericTextBox().Name("DepreciationPeriod").MinValue(0).MaxValue(12)
                                .HtmlAttributes(new { style = string.Format("width:{0}px", 50) })
                                %>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="editor-label">
                                <%: Html.LabelFor(model => model.Per) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().DropDownListFor(model => model.Per.Id)

                                      .BindTo(DropDownListHelpers.ListOfTimeIntervals)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 100) })
                                %>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Provider) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.Provider) %>
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
