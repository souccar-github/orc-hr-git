<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<table width="100%">
    <tr>
        <td style="width: 1px; vertical-align: middle">
            <a href="<%= Url.Action("Index", "ObjectiveModule", new { area = "Objective" }) %>">
                <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/120.png") %>" title='<%: Resources.Shared.Buttons.Ribbon.Home %>' alt="<%: Resources.Shared.Buttons.Ribbon.Home %>"
                    height="48" width="48" />
            </a>
        </td>
        <td style="width: 1px; vertical-align: middle">
            <div id="FA">
                <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/31.png") %>" onclick="GetObjectiveFunctions()"
                    title='<%: Resources.Shared.Buttons.Ribbon.Functions %>' alt="<%: Resources.Shared.Buttons.Ribbon.Functions %>" height="48" width="48" align="middle" />
                <script type="text/javascript" language="javascript">
                    function GetObjectiveFunctions() {
                        $('#ObjectiveFunctionsArea').load('<%: Url.Action("GetFunctionsPartial", "ObjectiveModule", new { area = "Objective" }) %>').fadeOut().fadeIn();
                    }
                </script>
            </div>
        </td>
        <td style="width: 1px; vertical-align: middle">
            <div id="IN">
                <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/5.png") %>" onclick="GetObjectiveIndexes()"
                    title='<%: Resources.Shared.Buttons.Ribbon.Indexes %>' alt="<%: Resources.Shared.Buttons.Ribbon.Indexes %>" height="48" width="48" align="middle" />
                <script type="text/javascript" language="javascript">
                    function GetObjectiveIndexes() {
                        $('#ObjectiveFunctionsArea').load('<%: Url.Action("GetIndexesPartial", "ObjectiveModule", new { area = "Objective" }) %>').fadeOut().fadeIn();
                    }
                </script>
            </div>
        </td>
        <td style="vertical-align: middle; width: 90%">
            <div id="ObjectiveFunctionsArea" align="left" style="margin-left: 20px;">
                <% Html.RenderAction("GetLatestSectionPartial", "ObjectiveModule", new { area = "Objective" }); %>
            </div>
        </td>
    </tr>
</table>
