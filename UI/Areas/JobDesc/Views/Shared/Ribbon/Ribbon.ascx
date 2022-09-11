<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<table width="100%">
    <tr>
        <td style="width: 1px; vertical-align: middle">
            <a href="<%= Url.Action("Index", "JobDesc", new { area = "JobDesc" }) %>">
                <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/120.png") %>" title='<%:Resources.Shared.Buttons.Ribbon.Home %>' alt="<%:Resources.Shared.Buttons.Ribbon.Home %>"
                    height="48" width="48" align="middle" />
            </a>
        </td>
        <td style="width: 1px; vertical-align: middle">
            <div id="FA">
                <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/31.png") %>" onclick="GetJobFunctions()"
                    title="<%:Resources.Shared.Buttons.Ribbon.Functions %>" alt="<%:Resources.Shared.Buttons.Ribbon.Functions %>" height="48" width="48" align="middle" />
                <script type="text/javascript" language="javascript">
                    function GetJobFunctions() {
                        $('#JobFunctionsArea').load('<%: Url.Action("GetFunctionsPartial", "JobDesc", new { area = "JobDesc" }) %>').fadeOut().fadeIn();
                    }
                </script>
            </div>
        </td>
        <td style="width: 1px; vertical-align: middle">
            <div id="IN">
                <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/5.png") %>" onclick="GetJobIndexes()"
                    title="<%:Resources.Shared.Buttons.Ribbon.Indexes %>" alt="<%:Resources.Shared.Buttons.Ribbon.Indexes %>" height="48" width="48" align="middle" />
                <script type="text/javascript" language="javascript">
                    function GetJobIndexes() {
                        $('#JobFunctionsArea').load('<%: Url.Action("GetIndexesPartial", "JobDesc", new { area = "JobDesc" }) %>').fadeOut().fadeIn();
                    }
                </script>
            </div>
        </td>
        <td align="center" style="vertical-align: middle; width:90%">
            <div id="JobFunctionsArea" align="left" style="margin-left:20px;">
                <% Html.RenderAction("GetLatestSectionPartial", "JobDesc", new { area = "JobDesc" }); %>
            </div>
        </td>
        <%--<td style="width: 20px; vertical-align: middle" align="right">
            <a>
                <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/61.png") %>" onclick="ShowDialog()"
                    title='Module Map' alt="Job Description Module Map" height="48" width="48" align="middle" />
            </a>
            <script type="text/javascript" language="javascript">
                $(document).ready(function () {
                    $("#dialog").dialog("destroy");
                    $("#dialog-form").dialog({
                        autoOpen: false,
                        height: 'auto',
                        width: 'auto',
                        modal: true,
                        resizable: false,
                        buttons: {
                            Hide: function () {
                                $(this).dialog('close');
                            }
                        }
                    });
                });

                function ShowDialog() {
                    $('#dialog-form').dialog('open');
                }
            </script>
        </td>--%>
    </tr>
</table>
