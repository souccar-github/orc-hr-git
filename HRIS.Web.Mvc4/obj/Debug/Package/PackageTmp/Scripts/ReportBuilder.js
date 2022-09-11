var CurrentFiltersFieldsNames = new Array();
var FiltersStringOperators = new Array();
var FiltersNumericAndDateOperators = new Array();
var FielterIdCounter = 0;
var IsFilterAddMode = 0;
var OriginalEditedFilterModel;

var CurrentAggregateFiltersFieldsNames = new Array();
var CurrentAggregateOperationsFieldsNames = new Array();
var AggregateFilterOperators = new Array();
var AggregateOperationsSubProperty = new Array();
var AggregateFilterSubProperty = new Array();
var AggregateFilterFunctions = new Array();
var AggregateOperationsFunctions = new Array();
var AggregateFielterIdCounter = 0;
var IsAggregateFilterAddMode = 0;
var OriginalEditedAggregateFilterModel;
var AggregateOperationIdCounter = 0;
var IsAggregateOperationAddMode = 0;
var OriginalEditedAggregateOperationModel;

var dateFormat = "dd/MM/yyyy";
var SelectedTreeNode;
var QueryTree;
var TreeNodeSelectedFirstTime = 1;

function SubmitQueryTree() {
    var treeview = $("#tree").data("kendoTreeView");
    var root = $("#tree").find('.k-item').first();
    treeview.select(root);
    treeview.trigger('select', { node: root });
    treeview.select($());
    QueryTree = SelectedTreeNode;
    $.ajax(
        {
            url: window.applicationpath + "ReportGenerator/ReportBuilder/SaveQueryTree",
            type: "POST",
            data: JSON.stringify({ queryTree: QueryTree, requestInformation: window.requestInformation }),
            contentType: 'application/json',
            success: function () {
            }
        });
}

function treeview_databound(e) {
    var treeview = e.sender;
    var root = $("#tree").find('.k-item').first();
    treeview.select(root);
    treeview.trigger('select', { node: root });
    treeview.select($());
}

function Aggregates_OnChange() {
    var aggregate = $("#Aggregates").val();
    $.ajax(
            {
                url: window.applicationpath + "ReportGenerator/ReportBuilder/GetDataSources",
                type: "POST",
                data: { aggregateClass: aggregate },
                success: function (response) {
                    QueryTree = response;
                    var treeData = new Array();
                    treeData.push(QueryTree);
                    var treeDataSource = new kendo.data.HierarchicalDataSource({
                        data: treeData,
                        schema: {
                            model: {
                                id: "FullClassPath",
                                children: "Nodes",
                                hasChildren: "Nodes.length>0"
                            }
                        }
                    });

                    if ($("#tree").data("kendoTreeView") != null) {
                        $("#tree").data("kendoTreeView").setDataSource(treeDataSource);
                    } else {
                        $("#tree").kendoTreeView({
                            dataSource: treeDataSource,
                            dataTextField: ["DisplayName", "DisplayName"],
                            select: onSelectNode,
                            dataBound: treeview_databound
                        });
                    }
                }
                ,
                error: function (data) {
                    alert(data);
                }
            }
        );
}
function refreshSelectList(item) {
    var template = kendo.template($("#fieldListTemplate").html());
    var ordreList = _.sortBy(item.Leaves, function (leaf) { return leaf.Selected; });
    $("#fields").html(template(ordreList));
    template = kendo.template($("#selectedFieldTemplate").html());
    $("#selectedFields").html(template(ordreList));
}

function refreshGroupList(item) {
    var template = kendo.template($("#groupListTemplate").html());
    var ordreList = _.sortBy(item.Leaves, function (leaf) { return leaf.GroupDescriptor.GroupByOrder; });
    $("#groups").html(template(ordreList));
    template = kendo.template($("#selectedGroupTemplate").html());
    $("#selectedGroups").html(template(ordreList));
}

function refreshSortList(item) {
    var template = kendo.template($("#sortListTemplate").html());
    var ordreList = _.sortBy(item.Leaves, function (leaf) { return leaf.SortDescriptor.SortOrder; });
    $("#sorts").html(template(ordreList));
    template = kendo.template($("#selectedSortTemplate").html());
    $("#selectedSorts").html(template(ordreList));
}

