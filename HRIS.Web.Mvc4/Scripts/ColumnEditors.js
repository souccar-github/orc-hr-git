//After Apply Master Detail Feature
function getDropDownEditor(container, options) {
  //  var DetailName = options.values.DetailName;
    //Store
   window.localStorage.setItem('DetailName', options.values.DetailName);
    var addabelIndexClass = "";
    if (options.values.ShowAddButton || options.values.ShowInfoButton) {
        addabelIndexClass = "addabelIndex";
    }

    var dropdownData = {
        optionLabel:window.gridModel.FilterDropdownLabel,
        autoBind: false,
        filter: "contains",
        filtering: function (e) {
             
            //get filter descriptor
            var filter = e.filter;

            // handle the event
        },
        
        dataTextField: options.values.DataTextField,
        dataValueField: options.values.DataValueField,
        select: function (e) {
            console.log(options.values.Id + '_Select');
            var dataItem = this.dataItem(e.item.index());
            options.model[options.field] = dataItem.Id;
            options.model[options.field + 'Text'] = dataItem.Name;
            if (options.model['KeyValueObj'] == null) {
                options.model['KeyValueObj'] = {};
            }
            options.model['KeyValueObj'][options.field] = { Id: dataItem.Id, Name: dataItem.Name };
            window.localStorage.setItem('detailData', options.model['KeyValueObj'][options.field]["Name"]);
            
        },
        close: function (e) {
            if (e.sender.select() == 0) {
                var dataItem = e.sender.dataItem(0);
                if (dataItem != null) {
                    options.model[options.field] = e.sender.dataItem(0).Id;
                    options.model[options.field + 'Text'] = e.sender.dataItem(0).Name;
                    if (options.model['KeyValueObj'] == null) {
                        options.model['KeyValueObj'] = {};
                    }
                    options.model['KeyValueObj'][options.field] = { Id: e.sender.dataItem(0).Id, Name: e.sender.dataItem(0).Name };
                }
            }
        },
        cascade: function (e) {

            forceCascade(e.sender.element.attr('data-field-name'), []);
            //console.log(options.values.Id + '_cascade');
            //var control = $('[data-cascade-from=' + e.sender.element.attr('data-field-name') + ']').data('kendoDropDownList');
            //if (this.value() == "" && control != null) {
            //    $('[data-cascade-from=' + e.sender.element.attr('data-field-name') + ']').val("");
            //    control.text('');
            //    control._cascade();
            //}
        },
        dataSource: {
            type: "json",
            serverFiltering: false,

            requestEnd: function (e) {
                if (e.type == "read") {
                    if (!options.values.Required) {
                         
                        if (e.response!=undefined)
                        e.response.Data[e.response.Data.length] = { Id: 0, Name: "" };
                    }
                } else if (e.type == "create") {
                    if (e.response.Errors != null) {
                        if (e.response["Errors"].Exception != null) {
                            ShowMessageBox("Error", e.response["Errors"].Exception, "k-icon w-b-error", [{ Title: "Ok", ClassName: "k-icon k-update" }]);
                        } else {
                            ShowMessageBox("Error", e.response["Errors"][0].Message, "k-icon w-b-error", [{ Title: "Ok", ClassName: "k-icon k-update" }]);
                        }
                    } else {
                        options.model[options.field] = e.response.Data.Id;
                        options.model.dirty = true;
                        var dropDownList = $("#dropDownList" + options.values.Id).data("kendoDropDownList");

                        dropDownList.select(function (dataItem) {
                            if (dataItem.Name == e.response.Data.Name) {
                                dataItem.Id = e.response.Data.Id;
                                return dataItem.Name;
                            }
                            return null;
                        });
                    }
                }
            },
            transport: {
                read: {
                    cache: true,
                    url: window.applicationpath + options.values.ReadUrl,
                    type: "POST",
                    dataType: "json",
                    data: { requestInformation: window.requestInformation, typeName: options.values.TypeFullName },
                    contentType: "application/json; charset=utf-8"
                },
                create: {
                    url: window.applicationpath + options.values.CreateUrl,
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8"
                },
                parameterMap: function (innerData, operation) {
                    if (operation == "read") {
                        return JSON.stringify(innerData);
                    }

                    var finalResult = {};
                    finalResult["indexName"] = options.values.TypeFullName;
                    finalResult["value"] = innerData.Name;

                    return JSON.stringify(finalResult);
                }
            },
            schema: {
                model: {
                    id: "Id"
                },
                data: "Data",
                errors: "Errors",
            },
            error: function (e) {
                this.cancelChanges();
            }
        }
    };
    var cascadeAttr = '';
    var fieldName = ' data-field-name="' + options.values.Id + '" ';
    if (options.values.HasParent) {
        dropdownData.cascadeFrom = 'dropDownList' + options.values.CascadeFrom;
        dropdownData.cascadeFromField = "ParentId";
        cascadeAttr = ' data-cascade-from="' + options.values.CascadeFrom + '" ';

    }
    $('<input ' + fieldName + cascadeAttr + ' class="dropDownList ' + addabelIndexClass + '"  id="dropDownList' + options.values.Id + '" data-bind="value: ' + options.field + '" name="' + options.field + '" />')
    .appendTo(container)
    .kendoDropDownList(dropdownData);
    if (options.values.ShowAddButton) {
        appendAddIndexWindowHtml(container, options.values.Id, options.values.WindowTitle, "dropDownList");
    }

    if (options.values.ShowInfoButton) {
        appendInfoButtonHtml(container, options.values.Id, options.values.IndexName, options.values.WindowTitle);
    }
}







