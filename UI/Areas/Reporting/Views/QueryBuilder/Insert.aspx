<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Reporting/Views/Shared/ReportingViewers.Master"
    Inherits="System.Web.Mvc.ViewPage<Souccar.ReportGenerator.Domain.QueryBuilder.Report>" %>

<%@ Import Namespace="UI.Areas.Reporting.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%using (Html.BeginForm("Save", "QueryBuilder"))
      {%>
    <fieldset class="ParentFieldset">
        <legend class="ParentLegend">
            <%: Resources.Areas.ReportGenerator.Domain.Entities.Report.ReportModel.CreatePageTitle %></legend>
        <table style="margin-left: 1px">
            <tr>
                <td>
                    <%:Html.ValidationMessage("InternalError")%>
                </td>
            </tr>
            <tr>
                <td>
                    <%:Html.ValidationMessageFor(model => model.Name)%>
                </td>
            </tr>
            <tr>
                <td>
                    <%:Html.ValidationMessageFor(model => model.Template)%>
                </td>
            </tr>

            <% for (int i = 0; i < ViewContext.ViewData.ModelState.Count(x => x.Key.Contains("InvalidNodes")); i++)
               {%>
            <tr>
                <td>
                    <%:Html.ValidationMessage("InvalidNodes"+i)%>

                </td>
            </tr>
            <% } %>
        </table>
        <input type="submit" value="<%: Resources.Shared.Buttons.Function.Save %>" onclick=" DisableSaveButton(); " />
        <%:Html.HiddenFor(model => model.Id)%>

        <table width="100%">
            <tr>
                <td style="width: 25%; vertical-align: top;">
                    <div class="editor-label">
                        <%: Html.LabelFor(model => model.Name) %>
                    </div>
                    <div class="editor-field">
                        <%:Html.EditorFor(model => model.Name)%>
                    </div>
                </td>
                <td style="width: 25%; vertical-align: top;">
                    <div class="editor-label">
                        <%: Html.LabelFor(model => model.Template) %>
                    </div>
                    <div class="editor-field">
                        <%:Html.Telerik().DropDownListFor(model => model.Template.Id)
                                        .BindTo(ReportingDropDownListHelpers.ListOfReportTemplates)
                                .HtmlAttributes(new {style = string.Format("width:{0}px", 200)})
                        %>
                    </div>
                </td>
                <td style="width: 25%; vertical-align: top;">
                    <div class="editor-label-required">
                        <%: Html.LabelFor(model => model.QueryTree.FullClassName) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.Telerik().ComboBoxFor(model => model.QueryTree.FullClassName)
                            .BindTo(ReportingDropDownListHelpers.ListOfAggregateClasses)
                            .ClientEvents(e => e.OnChange("aggregateChanged"))
                            .ClientEvents(e => e.OnLoad("aggregateLoad"))
                            .HtmlAttributes(new {style = string.Format("width:{0}px", 200)})
                        %>
                    </div>
                </td>
                <td style="width: 25%; vertical-align: top;"></td>
            </tr>
            <tr>
                <td>
                    <div id="QueryTree">
                    </div>
                </td>
                <td colspan="2">
                    <div id="QueryLeaf">
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <a href="<%:Url.Action("Index", "QueryBuilder")%>">
                        <input type="button" value="<%: Resources.Shared.Buttons.Function.Cancel %>" />
                    </a>
                </td>
                <td colspan="2">
                    <input type="submit" value="<%: Resources.Shared.Buttons.Function.Save %>" onclick="onReportSave()" />
                </td>
            </tr>
        </table>




    </fieldset>
    <script type="text/javascript">

        var lastTreeNodeSelectedValue = "";

        function onReportSave() {
            if (lastTreeNodeSelectedValue != "") {
                SaveQueryLeaf();
            }
        }

        function SaveQueryLeaf() {
            var lstSelectedFields = $('#lstSelectedFields' + ' option').toArray();
            var lstSelectedGroupingFields = $('#lstSelectedGroupingFields' + ' option').toArray();
            var lstSelectedSortingFields = $('#lstSelectedSortingFields' + ' option').toArray();

            var selectedFields = new Array();
            var selectedGroupingFields = new Array();
            var selectedSortingFields = new Array();
            var selectedSortDirection = new Array();

            for (var i = 0; i < lstSelectedFields.length; i++) {
                selectedFields.push(lstSelectedFields[i].value);
            }

            for (var y = 0; y < lstSelectedGroupingFields.length; y++) {
                selectedGroupingFields.push(lstSelectedGroupingFields[y].value);
            }



            for (var z = 0; z < lstSelectedSortingFields.length; z++) {
                selectedSortingFields.push(lstSelectedSortingFields[z].value);
                var indexOfAsc = lstSelectedSortingFields[z].text.indexOf("(Asc)");
                if (indexOfAsc == -1) {
                    selectedSortDirection.push("Desc");
                } else {
                    selectedSortDirection.push("Asc");
                }
            }

            nodefullClassPath = lastTreeNodeSelectedValue;

            var comboBox = $("#QueryTree_FullClassName").data("tComboBox");
            var aggregateFullClassName = comboBox.value();

            var selectedcmbLeafs = new Array();
            var selectedcmbOperators = new Array();
            var selectedtxtFilterValues = new Array();
            var cmbLeaf;
            var cmbOperator;
            for (var a = 1; a <= filterCounter; a++) {
                cmbLeaf = $("#cmbLeafs_" + a).data("tComboBox");
                if (cmbLeaf == null) {
                    continue;
                }
                selectedcmbLeafs.push(cmbLeaf.value());
                cmbOperator = $("#cmbOperator_" + a).data("tComboBox");
                selectedcmbOperators.push(cmbOperator.value());

                selectedtxtFilterValues.push(document.getElementById("txtFilterValue_" + a).value);
            }


            var aggregateFields = new Array();
            var aggregateFunction = new Array();
            var aggregateOperators = new Array();
            var aggregateValues = new Array();
            var cmbAggregate;
            var cmbAggregateOperator;
            var cmbAggregateFunction;
            for (var q = 1; q <= aggregateFilterCounter; q++) {
                cmbAggregate = $("#cmbAggregates_" + q).data("tComboBox");
                if (cmbAggregate == null) {
                    continue;
                }
                aggregateFields.push(cmbAggregate.value());

                cmbAggregateOperator = $("#cmbAggregateOperator_" + q).data("tComboBox");
                aggregateOperators.push(cmbAggregateOperator.value());

                cmbAggregateFunction = $("#cmbAggregateFunction_" + q).data("tComboBox");
                aggregateFunction.push(cmbAggregateFunction.value());

                aggregateValues.push(document.getElementById("txtAggregateFilterValue_" + q).value);
            }

            $.ajax({
                url: '<%:Url.Action("UpdateQueryLeaf", "QueryBuilder", new { area = "Reporting"})%>/',
                type: "POST",
                async: false,
                traditional: true,
                data: {
                    nodefullClassPath: nodefullClassPath, aggregateFullClassName: aggregateFullClassName, selectedFields: selectedFields,
                    selectedGroupingFields: selectedGroupingFields, selectedSortingFields: selectedSortingFields, selectedSortDirection: selectedSortDirection,
                    selectedcmbLeafs: selectedcmbLeafs, selectedcmbOperators: selectedcmbOperators, selectedtxtFilterValues: selectedtxtFilterValues,
                    aggregateFields:aggregateFields, aggregateFunction:aggregateFunction, aggregateOperators:aggregateOperators, aggregateValues:aggregateValues,
                    lastTreeNodeSelectedValue: lastTreeNodeSelectedValue
                },
                success: function (result) {
                    if (result.Success == false) {
                        alert('Error While Update Please Try Again!');
                    }
                }
            });
        }
        
        function aggregateLoad() {
            var comboBox = $("#QueryTree_FullClassName").data("tComboBox");
            var aggregateFullClassName = comboBox.value();
            if (aggregateFullClassName == '') {
                return;
            }
            $.ajax({
                url: '<%= Url.Action("LoadReservedQueryTree","QueryBuilder") %>/', type: "POST",
                data: {},
                success: function (result) {
                    if (result.Success != null) {
                        if (result.Success == false) {
                            alert(result.Message);
                        }
                        else {
                            $('#QueryTree').html(result.PartialViewHtml);
                            $('#QueryLeaf').html('');
                        }
                    }
                }
            });
        }

        function aggregateChanged() {
            lastTreeNodeSelectedValue = "";
            var comboBox = $("#QueryTree_FullClassName").data("tComboBox");
            var aggregateFullClassName = comboBox.value();
            if (aggregateFullClassName == '') {
                return;
            }
            $.ajax({
                url: '<%= Url.Action("LoadQueryTree","QueryBuilder") %>/', type: "POST",
                data: { aggregateFullClassName: aggregateFullClassName },
                success: function (result) {
                    if (result.Success != null) {
                        if (result.Success == false) {
                            alert(result.Message);
                        }
                        else {
                            $('#QueryTree').html(result.PartialViewHtml);
                            $('#QueryLeaf').html('');
                        }
                    }
                }
            });
        }

        function TreeNodeSelected(e) {
            if (lastTreeNodeSelectedValue != "") {
                SaveQueryLeaf();
            }

            var treeview = $('#TreeView').data('tTreeView');
            var nodefullClassPath = treeview.getItemValue(e.item);
            lastTreeNodeSelectedValue = nodefullClassPath;

            var comboBox = $("#QueryTree_FullClassName").data("tComboBox");
            var aggregateFullClassName = comboBox.value();

            $.ajax({
                url: '<%= Url.Action("LoadQueryLeaf","QueryBuilder") %>/', type: "POST",
                data: { nodefullClassPath: nodefullClassPath, aggregateFullClassName: aggregateFullClassName },
                success: function (result) {
                    if (result.Success != null) {
                        if (result.Success == false) {
                            alert(result.Message);
                        }
                        else {
                            $('#QueryLeaf').html(result.PartialViewHtml);
                        }
                    }
                }
            });
        }

        function RemoveFilterRow(obj) {
            var rowNumber = $(obj).attr('id').split('_')[1];
            $("#divFilterRow_" + rowNumber).remove();
        }

        function RemoveAggregateFilterRow(obj)
        {
            var rowNumber = $(obj).attr('id').split('_')[1];
            $("#divAggregateFilterRow_" + rowNumber).remove();
        }

        function OrderListItems(tabe, clickedButton) {
            var rightList = '';
            var selectedItems;
            if (tabe == 'fields') {
                rightList = '#lstSelectedFields';
            } else if (tabe == 'Grouping') {
                rightList = '#lstSelectedGroupingFields';
            }
            else if (tabe == 'Sorting') {
                rightList = '#lstSelectedSortingFields';
            }

            selectedItems = $(rightList + ' option').toArray();

            if (clickedButton == 'Up') {
                for (var i = 0; i < selectedItems.length; i++) {
                    if (selectedItems[i].selected) {

                        if (i == 0) {
                            continue;
                        }
                        var tmp = selectedItems[i];
                        selectedItems[i] = selectedItems[i - 1];
                        selectedItems[i - 1] = tmp;
                    }
                }
            }
            else if (clickedButton == 'Down') {
                for (var y = selectedItems.length - 1; y >= 0; y--) {
                    if (selectedItems[y].selected) {

                        if (y == selectedItems.length - 1) {
                            continue;
                        }
                        var tmp1 = selectedItems[y];
                        selectedItems[y] = selectedItems[y + 1];
                        selectedItems[y + 1] = tmp1;
                    }
                }
            }
            else if (clickedButton == 'Ordering') {
                selectedItems = $(rightList + ' :selected').toArray();
                SetAscDescToSelectedItems(selectedItems);
            }

            $(rightList).append(selectedItems);
        }

        function SetAscDescToSelectedItems(selectedItems) {
            for (var x = 0; x < selectedItems.length; x++) {
                var indexOfAsc = selectedItems[x].text.indexOf("(Asc)");
                var indexOfDesc = selectedItems[x].text.indexOf("(Desc)");

                if (indexOfAsc == -1 && indexOfDesc == -1) {
                    selectedItems[x].text += " (Asc)";
                } else if (indexOfAsc != -1) {
                    selectedItems[x].text = selectedItems[x].text.replace(' (Asc)', ' (Desc)');
                }
                else if (indexOfDesc != -1) {
                    selectedItems[x].text = selectedItems[x].text.replace(' (Desc)', ' (Asc)');
                }
            }
        }

        function RemoveAscDescToSelectedItems(selectedItems) {
            for (var x = 0; x < selectedItems.length; x++) {
                var indexOfAsc = selectedItems[x].text.indexOf("(Asc)");
                var indexOfDesc = selectedItems[x].text.indexOf("(Desc)");
                if (indexOfAsc != -1) {
                    selectedItems[x].text = selectedItems[x].text.replace(' (Asc)', '');
                }
                else if (indexOfDesc != -1) {
                    selectedItems[x].text = selectedItems[x].text.replace(' (Desc)', '');
                }
            }
        }

        function selectClicked(tabe, clickedButton) {
            var rightList = '';
            var leftList = '';
            var selectedItems;
            if (tabe == 'fields') {
                rightList = '#lstSelectedFields';
                leftList = '#lstAllFields';
            }
            else if (tabe == 'Grouping') {
                rightList = '#lstSelectedGroupingFields';
                leftList = '#lstAllGroupingFields';
            }
            else if (tabe == 'Sorting') {
                rightList = '#lstSelectedSortingFields';
                leftList = '#lstAllSortingFields';
            }

            if (clickedButton == '>') {
                selectedItems = $(leftList + ' :selected').toArray();

                if (tabe == 'Sorting') {
                    SetAscDescToSelectedItems(selectedItems);
                }


                $(rightList).append(selectedItems);
            }
            else if (clickedButton == '<') {
                selectedItems = $(rightList + ' :selected').toArray();

                if (tabe == 'Sorting') {
                    RemoveAscDescToSelectedItems(selectedItems);
                }

                $(leftList).append(selectedItems);
            }
            else if (clickedButton == '>>') {
                selectedItems = $(leftList + ' option').toArray();

                if (tabe == 'Sorting') {
                    SetAscDescToSelectedItems(selectedItems);
                }

                $(rightList).append(selectedItems);
            }
            else if (clickedButton == '<<') {
                selectedItems = $(rightList + ' option').toArray();
                if (tabe == 'Sorting') {
                    RemoveAscDescToSelectedItems(selectedItems);
                }

                $(leftList).append(selectedItems);
            }
        }

        var filterCounter = 0;
        var aggregateFilterCounter = 0;
        function AddFilterRow() {
            var lastFilterValueObject = document.getElementById("txtFilterValue_" + filterCounter);
            var lastFilterLeafObject = $("#cmbLeafs_" + filterCounter).data("tComboBox");
            var lastFilterOperatorObject = $("#cmbOperator_" + filterCounter).data("tComboBox");


            if (lastFilterValueObject != null && lastFilterLeafObject != null && lastFilterOperatorObject != null) {
                if (lastFilterValueObject.value == "" || lastFilterLeafObject.value() == "" || lastFilterOperatorObject.value() == "") {
                    alert('<%: Resources.Areas.ReportGenerator.Domain.Entities.Report.ReportRules.ReqLastFilterValue%>');
                    return;
                }
            }

            var treeview = $('#TreeView');
            var selectedItem = treeview.find('.t-state-selected').closest('.t-item');
            var nodefullClassPath = treeview.data('tTreeView').getItemValue(selectedItem);

            $.ajax({
                url: '<%= Url.Action("LoadFilterRow","QueryBuilder") %>/', type: "POST",
                data: { nodefullClassPath: nodefullClassPath },
                success: function (result) {
                    filterCounter = filterCounter + 1; $('#divFilters').append('<div id=divFilterRow_' + filterCounter + '>' + result.PartialViewHtml + '</div>');
                    $('#cmbLeafs').attr('id', 'cmbLeafs_' + filterCounter);
                    $('#cmbOperator').attr('id', 'cmbOperator_' + filterCounter);
                    $('#txtFilterValue').attr('id', 'txtFilterValue_' + filterCounter);
                    $('#RemoveFilterRow').attr('id', 'RemoveFilterRow_' + filterCounter);
                    $('#divFilterValue').attr('id', 'divFilterValue_' + filterCounter);

                }
            });
        }

        function AddAggregateFilterRow()
        {
            var lastFilterValueObject = document.getElementById("txtAggregateFilterValue_" + aggregateFilterCounter);
            var lastFilterLeafObject = $("#cmbAggregates_" + aggregateFilterCounter).data("tComboBox");
            var lastFilterFunctionObject = $("#cmbAggregateFunction_" + aggregateFilterCounter).data("tComboBox");
            var lastFilterOperatorObject = $("#cmbAggregateOperator_" + aggregateFilterCounter).data("tComboBox");


            if (lastFilterValueObject != null && lastFilterLeafObject != null && lastFilterOperatorObject != null &&
                lastFilterFunctionObject != null) {
                if (lastFilterValueObject.value == "" || lastFilterLeafObject.value() == "" || lastFilterOperatorObject.value() == "" ||
                    lastFilterFunctionObject.value() == "") {
                    alert('<%: Resources.Areas.ReportGenerator.Domain.Entities.Report.ReportRules.ReqLastFilterValue%>');
                    return;
                }
            }

            var treeview = $('#TreeView');
            var selectedItem = treeview.find('.t-state-selected').closest('.t-item');
            var nodefullClassPath = treeview.data('tTreeView').getItemValue(selectedItem);

            $.ajax({
                url: '<%= Url.Action("LoadAggregateFilterRow","QueryBuilder") %>/', type: "POST",
                data: { nodefullClassPath: nodefullClassPath },
                success: function (result)
                {
                    aggregateFilterCounter = aggregateFilterCounter + 1;
                    $('#divAggregateFilters').append('<div id=divAggregateFilterRow_' + aggregateFilterCounter + '>' + result.PartialViewHtml + '</div>');
                    $('#cmbAggregates').attr('id', 'cmbAggregates_' + aggregateFilterCounter);
                    $('#cmbAggregateOperator').attr('id', 'cmbAggregateOperator_' + aggregateFilterCounter);
                    $('#cmbAggregateFunction').attr('id', 'cmbAggregateFunction_' + aggregateFilterCounter);
                    $('#txtAggregateFilterValue').attr('id', 'txtAggregateFilterValue_' + aggregateFilterCounter);
                    $('#RemoveAggregateFilterRow').attr('id', 'RemoveAggregateFilterRow_' + aggregateFilterCounter);
                    $('#divAggregateFilterValue').attr('id', 'divAggregateFilterValue_' + aggregateFilterCounter);

                }
            });
        }
        
        function LeafChanged(e) {
            var leafPropertyFullName = e.value;
            var filterRowNumber = $(this).attr('id').split('_')[1];

            var treeview = $('#TreeView');
            var selectedItem = treeview.find('.t-state-selected').closest('.t-item');
            var nodefullClassPath = treeview.data('tTreeView').getItemValue(selectedItem);

            $.ajax({
                url: '<%= Url.Action("GetAvailableFilterOperators","QueryBuilder") %>/', type: "POST",
                data: { leafPropertyFullName: leafPropertyFullName, nodefullClassPath: nodefullClassPath },
                success: function (result) {

                    var comboBox1 = $("#cmbOperator_" + filterRowNumber).data("tComboBox");
                    comboBox1.dataBind(result.Data);

                    if (result.ContentType == "DateTime") {
                        $('#divFilterValue_' + filterRowNumber).html('');
                        $('#divFilterValue_' + filterRowNumber).html('<input type="text" id="txtFilterValue_' + filterRowNumber + '">');
                        $('#txtFilterValue_' + filterRowNumber).tDatePicker({ format: 'dd/MM/yyyy' });
                        $('#txtFilterValue_' + filterRowNumber).attr('disabled', 'disabled');
                    }
                    else {
                        $('#divFilterValue_' + filterRowNumber).html('');
                        $('#divFilterValue_' + filterRowNumber).html('<input type="text" id="txtFilterValue_' + filterRowNumber + '">');

                        if (result.ContentType == "Byte" || result.ContentType == "SByte" || result.ContentType == "Int16" || result.ContentType == "UInt16" ||
                            result.ContentType == "Int32" || result.ContentType == "UInt32" || result.ContentType == "Int64" || result.ContentType == "UInt64") {
                            $('#txtFilterValue_' + filterRowNumber).keypress(function (event) {
                                ValidateIntigerFilterValue(event, filterRowNumber);
                            });

                        }
                        else if (result.ContentType == "Double" || result.ContentType == "Single" || result.ContentType == "Decimal") {
                            $('#txtFilterValue_' + filterRowNumber).keypress(function (event) {
                                ValidateFloatFilterValue(event, filterRowNumber);
                            });
                            $('#txtFilterValue_' + filterRowNumber).focusout(function () {
                                CheckLastNumericDot(filterRowNumber);
                            });

                        }
                    }
                }
            });
        }

        function CheckLastNumericDot(filterRowNumber) {
            var filterValue = $('#txtFilterValue_' + filterRowNumber).val();

            if (filterValue.indexOf('.') == filterValue.length - 1) {
                $('#txtFilterValue_' + filterRowNumber).val(filterValue.substring(0, filterValue.length - 1));
            }
        }

        function ValidateIntigerFilterValue(event, filterRowNumber) {
            if (event.keyCode < 48 || event.keyCode > 57) {
                event.preventDefault();
            }
        }

        function ValidateFloatFilterValue(event, filterRowNumber) {
            if ((event.keyCode < 48 && event.keyCode != 46) || event.keyCode > 57) {
                event.preventDefault();
            }
            else
                if (event.keyCode == 46) {
                    var filterValue = $('#txtFilterValue_' + filterRowNumber).val();
                    if (filterValue.length == 0) {
                        event.preventDefault();
                    }
                    if (filterValue.indexOf('.') > -1) {
                        event.preventDefault();
                    }
                }
        }
    </script>
    <%
      }%>
</asp:Content>
