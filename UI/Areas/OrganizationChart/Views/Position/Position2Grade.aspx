<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/OrganizationChart/Views/Shared/OrganizationChart.master"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<HRIS.Domain.OrgChart.ValueObjects.Node>>" %>

<%@ Import Namespace="HRIS.Domain.OrgChart.ValueObjects" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ParentFieldset">
        <legend class="ParentLegend"><%:Resources.Areas.OrgChart.ValueObjects.Position.PositionModel.PositionTitle%></legend>
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="width: 50%; vertical-align: top" colspan="2">
                    <div class="display-label">
                    </div>
                </td>
                <td style="width: 50%; vertical-align: top" align="right">
                    <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                       {%>
                    <input type="button" value="Add" onclick="AddPosition()" />
                    <% } %>
                    <script type="text/javascript">
                        function AddPosition() {
                            $('#result').load('<%: Url.Action("Insert", "Position") %>');
                        }
                    </script>
                </td>
            </tr>
        </table>
        <br />
        <%
            Html.Telerik().Grid<Position>("positions")
                .Name("PositionsGrid")
                .DataKeys(k => k.Add(o => o.Id))
                .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Position"))
                .Columns(c =>
                {
                    c.Bound(o => o.Id).Width(100).Title("Number");
                    //c.Bound(o => o.Grade.SingleOrDefault().Id).Title("Grade Number");
                    c.Bound(o => o.Code).Width(100);
                    //c.Bound(o => o.Status.Name).Width(100).Title("Status");
                    c.Bound(o => o.Level.Name).Width(100).Title("Level");
                    c.Bound(o => o.Type.Name).Width(100).Title("Type");
                    c.Command(s => s.Delete()).Width(100).Title("Commands");
                })
                 .ClientEvents(events => events.OnRowSelected("loadPartialView"))
                .Pageable(p => p.PageSize(5)).Sortable().Filterable().Selectable().Render();
        %>
    </fieldset>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="width: 90%">
                <div id="PositionsGrid">
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div id="result">
                </div>
            </td>
        </tr>
    </table>
<script type="text/javascript">
    function loadPartialView(e) {

        $('#result').fadeIn();

        var x = e.row.cells[1].innerHTML;
        $('#result').load('<%: Url.Action("BasicMasterInfo", "Grade") %>', { selectedPositionId: x.toString() });

    }
</script>
</asp:Content>

