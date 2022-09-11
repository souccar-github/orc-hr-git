<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.ProjectManagment.ValueObjects.TeamMember>" %>
<%@ Import Namespace="UI.Helpers" %>
<script type="text/javascript">

    function TreeOnSelect(nodeId) {

        $.ajax({
            url: '<%:Url.Action("GetPositions", "TeamMember")%>/',
            type: "POST",
            data: { nodeId: nodeId },
            dataType: "json",
            success: function (result) {
                if (result.Success) {
                    $('#NodePositions').html(result.PartialViewHtml);
                    $('#NodePositions').fadeIn('slow');
                    //nodePositionsEvent(-1);

                    $.ajax({
                        url: '<%:Url.Action("SetRelatedNodeToTheSession", "TeamMember")%>/',
                        type: "POST",
                        data: { nodeId: nodeId }
                    });
                }
            }
        });
    }

    function nodePositionsEvent(x) {

        var id;
        if (x == -1) {
            id = x;
        } else {
            var comboBox = $("#nodePositions").data("tComboBox");
            id = comboBox.value();
        }
       
        $.ajax({
            url: '<%:Url.Action("GetEmployees", "TeamMember")%>/',
            type: "POST",
            data: { positionId: id },
            success: function (result) {
                if (result.Success) {
                    $('#PositionEmployees').html(result.PartialViewHtml);
                    $('#PositionEmployees').fadeIn('slow');

                    $.ajax({
                        url: '<%:Url.Action("SetRelatedPositionToTheSession", "TeamMember")%>/',
                        type: "POST",
                        data: { positionId: id }
                    });
                }
            }
        });
    }

    function positionEmployeeEvent(x) {

        var id;
        if (x == -1) {
            id = x;
        } else {
            var comboBox = $("#positionEmployees").data("tComboBox");
            id = comboBox.value();
        }

       $.ajax({
            url: '<%:Url.Action("SetRelatedEmployeeToTheSession", "TeamMember")%>/',
            type: "POST",
            data: { employeeId: id }
        });
    }

    function BuildContextMenu() {

    }

</script>
<table>
    <tr>
        <td>
            <%:Html.ValidationMessageFor(model => model.Id)%>
        </td>
    </tr>
    <tr>
        <td>
            <%:Html.ValidationMessageFor(model => model.Employee)%>
        </td>
    </tr>
</table>
<fieldset class="ParentFieldset">
    <table width="100%">
        <tr>
            <td>
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
                                url: '<%:Url.Action("BackOneLevel", "TeamMember")%>/',
                                type: "POST",
                                data: { reset: x },
                                success: function (result) {
                                    if (result.Success) {
                                        $("#center-container").html(result.PartialViewHtml);
                                    }
                                }
                            });
                        }
                    </script>
                </div>
            </td>
            <td>
                <div id="container">
                    <div id="center-container">
                        <%
                            Html.RenderPartial("Components/Tree");%>
                    </div>
                    <div id="log">
                    </div>
                </div>
            </td>
            <td>
                <%:Html.HiddenFor(model => model.Id)%>
                <%:Html.HiddenFor(model => model.Position)%>
                <%:Html.HiddenFor(model => model.Employee.Id)%>
                <div class="editor-label-required">
                    <%:Html.LabelFor(model => model.Position)%>
                </div>
                <div id="NodePositions">
                    <fieldset style="height: auto; width: 300px">
                        <% if (Model.IsTransient())
                           { %>
                        <%:Html.Telerik().ComboBoxFor(model => model.Position.Id)
                           .Name("nodePositions")
                           .AutoFill(true)
                           .HtmlAttributes(new {style = string.Format("width:{0}px", 300)})
                           .Filterable(filtering => filtering.FilterMode(AutoCompleteFilterMode.Contains))
                           .HighlightFirstMatch(true)
                           .SelectedIndex(0)
                           .ClientEvents(events => events.OnClose("nodePositionsEvent"))
                        %>
                        <% }
                           else
                           { %>
                        <%:Html.Telerik().ComboBoxFor(model => model.Position.Id)
                           .Name("nodePositions")
                           .AutoFill(true)
                           .BindTo(DropDownListHelpers.ListOfSelectedNodePosition(Model.Node.Id))
                           .HtmlAttributes(new {style = string.Format("width:{0}px", 300)})
                           .Filterable(filtering => filtering.FilterMode(AutoCompleteFilterMode.Contains))
                           .HighlightFirstMatch(true)
                           .SelectedIndex(0)
                           .ClientEvents(events => events.OnClose("nodePositionsEvent"))
                        %>
                        <% } %>
                    </fieldset>
                </div>
                <div class="editor-label-required">
                    <%:Html.LabelFor(model => model.Employee)%>
                </div>
                <div id="PositionEmployees">
                    <fieldset style="height: auto; width: 300px">
                        <% if (Model.IsTransient())
                           { %>
                        <%:Html.Telerik().ComboBoxFor(model => model.Employee.Id)
                                  .Name("positionEmployees")
                                  .AutoFill(true)
                                  .HtmlAttributes(new {style = string.Format("width:{0}px", 300)})
                                  .Filterable(filtering => filtering.FilterMode(AutoCompleteFilterMode.Contains))
                                  .HighlightFirstMatch(true)
                                  .SelectedIndex(0)
                                  .ClientEvents(events => events.OnClose("positionEmployeeEvent"))
                        %>
                        <% }
                           else
                           { %>
                        <%:Html.Telerik().ComboBoxFor(model => model.Employee.Id)
                           .Name("positionEmployees")
                           .AutoFill(true)
                           .BindTo(DropDownListHelpers.ListOfSelectedPositionEmployees(Model.Position.Id))
                           .HtmlAttributes(new {style = string.Format("width:{0}px", 300)})
                           .Filterable(filtering => filtering.FilterMode(AutoCompleteFilterMode.Contains))
                           .HighlightFirstMatch(true)
                           .ClientEvents(events => events.OnClose("positionEmployeeEvent"))
                        %>
                        <% } %>
                    </fieldset>
                </div>
            </td>
            <td style="width: 100%; vertical-align: top">
                <div class="editor-label-required">
                    <%:Html.LabelFor(model => model.IsEvaluator)%>
                </div>
                <div class="editor-field">
                    <%:Html.EditorFor(model => model.IsEvaluator)%>
                </div>
            </td>
        </tr>
    </table>
</fieldset>
<table width="100%">
    <tr>
        <td style="width: 10%; vertical-align: top">
            <input type="submit" value="<%: Resources.Shared.Buttons.Function.Save %>" onclick=" DisableSaveButton() " />
        </td>
    </tr>
</table>
