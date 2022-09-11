<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<p>
    <table width="100%">
        <tr>
            <%--<td>
                <input type="button" onclick=" openWindow(); " id="restoreButton" value="<%:Resources.Areas.OrgChart.ValueObjects.Node.Buttons.NodesOperationsWindow %>"
                    style="width: auto" />
            </td>--%>
            <td align="right">
                <input type="button" onclick=" Reset(); " value="<%:Resources.Areas.OrgChart.ValueObjects.Node.Buttons.BackOneLevel %>"
                    style="width: auto" />
                <input type="button" onclick=" Reset(1); " value="<%:Resources.Areas.OrgChart.ValueObjects.Node.Buttons.ResetTree %>"
                    style="width: auto" />
            </td>
        </tr>
    </table>
</p>
<script type="text/javascript">
    function Reset(x) {
        $.ajax({
            url: '<%=Url.Action("BackOneLevel", "Node")%>/',
            type: "POST",
            data: { reset: x },
            success: function (result) {
                if (result.Success) {
                    $("#center-container").html(result.PartialViewHtml);
                    $("#SelectedNodeID").attr("value", "0");
                    $("#SelectedNodeCode").attr("value", "0000");
                    $('#Nodes').hide();
                }
            }
        });
    }
</script>
<%
    Html.Telerik().Window()
        .Name("Window")
        .Title(Resources.Areas.OrgChart.ValueObjects.Node.NodeModel.WindowNodeOperationsTitle)
        .Draggable(true)
        .Content(() =>
                     {%>
<div id="operations">
    <a href="#" onclick=" PanelBarOnSelect('add'); ">
        <%:Resources.Areas.OrgChart.ValueObjects.Node.Buttons.AddSubNode %></a>
    <br />
    <a href="#" onclick=" PanelBarOnSelect('edit'); ">
        <%:Resources.Areas.OrgChart.ValueObjects.Node.Buttons.EditNode %></a>
    <br />
    <a href="#" onclick=" PanelBarOnSelect('delete'); ">
        <%:Resources.Areas.OrgChart.ValueObjects.Node.Buttons.DeleteNode %></a>
    <br />
    <a href="#" onclick=" PanelBarOnSelect('addPos'); ">
        <%:Resources.Areas.OrgChart.ValueObjects.Node.Buttons.AddEditPosition %></a>
    <br />
    <a href="#" onclick=" PanelBarOnSelect('showPos'); ">
        <%:Resources.Areas.OrgChart.ValueObjects.Node.Buttons.ShowPositions %></a>
    <br />
</div>
<%
                     })
        .Width(150)
        .Height(80)
        .Render();
%>
<script type="text/javascript">
    function PanelBarOnSelect(type) {

        var nodeID = $("#SelectedNodeID").attr("value");
        var nodeCode = $("#SelectedNodeCode").attr("value");
        var txt = $('#ParentName').attr("value");

        if (txt == "") {
            alert('<%:Resources.Areas.OrgChart.ValueObjects.Node.NodeRules.PanelBarOnSelectSelectNodeMessage%>');
            return;
        }

        if (nodeID == "0") {
            alert('<%:Resources.Areas.OrgChart.ValueObjects.Node.NodeRules.PanelBarOnSelectSelectNodeMessage%>');
            return;
        }

        switch (type) {
        case 'add':
            onAdd(nodeID, nodeCode);
            break;
        case 'edit':
            onEdit(nodeID);
            break;
        case 'delete':
            onDelete(nodeID);
            break;
        case 'addPos':
            AddPosition(nodeID, nodeCode);
            break;
        case 'showPos':
            ShowDialog(nodeID, nodeCode);
            break;
        default:
            alert('<%:Resources.Shared.Messages.General.YouCanNotDoThis%>');
            break;
        }

    }

    function openWindow() {
        var window = $("#Window").data("tWindow");
        window.open();

    }
</script>
