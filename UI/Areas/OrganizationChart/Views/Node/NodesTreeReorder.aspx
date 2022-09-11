<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/OrganizationChart/Views/Shared/OrganizationChart.master"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<HRIS.Domain.OrgChart.ValueObjects.Node>>" %>

<%@ Import Namespace="HRIS.Domain.OrgChart.ValueObjects" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset class="ParentFieldset">
        <legend>
            <%: Resources.Areas.OrgChart.ValueObjects.Node.NodeModel.ReorderNodesTitle %></legend>
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
                                        item.Value = node.Id.ToString(); item.Expanded = true;}
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
                <td style="width: 50%; vertical-align: middle">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="width: 10%; vertical-align: middle; text-align: right">
                                <input type="button" value="<<" onclick="ClearParentDIVContent()" style="width: 25px;
                                    height: 25px; text-align: center; vertical-align: middle;" />
                            </td>
                            <td style="width: 90%; vertical-align: top">
                                <fieldset>
                                    <legend class="ParentLegend">
                                        <%:Resources.Areas.OrgChart.ValueObjects.Node.NodeModel.ParentTitle%></legend>
                                    <div id="div-parent" style="border: thin solid #000000; height: 150px; overflow: auto">
                                    </div>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%; vertical-align: middle; text-align: right">
                                <input type="button" value="<<" onclick="ClearChildrenDIVContent()" style="width: 25px;
                                    height: 25px; text-align: center; vertical-align: middle;" />
                            </td>
                            <td style="width: 90%; vertical-align: top">
                                <fieldset>
                                    <legend class="ParentLegend">
                                        <%:Resources.Areas.OrgChart.ValueObjects.Node.NodeModel.ChildrenTitle%></legend>
                                    <div id="div-child" style="border: thin solid #000000; height: 150px; overflow: auto">
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
                    <input type="button" value="<%:Resources.Areas.OrgChart.ValueObjects.Node.Buttons.SetNewOrder%>"
                        onclick="Merge()" style="width: 120px" />
                </td>
            </tr>
        </table>
        <div id="div-parent-hidden" style="border: thin solid #000000; height: 1px; overflow: auto;
            visibility: hidden">
        </div>
        <div id="div-child-hidden" style="border: thin solid #000000; height: 1px; overflow: auto;
            visibility: hidden">
        </div>
        <script type="text/javascript">

			var ParentItem;

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

			function ClearParentDIVContent() {
				EmptyDiv('div-parent');
				var treeView = $('#TreeView').data('tTreeView');
				if (treeView != null && ParentItem != null) {
					treeView.enable(ParentItem.item);
				}
			}

			function ClearChildrenDIVContent() {
				EmptyDiv('div-child');
			}

			function ClearDIVs() {
				ClearParentDIVContent();
				ClearChildrenDIVContent();
			}

			function isDropAllowed(e) {
				var dropContainerParent = $(e.dropTarget).closest('#div-parent');
				var dropContainerChildren = $(e.dropTarget).closest('#div-child');
				var treeView = $('#TreeView').data('tTreeView');
				var SourceItem = $(e.item).closest('.t-item');

				if (dropContainerParent.length > 0) {
					if (document.getElementById("div-parent").innerHTML.length == 0 && IsValueExistsInDiv("div-child-hidden", treeView.getItemValue(SourceItem)) == false) {
						return true;
					}
					else {
						return false;
					}
				}

				if (dropContainerChildren.length > 0) {
					if (IsValueExistsInDiv("div-child-hidden", treeView.getItemValue(SourceItem)) == false && IsValueExistsInDiv("div-parent-hidden", treeView.getItemValue(SourceItem)) == false && $(e.item).closest('.t-item').parent().closest('.t-item').find(':input[name*="Value"]').val() != null) {
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
					var ParentDropContainer = $(e.dropTarget).closest('#div-parent');
					var ChildrenDropContainer = $(e.dropTarget).closest('#div-child');
					var treeview = $('#TreeView').data('tTreeView');

					if (ParentDropContainer.length > 0) {
						$('<div>' + treeview.getItemText(e.item) + '</div>')
					.hide().appendTo(ParentDropContainer).slideDown('fast');
						e.preventDefault();

						if (document.getElementById('div-parent-hidden').innerHTML != "") {
							document.getElementById('div-parent-hidden').innerHTML += '<br>';
						}
						document.getElementById('div-parent-hidden').innerHTML += '<div>' + treeview.getItemValue(e.item) + '</div>';
						ParentItem = e;
					}
					else if (ChildrenDropContainer.length > 0) {
						if (document.getElementById('div-parent-hidden').innerHTML == "") {
							alert('<%:Resources.Areas.OrgChart.ValueObjects.Node.NodeRules.OnNodeDropSelectParentMessage %>');
						    return;
						}
						$('<div>' + treeview.getItemText(e.item) + '</div>')
					.hide().appendTo(ChildrenDropContainer).slideDown('fast');
						e.preventDefault();

						if (document.getElementById('div-child-hidden').innerHTML != "") {
							document.getElementById('div-child-hidden').innerHTML += '<br>';
						}
						document.getElementById('div-child-hidden').innerHTML += '<div>' + treeview.getItemValue(e.item) + '</div>';
					}

				}
			}

			function onLoad(e) {
				ClearDIVs();
				$('.t-in', this).live('contextmenu',
			function (e) {
				// prevent the browser context menu from opening
				e.preventDefault();
			});
			}

			function Merge() {
				var ParentDivContent = document.getElementById('div-parent-hidden').childNodes;
				var ChildrenDivContent = document.getElementById('div-child-hidden').childNodes;
				var ChildrenIDs = new Array();
				var ParentID;

				if (ParentDivContent.length == 0 || ChildrenDivContent.length == 0) {
					alert('<%:Resources.Areas.OrgChart.ValueObjects.Node.NodeRules.MergeSetParentChildrenMessage %>');
					return;
				}

				ParentID = ParentDivContent[0].innerHTML;

				var y = 0
				for (var i = 0; i < ChildrenDivContent.length; i++) {
					if (ChildrenDivContent[i].innerHTML != "") {
						ChildrenIDs[y] = ChildrenDivContent[i].innerHTML;
						y++;
					}
				}
               
				if (confirm( '<%:Resources.Shared.Messages.General.Confirm %>')) {

					$.ajax({
						type: "POST",
						traditional: true,
						url: '<%=Url.Action("NodesReorder", "Node") %>',
						data: { parentId: ParentID, childrenIDs: ChildrenIDs },
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
