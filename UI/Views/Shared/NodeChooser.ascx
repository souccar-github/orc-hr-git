<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="UI.Helpers" %>
<table class="page">
    <tr>
        <td style="width: 33.3%">
            <div class="editor-field">
                <% if (ViewData["NodeChooserNode"] != null)
                   { %>
                <%: ViewData["NodeChooserNode"].ToString()%>
                <% } %>
            </div>
            <fieldset style="background-color: goldenrod; height: auto; width: 30px">
                <%: Html.Telerik().ComboBox()
                                      .Name("nodesList")
                                      .AutoFill(true)
                                      .BindTo(DropDownListHelpers.ListOfNodes())
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 300) })
                                      .Filterable(filtering => filtering.FilterMode(AutoCompleteFilterMode.Contains))
                                      .HighlightFirstMatch(true)
                                      .ClientEvents(events => events.OnClose("nodesListEvent"))
                    
                %>
            </fieldset>
        </td>
        <td style="width: 33.3%">
            <div class="editor-label">
                <% if (ViewData["NodeChooserPosition"] != null)
                   { %>
                <%: ViewData["NodeChooserPosition"].ToString()%>
                <% } %>
            </div>
            <div id="nodePositions-input">
            </div>
        </td>
        <td style="width: 33.3%">
            <div class="editor-label">
                <% if (ViewData["NodeChooserEmployee"] != null)
                   { %>
                <%: ViewData["NodeChooserEmployee"].ToString()%>
                <% } %>
            </div>
            <div id="positionEmployees-input">
            </div>
        </td>
    </tr>
</table>
<script type="text/javascript">

    function nodesListEvent() {

        var comboBox = $("#nodesList").data("tComboBox");
        var nodeId = comboBox.value();

        $.ajax({
            url: '<%:Url.Action("GetNodePositions", "Home", new { area = ""})%>/',
            type: "POST",
            data: { nodeId: nodeId },
            success: function (result) {
                if (result.Success) {
                    $('#nodePositions-input').html(result.PartialViewHtml);
                    $('#nodePositions-input').fadeIn('fast');
                    $('#positionEmployees-input').fadeOut('fast');

                    $.ajax({
                        url: '<%:Url.Action("SetRelatedNodeToTheSession", "Home", new { area = ""})%>/',
                        type: "POST",
                        data: { nodeId: nodeId }
                    });
                }
            }
        });
    }

    function nodePositionsEvent() {

        var comboBox = $("#nodePositions").data("tComboBox");
        var positionComboBoxId = comboBox.value();

        $.ajax({
            url: '<%:Url.Action("GetPositionEmployees", "Home", new { area = ""})%>/',
            type: "POST",
            data: { positionId: positionComboBoxId },
            success: function (result) {
                if (result.Success) {
                    $('#positionEmployees-input').html(result.PartialViewHtml);
                    $('#positionEmployees-input').fadeIn('fast');
                    $.ajax({
                        url: '<%:Url.Action("SetRelatedPositionToTheSession", "Home", new { area = ""})%>/',
                        type: "POST",
                        data: { positionId: positionComboBoxId }
                    });
                }
            }
        });
    }

    function positionEmployeesEvent() {

        var comboBox = $("#positionEmployees").data("tComboBox");
        var employeeComboBoxId = comboBox.value();

        $.ajax({
            url: '<%:Url.Action("SetRelatedEmployeeToTheSession", "Home", new { area = ""})%>/',
            type: "POST",
            data: { employeeId: employeeComboBoxId }
        });
    }
    
</script>