function onSelectNode(e) {
    SelectedTreeNode = this.dataItem(e.node);
    if (TreeNodeSelectedFirstTime == 1) {
        TreeNodeSelectedFirstTime = 0;
        SelectedTreeNode.Nodes = QueryTree.Nodes;
        return;
    }
    var item = SelectedTreeNode;
    refreshSelectList(item);
    refreshGroupList(item);
    refreshSortList(item);
    $("#tabstrip").show();

    IntializeFiltersDataSource(item);
    IntializeAggregateFiltersDataSource(item);
    IntializeAggregateOperationsDataSource(item);
}

function GetFilterFieldNameById(id) {
    for (var i = 0; i < CurrentFiltersFieldsNames.length; i++) {
        if (CurrentFiltersFieldsNames[i].value == id) {
            return CurrentFiltersFieldsNames[i].text;
        }
    }
    return id;
}

function GetFilterOperatorNameById(id) {
    for (var i = 0; i < FiltersStringOperators.length; i++) {
        if (FiltersStringOperators[i].value == id) {
            return FiltersStringOperators[i].text;
        }
    }

    for (var x = 0; x < FiltersNumericAndDateOperators.length; x++) {
        if (FiltersNumericAndDateOperators[x].value == id) {
            return FiltersNumericAndDateOperators[x].text;
        }
    }

    return id;
}

function GetAggregateFilterFieldNameById(id) {
    for (var i = 0; i < CurrentAggregateFiltersFieldsNames.length; i++) {
        if (CurrentAggregateFiltersFieldsNames[i].value == id) {
            return CurrentAggregateFiltersFieldsNames[i].text;
        }
    }
    return id;
}
function GetAggregateFilterSubPropertyById(id) {
    for (var i = 0; i < AggregateFilterSubProperty.length; i++) {
        if (AggregateFilterSubProperty[i].value == id) {
            return AggregateFilterSubProperty[i].text;
        }
    }
    return id;
}
function GetAggregateFilterOperatorNameById(id) {
    for (var i = 0; i < AggregateFilterOperators.length; i++) {
        if (AggregateFilterOperators[i].value == id) {
            return AggregateFilterOperators[i].text;
        }
    }
    return id;
}

function GetAggregateFunctionsNameById(id) {
    for (var i = 0; i < AggregateFilterFunctions.length; i++) {
        if (AggregateFilterFunctions[i].value == id) {
            return AggregateFilterFunctions[i].text;
        }
    }
    return id;
}
function GetAggregateOperationsFieldNameById(id) {
    for (var i = 0; i < CurrentAggregateOperationsFieldsNames.length; i++) {
        if (CurrentAggregateOperationsFieldsNames[i].value == id) {
            return CurrentAggregateOperationsFieldsNames[i].text;
        }
    }
    return id;
}
function GetAggregateOperationsSubPropertyNameById(id) {
    for (var i = 0; i < AggregateOperationsSubProperty.length; i++) {
        if (AggregateOperationsSubProperty[i].value == id) {
            return AggregateOperationsSubProperty[i].text;
        }
    }
    return id;
}
function GetAggregateOperationsFunctionsNameById(id) {
    for (var i = 0; i < AggregateOperationsFunctions.length; i++) {
        if (AggregateOperationsFunctions[i].value == id) {
            return AggregateOperationsFunctions[i].text;
        }
    }
    return id;
}

