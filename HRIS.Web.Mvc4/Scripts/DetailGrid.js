//After Apply Master Detail Feature
function grnerateGridDetail(e, control, detailGridModel) {
    var gridDetail = control;
    if (window.gridModel.DetailModels == null)
        window.gridModel.DetailModels = {};
    window.gridModel.DetailModels[detailGridModel.DetailName] = detailGridModel;

    var filteredFields = [];

    var view = getViewById(detailGridModel.Views, detailGridModel.CurrentViewId);

    var template = "";
    if (view.EditorTemplate != "") {
        template = kendo.template($('#' + view.EditorTemplate).html());
    }

    var height = 400;

    var gridObj = new Grid(detailGridModel);

    var actionListTemplate = kendo.template($("#ActionListTemplate").html());
    var columns = gridObj.GetDetailMainColumns();
    //  var columns = gridObj.GetMainColumns();
    var DetailName = detailGridModel.DetailName;
    detailGridModel.KendoGridObject = control.kendoGrid({
        dataSource: gridObj.GetDataSourceForDetail(detailGridModel.ViewModelTypeFullName, detailGridModel.TypeFullName, DetailName, e.model),

        dataBound: function (e) {

            var gridDetailName = gridDetail;
            if (e.sender.dataSource.view().length == 0) {
                var colspan = e.sender.thead.find("th").length;
                //        //insert empty row with colspan equal to the table header th count
                var emptyRow = "<tr><td colspan='" + colspan + "'></td></tr>";
                e.sender.tbody.html(emptyRow);
            }

            e.sender.tbody.off('dblclick').on('dblclick', 'tr', function (element) {
                var uid = element.currentTarget.getAttribute("data-uid");
                var dataItem = gridDetail.data("kendoGrid").dataSource.getByUid(uid);
                //alert(JSON.stringify(dataItem));
                if (element.ctrlKey) {
                    updateDetail(dataItem.Id, detailGridModel.DetailNO);
                } 
                else {
                    showDetailInformation(dataItem.Id, detailGridModel.DetailNO);
                }

            });
            //if (view.DataBoundHandler != "") {
            //    var dataBoundHandlerFanction = getFunctionDelegate(view.DataBoundHandler);
            //    if (dataBoundHandlerFanction != null)
            //        dataBoundHandlerFanction(e);
            //}
            initializeViewSelector(detailGridModel);
            //this line is written to solve kendo 'rtl' bug.
            $(".k-grid-content").scrollLeft($(".k-grid-content").scrollLeft() - 1);
            if ($("body").hasClass("local-rtl")) {
                $(".k-grid-content").scrollLeft($(".k-grid-content").scrollLeft() + 1000);
            }
        },

        toolbar: getToolbarCommandsForDetailGrid(detailGridModel.ToolbarCommands),

        selectable: "multiple, row",
        //sortable: {
        //    mode: "multiple"
        //},
        navigatable: true,
        //pageable: gridObj.PageableMessage,
        //filterable : gridObj.FilterableMessage,
        columns: columns,
        height: height,
        editable: {
            update: true,
            confirmation: detailGridModel.Confirmation,
            mode: view.EditorMode,
            template: template,
            window: {
                title: detailGridModel.EditWindowTitle
            }
        },
        edit: function (e) {
            
            e.container.parent().hide();
            //JSON.stringify(myObject);
            window.localStorage.setItem("orginaldata", JSON.stringify(e.model));

            //Remove first column: Actions
            //e.container.find('.k-edit-label:eq(0)').remove();
            //e.container.find('.k-edit-field:eq(0)').remove();
            ///  addGridNotification(detailGridModel, e);
            if (view.ShowTwoColumns) {
                e.container.parent().addClass('default-window-edit-two-column');
            }
            else {
                e.container.parent().addClass('default-window-edit-one-column');
            }
            //Remove read only columns
            for (var j = 0; j < detailGridModel.SchemaFields.length; j++) {
                var field = detailGridModel.SchemaFields[j];
                if (!field.Editable) {
                    e.container.find("label[for='" + field.Name + "']").parents(".k-edit-label").next().remove();
                    e.container.find("label[for='" + field.Name + "']").parents(".k-edit-label").remove();
                }
            }
            gridObj.AddFieldSets(e);
            gridObj.ShowTwoColumns(e);
            gridObj.AddRequiredStyle(e);
            gridObj.PreventCommaInNumericTextBox(e);
            // addFieldsets(e);
            //showTwoColumns(e);
            //addRequiredStyle(e);
            //preventCommaInNumericTextBox(e);
            e.container.parent().find("div.k-window-titlebar.k-header").prepend("<span class='maestro-popup-icon'></span>");
            if (view.EditHandler != "") {
                var handlerFanction = getFunctionDelegate(view.EditHandler);
                if (handlerFanction != null)
                    handlerFanction(e);
            }
            $('.clockpicker').clockpicker().find('input').change(function () {

                this.value = getTimeAsStandardToShowInClockTimePicker(this.value);
            });
            e.container.kendoValidator({ validateOnBlur: true });
            e.sender.editable.validatable._errorTemplate = kendo.template($('#TooltipInvalidMessageTemplate').html());
            //by yaseen for fix k-edit-buttons .k-widget.k-window
            e.container.find(".k-grid-update").addClass("primary-action");
            e.container.find(".k-grid-update").html(detailGridModel.Update);
            e.container.find(".k-grid-cancel").addClass("secondary-action");
            e.container.find(".k-grid-cancel").html(detailGridModel.Cancel);

            //$(".k-popup-edit-form.k-window-content.k-content").append($(".k-edit-buttons.k-state-default:not(.not-default-button)"));
            e.container.append(e.container.find(".k-edit-buttons.k-state-default:not(.not-default-button)"));

            if (e.model.Id == 0) {
                e.container.parent().find(".k-window-title").html(detailGridModel.Add + " " + detailGridModel.EntityTitle);
            } else {
                e.container.parent().find(".k-window-title").html(detailGridModel.Edit + " " + detailGridModel.EntityTitle);
            }

            e.container.parent().css({ left: ($(window).width() - e.container.parent().width()) / 2 });
            e.container.parent().css({ top: ($(window).height() - e.container.parent().height()) / 2 });
            e.container.parent().show();
            if (e.model.Id == 0)
                gridObj.PreventDefaultValueInDateTimeFields(e);
            else
                gridObj.PreventStandardDateValueInTimeFieldsForDetail(e);
            // updateEditFormAsTabs(e);
        },

        remove: function (e) {

             
            e.preventDefault();

            var grid = $(".grid-detail-" + DetailName
                    ).data("kendoGrid");



            grid.dataSource.remove(e.model);
            grid.dataSource.sync();
        },
        read: function (e) {
             

        },
        cancel: function (e) {
             
            e.preventDefault();
            orginaldata = JSON.parse(window.localStorage.getItem("orginaldata"));

            if ((e.model.Id == 0) || (e.model.id == 0)) {
                e.sender.dataSource.remove(e.model);
            }
            else {
                e.sender.dataSource.remove(e.model);
                for (var prop in orginaldata) {
                    var field = getFieldByName(window.parent.gridModel.DetailModels[DetailName].SchemaFields, prop);
                    if (field != null) {
                        if (field.Type == "date") {
                            var column = getColumnByName(view.Columns, prop);
                            if (column.IsTime) {
                                var date = orginaldata[prop];
                                var _date = new Date();
                                if (kendo.parseDate(orginaldata[prop], "hh:mm tt") != undefined)
                                    _date = new Date(kendo.parseDate(date, "hh:mm tt"));
                                else
                                    _date = new Date(date);
                                orginaldata[prop] = kendo.toString(_date, "hh:mm tt");
                                if (orginaldata[prop].indexOf('NaN/NaN/0NaN') != -1) {
                                    orginaldata[prop] = $('[name="' + prop + '"]').val();
                                } 

                            }

                            else {
                                var date = new Date(kendo.parseDate(orginaldata[prop], "dd/mm/yyyy"));
                                orginaldata[prop] = kendo.toString(date, "dd/mm/yyyy");
                                //// test[prop] = kendo.toString(new Date(date), "dd/MM/yyyy "); //if (objProp.Value.indexOf('NaN/NaN/0NaN') != -1) {
                                if (orginaldata[prop].indexOf('NaN/NaN/0NaN') != -1) {
                                    orginaldata[prop] = $('[name="' + prop + '"]').val();
                                }
                            }

                        }
                    }
                }


                e.sender.dataSource.add(orginaldata);


            }




            //   e.sender.clearSelection();

        }
    });
}

function getToolbarCommandsForDetailGrid(toolbar) {
    var idx = 0;
    var result = [];

    for (var i = 0; i < toolbar.length; i++) {
        var item = toolbar[i];
        if (item.Name == "create") {

        
        if (item.Additional == true) {
            continue;
        }
        ////if (item.Name != "create") {
        ////    continue;
        ////}
        var command = {};
        if (item.Name != null && item.Name != "") {
            command.name = item.Name;
        }

        if (item.Text != null && item.Text != "") {
            command.text = item.Text;
        }

        if (item.ClassName != null && item.ClassName != "") {
            command.className = item.ClassName;
        }

        if (item.ImageClass != null && item.ImageClass != "") {
            command.imageClass = item.ImageClass;
        }

        if (item.Template != null && item.Template != "") {
            command.template = kendo.template($("#" + item.Template).html());
        }

        result[idx] = command;
        idx++;
        }
    }
    return result;
}
