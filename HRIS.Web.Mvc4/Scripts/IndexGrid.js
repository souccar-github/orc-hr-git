
function generateGridIndex(data) {
    var selectedUid = null;
    var view = getViewById(data.Views, data.CurrentViewId);
    var gridObj = new Grid(data);
    //var columns = getColumns(data);
    //var command = [];
    //if (data.AuthorizedToEdit)
    //    command.push({ name: "edit", text: { edit: data.Edit, cancel: data.Cancel, update: data.Update } });
    //if (data.AuthorizedToDelete)
    //    command.push({ name: "destroy", text: data.Delete });

    //columns[columns.length] = { command: command, width: "185px" };
    var columns = gridObj.GetColumnsWithCommandColumn();
    var height = $(window).height() - 250;
    $("#" + data.Name).html("").kendoGrid({
        dataSource: getDataSourceGridIndex(data),
        selectable: "single",
        filterable: gridObj.FilterableMessage,
        sortable: true,
        navigatable: true,
        pageable: gridObj.PageableMessage,
        height: height,
        toolbar: getToolbarCommands(data.ToolbarCommands),
        columns: columns,
        editable: {
            mode: view.EditorMode,
            confirmation: data.Confirmation
        },
        save: function (e) {
            selectedUid = e.model.uid;
        },
        dataBound: function (e) {
            if (selectedUid != null) {
                this.select("tr[data-uid='" + selectedUid + "']");
                $("#grid_active_cell").removeAttr('id');
                this.tbody.find("tr[data-uid='" + selectedUid + "'] td:eq(0)").attr('id', 'grid_active_cell');

                selectedUid = null;
            }
            if ($("body").hasClass("local-rtl")) {
                this.tbody.parent().css('margin-right', '1px');
            }

        }
    });
    initializeToolbarCommands(data.ToolbarCommands, data.Name);

}

function getDataSourceGridIndex(data) {
    var view = getViewById(data.Views, data.CurrentViewId);
    var gridObj = new Grid(data);
    return {
        serverPaging: true,
        serverSorting: true,
        type: "POST",
        serverFiltering: true,
        pageSize: 10,
        requestEnd: function (e) {
            var dataItem = {};

            if (e.type == "create") {
                dataItem = getDataItemById(0);
            }
            else if (e.type == "update") {
                dataItem = getDataItemById(e.response.Data.Id);
            }
            else if (e.type == "destroy") {
                if (e.response[data.ErrorFieldName] != null) {
                    ShowMessageBox(data.ResError, e.response[data.ErrorFieldName], "k-icon w-b-error", [{ Title: "Ok", ClassName: "k-icon k-update" }]);
                }
            }

            if ((e.type == "create") || (e.type == "update")) {
                if (dataItem != null)
                for (var member in e.response.Data) {
                    dataItem[member] = e.response.Data[member];
                }

                if (e.response[data.ErrorFieldName] != null) {
                    if (e.response[data.ErrorFieldName].Exception != null) {
                        ShowMessageBox(data.ResError, e.response[data.ErrorFieldName].Exception, "k-icon w-b-error", [{ Title: data.ResOk, ClassName: "k-icon k-update" }]);
                    } else {
                        ShowMessageBox(data.ResError, e.response[data.ErrorFieldName][0].Message, "k-icon w-b-error", [{ Title: data.ResOk, ClassName: "k-icon k-update" }]);
                    }
                }
            }
        },
        transport: {
            read: {
                url: window.applicationpath + view.ReadUrl,
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: { name: data.TypeFullName }
            },
            update: {
                url: window.applicationpath + view.UpdateUrl,
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: { name: data.TypeFullName }
            },
            destroy: {
                url: window.applicationpath + view.DestroyUrl,
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: { name: data.TypeFullName }
            },
            create: {
                url: window.applicationpath + view.CreateUrl,
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: { name: data.TypeFullName }
            },
            parameterMap: function (oData, operation) {
                if (operation == "read") {
                    return JSON.stringify(oData);
                }
                var result = {};
                for (var prop in oData) {
                    if (prop == "name") {
                        result[prop] = oData[prop];
                    } else {
                        result["data[" + prop + "]"] = oData[prop];
                    }
                }
                return JSON.stringify(result);
            }
        },
        schema: {
            model: {
                id: "Id",
                fields: gridObj.GetSchemaFields()
            },
            data: data.DataFieldName,
            errors: data.ErrorFieldName,
            total: data.TotalCountFieldName
        },
        error: function (e) {
            $('#' + data.Name).data("kendoGrid").cancelChanges();
        }
    };
}
