<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/OrganizationChart/Views/Shared/OrganizationChart.master"
    Inherits="System.Web.Mvc.ViewPage<HRIS.Domain.OrgChart.Indexes.NodeType>" %>
<%@ Import Namespace="HRIS.Domain.OrgChart.Indexes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("IndexesNavigator"); %>
    <fieldset class="ParentFieldset">
        <legend class="ParentLegend"><%:Resources.Areas.OrgChart.Indexes.NodeType.NodeTypeModel.BasicInfoNodeTypesTitle %></legend>
        <br />
        <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
           {%>
        <a href="#" onclick="ShowInsert()">
            <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>" title=<%:Resources.Shared.Buttons.Function.Add %>
                alt=<%:Resources.Shared.Buttons.Function.Add %> height="36" width="36" align="middle" />
        </a>
        <% } %>
        <%:
            Html.Telerik().Grid<NodeType>("nodeTypes")
                .Name("NodeTypeGrid")
                .DataKeys(k => k.Add(o => o.Id))               
                .Columns(c =>
                {
                    c.Bound(o => o.Id).Width(1);
                    c.Bound(o => o.NodeOrder);                    
                    c.Bound(o => o.Name);
                    c.Command(s => s.Delete().ButtonType(GridButtonType.Image)).Width(1);
                })
                .DataBinding(d => d.Server().Delete("Delete", "NodeType"))
                .ClientEvents(events => events.OnRowSelect("loadPartialView"))
                .Pageable(p => p.PageSize(15))
                .Selectable()
        %>
    </fieldset>
    <div id="result" style="display: none">
    </div>
    <div id="err" style="color: Red; display: none">
        <% if (ViewData["Error"] != null)
           { %>
        <%: ViewData["Error"].ToString() %>
        <% } %>
    </div>
    <script type="text/javascript">

        function loadPartialView(e) {
            $("#err").hide();
            $('#result').fadeOut('fast');
            var x = e.row.cells[0].innerHTML;
            $('#result').load('<%: Url.Action("PartialMasterInfo", "NodeType") %>', { selectedRowId: x.toString() }, function () {
                $('#result').fadeIn('slow');
            });
        }

        function ShowInsert(e) {
            $("#err").hide();
            $('#result').fadeOut('fast');
            $('#result').load('<%: Url.Action("Insert", "NodeType") %>', function () {
                $('#result').fadeIn('slow');
            });
        }

        $(document).ready(function () {
            $('#OrgFunctionsArea').fadeOut();
            if ($("#err").is(':empty')) {
                $("#err").hide();
            }
            else {
                $("#err").show();
            }
        });
    </script>
</asp:Content>
