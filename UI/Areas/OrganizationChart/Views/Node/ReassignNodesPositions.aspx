<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/OrganizationChart/Views/Shared/OrganizationChart.master"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<HRIS.Domain.OrgChart.ValueObjects.Node>>" %>

<%@ Import Namespace="HRIS.Domain.OrgChart.ValueObjects" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ParentFieldset">
        <legend>
            <%: Resources.Areas.OrgChart.ValueObjects.Node.NodeModel.ReassignNodesPositionsTitle%></legend>
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="width: 50%; vertical-align: top">
                    <fieldset class="ParentFieldset">
                        <legend class="ParentLegend">
                            <%: Resources.Areas.OrgChart.ValueObjects.Node.NodeModel.DragDropNodesTitle%></legend>
                        <div id="TreeNodesArea">
                            <%if (Model.Count() > 0)
                              { %>
                            <%: Html.Telerik().TreeView()
								.ExpandAll(true)
								.DragAndDrop(true)                   
								.ClientEvents(e => e.OnNodeDragging("OnNodeDragging"))
								.ClientEvents(e => e.OnNodeDrop("OnNodeDrop"))                         
								.ClientEvents(e => e.OnLoad("onLoad"))
								.Name("TreeView").BindTo(Model, map => map.For<Node>(bin => bin
									.ItemDataBound((item, node) => { item.Text = string.IsNullOrEmpty(node.Name) ?
                                        Resources.Areas.OrgChart.ValueObjects.Node.NodeRules.UnNamedNodeMessage : node.Name; 
                                        item.Value = node.Id.ToString(); item.Expanded = true;
                                    }
																								).Children(node2 => node2.Children.OrderBy(o => o.Name))
																								   ))
                            %>
                            <%}
                              else
                              {
                            %>
                            <%: Html.Encode(Resources.Areas.OrgChart.ValueObjects.Node.NodeRules.NoNodesDefinedMessage) %>
                            <%} %>
                        </div>
                        <br />
                    </fieldset>
                </td>
                <td style="width: 50%; vertical-align: middle; text-align: left">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="width: 10%; vertical-align: middle; text-align: right">
                                <input type="button" value="<<" onclick="ClearSourceDIVContent()" style="width: 25px;
                                    height: 25px; text-align: center; vertical-align: middle;" />
                            </td>
                            <td style="width: 90%; vertical-align: top">
                                <fieldset>
                                    <legend class="ParentLegend">
                                        <%:Resources.Areas.OrgChart.ValueObjects.Node.NodeModel.SourceTitle%></legend>
                                    <div id="div-Source" style="border: thin solid #000000; height: 150px; overflow: auto;
                                        text-align: left;">
                                    </div>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%; vertical-align: middle; text-align: right">
                                <input type="button" value="<<" onclick="ClearDestinationDIVContent()" style="width: 25px;
                                    height: 25px; text-align: center; vertical-align: middle;" />
                            </td>
                            <td style="width: 90%; vertical-align: top">
                                <fieldset>
                                    <legend class="ParentLegend">
                                        <%:Resources.Areas.OrgChart.ValueObjects.Node.NodeModel.DestinationTitle%></legend>
                                    <div id="div-Destination" style="border: thin solid #000000; height: 150px; overflow: auto;
                                        text-align: left;">
                                    </div>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 100%; vertical-align: top" colspan="3">
                    <br />
                    <input type="button" value="<%:Resources.Areas.OrgChart.ValueObjects.Node.Buttons.MoveAllPositions%>"
                        onclick="Reassign()" style="width: 170px" />
                    <input type="button" value="<%:Resources.Areas.OrgChart.ValueObjects.Node.Buttons.ChoosePositionsToMove%>"
                        onclick="Next()" style="width: 215px" />
                </td>
            </tr>
        </table>
        <div id="dialog-form" title="Positions">
        </div>
        <div id="div-Destination-hidden" style="border: thin solid #000000; height: 1px;
            overflow: auto; visibility: hidden">
        </div>
        <div id="div-Source-hidden" style="border: thin solid #000000; height: 1px; overflow: auto;
            visibility: hidden">
        </div>
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
                        Cancel: function () {
                            $(this).dialog('close');
                        }
                    }
                });
            });
        </script>
        <script type="text/javascript">

			var DestinationItem;

			function IsValueExistsInDiv(DivId, Value) {
			    var DivContent = document.getElementById(DivId.toString()).childNodes;
			    for (var i = 0; i < DivContent.length; i++) {
			        if (DivContent[i].innerHTML == Value.toString()) {
			            return true;
			        }
			    }
			    return false;
			}

			function EmptyDiv(e) {
				document.getElementById(e.toString()).innerHTML = "";
				document.getElementById(e.toString() + '-hidden').innerHTML = "";
			}

			function ClearDestinationDIVContent() {
				EmptyDiv('div-Destination');
				var treeView = $('#TreeView').data('tTreeView');
				if (treeView != null && DestinationItem != null) {
					treeView.enable(DestinationItem.item);
				}
			}

			    function ClearSourceDIVContent() {
				    EmptyDiv('div-Source');
			    }

			function ClearDIVs() {
				ClearDestinationDIVContent();
				ClearSourceDIVContent();
			}

			function isDropAllowed(e) {
				var dropContainerDestination = $(e.dropTarget).closest('#div-Destination');
				var dropContainerSource = $(e.dropTarget).closest('#div-Source');
				var treeView = $('#TreeView').data('tTreeView');
				var SourceItem = $(e.item).closest('.t-item');
				var Node = $(e.item);

				if (dropContainerDestination.length > 0) {
					if (document.getElementById("div-Destination").innerHTML.length == 0 &&
				IsValueExistsInDiv("div-Source-hidden", treeView.getItemValue(SourceItem)) == false) {
						return true;
					}
					else {
						return false;
					}
				}

				if (dropContainerSource.length > 0) {
					if (document.getElementById("div-Source").innerHTML.length == 0 &&
				IsValueExistsInDiv("div-Destination-hidden", treeView.getItemValue(SourceItem)) == false &&
					//Node.closest(".t-item").find(".t-group").children().length == 0 &&
				$(e.item).closest('.t-item').parent().closest('.t-item').find(':input[name*="Value"]').val() != null) {
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
					var SourceDropContainer = $(e.dropTarget).closest('#div-Source');
					var treeview = $('#TreeView').data('tTreeView');

					if (DestinationDropContainer.length > 0) {
						$('<div>' + treeview.getItemText(e.item) + '</div>')
					.hide().appendTo(DestinationDropContainer).slideDown('fast');
						e.preventDefault();

						if (document.getElementById('div-Destination-hidden').innerHTML != "") {
							document.getElementById('div-Destination-hidden').innerHTML += '<br>';
						}
						document.getElementById('div-Destination-hidden').innerHTML += '<div>' + treeview.getItemValue(e.item) + '</div>';
						DestinationItem = e;
					}
					else if (SourceDropContainer.length > 0) {
						$('<div>' + treeview.getItemText(e.item) + '</div>')
					.hide().appendTo(SourceDropContainer).slideDown('fast');
						e.preventDefault();

						if (document.getElementById('div-Source-hidden').innerHTML != "") {
							document.getElementById('div-Source-hidden').innerHTML += '<br>';
						}
						document.getElementById('div-Source-hidden').innerHTML += '<div>' + treeview.getItemValue(e.item) + '</div>';
					}

				}
			}

			function onLoad(e) {
				ClearDIVs();
				$('.t-in', this).live('contextmenu',
		function (e) {
			// prevent the browser context menu from opening
			e.preventDefault();

			// the node for which the 'contextmenu' event has fired
			var $node = $(this).closest('.t-item');

			// default "slide" effect settings
			var fx = $.telerik.fx.slide.defaults();

			// context menu definition - markup and event handling
			var $contextMenu =
						$('<div class="t-animation-container" id="contextMenu">' +
							'<ul class="t-widget t-group t-menu t-menu-vertical" style="display:none">' +
							'<li class="t-item"><a href="#" class="t-link"><%:Resources.Areas.OrgChart.ValueObjects.Node.Buttons.MoveAllPositions%></a></li>' +
							'</ul>' +
						'</div>')
						.css( //positioning of the menu
								{
								position: 'absolute',
								left: e.pageX, // x coordinate of the mouse
								top: e.pageY   // y coordinate of the mouse
				}
							)
						.appendTo(document.body)
						.find('.t-item') // select the menu items
						.mouseenter(
						function () {
							// hover effect
							$(this).addClass('t-state-hover');
						}
									)
						.mouseleave(
						function () {
							// remove the hover effect
							$(this).removeClass('t-state-hover');
						}
									)
						.click(
						function (e) {
							e.preventDefault();
							// dispatch the click
							onItemClick($(this), $node);
						}
								)
						.end();

			// show the menu with animation
			if (!IsSelectedNode($node)) {
				$contextMenu.remove();
			} else {
				if ($($node).closest('.t-item').parent().closest('.t-item').find(':input[name*="Value"]').val() != null) {
					$.telerik.fx.play(fx, $contextMenu.find('.t-group'), { direction: 'bottom' });
				}
			}

			// handle globally the click event in order to hide the context menu
			$(document).click(
function (e) {
	// hide the context menu and remove it from DOM
	$.telerik.fx.rewind(fx, $contextMenu.find('.t-group'), { direction: 'bottom' },
							function () {
								$contextMenu.remove();
							}
												 );
});

		});
			}

			function IsSelectedNode(ClickedNode) {
				var TreeView = $('#TreeView');
				var SelectedNode = TreeView.find('.t-state-selected').closest('.t-item');

				var TreeView2 = $('#TreeView').data('tTreeView');
				var ClickedNodeValue = TreeView2.getItemValue(ClickedNode);
				var SelectedNodeValue = TreeView2.getItemValue(SelectedNode);

				if (ClickedNodeValue == SelectedNodeValue) {
					return true;
				}
				return false;
			}

			function onItemClick($item, $node) {

				var treeView = $('#TreeView').data('tTreeView');
				var nodeText = treeView.getItemText($node);
				var menuItemText = $item.text();

				if (menuItemText == '<%:Resources.Areas.OrgChart.ValueObjects.Node.Buttons.MoveAllPositions%>') {
					ShowDialog($node);

				}
			}

			function ShowDialog($node) {

				var treeView = $('#TreeView').data('tTreeView');
				var selectedItemID = treeView.getItemValue($node);

				$.ajax({
					type: "POST",
					traditional: true,
					url: '<%: Url.Action("ReadOnly", "Position") %>',
					data: { nodeID: selectedItemID },
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

			function Reassign() {
				var DestinationDivContent = document.getElementById('div-Destination-hidden').childNodes;
				var SourceDivContent = document.getElementById('div-Source-hidden').childNodes;
				var SourceID;
				var DestinationID;

				if (DestinationDivContent.length == 0 || SourceDivContent.length == 0) {
					alert('<%:Resources.Areas.OrgChart.ValueObjects.Node.NodeRules.SetDestinationSourceMessage %>');
					return;
				}

				DestinationID = DestinationDivContent[0].innerHTML;
				SourceID = SourceDivContent[0].innerHTML;

				if (confirm('<%:Resources.Shared.Messages.General.Confirm %>')) {

					$.ajax({
						type: "POST",
						traditional: true,
						url: '<%=Url.Action("ReassignNodesPositions", "Node") %>',
						data: { destination: DestinationID, source: SourceID },
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

			function Next() {
		
				var DestinationDivContent = document.getElementById('div-Destination-hidden').childNodes;
		  
				var SourceDivContent = document.getElementById('div-Source-hidden').childNodes;
		
				var SourceID;
		   
				var DestinationID;
				
				if (DestinationDivContent.length == 0 || SourceDivContent.length == 0) {
					
					alert('<%:Resources.Areas.OrgChart.ValueObjects.Node.NodeRules.SetDestinationSourceMessage %>');
					
					return;
				}
				
				DestinationID = DestinationDivContent[0].innerHTML;
				SourceID = SourceDivContent[0].innerHTML;


				var url = '<%: Url.Action("LoadReassignPositions", "Node", new { destinationId = "Value1" }) %>';

				url = url.replace("Value1", DestinationID + "&sourceId=Value2");
				url = url.replace("Value2", SourceID);
				window.location = url;

				//alert(url);
				//$(location).attr('href', url);
			}

		
        </script>
    </fieldset>
</asp:Content>