function IntializeFiltersDataSource(selectedTreeNodeData) {
    var nodeFiltersDataSource = new Array();
    CurrentFiltersFieldsNames = new Array();
    FielterIdCounter = 0;
    IsFilterAddMode = 0;

    var type = "";
    for (var i = 0; i < selectedTreeNodeData.Leaves.length; i++) {

        if (selectedTreeNodeData.Leaves[i].PropertyType.indexOf("String") != -1) {
            type = "String";
        }
        else if (selectedTreeNodeData.Leaves[i].PropertyType.indexOf("Int") != -1 || selectedTreeNodeData.Leaves[i].PropertyType.indexOf("Double") != -1) {
            type = "Integer";
        }
        else if (selectedTreeNodeData.Leaves[i].PropertyType.indexOf("DateTime") != -1) {
            type = "DateTime";
        }

        var fieldName = {
            text: selectedTreeNodeData.Leaves[i].DisplayName,
            value: selectedTreeNodeData.Leaves[i].PropertyFullPath + "-" + type
        };
        CurrentFiltersFieldsNames.push(fieldName);

        for (var x = 0; x < selectedTreeNodeData.Leaves[i].FilterDescriptors.length; x++) {
            var filter = {
                "ID": ++FielterIdCounter,
                "Field": selectedTreeNodeData.Leaves[i].PropertyFullPath + "-" + type,
                "Operator": selectedTreeNodeData.Leaves[i].FilterDescriptors[x].FilterOperator,
                "Value": type == "DateTime" ? kendo.toString(new Date(selectedTreeNodeData.Leaves[i].FilterDescriptors[x].StringValue), dateFormat) : selectedTreeNodeData.Leaves[i].FilterDescriptors[x].StringValue,
                "Type": type
            };
            nodeFiltersDataSource.push(filter);
        }
    }


    dataSource = new kendo.data.DataSource({
        data: nodeFiltersDataSource,
        pageSize: 4,
        schema: {
            model: {
                id: "ID",
                fields: {
                    ID: { type: "string", editable: false, nullable: true },
                    Field: { type: "string" },
                    Operator: { type: "string" },
                    Value: { type: "string" },
                    Type: { type: "string" }
                }
            }
        }
    });

    if ($("#filtersPager").data("kendoPager") != null) {
        $("#filtersPager").data("kendoPager").setDataSource(dataSource);
        $("#filtersListView").data("kendoListView").setDataSource(dataSource);
    } else {
        $("#filtersPager").kendoPager({
            dataSource: dataSource
        });

        var listView = $("#filtersListView").kendoListView({
            dataSource: dataSource,
            editable: true,
            template: kendo.template($("#filtersDisplayModeTemplate").html()),
            editTemplate: kendo.template($("#filtersEditModeTemplate").html()),
            edit: function (e) {
                OriginalEditedFilterModel = jQuery.extend({}, e.model);
                if (e.model.ID == null) {
                    IsFilterAddMode = 1;
                } else {
                    IsFilterAddMode = 0;
                }
                FilterFieldName_Changed();
            },
            save: function (e) {
                FilterFieldName_Save(e);
            },
            remove: function (e) {
                FilterFieldName_Remove(e);
            }
        }).data("kendoListView");


        $(".k-add-button").click(function (e) {
            listView.add();
            e.preventDefault();
        });
    }
}

