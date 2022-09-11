<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Personnel/Views/Shared/Personnel.master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<HRIS.Domain.OrgChart.ValueObjects.Position>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%">
        <tr>
            <td style="width: 50%; vertical-align: top">
                <fieldset class="ParentFieldset">
                    <legend class="ParentLegend">Please Drag & Drop Organizational Objective You Want To
                        Bind With Objective "
                        <% if (ViewData["ObjectiveName"] != null)
                           { %>
                        <%:ViewData["ObjectiveName"].ToString() %>
                        <% } %>
                        "</legend>
                    <div id="TreeNodesArea">
                        <%if (Model.Count() > 0)
                          { %>
                        <%: Html.Telerik().TreeView()
								.ClientEvents(e => e.OnSelect("onSelect"))
								.ShowLines(true)
								.HighlightPath(true)
								.Name("TreeView").BindTo(Model, mapp => mapp.For<HRIS.Domain.OrgChart.ValueObjects.Node>(bin => bin.ItemDataBound((item, node) => { 
									item.Text = string.IsNullOrEmpty(node.Name) ? "UnNamed" : node.Name + " (code: " + node.Code + ")"; 
									item.Value = node.Id.ToString();
									item.Expanded = true;
								}).Children(node2 => node2.Children.OrderBy(o => o.Name))))
                            %>
                        <%}
                          else
                          {
                        %>
                        <%: Html.Encode("No Organizational Objective Defined To View") %>
                        <%} %>
                    </div>
                    <br />
                </fieldset>
            </td>
            <td style="width: 50%; vertical-align: middle; text-align: left">
                <table>
                    <tr>
                        <td style="width: 10%; vertical-align: middle; text-align: right">
                            <input type="button" value="<<" onclick="EmptyDiv()" style="width: 25px; height: 25px;
                                text-align: center; vertical-align: middle;" />
                        </td>
                        <td style="width: 90%; vertical-align: top">
                            <fieldset>
                                <legend class="ParentLegend">Selected Organizational Objectives:</legend>
                                <div class="drop-container t-group" id="Dest">
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
                <input type="button" value="Cancel" onclick="Return2Objective()" style="width: 170px" />
                <input type="button" value="Assign Selected" onclick="Assign2Objective()" style="width: 170px" />
            </td>
        </tr>
    </table>
    <div id="div-Destination-hidden" style="border: thin solid #000000; height: 1px;
        overflow: auto; visibility: hidden">
    </div>
    <script type="text/javascript">
        function onNodeDrop(e) {
            var dropContainer = $(e.dropTarget).closest('.drop-container');
            var treeview = $('#TreeView').data('tTreeView');
            var nodeText = treeview.getItemText(e.item);            
            if (dropContainer.length > 0 && dropContainer[0].innerHTML.indexOf(nodeText) == -1) {
                $('<div><strong>' + nodeText + '</strong></div>')
                    .hide().appendTo(dropContainer).slideDown('fast');
                e.preventDefault();
                if ($("#div-Destination-hidden").children() == "") {                   
                    $("#div-Destination-hidden").attr("innerHTML", '<br>');
                }
                var contenct = $("#div-Destination-hidden").attr("innerHTML");
                $("#div-Destination-hidden").attr("innerHTML", contenct + '<div>' + treeview.getItemValue(e.item) + '</div>');
            }
   

        }

       
        
        function EmptyDiv(e) {
            $("#Dest").attr("innerHTML", "");
            $("#div-Destination-hidden").attr("innerHTML", "");
        }

        function Assign2Objective(e) {

            var destinationTextDivContent = $("#Dest").children(['div']);
            if (destinationTextDivContent.length == 0) {
                alert('Please Drag & Drop Organizational Objective To Proceed!');
                return;
            }
            
            if (confirm('Are You Sure You Want To Proceed With This Operation?')) {

                var organizationalIDs = new Array();

                var count = 0;

                var SourceID = <%:TempData["ObjectiveID"].ToString() %> ;
                
                var destinationDivContent = $("#div-Destination-hidden").children();

                for (var i = 0; i < destinationDivContent.length; i++) {
                    if (destinationDivContent[i].innerHTML != "") {
                        organizationalIDs[count] = destinationDivContent[i].innerHTML;
                        count++;
                    }
                }

                $.ajax({
                    type: "POST",
                    traditional: true,
                    url: '<%=Url.Action("AssignOrganizational", "OrganizationalObjective") %>',
                    data: {objectiveId:SourceID, OrgObjectivesIds: organizationalIDs },
                    success: function (result) {
                        if (true) {

                            if (result.Success == false) {
                                alert(result.Message);
                            } else {
                                var url = '<%:Url.Action("index", "BasicObjective", new {id = "Value"})%>';
                                url = url.replace("Value", SourceID);
                                window.location.replace(url);
                            }
                        }

                    }
                });
                
                
            }

        }
        
    function Return2Objective (e) {
       
        var objID = <%:TempData["ObjectiveID"].ToString() %> ;
        var url = '<%:Url.Action("index", "BasicObjective", new {id = "Value"})%>';
        url = url.replace("Value", objID);
        window.location.replace(url);
        
    }
    </script>
    <style type="text/css">
        #TreeView, .drop-container
        {
            border-width: 1px;
            border-style: solid;
            width: 24em;
            float: left;
        }
        
        #TreeView
        {
            height: 24em;
            padding: .5em;
        }
        
        .drop-container
        {
            height: 8em;
            overflow: auto;
            margin-bottom: 1em;
            padding: .70em;
        }
        .pane
        {
            float: left;
            margin: -2em 6em 2em 0;
        }
    </style>
</asp:Content>
