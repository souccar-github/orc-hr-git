<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/OrganizationChart/Views/Shared/OrganizationChart.master"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<HRIS.Domain.OrgChart.ValueObjects.Position>>" %>

<%@ Import Namespace="HRIS.Domain.OrgChart.ValueObjects" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ParentFieldset">
        <legend><%:string.Format( Resources.Areas.OrgChart.ValueObjects.Node.NodeModel.ReassignPositionsTitle.ToLower(),
            ViewData["SourceNodeCode"] ,ViewData["DestinationNodeCode"] )%></legend>
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="width: 50%; vertical-align: top" rowspan="2">
                    <fieldset class="ParentFieldset">
                        <legend class="ParentLegend">
                            <%: Resources.Areas.OrgChart.ValueObjects.Node.NodeModel.DragDropPositionsTitle%></legend>
                        <div id="TreePositionsArea">
                            <%if (Model.Count() > 0)
                              { %>
                            <%: Html.Telerik().TreeView()
								.ExpandAll(true)
								.DragAndDrop(true)                   
								.ClientEvents(e => e.OnNodeDragging("OnNodeDragging"))
								.ClientEvents(e => e.OnNodeDrop("OnNodeDrop"))
								.ClientEvents(e => e.OnLoad("onLoad"))
								.Name("TreeView").BindTo(Model, map => map.For<Position>(bin => bin
								.ItemDataBound((item, position) => { item.Text = string.IsNullOrEmpty(position.Code) ? Resources.Areas.OrgChart.ValueObjects.Node.NodeRules.UnNamedNodeMessage
								 : position.Code; item.Value = position.Id.ToString(); item.Expanded = true; }
										)))
                            %>
                            <%}
                              else
                              {
                            %>
                            <%: Html.Encode(Resources.Areas.OrgChart.ValueObjects.Node.NodeRules.NoPositionsDefinedMessage)%>
                            <%} %>
                        </div>
                        <br />
                    </fieldset>
                </td>
                <td style="width: 10%; vertical-align: middle; text-align: right">
                    <input type="button" value="<<" onclick="EmptyDiv('div-Destination')" style="width: 25px;
                        height: 25px; text-align: center; vertical-align: middle;" />
                </td>
                <td style="width: 40%; vertical-align: top">
                    <fieldset>
                        <legend class="ParentLegend">
                            <%: Resources.Areas.OrgChart.ValueObjects.Node.NodeModel.DestinationTitle%></legend>
                        <div id="div-Destination" style="border: thin solid #000000; height: 150px; overflow: auto">
                        </div>
                    </fieldset>
                </td>
            </tr>
        </table>
        <input type="button" value="<%: Resources.Areas.OrgChart.ValueObjects.Node.Buttons.Reassign%>"
            onclick="Reassign()" style="width: 100px" />
        <%: Html.ActionLink(Resources.Shared.Buttons.Function.Back, "LoadReassignNodesPositions")%>
        <div id="div-Destination-hidden" style="border: thin solid #000000; height: 1px;
            overflow: auto; visibility: hidden">
        </div>
        <script type="text/javascript">

		function IsValueExistsInDiv(DivId, Value) {

			var DivContent = document.getElementById(DivId.toString()).childNodes;
			for (var i = 0; i < DivContent.length; i++) {
				if (DivContent[i].innerHTML == Value.toString()) {
					return true;
				}
			}
			return false;
		}

		function isDropAllowed(e) {

			var dropContainerDestination = $(e.dropTarget).closest('#div-Destination');
			var treeView = $('#TreeView').data('tTreeView');
			var SourceItem = $(e.item).closest('.t-item');
			var Node = $(e.item);

			if (dropContainerDestination.length > 0) {
				if (
				IsValueExistsInDiv("div-Destination-hidden", treeView.getItemValue(SourceItem)) == false) {
					return true;
				}
				else {
					return false;
				}
			}
			return false;
		}

		function OnNodeDragging(e) {

			if (!isDropAllowed(e))
				e.setStatusClass('t-denied');
			else
				e.setStatusClass('t-add');
		}

		function OnNodeDrop(e) {

			if (!isDropAllowed(e)) {
				e.preventDefault();
			}
			else {

				var DestinationDropContainer = $(e.dropTarget).closest('#div-Destination');
				var treeview = $('#TreeView').data('tTreeView');

				if (DestinationDropContainer.length > 0) {
					$('<div>' + treeview.getItemText(e.item) + '</div>')
					.hide().appendTo(DestinationDropContainer).slideDown('fast');
					e.preventDefault();

					if (document.getElementById('div-Destination-hidden').innerHTML != "") {
						document.getElementById('div-Destination-hidden').innerHTML += '<br>';
					}
					document.getElementById('div-Destination-hidden').innerHTML += '<div>' + treeview.getItemValue(e.item) + '</div>';
				}
			}
		}

		function onLoad(e) {
			EmptyDiv('div-Destination');
		}

		function EmptyDiv(e) {

			document.getElementById(e.toString()).innerHTML = "";
			document.getElementById(e.toString() + '-hidden').innerHTML = "";
		}
		
		function Reassign() {

			if (confirm('<%: Resources.Shared.Messages.General.Confirm%>')) {

				var DestinationDivContent = document.getElementById('div-Destination-hidden').childNodes;
				var PositionsIDs = new Array();

				if (DestinationDivContent.length == 0) {
					alert('<%: Resources.Areas.OrgChart.ValueObjects.Node.NodeRules.DragDropPositionsMessage%>');
					return;
				}

				var y = 0
				for (var i = 0; i < DestinationDivContent.length; i++) {
					if (DestinationDivContent[i].innerHTML != "") {
						PositionsIDs[y] = DestinationDivContent[i].innerHTML;
						y++;
					}
				}

				var SourceID = <%:ViewData["SourceID"] %>
				var DestID = <%:ViewData["DestinationID"] %>

				//alert("D "+DistinationID + "S "+ SourceID + "P "+ PositionsIDs);
				
				$.ajax({
					type: "POST",
					traditional: true,
					url: '<%=Url.Action("ReassignPositions", "Node") %>',
					data: { destination: DestID, source: SourceID,  positionsIDs: PositionsIDs},
					success: function (result) {
						if (true) {

							if (result.Success == false) {
								alert(result.Message);
							} else {
								location.reload();
							}
						}

					}
				});
			}
		}
        </script>
    </fieldset>
</asp:Content>