function IntializeAggregateFiltersDataSource(selectedTreeNodeData) {
    var nodeAggregateFiltersDataSource = new Array();
    CurrentAggregateFiltersFieldsNames = new Array();
    AggregateFielterIdCounter = 0;
    IsAggregateFilterAddMode = 0;

    for (var i = 0; i < selectedTreeNodeData.Nodes.length; i++) {
        var fieldName = {
            text: selectedTreeNodeData.Nodes[i].DisplayName,
            value: selectedTreeNodeData.Nodes[i].PropertyName
        };
        CurrentAggregateFiltersFieldsNames.push(fieldName);
    }

    for (var x = 0; x < selectedTreeNodeData.AggregateFilters.length; x++) {
        var filter = {
            "ID": ++AggregateFielterIdCounter,
            "Field": selectedTreeNodeData.AggregateFilters[x].PropertyName,
            "AggregateFunction": selectedTreeNodeData.AggregateFilters[x].AggregateFunction,
            "SubField": selectedTreeNodeData.AggregateFilters[x].SubPropertyName,
            "Operator": selectedTreeNodeData.AggregateFilters[x].FilterOperator,
            "Value": selectedTreeNodeData.AggregateFilters[x].StringValue
        };
        nodeAggregateFiltersDataSource.push(filter);
    }


    dataSource = new kendo.data.DataSource({
        data: nodeAggregateFiltersDataSource,
        pageSize: 4,
        schema: {
            model: {
                id: "ID",
                fields: {
                    ID: { type: "string", editable: false, nullable: true },
                    Field: { type: "string" },
                    AggregateFunction: { type: "string" },
                    SubField: { type: "string" },
                    Operator: { type: "string" },
                    Value: { type: "string" }
                }
            }
        }
    });

    if ($("#aggregatesPager").data("kendoPager") != null) {
        $("#aggregatesPager").data("kendoPager").setDataSource(dataSource);
        $("#aggregatesListView").data("kendoListView").setDataSource(dataSource);
    } else {
        $("#aggregatesPager").kendoPager({
            dataSource: dataSource
        });

        var listView = $("#aggregatesListView").kendoListView({
            dataSource: dataSource,
            editable: true,
            template: kendo.template($("#aggregateFilterDisplayModeTemplate").html()),
            editTemplate: kendo.template($("#aggregateFilterEditModeTemplate").html()),
            edit: function (e) {
                OriginalEditedAggregateFilterModel = jQuery.extend({}, e.model);
                if (e.model.ID == null) {
                    IsAggregateFilterAddMode = 1;
                } else {
                    IsAggregateFilterAddMode = 0;
                }
                AggregateFilterFieldName_Changed();
            },
            save: function (e) {
                AggregateFilterFieldName_Save(e);
            },
            remove: function (e) {
                AggregateFilterFieldName_Remove(e);
            }
        }).data("kendoListView");


        $(".k-add-button").click(function (e) {
            listView.add();
            e.preventDefault();
        });
    }
}
function IntializeAggregateOperationsDataSource(selectedTreeNodeData) {
    var nodeAggregateOperationsDataSource = new Array();
    CurrentAggregateOperationsFieldsNames = new Array();
    AggregateOperationIdCounter = 0;
    IsAggregateOperationAddMode = 0;

    for (var i = 0; i < selectedTreeNodeData.Nodes.length; i++) {
        var fieldName = {
            text: selectedTreeNodeData.Nodes[i].DisplayName,
            value: selectedTreeNodeData.Nodes[i].PropertyName
        };
        CurrentAggregateOperationsFieldsNames.push(fieldName);
    }

    for (var x = 0; x < selectedTreeNodeData.AggregateOperations.length; x++) {
        var operation = {
            "ID": ++AggregateOperationIdCounter,
            "Field": selectedTreeNodeData.AggregateOperations[x].PropertyName,
            "AggregateOperationsFunction": selectedTreeNodeData.AggregateOperations[x].AggregateFunction,
            "SubField": selectedTreeNodeData.AggregateOperations[x].SubPropertyName,
            "DisplayName": selectedTreeNodeData.AggregateOperations[x].DisplayName
        };
        nodeAggregateOperationsDataSource.push(operation);
    }

    console.log(nodeAggregateOperationsDataSource);
    dataSource = new kendo.data.DataSource({
        data: nodeAggregateOperationsDataSource,
        pageSize: 4,
        schema: {
            model: {
                id: "ID",
                fields: {
                    ID: { type: "string", editable: false, nullable: true },
                    Field: { type: "string" },
                    AggregateOperationsFunction: { type: "string" },
                    SubField: { type: "string" },
                    DisplayName: { type: "string" }
                }
            }
        }
    });
    debugger;
    if ($("#aggregatesOperationsPager").data("kendoPager") != null) {
        $("#aggregatesOperationsPager").data("kendoPager").setDataSource(dataSource);
        $("#aggregatesOperationsListView").data("kendoListView").setDataSource(dataSource);
    } else {
        $("#aggregatesOperationsPager").kendoPager({
            dataSource: dataSource
        });

        var listView = $("#aggregatesOperationsListView").kendoListView({
            dataSource: dataSource,
            editable: true,
            template: kendo.template($("#aggregateOperationsDisplayModeTemplate").html()),
            editTemplate: kendo.template($("#aggregateOperationsEditModeTemplate").html()),
            edit: function (e) {
                OriginalEditedAggregateOperationModel = jQuery.extend({}, e.model);
                if (e.model.ID == null) {
                    IsAggregateOperationAddMode = 1;
                } else {
                    IsAggregateOperationAddMode = 0;
                }
                AggregateOperationsFieldsName_Changed();
            },
            save: function (e) {
                AggregateOperationFieldName_Save(e);
            },
            remove: function (e) {
                AggregateOperationFieldName_Remove(e);
            }
        }).data("kendoListView");


        $(".k-add-button").click(function (e) {
            listView.add();
            e.preventDefault();
        });
    }
}
function AggregateOperationsFieldsName_Changed() {
    $("#AggregateOperationsFieldName").data("kendoDropDownList").bind("change", function (e) {
        debugger;
        var item = SelectedTreeNode;
        for (var j = 0; j < item.Nodes.length; j++) {
            if (item.Nodes[j].PropertyName == e.sender.value()) {
                AggregateOperationsSubProperty = new Array();
                for (var i = 0; i < item.Nodes[j].Leaves.length; i++) {
                    var fieldName = {
                        text: item.Nodes[j].Leaves[i].DisplayName,
                        value: item.Nodes[j].Leaves[i].PropertyName
                    };
                    AggregateOperationsSubProperty.push(fieldName);
                }
                console.log(AggregateOperationsSubProperty);
                $("#AggregateOperationsSubProperty").data("kendoDropDownList").setDataSource(AggregateOperationsSubProperty);
            }
        }
    });
}
function AggregateFilterFieldName_Changed() {
    $("#AggregateFieldName").data("kendoDropDownList").bind("change", function (e) {
        var item = SelectedTreeNode;
        for (var j = 0; j < item.Nodes.length; j++) {
            if (item.Nodes[j].PropertyName == e.sender._selectedValue) {
                AggregateFilterSubProperty = new Array();
                for (var i = 0; i < item.Nodes[j].Leaves.length; i++) {
                    var fieldName = {
                        text: item.Nodes[j].Leaves[i].DisplayName,
                        value: item.Nodes[j].Leaves[i].PropertyName
                    };
                    AggregateFilterSubProperty.push(fieldName);
                }
                $("#AggregateFilterSubProperty").data("kendoDropDownList").setDataSource(AggregateFilterSubProperty);
            }
        }
    });
}
function FilterFieldChanged() {

    var fieldType = $("#FilterFieldName").data("kendoDropDownList").value().split("-")[1];
    if (fieldType == "String") {
        $("#FilterFieldOperator").data("kendoDropDownList").setDataSource(FiltersStringOperators);
    }
    else
        if (fieldType == "Integer") {
            $("#FilterFieldValue").kendoNumericTextBox();
            $("#FilterFieldOperator").data("kendoDropDownList").setDataSource(FiltersNumericAndDateOperators);
        }
        else
            if (fieldType == "DateTime") {
                $("#FilterFieldValue").kendoDatePicker({ format: dateFormat });
                $("#FilterFieldOperator").data("kendoDropDownList").setDataSource(FiltersNumericAndDateOperators);
            }
}

