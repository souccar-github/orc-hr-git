<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.OrgChart.ValueObjects.Asset>" %>
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
    <% foreach (var item in (IEnumerable<HRIS.Domain.OrgChart.ValueObjects.Asset>)ViewData["ValueObjectsList"])
       { %>
    <tr>
        <td style="width: 20%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.SerialNo) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.SerialNo, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Name) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.Name, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.PurchaseDate) %>
            </div>
            <div class="display-field">
                <%: Html.Encode(String.Format("{0:MM/dd/yyyy}", item.PurchaseDate))%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.ExpiryDate) %>
            </div>
            <div class="display-field">
                <%: Html.Encode(String.Format("{0:MM/dd/yyyy}", item.ExpiryDate))%>
            </div>
        </td>
        <td style="width: 20%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Status) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Status.Name) %>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.CurrencyType) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.CurrencyType.Name) %>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Type) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Type.Name) %>
            </div>
        </td>
        <td style="width: 20%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.UnitCost) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.UnitCost) %>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.DepreciationAmount) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.DepreciationAmount) %>
                %
            </div>
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <div class="display-label">
                            <%: Html.LabelFor(model => model.DepreciationPeriod) %>
                        </div>
                        <div class="display-field">
                            <%: Html.DisplayFor(model => item.DepreciationPeriod)%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="display-label">
                            <%: Html.LabelFor(model => model.Per) %>
                        </div>
                        <div class="display-field">
                            <%: Html.DisplayFor(model => item.Per.Name)%>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
        <td style="width: 20%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.ProductLifeCycle) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.ProductLifeCycle, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Provider) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.Provider, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Description) %>
            </div>
            <div class="display-field">
                <%: Html.TextAreaFor(model => item.Description, new { @readonly = true, @class = "MultiLine" })%>
            </div>
        </td>
        <td align="right">
            <% if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
               {%>
            <%: Ajax.ActionLink(Resources.Shared.Buttons.Function.Delete, "Delete", "Assets", new { Id = item.Id }, new AjaxOptions { Confirm = Resources.Shared.Messages.General.DeleteConfirm, HttpMethod = "Delete", OnComplete = "JsonDelete_OnComplete" })%>
            <% } %>
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <%: Ajax.ActionLink(Resources.Shared.Buttons.Function.Edit, "JsonEdit", "Assets", new { Id = item.Id }, new AjaxOptions { OnComplete = "JsonEdit_OnComplete" })%>
            <% } %>
        </td>
    </tr>
    <% } %>
</table>
<script type="text/javascript">

    function JsonDelete_OnComplete(context) {

        var JsonDelete = context.get_response().get_object();
        if (JsonDelete.Success) {
            $("#ValueObjectsList").html(JsonDelete.PartialViewHtml);
        }
    };

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
