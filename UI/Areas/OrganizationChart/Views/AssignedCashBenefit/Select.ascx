<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.OrgChart.ValueObjects.AssignedGrade.AssignedCashBenefit>" %>
<%@ Import Namespace="HRIS.Domain.OrgChart.ValueObjects" %>
<%@ Import Namespace="Infrastructure.Validation" %>
<div style="color: Maroon; font-size: smaller;">
    <%
        if (ViewData["ExpiredRules"] != null)
        {
            foreach (BrokenBusinessRule brokenBusinessRule in ViewData["ExpiredRules"] as IList<BrokenBusinessRule>)
            {%>
    <%:Html.DisplayTextFor(model => brokenBusinessRule.Rule)%>
    <br />
    <%
        }
        }%>
    <br />
</div>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <% foreach (var item in (IEnumerable<HRIS.Domain.OrgChart.ValueObjects.AssignedGrade.AssignedCashBenefit>)ViewData["ValueObjectsList"])
       { %>
    <tr style="border-width: 11; border-color: Black; border-spacing: 11">
        <td style="width: 33.3%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Type) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Type.Name)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Occurrence) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Occurrence.Name)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Status) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Status.Name)%>
            </div>
        </td>
        <td style="width: 33.3%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.EmployeePaymentAmount) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.EmployeePaymentAmount)%>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.CompanyPaymentAmount) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.CompanyPaymentAmount) %>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.CompanyPaymentRatio) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.CompanyPaymentRatio)%>
                %
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.CompanyDeductionRatio) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.CompanyDeductionRatio)%>
                %
            </div>
        </td>
        <td style="width: 33.3%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.ActiveDate) %>
            </div>
            <div class="display-field">
                <%: Html.Encode(String.Format("{0:MM/dd/yyyy}", item.ActiveDate))%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.InactiveDate) %>
            </div>
            <div class="display-field">
                <%: Html.Encode(String.Format("{0:MM/dd/yyyy}", item.InactiveDate))%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Description) %>
            </div>
            <div class="display-field">
                <%: Html.TextAreaFor(model => item.Description, new { @readonly = true, @class = "MultiLine" })%>
            </div>
        </td>
        <td align="right">
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <%: Ajax.ActionLink(Resources.Shared.Buttons.Function.Edit, "JsonEdit", "AssignedCashBenefit", new { Id = item.Id }, new AjaxOptions { OnComplete = "JsonEdit_OnComplete" })%>
            <% } %>
        </td>
    </tr>
    <% } %>
</table>
<script type="text/javascript">

    function JsonEdit_OnComplete(context) {

        var JsonEdit = context.get_response().get_object();
        if (JsonEdit.Success) {
            $("#addValueObjectArea").html(JsonEdit.PartialViewHtml);
            Toggle("edit");
        }
        else {
            $("#ValueObjectsList").html(JsonAdd.PartialViewHtml);
            $("#addValueObjectArea").slideToggle("fast");
        }
    };

</script>
