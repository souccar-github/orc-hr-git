<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/OrganizationChart/Views/Shared/OrganizationChart.master"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ParentFieldset">
        <div id="dialog-form" title=<%:Resources.Areas.OrgChart.ValueObjects.Position.PositionModel.PositionTitle %>>
        </div>
        <legend class="ParentLegend"><%:Resources.Areas.OrgChart.Entities.Organization.OrganizationModel.OrganizationHierarchyTitle %></legend>
        <input id="SelectedNodeID" type="hidden" value="0" />
        <input id="SelectedNodeCode" type="hidden" value="0000" />
        <input id="ParentName" type="hidden" value="" />
        <% Html.RenderPartial("OperationWindow"); %>
        <% Html.RenderPartial("ContextMenu"); %>
        <table>
            <tr>
                <td style="width: 10%">
                    <div id="container">
                        <div id="log">
                        </div>
                        <div id="center-container">
                            <% Html.RenderPartial("Tree"); %>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="Nodes" style="display: none">
                    </div>
                </td>
            </tr>
        </table>
    </fieldset>
    <script type="text/javascript">
        //////////////////////////// Show Position Dialog & Context Menu Hiding when scroll/////////////////  

        $(window).scroll(function (e) {
            $("#myMenu").hide();
        });

        $(document).ready(function () {

            var translations = {};
            translations["cancel"] = "<%: Resources.Shared.Buttons.Function.Cancel %>";
            var buttonsOpts = {};
            buttonsOpts[translations["cancel"]] = function () {
                $(this).dialog('close');
            };



            $("#dialog").dialog("destroy");
            $("#dialog-form").dialog({
                autoOpen: false,
                height: 'auto',
                width: 'auto',
                modal: true,
                resizable: false,
                buttons: buttonsOpts
            });
        });

        function ShowDialog(nodeID, nodeCode) {

            if (nodeCode == "0000") {
                alert('<%:Resources.Areas.OrgChart.Entities.Organization.OrganizationRules.NoPositionsMessage %>');
                return;
            }

            $.ajax({
                type: "POST",
                traditional: true,
                url: '<%: Url.Action("ReadOnly", "Position") %>',
                data: { nodeId: nodeID },
                success: function (result) {
                    if (result.Success == false) {
                        alert(result.Message);
                    } else {
                        $('#dialog-form').html(result.PartialViewHtml);
                        $('#dialog-form').dialog('open');
                    }
                }
            });
        }

        /////////////////////////////////////////////////////////////////



        ///////////// New Tree Functions Section

        function TreeOnSelect(nodeId, nodeCode) {

            var name;

            $.ajax({
                url: '<%= Url.Action("JsonSelect","Node") %>/', type: "POST",
                data: { nodeId: nodeId },
                success: function (result) {
                    $('#Nodes').show();
                    $('#Nodes').html(result.PartialViewHtml);
                    $("#ParentName").attr("value", result.ParentName);

                }
            });

            $("#ParentName").attr("value", name);
            $("#SelectedNodeID").attr("value", nodeId);
            $("#SelectedNodeCode").attr("value", nodeCode);
        }

        function onEdit(nodeID) {

            $.ajax({
                url: '<%= Url.Action("JsonEdit","Node") %>/', type: "POST",
                data: { nodeId: nodeID },
                success: function (result) {
                    if (result.Success != null) {
                        if (result.Success == false) {
                            alert(result.Message);
                        }
                        else {
                            $('#Nodes').show();
                            $('#Nodes').html(result.PartialViewHtml);
                        }
                    }
                }
            });
        }

        function onAdd(nodeID) {

            $.ajax({
                url: '<%= Url.Action("JsonAdd","Node") %>/', type: "POST",
                data: { nodeId: nodeID },
                success: function (result) {
                    $('#Nodes').show();
                    $('#Nodes').html(result.PartialViewHtml)
                }
            });
        }

        function onDelete(nodeID) {
            // var treeView = $('#TreeView').data('tTreeView');

            if (confirm('<%:Resources.Shared.Messages.General.DeleteConfirm %>')) {

                $.ajax({
                    url: '<%= Url.Action("Delete","Node") %>/', type: "POST",
                    data: { nodeId: nodeID },
                    success: function (result) {
                        if (result.Success == false) {
                            alert(result.Message);
                        } else {
                            $("#center-container").html(result.PartialViewHtml);
                        }
                    }
                });
            }
        }

        function BuildContextMenu() {
            // Building Context Menu Foe each Node
            //$(".node").each() get all nodes that have node class
            $(".node").each(function (i) {
                // Show menu when #myMenu is clicked
                $(this).contextMenu({
                    menu: 'myMenu'
                },
					function (action, el, pos) {
					    var nodeID = $(el).attr('id');
					    var nodeCode = "";
					    $.ajax({
					        type: "POST",
					        url: '<%: Url.Action("GetNodeCode", "Node") %>',
					        data: { nodeID: nodeID },
					        success: function (result) {
					            nodeCode = result.Message;
					            name = result.ParentName;
					            $("#SelectedNodeID").attr("value", nodeID);
					            $("#SelectedNodeCode").attr("value", nodeCode);
					            $("#ParentName").attr("value", name);

					            switch (action) {
					                case 'add': onAdd(nodeID, nodeCode); break;
					                case 'edit': onEdit(nodeID); break;
					                case 'delete': onDelete(nodeID); break;
					                case 'addPos': AddPosition(nodeID, nodeCode); break;
					                case 'showPos': ShowDialog(nodeID, nodeCode); break;
					                default: alert('<%:Resources.Shared.Messages.General.YouCanNotDoThis %>'); break;
					            };
					        },
					        error: function (result) {
					            alert('<%:Resources.Areas.OrgChart.ValueObjects.Node.NodeRules.ErrorGetNodeInfoMessage %>');
					            return;

					        }
					    });
					});

            });

        }

        /////////////////////////////////////////////////////////

        function AddPosition(nodeID, nodeCode) {

            if (nodeID == "0") {
                alert('<%:Resources.Areas.OrgChart.ValueObjects.Node.NodeRules.SelectParentNodeMessage %>');
            } else if (nodeCode != "0000") {

                var url = '<%: Url.Action("GoToPositions", "Node", new { nodeID = "Value1"}) %>';
                url = url.replace("Value1", encodeURIComponent(nodeID));

                window.location.replace(url);
            }
            else {
                alert('<%:Resources.Areas.OrgChart.ValueObjects.Node.NodeRules.PositionsUnderRootNotAllowedMessage %>');
            }
        }

    </script>
</asp:Content>
