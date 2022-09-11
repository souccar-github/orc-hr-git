<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<table width="100%">
    <tr>
        <td style="width: 1px; vertical-align: middle">
            <a href="<%= Url.Action("Index", "PMSComprehensive", new { area = "PMSComprehensive" }) %>">
                <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/120.png") %>" title='Home' alt="Home"
                    height="48" width="48" />
            </a>
        </td>
        <td style="width: 1px; vertical-align: middle">
            <div id="FA">
                <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/31.png") %>" onclick="GetAppraisalFunctions()"
                    title='Functions' alt="Functions" height="48" width="48" align="middle" />
                <script type="text/javascript" language="javascript">
                    function GetAppraisalFunctions() {
                        $('#PMSComprehensiveFunctionsArea').load('<%: Url.Action("GetFunctionsPartial", "PMSComprehensive", new { area = "PMSComprehensive" }) %>').fadeOut().fadeIn();
                    }
                </script>
            </div>
        </td>
        <td style="width: 1px; vertical-align: middle">
            <div id="IN">
                <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/5.png") %>" onclick="GetAppraisalIndexes()"
                    title='Indexes' alt="Indexes" height="48" width="48" align="middle" />
                <script type="text/javascript" language="javascript">
                    function GetAppraisalIndexes() {
                        $('#PMSComprehensiveFunctionsArea').load('<%: Url.Action("GetIndexesPartial", "PMSComprehensive", new { area = "PMSComprehensive" }) %>').fadeOut().fadeIn();
                    }
                </script>
            </div>
        </td>
        <td style="vertical-align: middle; width: 90%">
            <div id="PMSComprehensiveFunctionsArea" align="left" style="margin-left: 20px;">
                <% Html.RenderAction("GetLatestSectionPartial", "PMSComprehensive", new { area = "PMSComprehensive" }); %>
            </div>
        </td>
    </tr>
</table>
