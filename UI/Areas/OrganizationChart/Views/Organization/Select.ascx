<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.OrgChart.Entities.Organization>" %>
<%@ Import Namespace="Infrastructure.Validation" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%:Resources.Areas.OrgChart.Entities.Organization.OrganizationModel.OrganizationDetailsTitles %></legend>
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
            }
        %>
        <br />
    </div>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td align="left">
                <% if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                   {%>
                <%: Ajax.ActionLink(Resources.Shared.Buttons.Function.Delete, "Delete", new AjaxOptions { Confirm = Resources.Shared.Messages.General.DeleteConfirm, OnComplete = "JsonDelete_OnComplete" })%>
                <% } %>
                <script type="text/javascript">
                    function JsonDelete_OnComplete(context) {

                        var JsonDelete = context.get_response().get_object();
                        if (JsonDelete.Success) {
                            $("#result").html(JsonDelete.PartialViewHtml);
                        }
                    };
                </script>
            </td>
            <td align="right">
                <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
                   {%>
                <input type="button" value=<%:Resources.Shared.Buttons.Function.Edit %> onclick="ShowOrganization()" />
                <% } %>
                <script type="text/javascript">
                    function ShowOrganization() {
                        $('#result').load('<%: Url.Action("Edit", "Organization") %>');
                    }
                </script>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="width: 100%; vertical-align: top">
                <br />
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="width: 20%">
                            <div class="display-label">
                                <%: Html.LabelFor(model => model.Name) %>
                            </div>
                            <div class="display-field">
                                <%: Html.DisplayFor(model => model.Name) %>
                            </div>
                        </td>
                        <td style="width: 20%">
                            <div class="display-label">
                                <%: Html.LabelFor(model => model.MotherCompanyName) %>
                            </div>
                            <div class="display-field">
                                <%: Html.DisplayFor(model => model.MotherCompanyName)%>
                            </div>
                        </td>
                        <td style="width: 33%">
                            <div class="display-label">
                                <%: Html.LabelFor(model => model.Location) %>
                            </div>
                            <div class="display-field">
                                <%: Html.DisplayFor(model => model.Location.Name)%>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="width: 100%; vertical-align: top">
                <br />
                <%: Html.ActionLink(Resources.Areas.OrgChart.Entities.Organization.Buttons.DefineNodeTypes, "Index", "NodeType") %>
            </td>
        </tr>
        <tr>
            <td>
                

            </td>
        </tr>
    </table>
</fieldset>
