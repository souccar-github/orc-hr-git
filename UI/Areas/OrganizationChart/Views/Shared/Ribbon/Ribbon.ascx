<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<table width="100%">
    <tr>
        <td style="width: 1px; vertical-align: middle">
            <a href="<%= Url.Action("Index", "OrganizationChart", new { area = "OrganizationChart" }) %>">
                <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/120.png") %>" title="<%:Resources.Shared.Buttons.Ribbon.Home %>"
                    alt="<%:Resources.Shared.Buttons.Ribbon.Home %>" height="48" width="48" align="middle" />
            </a>
        </td>
        <td style="width: 1px; vertical-align: middle">
            <div id="FA">
                <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/31.png") %>" onclick="GetOrgFunctions()"
                    title="<%:Resources.Shared.Buttons.Ribbon.Functions %>" alt="<%:Resources.Shared.Buttons.Ribbon.Functions %>"
                    height="48" width="48" align="middle" />
                <script type="text/javascript">
                    function GetOrgFunctions() {
                        $('#OrgFunctionsArea').load('<%: Url.Action("GetFunctionsPartial", "OrganizationChart", new { area = "OrganizationChart" }) %>').fadeOut().fadeIn();
                    }
                </script>
            </div>
        </td>
        <td style="width: 1px; vertical-align: middle">
            <div id="IN">
                <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/5.png") %>" onclick="GetOrgIndexes()"
                    title="<%:Resources.Shared.Buttons.Ribbon.Indexes %>" alt="<%:Resources.Shared.Buttons.Ribbon.Indexes %>"
                    height="48" width="48" align="middle" />
                <script type="text/javascript">
                    function GetOrgIndexes() {
                        $('#OrgFunctionsArea').load('<%: Url.Action("GetIndexesPartial", "OrganizationChart", new { area = "OrganizationChart" }) %>').fadeOut().fadeIn();
                    }
                </script>
            </div>
        </td>
        <td align="center" style="vertical-align: middle; width: 90%">
            <div id="OrgFunctionsArea" align="left">
                <% Html.RenderAction("GetLatestSectionPartial", "OrganizationChart", new { area = "OrganizationChart" }); %>
            </div>
        </td>
    </tr>
</table>