function FilterFieldName_Changed() {

    FilterFieldChanged();
    $("#FilterFieldName").bind("change", function () {
        $("#FilterFieldValue").parents("dd")
            .html("<input type='text' class='k-textbox' data-bind='value:StringValue' id='FilterFieldValue' required='required' validationMessage='required' /><span data-for='FilterFieldValue' class='k-invalid-msg'></span>");
        FilterFieldChanged();
    });
}

function FilterFieldName_Save(e) {

    FielterIdCounter++;

    e.model.ID = IsFilterAddMode == 1 ? FielterIdCounter : e.model.ID;
    e.model.Type = $("#FilterFieldName").data("kendoDropDownList").value().split("-")[1];
    e.model.Field = $("#FilterFieldName").data("kendoDropDownList").value();
    e.model.Operator = $("#FilterFieldOperator").data("kendoDropDownList").value();
    e.model.StringValue = $("#FilterFieldValue").val();
    SaveFielterChangesToTreeDataSource(e);

}

function FilterFieldName_Remove(e) {
    var item = SelectedTreeNode;
    item.Leaves.forEach(function (leaf) {
        if (leaf.PropertyFullPath == e.model.Field.substring(0, e.model.Field.indexOf("-"))) {

            leaf.FilterDescriptors = $.grep(leaf.FilterDescriptors, function (par) {
                return par.FilterOperator != e.model.Operator || par.StringValue != e.model.StringValue;
            });
        }
    });
}

