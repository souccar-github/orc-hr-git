<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<table width="100%">
    <tr>
        <td style="width: 1px; vertical-align: middle">
            <a href="<%= Url.Action("Index", "ProjectManagement", new { area = "ProjectManagement" }) %>">
                <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/120.png") %>" title='<%:Resources.Shared.Buttons.Ribbon.Home %>' alt="<%:Resources.Shared.Buttons.Ribbon.Home %>"
                    height="48" width="48" />
            </a>
        </td>
        <td style="width: 1px; vertical-align: middle">
            <div id="FA">
                <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/31.png") %>" onclick="GetProjectFunctions()"
                    title="<%:Resources.Shared.Buttons.Ribbon.Functions %>" alt="<%:Resources.Shared.Buttons.Ribbon.Functions %>" height="48" width="48" align="middle" />
                <script type="text/javascript" language="javascript">
                    function GetProjectFunctions() {
                        $('#ProjectFunctionsArea').load('<%: Url.Action("GetFunctionsPartial", "ProjectManagement", new { area = "ProjectManagement" }) %>').fadeOut().fadeIn();
                    }
                </script>
            </div>
        </td>
        <td style="width: 1px; vertical-align: middle">
            <div id="IN">
                <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/5.png") %>" onclick="GetProjectIndexes()"
                    title="<%:Resources.Shared.Buttons.Ribbon.Indexes %>" alt="<%:Resources.Shared.Buttons.Ribbon.Indexes %>" height="48" width="48" align="middle" />
                <script type="text/javascript" language="javascript">
                    function GetProjectIndexes() {
                        $('#ProjectFunctionsArea').load('<%: Url.Action("GetIndexesPartial", "ProjectManagement", new { area = "ProjectManagement" }) %>').fadeOut().fadeIn();
                    }
                </script>
            </div>
        </td>
        <td style="vertical-align: middle; width: 90%">
            <div id="ProjectFunctionsArea" align="left" style="margin-left: 20px;">
                <% Html.RenderAction("GetLatestSectionPartial", "ProjectManagement", new { area = "ProjectManagement" }); %>
            </div>
        </td>
    </tr>
</table>
