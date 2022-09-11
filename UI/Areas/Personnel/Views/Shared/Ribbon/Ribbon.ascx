<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<table width="100%">
    <tr>
        <td style="width: 1px; vertical-align: middle">
            <a href="<%= Url.Action("Index", "Personnel", new { area = "Personnel" }) %>">
                <img alt="<%:Resources.Shared.Buttons.Ribbon.Home %>" height="48" src="<%= Url.Content("~/Content/Ribbon/Icons/48/120.png") %>"
                    title='<%:Resources.Shared.Buttons.Ribbon.Home %>' width="48" />
            </a>
        </td>
        <td style="width: 1px; vertical-align: middle">
            <div id="FA">
                <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/31.png") %>" onclick="GetPersonnelFunctions()"
                    title='Functions' alt="Functions" height="48" width="48" align="middle" />
                <script type="text/javascript" language="javascript">
                    function GetPersonnelFunctions() {
                        $('#PersonnelFunctionsArea').load('<%: Url.Action("GetFunctionsPartial", "Personnel", new { area = "Personnel" }) %>').fadeOut().fadeIn();
                    }
                </script>
            </div>
        </td>
        <td style="width: 1px; vertical-align: middle">
            <div id="IN">
                <img alt="Indexes" height="48" onclick="GetPersonnelIndexes()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/5.png") %>"
                    title='Indexes' width="48" />
                <script type="text/javascript" language="javascript">
                    function GetPersonnelIndexes() {
                        $('#PersonnelFunctionsArea').load('<%: Url.Action("GetIndexesPartial", "Personnel", new { area = "Personnel" }) %>').fadeOut().fadeIn();
                    }
                </script>
            </div>
        </td>
        <td style="vertical-align: middle; width: 90%">
            <div id="PersonnelFunctionsArea" align="left" style="margin-left: 20px;">
                <% Html.RenderAction("GetLatestSectionPartial", "Personnel", new { area = "Personnel" }); %>
            </div>
        </td>
    </tr>
</table>
