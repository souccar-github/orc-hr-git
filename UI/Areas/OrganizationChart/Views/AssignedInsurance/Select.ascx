<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.OrgChart.ValueObjects.AssignedGrade.AssignedInsurance>" %>
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
    <% foreach (var item in (IEnumerable<HRIS.Domain.OrgChart.ValueObjects.AssignedGrade.AssignedInsurance>)ViewData["ValueObjectsList"])
       { %>
    <tr>
        <td style="width: 33%; vertical-align: top">
            <%: Html.HiddenFor(model => model.Id) %>
            <div class="display-label">
                <%: Html.LabelFor(model => model.InsuranceNo) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.InsuranceNo, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Type) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Type.Name)%>
            </div>
        </td>
        <td style="width: 33%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.InsuranceCompany) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.InsuranceCompany.Name, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.CompanyAddress) %>
            </div>
            <div class="display-field">
                <%: Html.TextAreaFor(model => item.CompanyAddress, new { @readonly = true, @class = "MultiLineInputSelectMode" })%>
            </div>
        </td>
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.ActiveDate) %>
            </div>
            <div class="display-field">
                <%: Html.Encode(String.Format("{0:MM/dd/yyyy}", item.ActiveDate))%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.ExpiryDate) %>
            </div>
            <div class="display-field">
                <%: Html.Encode(String.Format("{0:MM/dd/yyyy}", item.ExpiryDate))%>
            </div>
        </td>
        <td style="width: 33%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.InsuranceCoverageRatio) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.InsuranceCoverageRatio)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.RepresentativeContact) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.RepresentativeContact, new { @readonly = true, @class = "SingleLine" })%>
            </div>
        </td>
        <td align="right">
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <%: Ajax.ActionLink(Resources.Shared.Buttons.Function.Edit, "JsonEdit", "AssignedInsurance", new { Id = item.Id }, new AjaxOptions { OnComplete = "JsonEdit_OnComplete" })%>
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
