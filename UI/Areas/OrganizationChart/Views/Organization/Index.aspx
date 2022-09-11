<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/OrganizationChart/Views/Shared/OrganizationChart.master"
    Inherits="System.Web.Mvc.ViewPage<HRIS.Domain.OrgChart.Entities.Organization>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 100%;">
        <tr>
            <td style="width: 100%; vertical-align: top">
                <div id="result">
                    <input id="SelectedNodeID" type="hidden" value="0" />
                    <input id="SelectedNodeCode" type="hidden" value="0000" />
                </div>
            </td>
        </tr>
    </table>
    <script type="text/javascript" language="javascript">
        $('#result').load('<%: Url.Action("Load", "Organization") %>');
    </script>
</asp:Content>
