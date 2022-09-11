<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<table style="margin-left:0; margin-top:0; height:auto">
    <tr style="height: 100%; width:100%" >
        <td style="width: 30%; vertical-align: top">
            <div id="container">
                <%Html.RenderPartial("Tree/NodePositionsTree");%>
            </div>
        </td>
        <td style="width: 70%; vertical-align: top">
            <div id="NodePositions">
                <fieldset class="ParentFieldset">
                    <legend class="ParentLegend"><%: Resources.Areas.OrgChart.ValueObjects.AssignEmployeeToPosition.AssignEmployeeToPositionModel.EmployeeWeightFieldsetTitle %></legend>
                    <%Html.RenderPartial("DropDownLists/PositionEmployeesList");%>
                    <div id="WeightTextBoxDiv" hidden="true">
                        <%Html.RenderPartial("WeightTextBox");%>
                    </div>
                    <br/>
                    <table width="100%">
                        <tr>
                            <td>
                                <input type="button" onclick=" AssignEmployeeToPosition(); " value=<%: Resources.Shared.Buttons.Function.Assign %>, style="width: auto" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <div id="PositionFulfillmentGrid" style="overflow: auto">
                <fieldset class="ParentFieldset">
                    <legend class="ParentLegend"><%: Resources.Areas.OrgChart.ValueObjects.AssignEmployeeToPosition.AssignEmployeeToPositionModel.PositionFulfillmentTitle %></legend>
                        <%Html.RenderPartial("~/Areas/OrganizationChart/Views/PositionFulfillment/Grid.ascx");%>
                </fieldset>
            </div>
            <br/>
            <div id="result" style="overflow: auto">
            </div>
        </td>
    </tr>
    
</table>

<script type="text/javascript">

    var positionId = 0;
    var employeeId = 0;
    var weight = 0;
    var itemValue = "";

    function onNodeClicked(e) {

        EmptyDataEntryFields();

        var treeview = $('#NodePositionsTreeView').data('tTreeView');
        itemValue = treeview.getItemValue(e.item);

        if (itemValue != "Node") {
            positionId = itemValue;
            document.getElementById('result').style.visibility = 'visible';
            FillDataGrid();
        }
        else {
            positionId = 0;
            document.getElementById('result').style.visibility = 'hidden';
        }
            
    }

    function employeesValueChanged() {

        var comboBox = $("#AutoCompleteEmployeesComboBox").data("tComboBox");
        if (comboBox.value() != null && comboBox.value() != 0) {
            employeeId = comboBox.value();

            $.ajax({
                url: '<%:Url.Action("IsPrimaryPositionFulfillment", "AssignEmployeeToPosition", new { area = "OrganizationChart"})%>/',
                type: "POST",
                data: { employeeId: employeeId },
                success: function (result) {
                    if (result.Success) {
                        $("#WeightTextBoxDiv").attr('hidden', true);
                    }
                    else {
                        $("#WeightTextBoxDiv").attr('hidden', false);
                    }
                }
            });
        }
            
        else
            employeeId = 0;
    }

    function weightValueChanged(e) {

        if (e.newValue != null && e.newValue != 0)
            weight = e.newValue;
        else
            weight = 0;
    }

    function AssignEmployeeToPosition() {

        $.ajax({
            url: '<%: Url.Action("AddPositionFulfillment", "AssignEmployeeToPosition")%>/', type: "POST",
            data: {
                positionId: positionId,
                employeeId: employeeId,
                weight: weight
            },
            success: function (result) {
                alert(result.Msg);
                EmptyDataEntryFields();
                FillDataGrid();
            }

        });
    }

    function FillDataGrid() {

        $.ajax({
            url: '<%:Url.Action("GetPositionFulfillments", "AssignEmployeeToPosition", new { area = "OrganizationChart"})%>/',
            type: "POST",
            data: { id: positionId },
            success: function (result) {
                if (result.Success) {
                    $('#PositionFulfillmentGrid').html(result.PartialViewHtml);
                    $('#PositionFulfillmentGrid').fadeIn('fast');
                }
            }
        });
    }
    
    function EmptyDataEntryFields() {

        $('#AutoCompleteEmployeesComboBox').data('tComboBox').value('');
        $('#AutoCompleteEmployeesComboBox').data('tComboBox').text('');
        employeeId = 0;

        $('#Weight').data('tTextBox').value('');
        weight = 0;
    }

    </script>
