<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="UI.Helpers" %>
<input id="SelectedNodeID" type="hidden" value="0" />
<table>
    <tr>
        <td style="width: 10%">
            <div id="container">
                <div id="center-container">
                    <%
                        Html.RenderPartial("OrgTree/Tree");%>
                </div>                
                <div id="log">
                </div>
            </div>
        </td>
        <td style="vertical-align: top">
            <div>
                <table width="100%">
                    <tr>
                        <td>
                            <input type="button" onclick=" Reset(); " value="Back One Level" style="width: auto" />
                            <input type="button" onclick=" Reset(1); " value="Reset Tree" style="width: auto" />
                        </td>
                    </tr>
                </table>
                <script type="text/javascript">
                    function Reset(x) {
                        $.ajax({
                            url: '<%:Url.Action("BackOneLevel", "BasicObjective")%>/',
                            type: "POST",
                            data: { reset: x },
                            success: function (result) {
                                if (result.Success) {
                                    $("#center-container").html(result.PartialViewHtml);
                                    $("#SelectedNodeID").attr("value", "0");
                                    $("#NodePositions").hide();
                                }
                            }
                        });
                    }
                </script>
            </div>
            <br />
            <div class="editor-label-required">
                Owner Job Title
            </div>
            <div id="NodePositions">
                <fieldset>
                    <%:Html.Telerik().ComboBox()
                                  .Name("nodePositions")
                                  .AutoFill(true)
                                  .HtmlAttributes(new {style = string.Format("width:{0}px", 300)})
                                  .Filterable(filtering => filtering.FilterMode(AutoCompleteFilterMode.Contains))
                                  .HighlightFirstMatch(true)
                                  .SelectedIndex(-1)
                                  .ClientEvents(events => events.OnClose("nodePositionsEvent"))
                    %>
                </fieldset>
            </div>
        </td>
    </tr>
</table>
<script type="text/javascript">

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

    function TreeOnSelect(nodeId) {

        $.ajax({
            url: '<%:Url.Action("GetPositions", "BasicObjective")%>/',
            type: "POST",
            data: { nodeId: nodeId },
            success: function (result) {
                if (result.Success) {
                    $('#NodePositions').html(result.PartialViewHtml);
                    $('#NodePositions').fadeIn('fast');

                    $.ajax({
                        url: '<%:Url.Action("SetRelatedNodeToTheSession", "BasicObjective")%>/',
                        type: "POST",
                        data: { nodeId: nodeId }
                    });
                }
            }
        });
    }

    function nodePositionsEvent() {

        var comboBox = $("#nodePositions").data("tComboBox");
        var id = comboBox.value();

        $.ajax({
            url: '<%:Url.Action("GetEmployees", "BasicObjective")%>/',
            type: "POST",
            data: { positionId: id },
            success: function (result) {
                if (result.Success) {

                    $.ajax({
                        url: '<%:Url.Action("SetRelatedPositionToTheSession", "BasicObjective")%>/',
                        type: "POST",
                        data: { positionId: id },
                        success: function () {
                            var url = '<%:Url.Action("Index", "BasicObjective")%>';
                            window.location.replace(url);
                        }
                    });
                }
            }
        });

    }

    function BuildContextMenu() {
    }
    
</script>
