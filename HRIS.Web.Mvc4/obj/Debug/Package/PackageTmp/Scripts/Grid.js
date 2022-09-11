//After Apply Master Detail Feature
function Grid(gridModel) {
        this.GridModel = gridModel;
 
        this.CreateSimpleGrid = function() {
            var height = $(window).height() - 250;
            height += 108;
            if (height < 380) {
                height = 380;
            }
            $("#" + this.GridModel.Name).html("").kendoGrid({
                dataSource: this.GetSimpleDataSource(),
                dataBound: function(e) {
                    if (e.sender.dataSource.view().length == 0) {
                        var colspan = e.sender.thead.find("th").length;
                        //insert empty row with colspan equal to the table header th count
                        var emptyRow = "<tr><td colspan='" + colspan + "'></td></tr>";
                        e.sender.tbody.html(emptyRow);
                    }
                },
                selectable: "multiple, row",
                filterable: this.FilterableMessage,
                sortable: { mode: "multiple" },
                navigatable: true,
                pageable: this.PageableMessage,
                toolbar: getToolbarCommands(this.GridModel.ToolbarCommands),
                columns: this.GetSimpleColumns(),
                height: height,
            });
            initializeToolbarCommands(this.GridModel.ToolbarCommands, this.GridModel.Name);
        };
        this.GetSimpleColumns = function() {
            var gridModel = this.GridModel;
            var idx = 0;
            var result = [];
            var view = getViewById(gridModel.Views, gridModel.CurrentViewId);

            for (var i = 0; i < view.Columns.length; i++) {
               
                var column = view.Columns[i];

                if (column.FieldName != "Id") {
                    var field = getFieldByName(gridModel.SchemaFields, column.FieldName);
                  
                    if (column.Type == "Simple") {
                        var values = {};

                        if (field.Type == "date") {
                             
                            if (column.IsTime) {
                                var filterable = this.datefilter;
                                if (column.Filterable == true)
                                {
                                  
                                    filterable["ui"] = function (element) {
                                        element.kendoTimePicker({ format: "hh:mm tt" });
                                    };
                                }
                                else
                                {
                                    filterable = false;
                                }

                                result[idx] = {
                                    field: column.FieldName,
                                    width: column.Width,
                                    title: column.Title,
                                    hidden: column.Hidden,
                                    format: "{0:hh:mm tt}",
                                    values: {
                                        IsDateTime: column.IsDateTime,
                                        IsTime: column.IsTime,
                                        DetailName: column.DetailName,
                                    
                                    },
                                    editor: getDatePickerEditor,
                                    sortable: column.Sortable,
                                    filterable: filterable,

                                    attributes: { "style": "text-align: center;" }
                                };
                           
                            }
                            else if (column.IsDateTime) {



                                var filterableIsDateTime = $.extend(true, [], this.datefilter);
                                if (column.Filterable == true) {
                                    filterableIsDateTime["ui"] = function (element) {
                                        element.kendoDateTimePicker({ format: "dd/MM/yyyy hh:mm tt" });
                                    };
                                }

                                else {
                                    filterable2 = false;
                                }

                                result[idx] = {
                                    field: column.FieldName,
                                    width: column.Width,
                                    title: column.Title,
                                    hidden: column.Hidden,
                                    format: "{0:dd/MM/yyyy hh:mm tt}",
                                    values: {
                                        IsDateTime: column.IsDateTime,
                                        IsTime: column.IsTime,
                                        DetailName: column.DetailName,

                                    },
                                    editor: getDatePickerEditor,
                                    sortable: column.Sortable,
                                    filterable: filterableIsDateTime,

                                    attributes: { "style": "text-align: center;" }
                                };

                              
                            }
                            else {



                                var filterable2 = $.extend(true, [], this.datefilter);
                                if (column.Filterable == true) {



                                    filterable2["ui"] = function (element) {
                                        element.kendoDatePicker({ format: "dd/MM/yyyy" });
                                    };
                                }




                                else {
                                    filterable2 = false;
                                }

                                result[idx] = {
                                    field: column.FieldName,
                                    width: column.Width,
                                    title: column.Title,
                                    hidden: column.Hidden,
                                    format: "{0:dd/MM/yyyy}",
                                    values: {
                                        IsDateTime: column.IsDateTime,
                                        IsTime: column.IsTime,
                                        DetailName: column.DetailName,

                                    },
                                    editor: getDatePickerEditor,
                                    sortable: column.Sortable,
                                    filterable: filterable2,

                                    attributes: { "style": "text-align: center;" }
                                };

                             

                            }






                        }

                        else {
                            
                            if (field.Type == "boolean") {
                                result[idx] = {
                                    field: column.FieldName,
                                    width: column.Width,
                                    title: column.Title,
                                    hidden: column.Hidden,
                                    sortable: column.Sortable,
                                    filterable: column.Filterable,
                                    attributes: { "style": "text-align: center;" },
                                    template: kendo.template("#if(" + column.FieldName + " == true){#" +
                                        "#= '<input type=\"checkbox\" checked=\"checked\" disabled=\"disabled\" />' #" +
                                        "#}else{#" +
                                        "#= '<input type=\"checkbox\" disabled=\"disabled\" />' #" +
                                        "#}#"
                                    )
                                };
                            } else if (field.Type == "number") {
                                result[idx] = {
                                    field: column.FieldName,
                                    width: column.Width,
                                    title: column.Title,
                                    hidden: column.Hidden,
                                    sortable: column.Sortable,
                                    filterable: column.Filterable,
                                    attributes: { "style": "text-align: center;" }
                                };
                                if (!column.ShowCommaSeparator) {
                                    result[idx].format = "{0:n0}";
                                    //result[idx].decimals = 0;
                                }
                            } else if (field.Type == "image") {
                                var path = window.applicationpath + column.ImagePath;
                                result[idx] = {
                                    field: column.FieldName,
                                    width: column.Width,
                                    sortable: false,
                                    filterable: false,
                                    hidden: column.Hidden,
                                    values: {
                                        TypeFullName: column.TypeFullName,
                                    },
                                    title: column.Title,
                                    attributes: { "style": "text-align: center;" },
                                    template: kendo.template('#var imgName="' + column.DefaultImageName + '";if(' + column.FieldName + '!= null&&' + column.FieldName + '!= ""){imgName=' + column.FieldName + '; if(!imageExists("' + path + '" + imgName' + ')){imgName="' + column.DefaultImageName + '";}}#<img src="' + path + '#:imgName#' + '" width="32" height="32" style=" vertical-align: middle; " alt="img">')
                                };
                            } else if (field.Type == "file") {
                                var path = window.applicationpath + column.ImagePath;
                                result[idx] = {
                                    field: column.FieldName,
                                    width: column.Width,
                                    sortable: false,
                                    filterable: false,
                                    values: {
                                        TypeFullName: gridModel.TypeFullName,
                                        AcceptExtension: column.FileAcceptExtension,
                                        FileSize: column.FileSize
                                    },
                                    hidden: column.Hidden,
                                    title: column.Title,
                                    attributes: { "style": "text-align: center;" },
                                    editor: getFileEditor,
                                    template: kendo.template('#=getoriginalFileName(' + column.FieldName + ')#')
                                };
                            } else {
                                result[idx] = {
                                    field: column.FieldName,
                                    width: column.Width,
                                    title: column.Title,
                                    hidden: column.Hidden,
                                    sortable: column.Sortable,
                                    filterable: column.Filterable,
                                };

                            }
                        }
                    } else if (column.Type == "TextArea") {
                        result[idx] = {
                            field: column.FieldName,
                            width: column.Width,
                            title: column.Title,
                            hidden: column.Hidden,
                            template: "<div style='display: block; overflow: auto; max-height: 60px;'>" +
                                "#if(" + column.FieldName + " != null){#" +
                                "#: " + column.FieldName + "# " +
                                "#}#" +
                                "</div>",
                            editor: getTextAreaEditor,
                            sortable: column.Sortable,
                            filterable: column.Filterable
                        };
                    } else if (column.Type == "DropDown") {
                      
                        var filterableDropDown = column.Filterable;
                        if (filterableDropDown)
                            filterableDropDown = { ui: getFilterForDropdown(column.ReadUrl, column.TypeFullName) };
                        result[idx] = {
                            field: column.FieldName,
                            width: column.Width,
                            title: column.Title,
                            hidden: column.Hidden,
                            editor: getDropDownEditor,
                            values: {
                                Id: column.FieldName,
                                WindowTitle: column.Title,
                                IndexName: column.IndexName,
                                TypeFullName: column.TypeFullName,
                                DataTextField: column.TextField,
                                DataValueField: column.ValueField,
                                ReadUrl: column.ReadUrl,
                                CreateUrl: column.CreateUrl,
                                ShowAddButton: column.ShowAddButton,
                                ShowInfoButton: column.ShowInfoButton,
                                HasParent: column.HasParent,
                                CascadeFrom: column.CascadeFrom,
                                DetailName: column.DetailName,
                                 Filter:"contains",
                                Required: fieldIsRequired(field)
                            },
                            template: kendo.template("#if(" + column.FieldName + "." + column.TextField + " == null){#" +
                                "#: '' #" +
                                "#}else{#" +
                                "#: " + column.FieldName + "." + column.TextField + " #" +
                                "#}#"
                            ),
                            sortable: column.Sortable,
                       filterable: filterableDropDown
                        };

                    } else if (column.Type == "AutoComplete") {
                        
                        result[idx] = {
                            field: column.FieldName,
                            width: column.Width,
                            title: column.Title,
                            hidden: column.Hidden,
                            editor: getAutoCompleteEditor,
                            values: {
                                Id: column.FieldName,
                                WindowTitle: column.Title,
                                IndexName: column.IndexName,
                                TypeFullName: column.TypeFullName,
                                DataTextField: column.TextField,
                                DataValueField: column.ValueField,
                                ReadUrl: column.ReadUrl,
                                CreateUrl: column.CreateUrl,
                                ShowAddButton: column.ShowAddButton,
                                ShowInfoButton: column.ShowInfoButton,
                                HasParent: column.HasParent,
                                CascadeFrom: column.CascadeFrom,
                                Required: fieldIsRequired(field)
                            },
                            template: kendo.template("#if(" + column.FieldName + "." + column.TextField + " == null){#" +
                                "#: '' #" +
                                "#}else{#" +
                                "#: " + column.FieldName + "." + column.TextField + " #" +
                                "#}#"
                            ),
                            sortable: column.Sortable,
                            filterable: column.Filterable
                        };
                    }
                    result[idx].footerTemplate = column.FooterTemplate;
                    if (column.GroupAggregates.length > 0) {
                        result[idx].aggregates = column.GroupAggregates;
                    }
                    idx++;
                }
            }
            return result;
        };
        function getFilterForDropdown(readUrl, typeName) {
            return function (element) {
                element.kendoDropDownList({
                    autoBind: false,
                    dataTextField: "Name",
                    filter: "contains",
                    filtering: function (e) {
                         
                        //get filter descriptor
                        var filter = e.filter;

                        // handle the event
                    },
                    dataValueField: "Id",
                    optionLabel: gridModel.FilterDropdownLabel,
                    dataSource: {
                        type: "json",
                        serverFiltering: false,
                        transport: {
                            read: {
                                cache: true,
                                url: window.applicationpath + readUrl,
                                type: "POST",
                                dataType: "json",
                                data: { requestInformation: window.requestInformation, typeName: typeName },
                                contentType: "application/json; charset=utf-8"
                            },
                            parameterMap: function (innerData, operation) {
                                if (operation == "read") {
                                    return JSON.stringify(innerData);
                                }
                            }
                        },
                        schema: {
                            model: {
                                id: "Id"
                            },
                            data: "Data"
                        }
                    }
                });
            };
        }

        this.GetColumnsWithCommandColumn = function() {
            var columns = this.GetSimpleColumns();
            var command = [];
            columns[columns.length] = { command: command, width: "185px" };
            if (this.GridModel.AuthorizedToEdit)
                command.push({ name: "edit", text: { edit: this.GridModel.Edit, cancel: this.GridModel.Cancel, update: this.GridModel.Update } });
            if (this.GridModel.AuthorizedToDelete)
                command.push({ name: "destroy", text: this.GridModel.Delete });
            // if (this.GridModel.)
           // command.push({ name: "destroy", text: this.GridModel.View });
            return columns;
           
        };
        this.GetDetailMainColumns = function () {
            var columns = this.GetSimpleColumns();
            if (this.GridModel.ActionList.GroupsCount != 0) {
                columns.unshift({
                    title: "",
                    attributes: { "style": "text-align: center;" },
                    template: '<span class="menu-icon show-menu-button-#=Id#' + this.GridModel.DetailNO + '" onclick="showActionList(#=Id#,' + this.GridModel.DetailNO + ')"></span>',
                    width: 40
                });
            }
            return columns;
        };
        function deleteIndex(e) {

            var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

            destroy(dataItem.Id);
        }

    this.GetSimpleDataSource = function() {
            var view = getViewById(this.GridModel.Views, this.GridModel.CurrentViewId);
            var result = {
                serverPaging: view.ServerPaging,
                serverSorting: view.ServerSorting,
                serverFiltering: view.ServerFiltering,
                serverAggregates: view.ServerAggregates,
                sort: getSortArray(view.SortFields),
                type: "POST",
                pageSize: 10,
                requestEnd: function(e) {
                },
                transport: {
                    read: {
                        url: window.applicationpath + view.ReadUrl,
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: { requestInformation: window.requestInformation, viewModelTypeFullName: this.GridModel.ViewModelTypeFullName }
                    },
                    parameterMap: function(oData, operation) {
                        if (operation == "read") {
                            if (oData.filter != null) {
                                for (var i = 0; i < oData.filter.filters.length; i++) {
                                    if (oData.filter.filters[i].filters != null) {
                                        for (var j = 0; j < oData.filter.filters[i].filters.length; j++) {
                                            var column = getColumnByName(view.Columns, oData.filter.filters[i].filters[j].field);
                                            if (column.IsTime)
                                                oData.filter.filters[i].filters[j].value.setFullYear(2000, 0, 01);


                                        }
                                    }
                                    else {
                                        var column = getColumnByName(view.Columns, oData.filter.filters[i].field);
                                        if (column.IsTime)
                                            oData.filter.filters[i].value.setFullYear(2000, 0, 01);
                                    }
                                }
                            }

                            oData.serverPaging = view.ServerPaging;
                            return JSON.stringify(oData);
                        }
                    }
                },
                schema: {
                    model: {
                        id: "Id",
                        fields: this.GetSchemaFields()
                    },
                    data: this.GridModel.DataFieldName,

                    total: this.GridModel.TotalCountFieldName
                }
            };

            var globalAggregate = getGlobalAggregate(view);
            if (globalAggregate.length > 0) {
                result.aggregate = globalAggregate;
            }
            return result;
        };

        this.GetMainColumns = function () {
            var columns = this.GetSimpleColumns();
             
            var view = getViewById(gridModel.Views, gridModel.CurrentViewId);
            if (this.GridModel.ActionList.GroupsCount != 0) {
                columns.unshift({
                    title: "",
                    attributes: { "style": "text-align: center;" },
                    template: '<span class="menu-icon show-menu-button-#=Id#" onclick="showActionList(#=Id#)"></span>',
                    width: 40
                });
            }
       
            if (!view.IsDetailOutSideGrid) {

                if (getRibbonHtml(window.requestInformation) != "") {
                    columns.unshift({
                        title: "",
                        attributes: { "style": "text-align: center;" },
                        template: '<span id="details" class="plus_icon show-menu-button-#=Id#" onclick="openDetailsWindow()" ></span>',
                        width: 40
                    });

                }
            }

            columns.push({
                command: [{
                    name: "edit",
                    text: { edit: gridModel.Edit, update: gridModel.Update, cancel: gridModel.Cancel }
                }],
                hidden: true
            });

            return columns;
        };

        //this.GetMainColumns = function() {
        //    var columns = this.GetSimpleColumns();

        //    if (this.GridModel.ActionList.GroupsCount != 0) {
        //        columns.unshift({
        //            title: "",
        //            attributes: { "style": "text-align: center;" },
        //            template: '<span class="menu-icon show-menu-button-#=Id#" onclick="showActionList(#=Id#)"></span>',
        //            width: 40
        //        });

        //        //if (getRibbonHtml(window.requestInformation) != "") {
        //        //    columns.unshift({
        //        //        title: "",
        //        //        attributes: { "style": "text-align: center;" },
        //        //        template: '<span id="details" class="plus_icon show-menu-button-#=Id#" onclick="openDetailsWindow()" ></span>',
        //        //        width: 40
        //        //    });
        //    }
        //    columns.push({
        //        command: [{
        //            name: "edit",
        //            text: { edit: gridModel.Edit, update: gridModel.Update, cancel: gridModel.Cancel }
        //        }],
        //        hidden: true
        //    });

        //    return columns;
        //};
        this.GetMainDataSource = function (filteredFields) {

          
            var gridObj = this.GridModel;
            var errorOperation = null;
            var view = getViewById(this.GridModel.Views, this.GridModel.CurrentViewId);
            var columns = view.Columns;
          
            var result = {
                serverPaging: view.ServerPaging,
                serverSorting: view.ServerSorting,
                serverFiltering: view.ServerFiltering,
                serverAggregates: view.ServerAggregates,
                filter: adjustFilterData(view.Filter, filteredFields),
                sort: getSortArray(view.SortFields),
                type: "POST",
                pageSize: 10,
                requestEnd: function(e) {
                  

                    var notsave = window.localStorage.getItem("notsave");
                    var dataItem = {};
                    if (e.type == "read") {
                        
                    } else if (e.type == "create") {
                        dataItem = getDataItemById(0);
                    } else if (e.type == "update") {
                        dataItem = getDataItemById(e.response.Data.Id);
                    } else if (e.type == "destroy") {
                         
                        if (e.response[gridObj.ErrorFieldName] != null) {
                            ShowMessageBox(gridObj.ResError, e.response[gridObj.ErrorFieldName].Exception
                            , "k-icon w-b-error", [{ Title: gridObj.ResOk, ClassName: "k-icon k-update" }]);
                            errorOperation = e.type;
                        }
                    }

                    if ((e.type == "create") || (e.type == "update")) {
                        for (var member in e.response.Data) {
                            dataItem[member] = e.response.Data[member];
                        }
                        var save_and_new_clicked = $(".k-button.k-button-icontext.k-grid-update.k-grid-save-and-new").data('clicked');
                        var save_and_copy_clicked = $(".k-button.k-button-icontext.k-grid-update.k-grid-save-and-Copy").data('clicked');
                        var edit_and_copy_clicked = $(".k-button.k-button-icontext.k-grid-update.k-grid-copy").data('clicked');
                        var edit_and_new_clicked = $(".k-button.k-button-icontext.k-grid-update.k-grid-edit-new").data('clicked');
                        if (e.response[gridObj.ErrorFieldName] != null) {
                             
                            if (e.response[gridObj.ErrorFieldName]["Exception"] != null) {
                                ShowMessageBox(gridObj.ResError, e.response[gridObj.ErrorFieldName].Exception, "k-icon w-b-error", [{ Title: gridObj.ResOk, ClassName: "k-icon k-update" }]);
                            } else {
                                for (var prop in e.response[gridObj.ErrorFieldName]) {
                                    var messageText = e.response[gridObj.ErrorFieldName][prop];
                                    if (prop == "") {
                                        ShowMessageBox(gridObj.ResError, messageText, "k-icon w-b-error", [{ Title: gridObj.ResOk, ClassName: "k-icon k-update" }]);
                                    } else {
                                        var t = kendo.template($('#TooltipInvalidMessageTemplate').html())({ message: messageText });
                                        var affectedControl = getControlByFieldName(prop, gridObj);
                                        if (affectedControl != null)
                                            affectedControl.after(t);
                                        else
                                            ShowMessageBox(gridObj.ResError, messageText, "k-icon w-b-error", [{ Title: gridObj.ResOk, ClassName: "k-icon k-update" }]);
                                    }

                                }
                            }
                            errorOperation = e.type;
                        }
                        else if (!save_and_new_clicked && !save_and_copy_clicked && !edit_and_copy_clicked && !edit_and_new_clicked) {
                            var grid = $('#' + gridObj.Name).data('kendoGrid');
                            grid.dataSource.read();
  
                        }
                    }
                    if (errorOperation == null && $('.k-button.k-button-icontext.k-grid-update.k-grid-save-and-new').data('clicked')) {
                        $(".k-button.k-button-icontext.k-grid-update.k-grid-save-and-new").data('clicked', false);
                        var grid = $('#' + gridObj.Name).data("kendoGrid");
                        var show = CheckBeforeCreateForSEAndCN();
                        if (show != 0)
                            grid.one("dataBinding", function (one) {
                                clearwindowdata(view);
                                one.preventDefault();
                                dataItem["id"] = 0;
                                if (view.EditHandler != "") {

                                    var handlerFanction = getFunctionDelegate(view.EditHandler);
                                    if (handlerFanction != null)
                                        handlerFanction(e);

                                }
                            });
                    }
                    else if ($(".k-button.k-button-icontext.k-grid-update.k-grid-save-and-Copy").data('clicked') && errorOperation == null) {
                        $(".k-button.k-button-icontext.k-grid-update.k-grid-save-and-Copy").data('clicked', false);
                        var grid = $('#' + gridObj.Name).data("kendoGrid");
                        var show = CheckBeforeCreateForSEAndCN();
                        if (show != 0)
                            grid.one("dataBinding", function (one) {
                                one.preventDefault();
                                dataItem["id"] = 0;

                            });
                        
                    }
                    else if ($(".k-button.k-button-icontext.k-grid-update.k-grid-copy").data('clicked') && errorOperation == null) {
                        $(".k-button.k-button-icontext.k-grid-update.k-grid-copy").data('clicked', false);
                        var grid = $('#' + gridObj.Name).data("kendoGrid");
                        var show = CheckBeforeCreateForSEAndCN();
                        if (show != 0)
                            grid.one("dataBinding", function (one) {
                                one.preventDefault();
                                dataItem["id"] = 0;

                            });
                    }
                    else if ($(".k-button.k-button-icontext.k-grid-update.k-grid-edit-new").data('clicked') && errorOperation == null) {
                        $(".k-button.k-button-icontext.k-grid-update.k-grid-edit-new").data('clicked', false);
                        var grid = $('#' + gridObj.Name).data("kendoGrid");
                        var show = CheckBeforeCreateForSEAndCN();
                        if (show != 0)
                            grid.one("dataBinding", function (one) {
                                clearwindowdata(view);
                                one.preventDefault();
                                dataItem["id"] = 0;

                            });
                    }
                    else if (notsave) {
                        if (errorOperation == null && e.type == "create") {
                            var grid = $('#' + gridObj.Name).data('kendoGrid');
                            grid.dataSource.read();

                            window.localStorage.setItem("notsave", false);
                        }
                    }

                    if (view.AfterRequestEnd != "" && e.response.Errors == null) {
                        var afterRequestEndFanction = getFunctionDelegate(view.AfterRequestEnd);
                        if (afterRequestEndFanction != null)
                            afterRequestEndFanction(e);
                    }
                },
                transport: {
                    read: {
                        url: window.applicationpath + view.ReadUrl,
                        type: "POST",
                        //async: false,
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: { requestInformation: window.requestInformation, viewModelTypeFullName: this.GridModel.ViewModelTypeFullName }
                    },
                    update: {
                        url: window.applicationpath + view.UpdateUrl,
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: { requestInformation: window.requestInformation, viewModelTypeFullName: this.GridModel.ViewModelTypeFullName }
                    },
                    destroy: {
                        url: window.applicationpath + view.DestroyUrl,
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: { requestInformation: window.requestInformation, viewModelTypeFullName: this.GridModel.ViewModelTypeFullName }
                    },
                    create: {
                         url: window.applicationpath + view.CreateUrl,
                        
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: { requestInformation: window.requestInformation, viewModelTypeFullName: this.GridModel.ViewModelTypeFullName}
                      
                    },
                    parameterMap: function(oData, operation) {
         
                        if (operation == "read") {
                            if (oData.filter != null) {
                                 
                                for (var i = 0; i < oData.filter.filters.length; i++) {
                                    if (oData.filter.filters[i].filters != null) {
                                        for (var j = 0; j < oData.filter.filters[i].filters.length; j++)
                                        {
                                            var column = getColumnByName(gridObj.Views[0].Columns, oData.filter.filters[i].filters[j].field);
                                            if (column.IsTime)
                                                oData.filter.filters[i].filters[j].value.setFullYear(2000, 0, 01);


                                          }
                                         }
                                     else {
                                        var column = getColumnByName(gridObj.Views[0].Columns, oData.filter.filters[i].field);
                                        if (column.IsTime)
                                            oData.filter.filters[i].value.setFullYear(2000, 0, 01);
                                         }
                                  }    
                            }

                            oData.serverPaging = view.ServerPaging;
                            return JSON.stringify(oData);
                        }
                        $('.k-tooltip-validation').hide();
                        var result = {};
                        result.viewModelTypeFullName = oData.viewModelTypeFullName;
                        if (oData.CustomInformation != null)
                            result.customInformation = JSON.stringify(oData.CustomInformation);
                        //for notification
                        result.notificationTitle = $("#notification_grid_title").val();
                        var notificationTextControl = $("#notification_grid_text").data("kendoEditor");
                        if (notificationTextControl != null)
                            result.notificationString = notificationTextControl.value();
                        var notificationReceiversControl = $("#notification_grid_receivers").data("kendoMultiSelect");
                        if (notificationReceiversControl != null)
                            result.recievers = notificationReceiversControl.value();
                        if (operation != "destroy")
                            if (window.parent.gridModel.DetailModels != null) {
                                result.Details = [];
                            var detailModels = window.parent.gridModel.DetailModels;
                            for (var prop in detailModels) {
                                var _detailName = detailModels[prop].DetailName;
                                if ($(".grid-detail-" + _detailName).data("kendoGrid")) {

                                    var list = $(".grid-detail-" + _detailName).data("kendoGrid").dataSource._data;
                                    var listData = [];
                                    for (var i = 0; i < list.length; i++) {
                                        var obj = { Properties: [] };
                                        for (var objPropName in list[i]) {
                                            var objProp = {};
                                            objProp.PropName = objPropName;
                                            console.log(objPropName);
                                            if (list[i][objPropName] != null && list[i][objPropName].Id != null)
                                                objProp.Value = list[i][objPropName].Id;
                                            else
                                                objProp.Value = list[i][objPropName];
                                            obj.Properties.push(objProp);
                                        }
                                        listData.push(obj);
                                    }
                                    result.Details.push({ DetailName: prop, TypeFullNameViewModel: window.gridModel.DetailModels[prop].ViewModelTypeFullName, List: listData, RemovedObjects: window.gridModel.DetailModels[prop].RemovedObjects, OldObjects: window.gridModel.DetailModels[prop].OldObjects });
                                }
                            }
                        }

                        for (var prop in oData) {
                            if (prop == "viewModelTypeFullName")
                                continue;
                            if (prop == "requestInformation") {
                                result[prop] = oData[prop];
                            } else {
                                var field = getFieldByName(gridObj.SchemaFields, prop);
                               
                                if (field != null) {
                                
                                    if (field.Type == "date") {


                                        if (oData[prop] == null) {

                                            result["data[" + prop + "]"] = null;
                                        } else {



                                            var column = getColumnByName(gridObj.Views[0].Columns, prop);
                                            if (column.IsTime) {
                                                var date = new Date(kendo.parseDate(oData[prop], "hh:mm tt"));
                                                result["data[" + prop + "]"] = kendo.toString(new Date(date), "01/01/2000 hh:mm tt");
                                            }

                                            else if (column.IsDateTime) {
                                                
                                                var date = new Date(kendo.parseDate(oData[prop], "dd/MM/yyyy hh:mm tt"));
                                                result["data[" + prop + "]"] = kendo.toString(new Date(date), "dd/MM/yyyy hh:mm tt");
                                            }
                                            else {
                                                var date = new Date(kendo.parseDate(oData[prop], "dd/MM/yyyy"));
                                                result["data[" + prop + "]"] = kendo.toString(new Date(date), "dd/MM/yyyy");

                                            }
                                            if (result["data[" + prop + "]"].indexOf('NaN/NaN/0NaN') != -1) {
                                                result["data[" + prop + "]"] = $('[name="' + prop + '"]').val();
                                            }
                                            if ($('[name="' + prop + '"]').length == 0) {
                                                result["data[" + prop + "]"] = null;
                                            }

                                        }

                                    } else if (field.Type == "complex") {
                                        if (operation == "create") {
                                            //result["data[" + prop + "]"] = oData[prop];
                                            result["data[" + prop + "]"] = $("[name='" + prop + "']").val();
                                        } else {
                                            result["data[" + prop + "]"] = $("[name='" + prop + "']").val();
                                            //if (oData[prop].Id != null) {
                                            //    result["data[" + prop + "]"] = oData[prop].Id;
                                            //} else {
                                            //    result["data[" + prop + "]"] = oData[prop];
                                            //}
                                        }
                                        if (isNaN(result["data[" + prop + "]"])) {
                                            var temp = $('[name="' + prop + '"]').val();
                                            temp = Number(temp);
                                            result["data[" + prop + "]"] = temp;
                                        }
                                    } else {
                                        result["data[" + prop + "]"] = oData[prop];
                                    }
                                }
                            }
                        }
                        result.columns = view.Columns;
                        return JSON.stringify(result);
                    }
                },
                schema: {
                    model: {
                        id: "Id",
                        fields: this.GetSchemaFields()
                    },
                    data: this.GridModel.DataFieldName,
                    errors: this.GridModel.ErrorFieldName,
                    total: this.GridModel.TotalCountFieldName
                },
                error: function(e) {
                    var grid = $('#' + gridObj.Name).data("kendoGrid");

                    if (errorOperation == "destroy") {
                        grid.cancelChanges();
                    }

                    errorOperation = null;

                    grid.one("dataBinding", function (one) {
                        


                        one.preventDefault();

                       

                    });
                }
            };
            var lastStep = requestInformation.NavigationInfo.Previous[requestInformation.NavigationInfo.Previous.length - 1];
            if (lastStep.FromBreadcrumb) {

                result.pageSize = lastStep.PageSize;
                result.page = lastStep.PageNumber;
                var temp = [];
                var temp1 = [];
                result.sort = adjustSortList(lastStep.Sort, temp1);
                result.filter = adjustFilterData(lastStep.Filter, temp);

            }

            var globalAggregate = getGlobalAggregate(view);
            if (globalAggregate.length > 0) {
                result.aggregate = globalAggregate;
            }
           
            return result;
        };

        this.GetDataSourceForDetail = function (viewModelTypeFullName, TypeFullName, DetailName, parent) {

            var first = true;
            var errorOperation = null;
            var gridObj = this.GridModel;
            window.gridModel.DetailModels[gridObj.DetailName].OldData = gridObj.LocalData;
            var view = getViewById(this.GridModel.Views, this.GridModel.CurrentViewId);
            var result = {
                data: gridObj.LocalData,
                requestStart: function (e) {
                 
                    if (e.type == 'read') {
                      
                        e.preventDefault();
                        if (first) {
                            first = false;
                            if (gridObj.LocalData.length == 0)
                                return;
                            for (var i = 0 ; i < gridObj.LocalData.Data.length; i++) {
                                var localData = gridObj.LocalData.Data[i];
                                for (var prop in localData) {
                                    var field = getFieldByName(window.parent.gridModel.DetailModels[DetailName].SchemaFields, prop);
                                    if (field != null) {

                                        if (field.Type == "date") {
                                            var column = getColumnByName(view.Columns, prop);
                                            if (column.IsTime) {
                                                var date = localData[prop];
                                                var _date = new Date();
                                                if (kendo.parseDate(localData[prop], "hh:mm tt") != undefined)
                                                    _date = new Date(kendo.parseDate(localData[prop], "hh:mm tt"));
                                                else
                                                    _date = new Date(date);
                                                localData[prop] = kendo.toString(_date, "hh:mm tt");
                                                //// test[prop] = kendo.toString(new Date(date), "dd/MM/yyyy "); //if (objProp.Value.indexOf('NaN/NaN/0NaN') != -1) {
                                                if (localData[prop].indexOf('NaN/NaN/0NaN') != -1) {
                                                    localData[prop] = $('[name="' + prop + '"]').val();
                                                }
                                            }
                                            else if (column.IsDateTime) {
                                                var date = localData[prop];
                                                var _date = new Date();
                                                if (kendo.parseDate(localData[prop], "dd/MM/yyyy hh:mm tt") != undefined)
                                                    _date = new Date(kendo.parseDate(localData[prop], "dd/MM/yyyy hh:mm tt"));
                                                else
                                                    _date = new Date(date);
                                                localData[prop] = kendo.toString(_date, "dd/MM/yyyy hh:mm tt");
                                             
                                            }

                                            else {
                                                var date = new Date(kendo.parseDate(localData[prop], "dd/MM/yyyy"));
                                                localData[prop] = kendo.toString(date, "dd/MM/yyyy");
                                                //// test[prop] = kendo.toString(new Date(date), "dd/MM/yyyy "); //if (objProp.Value.indexOf('NaN/NaN/0NaN') != -1) {
                                                if (localData[prop].indexOf('NaN/NaN/0NaN') != -1) {
                                                    localData[prop] = $('[name="' + prop + '"]').val();
                                                }
                                            }

                                        }
                                    }
                                }

                                $(".grid-detail-" + DetailName
                              ).data("kendoGrid").dataSource.add(localData);
                                $(".grid-detail-" + DetailName
                              ).data("kendoGrid").dataSource.sync();
                            }
                        }
                    }
                },

                requestEnd: function (e) {
                     

                    var dataItem = {};
                    if (e.type == "read") {

                    } else if (e.type == "create") {

                        dataItem = e.sender.get(0);
                        dataItem.IsVirtualNew = true;
                        //var newId = Souccar.getMaxValueByPropName(e.sender._data, "Id");
                        //if (newId == null)
                        //    newId = 1;
                        //else
                        //    newId = newId + 1;
                        //dataItem.Id = newId;
                    }
                    else if (e.type == "update") {
                        dataItem = e.sender.get(e.response.Data.Id);
                        dataItem.dirty = true;
                        dataItem.IsDirty = true;

                    } else if (e.type == "destroy") {

                        if (e.response[gridObj.ErrorFieldName] != null) {
                            ShowMessageBox(gridObj.ResError, e.response[gridObj.ErrorFieldName], "k-icon w-b-error", [{ Title: gridObj.ResOk, ClassName: "k-icon k-update" }]);
                            errorOperation = e.type;
                        } else {
                            if (gridObj.RemovedObjects == null)
                                gridObj.RemovedObjects = [];
                            var obj = { Properties: [] };
                            if (!e.sender._destroyed[0].IsVirtualNew) {
                                for (var objPropName in e.sender._destroyed[0]) {
                                    var objProp = {};
                                    objProp.PropName = objPropName;
                                    var field = getFieldByName(window.parent.gridModel.DetailModels[DetailName].SchemaFields, objPropName);
                                    if (field != null) {
                                        if (field.Type == "date") {
                                            var date = new Date(e.sender._destroyed[0][objPropName]);
                                            objProp.Value = kendo.toString(new Date(date), "dd/MM/yyyy hh:mm tt");
                                            if ($('[name="' + objPropName + '"]').length == 0) {
                                                objProp.Value = null;
                                            }
                                        }
                                        if (field.Type == "complex") {
                                            objProp.Value = $("[name='" + objPropName + "']").val();
                                            if (isNaN(objProp.Value)) {
                                                var temp = $('[name="' + objProp.Valu + '"]').val();
                                                temp = Number(temp);
                                                objProp.Value = temp;
                                            }
                                        }
                                        else {

                                            if (e.sender._destroyed[0][objPropName] != null && e.sender._destroyed[0][objPropName].Id != null)
                                                objProp.Value = e.sender._destroyed[0][objPropName].Id;
                                            else
                                                objProp.Value = e.sender._destroyed[0][objPropName];

                                        }

                                    }
                                    else {
                                        if (e.sender._destroyed[0][objPropName] != null && e.sender._destroyed[0][objPropName].Id != null)
                                            objProp.Value = e.sender._destroyed[0][objPropName].Id;
                                        else
                                            objProp.Value = e.sender._destroyed[0][objPropName];

                                    }

                                    obj.Properties.push(objProp);
                                }
                                gridObj.RemovedObjects.push(obj);
                            }
                        }
                    }

                    if ((e.type == "create") || (e.type == "update")) {
                         
                        for (var member in e.response.KeyValueObj) {
                             
                            if (member != "Id")
                                dataItem[member] = e.response.KeyValueObj[member];
                        }
                        if (e.response[gridObj.ErrorFieldName] != null) {
                            if (e.response[gridObj.ErrorFieldName]["Exception"] != null) {
                                ShowMessageBox(gridObj.ResError, e.response[gridObj.ErrorFieldName].Exception, "k-icon w-b-error", [{ Title: gridObj.ResOk, ClassName: "k-icon k-update" }]);
                            } else {
                                for (var prop in e.response[gridObj.ErrorFieldName]) {                                    
                                    var messageText = e.response[gridObj.ErrorFieldName][prop];
                                    if (prop == "") {
                                        ShowMessageBox(gridObj.ResError, messageText, "k-icon w-b-error", [{ Title: gridObj.ResOk, ClassName: "k-icon k-update" }]);
                                    } else {
                                        var t = kendo.template($('#TooltipInvalidMessageTemplate').html())({ message: messageText });
                                        var affectedControl = getControlByFieldName(prop, gridObj);
                                        if (affectedControl != null)
                                            affectedControl.after(t);
                                        else
                                            ShowMessageBox(gridObj.ResError, messageText, "k-icon w-b-error", [{ Title: gridObj.ResOk, ClassName: "k-icon k-update" }]);
                                    }

                                }
                            }
                            errorOperation = e.type;
                        }
                    }
                    if (view.AfterRequestEnd != "" && e.response.Errors == null) {
                        var afterRequestEndFanction = getFunctionDelegate(view.AfterRequestEnd);
                        if (afterRequestEndFanction != null)
                            afterRequestEndFanction(e);
                    }
                },

                transport: {
                    read:
                        {}
                   ,
                    create: {
                        url: window.applicationpath + "Crud/ValidateMasterDetail",
                        type: "POST",
                        dataType: "json",
                        //async: false,
                        contentType: "application/json; charset=utf-8",
                        data: { requestInformation: window.requestInformation, viewModelTypeFullName: this.GridModel.ViewModelTypeFullName, parent: parent }
                    },
                    update: {
                        url: window.applicationpath + "Crud/ValidateMasterDetail",
                        type: "POST",
                        dataType: "json",
                        //async: false,
                        contentType: "application/json; charset=utf-8",
                        data: { requestInformation: window.requestInformation, viewModelTypeFullName: this.GridModel.ViewModelTypeFullName, parent: parent },
                    }
                    , destroy: {}
                    ,
                    parameterMap: function (oData, operation) {
             
                        var list = window.gridModel.DetailModels[gridObj.DetailName].OldData.Data;
                        var listData = [];
                        if(list!=null)
                        for (var i = 0; i < list.length; i++) {
                                if (!((prop == DetailName) && (list[i]["Id"]) == orginaldata["Id"])) {
                                    var obj = { Properties: [] };
                                    for (var objPropName in list[i]) {
                                        var objProp = {};
                                        objProp.PropName = objPropName;
                                        console.log(objPropName);
                                        if (list[i][objPropName] != null && list[i][objPropName].Id != null)
                                            objProp.Value = list[i][objPropName].Id;
                                        else
                                            objProp.Value = list[i][objPropName];
                                        obj.Properties.push(objProp);
                                    }


                                    listData.push(obj);
                                }
                        }
                        window.gridModel.DetailModels[gridObj.DetailName].OldObjects = listData;
                        var orginaldata = null;

                        var resultdata = {};
                        if (operation == "read" || operation == "destroy") {
                            if (oData.filter != null)
                                for (var i = 0; i < oData.filter.filters.length; i++) {
                                     
                                    var column = getColumnByName(gridObj.Views[0].Columns, oData.filter.filters[i].field);
                                    if (column.IsTime)
                                        oData.filter.filters[i].value.setFullYear(2000, 0, 01);
                                }
                            return JSON.stringify(oData);
                        }

                        if (operation == "update") {
                            orginaldata = JSON.parse(window.localStorage.getItem("orginaldata"));

                            for (var objorginaldataName in orginaldata) {
                                var field = getFieldByName(window.parent.gridModel.DetailModels[DetailName].SchemaFields, objorginaldataName);
                                if (field != null) {
                                    

                                    if (orginaldata[objorginaldataName] != null && orginaldata[objorginaldataName].Id != null)
                                        orginaldata[objorginaldataName] = orginaldata[objorginaldataName].Id;
                                   
                                    
                                }

                            }
                        }

                        if (oData.CustomInformation != null)
                            resultdata.customInformation = JSON.stringify(oData.CustomInformation);

                        if (window.parent.gridModel.DetailModels != null) {
                            resultdata.Details = [];
                            var detailModels = window.parent.gridModel.DetailModels;
                            for (var prop in detailModels) {

                                if (window.parent.gridModel.DetailModels[prop].KendoGridObject.data("kendoGrid") != undefined) {

                                    var list = window.parent.gridModel.DetailModels[prop].KendoGridObject.data("kendoGrid").dataSource._data;
                                    var listData = [];
                                    for (var i = 0; i < list.length; i++) {
                                        if (operation == "update") {
                                            if (!((prop == DetailName) && (list[i]["Id"]) == orginaldata["Id"])) {
                                                var obj = { Properties: [] };
                                                for (var objPropName in list[i]) {
                                                    var objProp = {};
                                                    objProp.PropName = objPropName;
                                                    console.log(objPropName);
                                                    if (list[i][objPropName] != null && list[i][objPropName].Id != null)
                                                        objProp.Value = list[i][objPropName].Id;
                                                    else
                                                        objProp.Value = list[i][objPropName];
                                                    obj.Properties.push(objProp);
                                                }


                                                listData.push(obj);
                                            }
                                        }
                                        else {
                                           
                                            if (i == 0)
                                                continue;
                                            

                                                var obj = { Properties: [] };
                                                for (var objPropName in list[i]) {
                                                    var objProp = {};
                                                    objProp.PropName = objPropName;
                                                    console.log(objPropName);
                                                    if (list[i][objPropName] != null && list[i][objPropName].Id != null)
                                                        objProp.Value = list[i][objPropName].Id;
                                                    else
                                                        objProp.Value = list[i][objPropName];
                                                    obj.Properties.push(objProp);
                                                }
                                              
                                            
                                            listData.push(obj);
                                        }


                                    }
                                }

                                resultdata.Details.push({ DetailName: prop, TypeFullNameViewModel: window.gridModel.DetailModels[prop].ViewModelTypeFullName, List: listData, RemovedObjects: window.gridModel.DetailModels[prop].RemovedObjects,OldObjects: window.gridModel.DetailModels[gridObj.DetailName].OldObjects });
                            }
                        }

                        for (var prop in oData) {
                            if (prop == "viewModelTypeFullName")
                                continue;
                            if (prop == "requestInformation") {
                                resultdata[prop] = oData[prop];
                            } else {
                                var field = getFieldByName(gridObj.SchemaFields, prop);
                                if (field != null) {
                                    if (field.Type == "date") {
                                        
                                        if (oData[prop] == null) {
                                            
                                            resultdata["data[" + prop + "]"] = null;
                                        } else {
                                            var column = getColumnByName(view.Columns, prop);
                                          
                                            if (column.IsTime) {
                                                var date = new Date(kendo.parseDate(oData[prop], "hh:mm tt"));
                                                resultdata["data[" + prop + "]"] = kendo.toString(new Date(date), "01/01/2000 hh:mm tt");
                                            }


                                            else if (column.IsDateTime) {

                                                var date = new Date(kendo.parseDate(oData[prop], "dd/mm/yyyy hh:mm tt"));
                                                resultdata["data[" + prop + "]"] = kendo.toString(new Date(date), "dd/mm/yyyy hh:mm tt");
                                            }
                                            else
                                            {
                                                var date = new Date(kendo.parseDate(oData[prop], "dd/mm/yyyy"));
                                                resultdata["data[" + prop + "]"] = kendo.toString(new Date(date), "dd/MM/yyyy");

                                            }


                                            if (resultdata["data[" + prop + "]"].indexOf('NaN/NaN/0NaN') != -1) {
                                                resultdata["data[" + prop + "]"] = $('[name="' + prop + '"]').val();
                                            }
                                            if ($('[name="' + prop + '"]').length == 0) {
                                                resultdata["data[" + prop + "]"] = null;
                                            }
                                            
                                        }

                                    } else if (field.Type == "complex") {
                                        if (operation == "create") {
                                           
                                            resultdata["data[" + prop + "]"] = $("[name='" + prop + "']").val();
                                        } else {
                                            resultdata["data[" + prop + "]"] = $("[name='" + prop + "']").val();
                                          
                                        }
                                        if (isNaN(resultdata["data[" + prop + "]"])) {
                                            var temp = $('[name="' + prop + '"]').val();
                                            temp = Number(temp);
                                            resultdata["data[" + prop + "]"] = temp;
                                        }
                                       

                                    } else {
                                        resultdata["data[" + prop + "]"] = oData[prop];
                                    }
                                }
                            }


                        }
                         

                        for (var objparentName in parent) {
                           var field = getFieldByName(window.parent.gridModel.SchemaFields, objparentName);
                           if (field != null) {

                               if (field.Type == "complex") {

                                    parent[objparentName] = $("[name='" + objparentName + "']").val();


                                 if (isNaN(parent[objparentName])) {
                                      var temp = $("[name='" + objparentName + "']").val();
                                      temp = Number(temp);
                                      parent[objparentName] = temp;
                                 }
                                 continue;
                               }

                                if (field.Type == "date") {

                                    if (parent[objparentName] != null) {
                                        var date = new Date(parent[objparentName]);
                                        parent[objparentName] = kendo.toString(new Date(kendo.parseDate(parent[objparentName], "dd/MM/yyyy")), "dd/MM/yyyy ");

                                        if (parent[objparentName].indexOf('NaN/NaN/0NaN') != -1) {
                                            parent[objparentName] = $("[name='" + objparentName + "']").val();
                                        }

                                    }
                                }
                           }
                           if (checkPropertyNameInRequestInfo(objparentName)) {
                               
                               if (parent[objparentName] != null)
                                   parent[objparentName] = parent[objparentName].Id;
                               else
                                   parent[objparentName] = 0
                           }

                            if (parent[objparentName] != null && parent[objparentName].Id != null)
                                parent[objparentName] = parent[objparentName].Id;

                        }

                        resultdata.parent = parent;

                        resultdata.viewModelTypeFullName
                     = viewModelTypeFullName;
                        resultdata.TypeFullName
                       = TypeFullName;

                        resultdata.orginaldata = orginaldata;
                        return JSON.stringify(resultdata);


                    }
                },

                schema: {

                    model: {
                        id: "Id",
                        fields: this.GetSchemaFields()
                    },
                    data: gridObj.DataFieldName,

                    errors: gridObj.ErrorFieldName,
                    total: gridObj.TotalCountFieldName
                },

                error: function (e) {

                    var grid = $(".grid-detail-" + DetailName
                        ).data("kendoGrid");

                    if (errorOperation == "destroy") {
                        grid.cancelChanges();
                    }

                    errorOperation = null;


                    grid.one("dataBinding", function (one) {


                        one.preventDefault();


                    });

                }
            };
            var globalAggregate = getGlobalAggregate(view);
            if (globalAggregate.length > 0) {
                result.aggregate = globalAggregate;
            }



            return result;

        };
        this.GetSchemaFields = function () {
             
            var data = this.GridModel;
            var view = getViewById(data.Views, data.CurrentViewId);
            var result = {};
            for (var i = 0; i < data.SchemaFields.length; i++) {
                var field = data.SchemaFields[i];

                var validation = {};
                if (field.Type != "date") {
                    for (var member in field.ValidationRules) {
                        validation[member] = {};

                        for (var value in field.ValidationRules[member]) {
                            validation[member][value] = field.ValidationRules[member][value];
                        }
                    }
                }

                if (field.Type == "complex") {
                    result[field.Name] = {
                        editable: field.Editable,

                        validation: {
                            complexRequired: complexRequired
                        }
                    };
                }

















                else if (field.Type == "date") {

                    result[field.Name] = {
                        type: field.Type,
                        editable: field.Editable,
                        validation: {
                            dateValidation: dateValidation
                        }
                    };
                } else if (field.Type == "image") {
                    var column = getColumnByName(view.Columns, field.Name);
                    result[field.Name] = {
                        type: field.Type,
                        defaultValue: column.DefaultImageName,
                        editable: false
                    };
                } else if (field.Type == "file") {
                    var column = getColumnByName(view.Columns, field.Name);
                    result[field.Name] = {
                        type: field.Type,
                        defaultValue: "",
                        editable: field.Editable
                    };
                } else {
                    result[field.Name] = {
                        type: field.Type,
                        editable: field.Editable,
                        validation: validation
                    };
                }
            }

            return result;
        };

        this.AddFieldSets = function(e) {

            var view = getViewById(this.GridModel.Views, this.GridModel.CurrentViewId);
            if (!view.ShowGroup || view.Groups.length == 0) {
                e.container.find('.k-edit-form-container').prepend("<div class='controls-container controls-div'></div>");
                for (var k = 0; k < view.Columns.length; k++) {
                    var column = view.Columns[k];
                    if (column.FieldName != "Id") {
                        var control = e.container.find('[data-container-for=' + column.FieldName + ']');
                        if (control != null && control.length != 0) {
                            e.container.find(".controls-div").append("<div class='control control-" + column.FieldName + "'></div>");
                            e.container.find(".control-" + column.FieldName).append(control.prev());
                            e.container.find(".control-" + column.FieldName).append(control);
                        }
                    }
                }
            } else {

                e.container.find('.k-edit-form-container').prepend("<div class='controls-container fieldsets-div'></div>");
                for (var i = 0; i < view.Groups.length; i++) {
                    var group = view.Groups[view.Groups.length - (i + 1)];
                    var fieldset = "<fieldset class='fieldset-" + group.Name + "'>";
                    if (group.Name != "") {
                        fieldset += "<legend align='center'>" + group.Title + "</legend>";
                    }
                    fieldset += "</fieldset>";

                    e.container.find(".fieldsets-div").prepend(fieldset);
                    for (var j = 0; j < group.Columns.length; j++) {
                        var column = group.Columns[j];
                        var control = e.container.find('[data-container-for=' + column.FieldName + ']');
                        if (control != null && control.length != 0) {
                            e.container.find(".fieldset-" + group.Name).append("<div class='control control-" + column.FieldName + "'></div>");
                            e.container.find(".control-" + column.FieldName).append(control.prev());
                            e.container.find(".control-" + column.FieldName).append(control);
                        }
                    }
                }
            }
        };
        //this.IsMastergrid = function (e) {
        //  this.GridModel.ViewFactory.  
        //};
        this.ShowTwoColumns = function(e) {
            var view = getViewById(this.GridModel.Views, this.GridModel.CurrentViewId);
            if (!view.ShowTwoColumns) {
                e.container.find(".k-edit-form-container").addClass("one-column-popup");
                return;
            }
            e.container.find(".k-edit-form-container").addClass("two-columns-popup");
            if (!view.ShowGroup || view.Groups.length == 0) {
                var columnNumber = 0;
                for (var k = 0; k < view.Columns.length; k++) {
                    var column = view.Columns[k];
                    if (column.FieldName != "Id") {
                        var control = e.container.find(".control-" + column.FieldName);
                        if (control != null && control.length != 0) {
                            if (columnNumber++ % 2 == 0) {
                                control.addClass("first-column");
                            } else {
                                control.addClass("second-column");
                            }
                        }
                    }
                }
            } else {
                for (var i = 0; i < view.Groups.length; i++) {
                    var group = view.Groups[view.Groups.length - (i + 1)];
                    var columnNumber = 0;
                    for (var j = 0; j < group.Columns.length; j++) {
                        var column = group.Columns[j];
                        if (column.FieldName != "Id") {
                            var control = e.container.find(".control-" + column.FieldName);
                            if (control != null && control.length != 0) {
                                if (columnNumber++ % 2 == 0) {
                                    control.addClass("first-column");
                                } else {
                                    control.addClass("second-column");
                                }
                            }
                        }
                    }
                }
            }
        };
        this.AddRequiredStyle = function(e) {
            var view = getViewById(this.GridModel.Views, this.GridModel.CurrentViewId);
            for (var k = 0; k < view.Columns.length; k++) {
                var column = view.Columns[k];
                if (column.IsRequired) {
                    e.container.find(".control-" + column.FieldName).addClass("required");
                    e.container.find(("[for=" + column.FieldName + "]")).append("<span class='equired-star'>*</span>");
                }
            }
        };
        this.PreventCommaInNumericTextBox = function(e) {
            var view = getViewById(this.GridModel.Views, this.GridModel.CurrentViewId);

            for (var i = 0; i < view.Columns.length; i++) {
                var field = getFieldByName(this.GridModel.SchemaFields, view.Columns[i].FieldName);
                if (field.Type == "number") {
                    var name = "[name='" + view.Columns[i].FieldName + "']";
                    var control = e.container.find(name);
                    if (control != null && control.length != 0) {
                        //control.data("kendo-NumericTextBox").step(view.Columns[i].Step);
                        if (view.Columns[i].ShowCommaSeparator == false) {
                            //e.container.find(name).keydown(function (key) {
                            //    if (key.keyCode == 190 || key.keyCode == 110 || (key.keyCode = 48 && key.keyCode == 57) || (key.keyCode = 96 && key.keyCode == 105)) {
                            //        $(this).val(this.value.match(/[0-9]*/));
                            //    }
                            //});
                            e.container.find(name).keyup(function(key) {
                                if (key.keyCode == 190 || key.keyCode == 110 || (key.keyCode = 48 && key.keyCode == 57) || (key.keyCode = 96 && key.keyCode == 105)) {
                                    $(this).val(this.value.match(/[0-9]*/));
                                }
                            });
                        }
                    }
                }
            }
        };

        this.PreventStandardDateValueInTimeFieldsForDetail = function (e) {
            var view = getViewById(this.GridModel.Views, this.GridModel.CurrentViewId);

            for (var i = 0; i < view.Columns.length; i++) {
                var field = getFieldByName(this.GridModel.SchemaFields, view.Columns[i].FieldName);
                if (field.Type == "date") {
                    var column = view.Columns[i];
                    var name = "[name='" + column.FieldName + "']";
                    var control = e.container.find(name);

                    if (control != null && control.length != 0) {
                        if (column.IsTime) {
                            var controlValSplited = control.val().split(' ');
                            var val = controlValSplited.length > 1 && controlValSplited[1] == 'AM' ?
                                controlValSplited[0] : controlValSplited[0].split(':').length > 1 ?
                                (+controlValSplited[0].split(':')[0] + 12).toString() + ':' + controlValSplited[0].split(':')[1] :
                                new Date();
                            control.val(getTimeAsStandardToShowInClockTimePicker(new Date(2000, 01, 01, val.split(':')[0], val.split(':')[1])));
                        }
                    }
                }
            }
        };
        this.PreventStandardDateValueInTimeFields = function (e) {
            var view = getViewById(this.GridModel.Views, this.GridModel.CurrentViewId);

            for (var i = 0; i < view.Columns.length; i++) {
                var field = getFieldByName(this.GridModel.SchemaFields, view.Columns[i].FieldName);
                if (field.Type == "date") {
                    var column = view.Columns[i];
                    var name = "[name='" + column.FieldName + "']";
                    var control = e.container.find(name);

                    if (control != null && control.length != 0) {
                        if (column.IsTime) {
                            control.val(getTimeAsStandardToShowInClockTimePicker(control.val()));
                        }
                    }
                }
            }
        };
        this.PreventDefaultValueInDateTimeFields = function (e) {
            var view = getViewById(this.GridModel.Views, this.GridModel.CurrentViewId);

            for (var i = 0; i < view.Columns.length; i++) {
                var field = getFieldByName(this.GridModel.SchemaFields, view.Columns[i].FieldName);
                if (field.Type == "date") {
                    var column = view.Columns[i];
                    var name = "[name='" + column.FieldName + "']";
                    var control = e.container.find(name);

                    if (control != null && control.length != 0) {
                        if (!column.IsRequired) {
                            for (key in e.model)
                                if (column.FieldName == key)
                                    e.model[key] = null;
                            if (column.IsTime)
                                control.val(null);
                            else
                                control.data("kendoDatePicker").value(null);
                        }
                        else {
                            if (column.IsTime) {
                                control.val(getTimeAsStandardToShowInClockTimePicker(control.val()));
                            }
                        }
                    }
                }
            }
        };
    this.PageableMessage = {
            refresh: true,
            pageSizes: [10, 20, 50, 100, 500],
            messages: {
                display: this.GridModel.Display,
                empty: this.GridModel.Empty,
                page: this.GridModel.Page,
                of: this.GridModel.Of,
                itemsPerPage: this.GridModel.ItemsPerPage,
                first: this.GridModel.First,
                previous: this.GridModel.Previous,
                next: this.GridModel.Next,
                last: this.GridModel.Last,
                refresh: this.GridModel.Refresh
            }
        };
    this.FilterableMessage = {
        messages: {
            info: this.GridModel.FilterBy,
            filter: this.GridModel.Filter,
            clear: this.GridModel.Clear,
            isTrue: this.GridModel.IsTrue,
            isFalse: this.GridModel.IsFalse,
            and: this.GridModel.And,
            or: this.GridModel.Or
        },
        operators: {
            string: {
                eq: this.GridModel.EQ,
                neq: this.GridModel.NET,
                startswith: this.GridModel.StartsWith,
                contains: this.GridModel.Contains,
                endswith: this.GridModel.EndsWith
            },
            number: {
                eq: this.GridModel.EQ,
                neq: this.GridModel.NET,
                gte: this.GridModel.GTE,
                gt: this.GridModel.GT,
                lte: this.GridModel.LTE,
                lt: this.GridModel.LT
            },
            date: {
                eq: this.GridModel.EQ,
                neq: this.GridModel.NET,
                gte: this.GridModel.GTE,
                gt: this.GridModel.GT,
                lte: this.GridModel.LTE,
                lt: this.GridModel.LT
            },
            enums: {
                eq: this.GridModel.EQ,
                neq: this.GridModel.NET
            }
        },
        extra: false
    };
    this.datefilter = {
        messages: {
            info: this.GridModel.FilterBy,
            filter: this.GridModel.Filter,
            clear: this.GridModel.Clear,
            isTrue: this.GridModel.IsTrue,
            isFalse: this.GridModel.IsFalse,
            and: this.GridModel.And,
            or: this.GridModel.Or
        },
        operators: {

            string: {
                eq: this.GridModel.EQ,
                neq: this.GridModel.NET,
                startswith: this.GridModel.StartsWith,
                contains: this.GridModel.Contains,
                endswith: this.GridModel.EndsWith
            },
            number: {
                eq: this.GridModel.EQ,
                neq: this.GridModel.NET,
                gte: this.GridModel.GTE,
                gt: this.GridModel.GT,
                lte: this.GridModel.LTE,
                lt: this.GridModel.LT
            },

            enums: {
                eq: this.GridModel.EQ,
                neq: this.GridModel.NET,
                gte: ">=",
                gt: ">",
                lte: "<=",
                lt: "<"
            }

            ,
            date: {
                eq: this.GridModel.EQ,
                neq: this.GridModel.NET,
                gte: this.GridModel.GTE,
                gt: this.GridModel.GT,
                lte: this.GridModel.LTE,
                lt: this.GridModel.LT
            },
        },
        extra: true
    };

}



    