function SaveFielterChangesToTreeDataSource(e) {
    var item = SelectedTreeNode;

    if (OriginalEditedFilterModel.Field != e.model.Field && IsFilterAddMode == 0) {
        item.Leaves.forEach(function (leaf) {
            if (leaf.PropertyFullPath == OriginalEditedFilterModel.Field.substring(0, OriginalEditedFilterModel.Field.indexOf("-"))) {

                leaf.FilterDescriptors = $.grep(leaf.FilterDescriptors, function (par) {
                    return par.FilterOperator != OriginalEditedFilterModel.Operator || par.StringValue != OriginalEditedFilterModel.StringValue;
                });
            }
            else if (leaf.PropertyFullPath == e.model.Field.substring(0, e.model.Field.indexOf("-"))) {
                var filter = {
                    "FilterOperator": e.model.Operator,
                    "StringValue": e.model.StringValue,
                };
                leaf.FilterDescriptors[leaf.FilterDescriptors.length] = filter;
                leaf.FilterDescriptors.length++;
            }
        });
    } else {
        item.Leaves.forEach(function (leaf) {
            if (leaf.PropertyFullPath == e.model.Field.substring(0, e.model.Field.indexOf("-"))) {
                if (IsFilterAddMode == 1) {
                    var filter = {
                        "FilterOperator": e.model.Operator,
                        "StringValue": e.model.StringValue,
                    };
                    leaf.FilterDescriptors[leaf.FilterDescriptors.length] = filter;
                    leaf.FilterDescriptors.length++;
                } else {
                    leaf.FilterDescriptors.forEach(function (filterDescriptor) {
                        if (OriginalEditedFilterModel.Operator == filterDescriptor.FilterOperator &&
                            OriginalEditedFilterModel.StringValue == filterDescriptor.StringValue) {
                            filterDescriptor.FilterOperator = e.model.Operator;
                            filterDescriptor.StringValue = e.model.StringValue;
                        }
                    });
                }
            }
        });
    }
}