function forceCascade(fieldName, list) {
    //console.log(fieldName + '_cascade');
    //if (fieldName == null || fieldName == "")
    //    return;
    //var control = $('[data-field-name=' + fieldName + ']');
    //var cascadeControl = $('[data-cascade-from=' + fieldName + ']').data('kendoDropDownList');
    //if (control.val() == "" && cascadeControl != null) {
    //    $('[data-cascade-from=' + fieldName + ']').val("");
    //    cascadeControl.text('');
    //    forceCascade($('[data-cascade-from=' + fieldName + ']').attr('data-field-name'));
    //} else if (cascadeControl != null) {
    //    cascadeControl.refresh();
    //}
}
function getAutoCompleteEditor(container, options) {
    $('<input id="autoComplete' + options.values.Id + '" data-bind="value: ' + options.field + '.Name" name="' + options.field + '"/>')
    .appendTo(container)
    .kendoAutoComplete({
        autoBind: false,
        dataTextField: options.values.DataTextField,
        dataValueField: options.values.DataValueField,
        change: function (e) {
            for (var i = 0; i < e.sender.items().length; i++) {
                if (e.sender.value() == e.sender.items()[i].innerHTML) {
                    return;
                }
            }

            e.sender.value('');
        },
        select: function (e) {
            var dataItem = this.dataItem(e.item.index());
            options.model[options.field] = dataItem.Id;
        },
        close: function (e) {
            if (e.sender.select() == 0) {
                var dataItem = e.sender.dataItem(0);
                if (dataItem != null)
                    options.model[options.field] = e.sender.dataItem(0).Id;
            }
        },
        dataSource: {
            type: "json",
            serverFiltering: false,
            requestEnd: function (e) {
                if (e.type == "create") {
                    if (e.response.Errors != null) {
                        if (e.response[data.ErrorFieldName].Exception != null) {
                            ShowMessageBox("Error", "Could not " + e.type + " element: " + e.response[data.ErrorFieldName].Exception, "k-icon w-b-error", [{ Title: "Ok", ClassName: "k-icon k-update" }]);
                        } else {
                            ShowMessageBox("Error", "Could not " + e.type + " element: " + e.response[data.ErrorFieldName][0].Message, "k-icon w-b-error", [{ Title: "Ok", ClassName: "k-icon k-update" }]);
                        }
                    } else {
                        options.model[options.field] = e.response.Data.Id;
                        options.model.dirty = true;
                        $("#autoComplete" + options.values.Id).val(e.response.Data.Name);
                    }
                }
            },
            transport: {
                read: {
                    cache: true,
                    url: window.applicationpath + options.values.ReadUrl + options.values.TypeFullName
                },
                create: {
                    url: window.applicationpath + options.values.CreateUrl,
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8"
                },
                parameterMap: function (innerData, operation) {
                    if (operation == "read") {
                        return JSON.stringify(innerData);
                    }

                    var finalResult = {};
                    finalResult["indexName"] = options.values.TypeFullName;
                    finalResult["value"] = innerData.Name;

                    return JSON.stringify(finalResult);
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
    if (options.values.ShowAddButton) {
        appendAddIndexWindowHtml(container, options.values.Id, options.values.IndexName, "autoComplete");
    }
}

function getTextAreaEditor(container, options) {
    $('<textarea ' +
        //'id="textArea' + options.values.Id + '" ' +
        'data-bind="value: ' + options.field + '" ' +
        'name="' + options.field + '"' +
        'class="k-textbox">' +
      '</textarea>').appendTo(container);
   // window.localStorage.setItem('detailDataText', [options.field]);
}

function getFileEditor(container, options) {
    var oldFiles = getOldFileNames(options.model[options.field]);
    var acceptExtension = options.values.AcceptExtension;
    var acceptAttribute = "";
    if (acceptExtension != null && acceptExtension != "") {
        acceptAttribute = ' accept="' + acceptExtension + '" ';
    }
    $('<input name="files"  type="file" ' + acceptAttribute +
        ' id="' + options.field + '"' +
        'class="control-file">').appendTo(container)
        .kendoUpload({
            async: {
                saveUrl: window.applicationpath + 'Upload/SaveFilesInSession?typeName=' + options.values.TypeFullName + '&fieldName=' + options.field + '&acceptExtension=' + acceptExtension + '&fileSize=' + options.values.FileSize,
                removeUrl: window.applicationpath + 'Upload/removeFiles?typeName=' + options.values.TypeFullName + '&fieldName=' + options.field + '&fullFileName=' + options.model[options.field],
            }, localization: {
                select: "Browse"
            },
            multiple: false,
            files: oldFiles,
            success: function (data) {
                $.ajax({
                    url: window.applicationpath + 'Upload/GetFileName?typeName=' + window.gridModel.TypeFullName + '&fieldName=' + options.field,
                    type: "POST",
                    contentType: 'application/json',
                    success: function (fileName) {
                        options.model[options.field] = fileName;
                        options.model.dirty = true;
                    }
                });
            }, remove: function (data) {
                options.model[options.field] = "";
                options.model.dirty = true;
            }, error: function (data) {
                var messageText = data.XMLHttpRequest.response;
                var t = kendo.template($('#TooltipInvalidMessageTemplate').html())({ message: messageText });
                var affectedControl = $('#' + options.field);
                if (affectedControl != null)
                    affectedControl.after(t);
                else
                    ShowMessageBox(data.ResError, messageText, "k-icon w-b-error", [{ Title: data.ResOk, ClassName: "k-icon k-update" }]);

            }
        });
}

function getDatePickerEditor(container, options) {

    //var attributes = {};
    //var validationRole = getValidationForDate(options.field, options.values.DetailName);
    //if (validationRole.max != null) {
    //    attributes["max"] = validationRole.max;
    //}

    //if (validationRole.min != null) {
    //    attributes["min"] = validationRole.min;
    //}
    if (options.values.IsDateTime) {
        $('<input data-bind="value: ' + options.field + '" name="' + options.field + '" />').appendTo(container).kendoDateTimePicker({ format: "dd/MM/yyyy hh:mm tt" });
    } else if (options.values.IsTime) {
        $('<div class="input-group clockpicker" data-placement="bottom" data-align="left" data-autoclose="true"><input type="text" class="form-control" data-bind="value: ' + options.field + '" name="' + options.field + '" /> <span class="input-group-addon"><span class="k-icon k-i-clock"></span></span></div>').appendTo(container);
        //$('<input data-bind="value: ' + options.field + '" name="' + options.field + '" />').appendTo(container).kendoTimePicker({ format: "hh:mm tt" });
    } else {
        $('<input data-bind="value: ' + options.field + '" name="' + options.field + '" />').appendTo(container).kendoDatePicker({ format: "dd/MM/yyyy" });
    }
}