function AggregateFilterFieldName_Save(e) {
    AggregateFielterIdCounter++;
    e.model.ID = IsAggregateFilterAddMode == 1 ? AggregateFielterIdCounter : e.model.ID;
    e.model.Field = $("#AggregateFieldName").data("kendoDropDownList").value();
    e.model.AggregateFunction = $("#AggregateFunction").data("kendoDropDownList").value();
    e.model.Operator = $("#AggregateFieldOperator").data("kendoDropDownList").value();
    e.model.SubField = $("#AggregateFilterSubProperty").data("kendoDropDownList").value();
    e.model.StringValue = $("#AggregateFieldValue").val();

    SaveAggregateFielterChangesToTreeDataSource(e);
}
function AggregateOperationFieldName_Save(e) {
    AggregateOperationIdCounter++;
    e.model.ID = AggregateOperationIdCounter == 1 ? AggregateOperationIdCounter : e.model.ID;
    e.model.Field = $("#AggregateOperationsFieldName").data("kendoDropDownList").value();
    e.model.AggregateOperationsFunction = $("#AggregateOperationsFunction").data("kendoDropDownList").value();
    e.model.SubField = $("#AggregateOperationsSubProperty").data("kendoDropDownList").value();
    e.model.DisplayName = $("#AggregateOperationDisplayName").val();

    SaveAggregateOperationChangesToTreeDataSource(e);
}
function SaveAggregateOperationChangesToTreeDataSource(e) {
    var item = SelectedTreeNode;
    if (OriginalEditedAggregateOperationModel.Field != e.model.Field && IsAggregateOperationAddMode == 0) {
        item.AggregateOperations = $.grep(item.AggregateOperations, function (par) {
            return !(par.PropertyName == OriginalEditedAggregateOperationModel.Field && par.AggregateFunction == OriginalEditedAggregateOperationModel.AggregateOperationsFunction &&
                par.SubPropertyName == OriginalEditedAggregateOperationModel.SubField && par.DisplayName == OriginalEditedAggregateOperationModel.DisplayName);
        });
        var newOperation = {
            "PropertyName": e.model.Field,
            "SubPropertyName": e.model.SubField,
            "AggregateFunction": e.model.AggregateOperationsFunction,
            "DisplayName": e.model.DisplayName,
        };
        item.AggregateOperations[item.AggregateOperations.length] = newOperation;
    } else if (IsAggregateOperationAddMode == 1) {
        var Operation = {
            "PropertyName": e.model.Field,
            "SubPropertyName": e.model.SubField,
            "AggregateFunction": e.model.AggregateOperationsFunction,
            "DisplayName": e.model.DisplayName,
        };
        item.AggregateOperations[item.AggregateOperations.length] = Operation;
        item.AggregateOperations.length++;
    } else {
        item.AggregateOperations.forEach(function (aggregateOperation) {
            if (OriginalEditedAggregateOperationModel.Field == aggregateOperation.PropertyName &&
                OriginalEditedAggregateOperationModel.AggregateOperationsFunction == aggregateOperation.AggregateFunction &&
                OriginalEditedAggregateOperationModel.SubField == aggregateOperation.SubPropertyName &&
                OriginalEditedAggregateOperationModel.DisplayName == aggregateOperation.DisplayName) {
                aggregateOperation.PropertyName = e.model.Field;
                aggregateOperation.SubPropertyName = e.model.SubField;
                aggregateOperation.AggregateFunction = e.model.AggregateOperationsFunction;
                aggregateOperation.DisplayName = e.model.DisplayName;
            }
        });
    }
}
function SaveAggregateFielterChangesToTreeDataSource(e) {
    var item = SelectedTreeNode;

    if (OriginalEditedAggregateFilterModel.Field != e.model.Field && IsAggregateFilterAddMode == 0) {
        item.AggregateFilters = $.grep(item.AggregateFilters, function (par) {
            return !(par.PropertyName == OriginalEditedAggregateFilterModel.Field && par.AggregateFunction == OriginalEditedAggregateFilterModel.AggregateFunction &&
                par.FilterOperator == OriginalEditedAggregateFilterModel.Operator && par.StringValue == OriginalEditedAggregateFilterModel.StringValue && par.SubPropertyName == OriginalEditedAggregateFilterModel.SubField);
        });
        var newFilter = {
            "PropertyName": e.model.Field,
            "FilterOperator": e.model.Operator,
            "AggregateFunction": e.model.AggregateFunction,
            "SubPropertyName": e.model.SubField,
            "StringValue": e.model.StringValue,
        };
        item.AggregateFilters[item.AggregateFilters.length] = newFilter;
    } else if (IsAggregateFilterAddMode == 1) {
        var filter = {
            "PropertyName": e.model.Field,
            "FilterOperator": e.model.Operator,
            "AggregateFunction": e.model.AggregateFunction,
            "SubPropertyName": e.model.SubField,
            "StringValue": e.model.StringValue,
        };
        item.AggregateFilters[item.AggregateFilters.length] = filter;
        item.AggregateFilters.length++;
    } else {
        item.AggregateFilters.forEach(function (aggregateFilter) {
            if (OriginalEditedAggregateFilterModel.Field == aggregateFilter.PropertyName &&
                OriginalEditedAggregateFilterModel.SubField == aggregateFilter.SubPropertyName &&
                OriginalEditedAggregateFilterModel.AggregateFunction == aggregateFilter.AggregateFunction &&
                OriginalEditedAggregateFilterModel.Operator == aggregateFilter.FilterOperator &&
                OriginalEditedAggregateFilterModel.StringValue == aggregateFilter.StringValue) {
                aggregateFilter.PropertyName = e.model.Field;
                aggregateFilter.SubPropertyName = e.model.SubField;
                aggregateFilter.FilterOperator = e.model.Operator;
                aggregateFilter.AggregateFunction = e.model.AggregateFunction;
                aggregateFilter.StringValue = e.model.StringValue;
            }
        });
    }
}

function AggregateFilterFieldName_Remove(e) {
    var item = SelectedTreeNode;
    item.AggregateFilters = $.grep(item.AggregateFilters, function (par) {
        return !(par.PropertyName == e.model.Field && par.AggregateFunction == e.model.AggregateFunction &&
            par.FilterOperator == e.model.Operator && par.StringValue == e.model.StringValue && par.SubPropertyName == e.model.SubField);
    });
}
function AggregateOperationFieldName_Remove(e) {
    var item = SelectedTreeNode;
    item.AggregateOperations = $.grep(item.AggregateOperations, function (par) {
        return !(par.PropertyName == e.model.Field && par.AggregateFunction == e.model.AggregateOperationsFunction &&
            par.SubPropertyName == e.model.SubField && par.DisplayName == e.model.DisplayName);
    });
}