//After Apply Master Detail Feature

function dateValidation(input) {
    var field = getFieldByName(gridModel.SchemaFields, input.attr('name'));

    var column = getColumnByName(gridModel.Views[0].Columns, input.attr('name'));
    if (field != null) {
        if (field.Type != 'date') {
            return true;
        }
        if (column.IsDateTime) {
            if (column.IsRequired && !checkDateTime(input.val())) {
                input.attr("data-dateValidation-msg", gridModel.InvalidDateMessage);
                return false;
            } else
                return true;
        } else if (column.IsTime) {
            if (column.IsRequired && !checkTime(input.val())) {
                input.attr("data-dateValidation-msg", gridModel.InvalidTimeMessage);
                return false;
            } else
                return true;
        } else if (column.IsRequired && !checkDate(input.val())) {
            input.attr("data-dateValidation-msg", gridModel.InvalidDateMessage);
            return false;
        } else {
            return true;
        }
    }
    return true;
}
//Check Before Create before the save and new && save and copy && edit and new && edit and save
function CheckBeforeCreateForSEAndCN() {
    var show = 1;
    var msg = "";
    var _title = "Title";
    Souccar.ajax(window.applicationpath + "Crud/BeforeCreate", { requestInformation: window.requestInformation, viewModelTypeFullName: window.gridModel.ViewModelTypeFullName }, function (data) {
        if (data.Data == false) {
            show = 0;
            msg = data.message;
        }
    });


    if (show == 0) {
        Souccar.ajax(window.applicationpath + "Crud/GetTitleOfBeforeCreatePopup", { requestInformation: window.requestInformation }, function (data) {
            if (data != null) {
                _title = data.Title;
            }
        });
        var commands = [{ Title: "Ok", ClassName: "k-icon k-update" }];
        ShowMessageBox(_title, msg, "k-icon w - b - info", commands);
    }
    return show;
}
function imageExists(image_url) {
    var http = new XMLHttpRequest();
    http.open('HEAD', image_url, false);
    http.send();
    return http.status != 404;
}
function checkPropertyNameInRequestInfo(PropertyName) {
    for (var i = 0; i < window.requestInformation.NavigationInfo.Previous.length; i++) {
        if (window.requestInformation.NavigationInfo.Previous[i].Name == PropertyName)
            return true;
    }
    return false;
}
function checkDateTime(str) {
    var parts = str.split(' ');
    if (parts.length != 3)
        return false;

    return checkDate(parts[0]) && checkTime(parts[1] + ' ' + parts[2]);
}
function checkTime(str) {
    var parts = str.split(':');
    if (parts.length < 2)
        return false;
    var hour = parseInt(parts[0]);
    var minute = parseInt(parts[1]);
    if (isNaN(hour) || isNaN(minute)) {
        return false;
    }
    if (hour < 0 || minute < 0 || hour > 23 || minute > 60)
        return false;
    return true;
}

function checkDate(str) {
    var parts = str.split('/');
    if (parts.length < 3)
        return false;
    else {
        var day = parseInt(parts[0]);
        var month = parseInt(parts[1]);
        var year = parseInt(parts[2]);
        if (isNaN(day) || isNaN(month) || isNaN(year)) {
            return false;
        }
        if (day < 1 || year < 1)
            return false;
        if (month > 12 || month < 1)
            return false;
        if ((month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12) && day > 31)
            return false;
        if ((month == 4 || month == 6 || month == 9 || month == 11) && day > 30)
            return false;
        if (month == 2) {
            if (((year % 4) == 0 && (year % 100) != 0) || ((year % 400) == 0 && (year % 100) == 0)) {
                if (day > 29)
                    return false;
            } else {
                if (day > 28)
                    return false;
            }
        }
        return true;
    }
}
function getTimeAsStandardToShowInClockTimePicker(dateStr) {
    if (dateStr.toString().length == 0 || (dateStr.toString().split(':') != undefined && dateStr.toString().split(':').length == 2)) return dateStr;
    var hours = new Date(dateStr).getHours().toString().length < 2 ?
        '0' + new Date(dateStr).getHours().toString() :
        new Date(dateStr).getHours().toString();
    var minutes = new Date(dateStr).getMinutes().toString().length < 2 ?
        '0' + new Date(dateStr).getMinutes().toString() :
        new Date(dateStr).getMinutes().toString();
    return hours + ':' + minutes
}
function complexRequired(input) {
    var field = {};
  
    // Retrieve
    var length = window.requestInformation.NavigationInfo.Previous.length;
    window.localStorage.setItem('DetailName', window.requestInformation.NavigationInfo.Previous[length - 1].Name);
     if (window.gridModel.DetailModels == null) {
         field = getFieldByName(gridModel.SchemaFields, input.attr('name'));
     }
     else if (window.localStorage.getItem('DetailName') == 'null' || window.localStorage.getItem('DetailName') == '')
         field = getFieldByName(gridModel.SchemaFields, input.attr('name'));
     else if (window.gridModel.DetailModels[window.localStorage.getItem('DetailName')]!=null)
         field = getFieldByName(window.gridModel.DetailModels[window.localStorage.getItem('DetailName')].SchemaFields, input.attr('name'));
     else
         field = getFieldByName(window.gridModel.SchemaFields, input.attr('name'));
    if (field != null) {
        if (field.Type != 'complex') {
            return true;
        }

        if (fieldIsRequired(field)) {
            if (input.val() == "") {
                input.attr("data-complexRequired-msg", field.ValidationRules["required"]["message"]);
                return false;
            } else {
                return true;
            }
        }
    }

    return true;
}
function getFieldByName(fields, fieldName) {
    for (var i = 0; i < fields.length; i++) {
        if (fields[i].Name == fieldName) {
            return fields[i];
        }
    }

    return null;
}
function getColumnByName(columns, name) {
    for (var i = 0; i < columns.length; i++) {
        if (columns[i].FieldName == name) {
            return columns[i];
        }
    }

    return null;
}
function fieldIsRequired(field) {
    for (var member in field.ValidationRules) {
        if (member == "required")
            return true;
    }

    return false;
}

function getViewById(views, viewId) {
    for (var i = 0; i < views.length; i++) {
        if (views[i].Id == viewId)
            return views[i];
    }

    return null;
}

function getoriginalFileName(fileName) {
    if (fileName == null || fileName == "")
        return "";
    var list = fileName.split('_');
    return list[list.length - 1];
}


function getValidationForDate(fieldName, detailName) {

////    //function getValidationForDate(fieldName) {
////    //    var field = getFieldByName(window.gridModel.SchemaFields, fieldName);

    
//    console.log(detailName);
//    var field = {};
//    //   if (detailName == null)
//    if (window.gridModel.DetailModels == null)

//        field = getFieldByName(window.gridModel.SchemaFields, fieldName);
//    else
//        field = getFieldByName(window.gridModel.DetailModels[detailName].SchemaFields, fieldName);


//    var values = {};
//    for (var member in field.ValidationRules) {
//        if (member == "required")
//            continue;

//        values[member] = new Date(field.ValidationRules[member]["value"]);
//    }
//    return values;
}

function getOldFileNames(fileName) {
    var result = [];
    if (fileName == null || fileName == "")
        return result;
    var fileNameParts = fileName.split('_');
    result.push({ name: fileNameParts[3], size: fileNameParts[1], extension: fileNameParts[1] });
    return result;
}


function appendAddIndexWindowHtml(container, id, indexTitle, inputType) {
    container.append('&nbsp;' + kendo.template($("#IndexAddButton").html())({ Id: id, InputTypeName: inputType }));

    var windowId = 'window' + id;
    var valueTextBoxId = "IndexValueTxt" + id;

    $('<div id="' + windowId + '">' +
        kendo.template($("#IndexAddPopupWindow").html())(
            {
                Id: id,
                Name:id,
                LabelText: gridModel.NameTitle,
                // LabelText: "ddd",
                ButtonText: gridModel.AddTitle,
                InputTypeName: inputType,
                ValueTextBoxId: valueTextBoxId
            }) +
      '</div>')
    .appendTo(container)
    .kendoWindow({
        width: "440px",
        height: "160px",
        modal: true,
        resizable: false,
        title: gridModel.AddTitle + " " + indexTitle,
        open: function () {
            
            $("#" + windowId).data("kendoWindow").center();
            
        },
        close: function () {
            $("#" + valueTextBoxId).val("");
        },
        activate: function () {

            $("#IndexValueTxt" + id).select();
            $("#IndexValueTxt" + id).focus();
        }        
    });
}

function appendInfoButtonHtml(container, id, typeFullName, windowTitle) {
    container.append('&nbsp;' + kendo.template($("#ReferenceButton").html())({ Id: id, TypeName: typeFullName, WindowTitle: windowTitle }));
}

function openAddIndexWindow(windowId, inputId, inputType, fieldName) {
    var kendoNamespace = "";

    if (inputType == "dropDownList") {
        kendoNamespace = "kendoDropDownList";
    } else if (inputType == "autoComplete") {
        kendoNamespace = "kendoAutoComplete";
    }

    var inputElement = $("#" + inputId).data(kendoNamespace);
    if (inputElement.ul[0].childElementCount == 0) {
        inputElement.dataSource.read();
    }

    $("#" + windowId).data("kendoWindow").open();
    if ($("#" + windowId).parent().find("div.k-window-titlebar.k-header").find(".maestro-popup-icon").length == 0)
        $("#" + windowId).parent().find("div.k-window-titlebar.k-header").prepend("<span class='maestro-popup-icon default-popup-view-icon'></span>");
      $("#IndexValueTxt" + fieldName).focus();
 
   
}

function openDetailsWindow()
{
    if (document.getElementById("ribbonContainer"))
         document.getElementById("ribbonContainer").remove();
    var title = "Details";
    var buttons = [
        { Name: "requestdivision_cancel", CssClass: "cancel", Title: "Cancel" }
    ];
    var containerId = "requestDivision-container";
    var bodyHTML = "<div class='requestDivision'><div id='ribbonContainer'></div></div>";
    var popupDiv = $('<div id="requestdivisionWindow" style="display: none"></div>');
    

    createAndOpenCustomWindow(popupDiv, bodyHTML, containerId, title, buttons, false);

    $("#ribbonContainer").html("").append(getRibbonHtml(window.requestInformation));
    
    $('.customPopupView-body').css('height', '184px');
    $('.k-window').css("height", 247 + "px");
    $('.k-window').css("width", 462 + "px")
    
    $("#requestdivision_cancel").off('click').on('click', function () {
        $("#requestdivisionWindow").data("kendo-window").close();
    });
}

function addIndexValue(inputId, value, inputType) {
    var dataSource = null;

    if (inputType == "dropDownList") {
        dataSource = $('#' + inputId).data("kendoDropDownList").dataSource;
    } else if (inputType == "autoComplete") {
        dataSource = $('#' + inputId).data("kendoAutoComplete").dataSource;
    }

    dataSource.add({ Name: value });
    dataSource.sync();
}

function getSortArray(sortFields) {
    var idx = 0;
    var result = [];

    for (var member in sortFields) {
        result[idx] = {
            field: member,
            dir: sortFields[member]
        };

        idx++;
    }

    return result;
}

function adjustFilterData(filterData, filteredFields) {
    var result = {};

    for (var member in filterData) {
        if (filterData[member] != null) {
            if (filterData[member] instanceof Array) {
                result["filters"] = [];
                for (var i = 0; i < filterData[member].length; i++) {
                    result["filters"][i] = adjustFilterData(filterData[member][i], filteredFields);
                }
            } else {
                result[member.toLowerCase()] = filterData[member];

                if (member = 'Field') {
                    filteredFields[filteredFields.length] = filterData[member];
                }
            }
        }
    }

    return result;
}
function adjustSortList(sortList, sortedFields) {
    var result = [];
    if (sortList == null)
        return result;
    for (var i = 0; i < sortList.length; i++) {
        result[i] = { field: sortList[i].Field, dir: sortList[i].Dir }
        sortedFields[i] = sortList[i].Field;
    }
    return result;
}

function getControlByFieldName(fieldName, gridModel) {
    var control = $('[name="' + fieldName + '"]');
    if (control.length == 1)
        return control;
    if (gridModel == null)
        return null;
    for (var i = 0; i < gridModel.SchemaFields.length; i++) {
        if (gridModel.SchemaFields[i].Type == 'file') {
            control = $("#" + fieldName);
        }
    }
    if (control.length == 1)
        return control;
    return null;
}
function getGlobalAggregate(view) {
    var result = [];
    var idx = 0;
    for (var i = 0; i < view.Columns.length; i++) {
        for (var j = 0; j < view.Columns[i].GlobalAggregates.length; j++) {
            result[idx++] = { field: view.Columns[i].FieldName, aggregate: view.Columns[i].GlobalAggregates[j] };
        }
    }
    return result;
}

function addGridNotification(gridModel, e) {
    var template = kendo.template($("#GridNotificationTemplate").html());
    e.container.append(template({}));
    $("#notification_grid_text").kendoEditor({
        tools: [
            "bold",
            "italic",
            "underline",
            "strikethrough",
            "justifyLeft",
            "justifyCenter",
            "justifyRight",
            "justifyFull",
            "foreColor"
        ]
    });

    var dataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: window.applicationpath + "Reference/ReadUsers",
                type: "POST",
                //async: false,
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: { requestInformation: window.requestInformation }
            }, parameterMap: function (oData, operation) {
                if (operation == "read") {
                    return JSON.stringify(oData);
                }
            }
        }
    });
    $("#notification_grid_receivers").kendoMultiSelect({
        autoBind: false,
        placeholder: gridModel.Select,
        dataSource: dataSource,
        dataTextField: "Name",
        dataValueField: "Id"
    });

    $(".notification-grid-btn").off("click").on("click", function () {
        var width = $('.notification-grid-container').width();
        setCurrentLanguageDirection();
        delay(function () {
            if (getCurrentLanguageDirection() == "true") {
                if (!$(".notification-grid-container").hasClass("show-container")) {
                    e.container.parent().animate({
                        left: "-=" + (width / 2),
                    }, 300, function () {
                        $(".notification-grid-container,.notification-grid-btn").animate({
                            right: "-=" + width,
                        }, 300);
                        console.log('open');
                    });
                } else {
                    var first = true;
                    $(".notification-grid-container,.notification-grid-btn")
                        .animate({
                            right: "+=" + width,
                        }, 300, function () {
                            if (first) {
                                first = false;
                                e.container.parent().animate({
                                    left: "+=" + (width / 2),
                                }, 300);
                                console.log('close');
                            }
                        }
                    );
                }
            } else {

                if (!$(".notification-grid-container").hasClass("show-container")) {
                    e.container.parent().animate({
                        left: "+=" + (width / 2),
                    }, 300, function () {
                        $(".notification-grid-container,.notification-grid-btn").animate({
                            left: "-=" + width,
                        }, 300);
                        console.log('open');
                    });
                } else {
                    var first = true;
                    $(".notification-grid-container,.notification-grid-btn")
                        .animate({
                            left: "+=" + width,
                        }, 300, function () {
                            if (first) {
                                first = false;
                                e.container.parent().animate({
                                    left: "-=" + (width / 2),
                                }, 300);
                                console.log('close');
                            }
                        }
                    );
                }
            }
            $(".notification-grid-container").toggleClass("show-container");
        }, 500);
    });
        
}

function showActionList(id,detailNo) {
    var _detailName="";
    var _gridModel = gridModel;
    var buttonMenu = $(".show-menu-button-" + id);
    $('.maestro-menu').remove();
    var actionList = $('<div class="maestro-menu"></div>');
    $('body').append(actionList);
    var grid = $("#" + gridModel.Name).data("kendoGrid");
    if (detailNo != null) {
        var detailmodels = this.gridModel.DetailModels;
        for (_Model in detailmodels) {
            _gridModel = detailmodels[_Model];
            if (parseInt(_gridModel.DetailNO) == parseInt(detailNo)) {
                _detailName = _gridModel.DetailName;
                grid = $(".grid-detail-" + _detailName).data("kendoGrid");
                buttonMenu = $(".show-menu-button-" + id + detailNo);
                break;
            }
                
        }
    }
        
    var dataSource = grid.dataSource;

    var dataItem = dataSource.get(id);

    if (gridModel.ActionListHandler != "") {
        getFunctionDelegate(gridModel.ActionListHandler)(
            gridModel.ActionList.Commands,
            dataItem
        );
    }
    var defualt_window = $(".k-widget.k-window.default-window-edit-two-column");
    if (defualt_window == null || defualt_window.offset() == null)
        defualt_window = $(".k-widget.k-window.default-window-edit-one-column");
    var actionListTemplate = kendo.template($("#NewActionListTemplate").html());
    if (detailNo != null) {
        var actionListTemplate = kendo.template($("#DetailActionListTemplate").html());
        actionList.html(actionListTemplate({ actionList: _gridModel.ActionList, id: dataItem.Id, detailNO: detailNo }));
        defualt_window.append($(".maestro-menu"));
    }
    else
        actionList.html(actionListTemplate({ actionList: _gridModel.ActionList, id: dataItem.Id }));
    var menuHeight = actionList.height();
    actionList.width(actionList.width() + 4);
    var btnOffset = buttonMenu.offset().top;
    if (detailNo != null) {
        var windowHeight = defualt_window.height();
        var windowOffsetTop = defualt_window.offset().top;
        var windowOffsetleft = defualt_window.offset().left;
        if ((menuHeight + btnOffset - windowOffsetTop) < windowHeight) {
            actionList.css("top", (btnOffset - windowOffsetTop) + "px");
        } else {
            var temp = (menuHeight + btnOffset - windowOffsetTop) - windowHeight;
            actionList.css("top", (btnOffset - windowOffsetTop - temp) + "px");
        }
        if ($('body').hasClass('local-rtl')) {
            actionList.css("left", (buttonMenu.offset().left - windowOffsetleft - actionList.width()) + "px");

        } else {
            actionList.css("left", (buttonMenu.offset().left - windowOffsetleft + 20) + "px");
        }
    }
    else {
        var windowHeight = $(window).height();
        if ((menuHeight + btnOffset) < windowHeight) {
            actionList.css("top", (btnOffset) + "px");
        } else {
            var temp = (menuHeight + btnOffset) - windowHeight;
            actionList.css("top", (btnOffset - temp) + "px");
        }
        if ($('body').hasClass('local-rtl')) {
            actionList.css("left", (buttonMenu.offset().left - actionList.width()) + "px");

        } else {
            actionList.css("left", (buttonMenu.offset().left + 20) + "px");
        }
    }
    $(document).off("click").on("click", function (e) {
        if (detailNo != null) {
            if (!e.target.classList.contains("show-menu-button-" + id + detailNo)) {
                $('.maestro-menu').remove();
            }
        }
        else {
            if (!e.target.classList.contains("show-menu-button-" + id)) {
                $('.maestro-menu').remove();
            }
        }

    });
}

function getFunctionDelegate(name) {
    var functionName = window[name];
    if (typeof functionName === 'function') {
        return functionName;
    }

    return null;
}

function initializeToolbarCommands(toolbarCommands, gridName) {
    //Initialize additional commands menu
    var result = "";
    
    for (var i = 0; i < toolbarCommands.length; i++) {
        var item = toolbarCommands[i];

        if (item.Additional == false) {
            continue;
        }

        result += '<li><a href="\#" onclick="' + item.Handler + '()">' + item.Text + '</a></li>';
    }

    $("#additional_toolbar_commands_list ul[id='additional_menu']").html(result);

    var grid = $("#" + gridName).data("kendoGrid");
    var toolbar = $("#" + gridName).find(".k-toolbar.k-grid-toolbar");
    var dataSource = grid.dataSource;
    toolbar.find(".k-grid-clear-filters").bind('click', function () {
        dataSource.filter({});
        $('.k-state-active').removeClass('k-state-active');
        dataSource.fetch();
    });

    toolbar.find(".k-grid-clear-sorting").bind('click', function () {
        dataSource.sort({});
        dataSource.fetch();
    });
}


function exportDateToCSV() {
    var grid = $("#" + gridModel.Name).data("kendoGrid");
    var dataSource = grid.dataSource;
    
    $.ajax({
        url: window.applicationpath + "Export/ExportCSV",
        type: "POST",
        contentType: 'application/json',
        data: JSON.stringify({
            
            pageSize: dataSource.pageSize(),
            skip: dataSource.skip(),
            sort: dataSource.sort(),
            filter: dataSource.filter(),
            requestInformation: window.requestInformation,
            
            gridModel: window.gridModel
        }),
        success: function (data) {
            window.location = window.applicationpath + "Export/GetFile?fileKey=" + data.FileKey + "&fileType=" + data.FileType + "&fileName=" + data.FileName;
        }
    });
}

function update(id) {


   
    $("#grid").data("kendoGrid").editRow(getRowById(id));
}

function destroy(id) {
     
    
    var commands = [{ Title: window.gridModel.ResOk, ClassName: "k-icon k-update", Handler: DestroyConfirm ,HandlerParameter:id},
                        { Title: window.gridModel.Cancel, Name: "no", ClassName: "k-icon k-cancel" }];
  
    ShowMessageBox(window.gridModel.Info, window.gridModel.Confirmation, "k-icon w-b-info", commands);
}

function DestroyConfirm(id) {
     
    $("#grid").data("kendoGrid").removeRow(getRowById(id));
    }


function getDataItemById(id) {
    var grid = $("#grid").data("kendoGrid");
    return grid.dataSource.get(id);
}

function getRowById(id) {
    var grid = $("#grid").data("kendoGrid");
    return grid.tbody.find("tr[data-uid='" + getDataItemById(id).uid + "']");
}

function getItemByUid(uid) {
    return $("#grid").data("kendoGrid").dataSource.getByUid(uid);
}

function getDetailsItemByUid(gridName, uid) {
    return gridName.data("kendoGrid").dataSource.getByUid(uid);
}

function initializeViewSelector(data) {
    $("#" + data.Name).find("#viewSelector input").kendoDropDownList({
        dataTextField: "Title",
        dataValueField: "Id",
        value: data.CurrentViewId,

        dataSource: data.Views,
        select: function (e) {
            data.CurrentViewId = this.dataItem(e.item.index()).Id;
            //window.gridModel = data;
           generateGrid(data);
        }
    });
}

function clearwindowdata(view) {

    $(".k-input").empty();
    $("input:text").val("");
    $("input:checkbox").prop('checked', false);
    for (var i = 0; i < view.Columns.length; i++) {
        var column = view.Columns[i];
        if (column.Editable && column.FieldName != "Id") {

            var x = document.getElementsByName(column.FieldName);
            if ((column.Type == "TextArea") || (column.Type == "AutoComplete")) {


                x[0].value = "";
                


            }
            if (column.Type == "DropDown") {
                 
                $("#"+x[0].id).data("kendoDropDownList").select(-1);
            }

        }
    }

}
function onSelectIndexesDropDownList(e) {
    var dataItem = this.dataItem(e.item.index());
    onclickIndex(dataItem.Text, dataItem.Value);
}

function ToStringForDate(date) {
    if (date == null)
        return "";
    if (!(date instanceof Date)) {
        var milli = date.toString().replace(/\/Date\((-?\d+)\)\//, '$1');
        return kendo.toString(new Date(parseInt(milli)), "dd/MM/yyyy");
    }

    return kendo.toString(date, "dd/MM/yyyy");
}
function toStringForDateTime(date) {
    
    if (date == null)
        return "";
    if (!(date instanceof Date)) {
        var milli = date.toString().replace(/\/Date\((-?\d+)\)\//, '$1');
        return kendo.toString(new Date(parseInt(milli)), "dd/MM/yyyy hh:mm tt");
    }

    return kendo.toString(date, "dd/MM/yyyy hh:mm tt");
}
function toStringForTime(date) {
    if (date == null)
        return "";
    if (!(date instanceof Date)) {
        var milli = date.toString().replace(/\/Date\((-?\d+)\)\//, '$1');
        return kendo.toString(milli, "hh:mm tt");
    }

    return kendo.toString(date, "hh:mm tt");
}
function getViewHtml(gridModel, item) {
    var view = getViewById(gridModel.Views, gridModel.CurrentViewId);
    var result = "<div class='k-edit-form-container'><div class='k-edit-label'></div><div class='k-edit-field'></div>";
    for (var j = 0; j < gridModel.ActionList.Commands.length; j++) {
        if (gridModel.ActionList.Commands[j].Name != "View")
            result += '<span class="k-link" onclick="' + gridModel.ActionList.Commands[j].HandlerName + '(' + item.Id + ',  gridModel)">' + gridModel.ActionList.Commands[j].Name + '</span>&nbsp';
    }
    for (var i = 0; i < view.Columns.length; i++) {
        if (view.Columns[i].FieldName != "Id") {
            result += "<div class='k-edit-label'>";
            result += "<label for='" + view.Columns[i].FieldName + "'>" + view.Columns[i].Title + "</label>";
            result += "</div>";

            result += "<div class='k-edit-field' >";
            if (view.Columns[i].Type != "DropDown") {
                result += item[view.Columns[i].FieldName];
            } else {
                result += item[view.Columns[i].FieldName].Name;
            }
            result += "</div>";
        }
    }

    //result += "<a class='k-button k-button-icontext k-grid-cancel' href='#'><span class='k-icon k-cancel'></span>Cancel</a>";
    result += "</div>";
    return result;
}

function openReferenceWindow(fieldName, typeFullName, windowTitle) {
    var referenceId = $("[name=" + fieldName + "]").val();
    if (referenceId == null || referenceId == 0)
        return;
    openViewWindow(typeFullName, windowTitle, referenceId);
}

function openViewWindow(typeFullName, windowTitle, entityId) {
    $.ajax({
        url: window.applicationpath + 'Reference/GetReferenceInfo',
        type: "POST",
        contentType: 'application/json',
        data: JSON.stringify({ requestInformation: window.requestInformation, typeName: typeFullName, id: entityId }),

        success: function (data) {
            $(".windowReferenceInformation").remove();
            var view = getViewById(data.gridModel.Views, data.gridModel.CurrentViewId);
            var item = data.item;

            var title = data.gridModel.ViewIntormation + " " + windowTitle;
            var buttons = [{ Name: "view_reference_cancel", CssClass: "cancel", Title: data.gridModel.Cancel }];

            var containerId = "default-view-reference-popup-div";
            var bodyHTML = "";
            var div = $('<div class="windowReferenceInformation"></div>');
            createAndOpenCustomWindow(div, bodyHTML, containerId, title, buttons, view.ShowTwoColumns);
            updateViewReferenceWithNewElement(div, data.gridModel, view, item);
            div.find("#view_reference_cancel").off('click').on('click', function () {
                $(".windowReferenceInformation").data("kendo-window").destroy();
            });
            div.find("#view_reference_cancel").addClass("secondary-action");
        }
    });

}

function updateViewReferenceWithNewElement(control, gridModel, view, item) {
    var template = kendo.template($("#DefaultViewTemplate").html());
    control.find(".customPopupView-body").html(template({ GridModel: gridModel, View: view, Item: item, ViewNavigation: false }));


    if (view.ViewHandler != "") {
        var handlerFanction = getFunctionDelegate(view.ViewHandler);
        if (handlerFanction != null) {
            var handlerData = { sender: $('#' + gridModel.Name).data('kendoGrid'), container: $(".windowReferenceInformation"), model: item };
            handlerFanction(handlerData);
        }
    }
}


function showDetailInformation(id, detailNO) {
    localStorage.setItem("CurrentDetalNO", detailNO);
    var _gridModel = this.gridModel;
    if (detailNO != null) {
        var detailmodels = _gridModel.DetailModels;
        for (_Model in detailmodels) {
            _gridModel = detailmodels[_Model];
            if (parseInt(_gridModel.DetailNO) == parseInt(detailNO)) {
                break;
            }

        }
    }
    var _grid = $(".grid-detail-" + _gridModel.DetailName);
    var view = getViewById(_gridModel.Views, _gridModel.CurrentViewId);
    var item = getDetailDataItemById(id, _grid.data("kendoGrid"));

    var title = _gridModel.ViewIntormation + " " + _gridModel.EntityTitle;
    var buttons = [{ Name: "view_cancel", CssClass: "cancel", Title: _gridModel.Cancel }];
    if (_gridModel.AuthorizedToEdit) {
        buttons.unshift({ Name: "view_edit", CssClass: "edit", Title: _gridModel.Edit });
    }
    var containerId = "default-view-popup-div";
    var bodyHTML = "";
    $(".windowGridViewInformation").remove();
    var div = $('<div class="windowGridViewInformation"></div>');
    createAndOpenCustomWindow(div, bodyHTML, containerId, title, buttons, view.ShowTwoColumns);
    updateViewWithNewElement(view, item);
    //if (div.parent().find(".default-popup-view-icon").length == 0) {
    //    div.parent().find("div.k-window-titlebar.k-header").prepend("<span class='maestro-popup-icon default-popup-view-icon'></span>");
    //}
    div.find("#view_cancel").off('click').on('click', function () {
        div.data("kendo-window").close();
    });

    div.find("#view_cancel").addClass("secondary-action");
    div.find("#view_edit").addClass("primary-action");
}

function destroyDetail(id, detailNO) {
    var _ParentGrid = this.gridModel;
    var commands = [{ Title: window.gridModel.ResOk, ClassName: "k-icon k-update", Handler: DestroyDetailConfirm, HandlerParameter: { id, detailNO, _ParentGrid } },
                        { Title: window.gridModel.Cancel, Name: "no", ClassName: "k-icon k-cancel" }];

    ShowMessageBox(window.gridModel.Info, window.gridModel.Confirmation, "k-icon w-b-info", commands);
}
function DestroyDetailConfirm(id) {
    var _ParentGrid = id._ParentGrid;
    if (id.detailNO != null) {
        var detailmodels = _ParentGrid.DetailModels;
        for (_Model in detailmodels) {
            var _gridModel = detailmodels[_Model];
            if (parseInt(_gridModel.DetailNO) == parseInt(id.detailNO)) {
                var _detailName = _gridModel.DetailName;
                var grid = $(".grid-detail-" + _detailName);
                var _data = grid.data("kendoGrid").dataSource._data;
                for (_model in _data) {
                    if (_data[_model].Id == id.id) {
                        grid.data("kendoGrid").dataSource.remove(_data[_model]);
                        grid.data("kendoGrid").dataSource.sync();
                        break;
                    }
                }
            }

        }
    }
}
function updateDetail(id, detailNO) {
    var _ParentGrid = this.gridModel;
    if (detailNO != null) {
        var detailmodels = _ParentGrid.DetailModels;
        for (_Model in detailmodels) {
            var _gridModel = detailmodels[_Model];
            if (parseInt(_gridModel.DetailNO) == parseInt(detailNO)) {
                var _detailName = _gridModel.DetailName;
                var grid = $(".grid-detail-" + _detailName).data("kendoGrid");
                grid.editRow(getDetailRowById(id, grid));
                break;
            }

        }
    }
}
function getDetailDataItemById(id, _grid) {
    return _grid.dataSource.get(id);
}
function getDetailRowById(id, _grid) {
    return _grid.tbody.find("tr[data-uid='" + getDetailDataItemById(id, _grid).uid + "']");
}
function showInformation(id) {
    var cannotEditForMasterDetail = requestInformation.NavigationInfo.Previous[requestInformation.NavigationInfo.Previous.length - 1].IsMasterContainsThisDetail;
    var view = getViewById(gridModel.Views, gridModel.CurrentViewId);
    var item = getDataItemById(id);

    var title = gridModel.ViewIntormation + " " + gridModel.EntityTitle;
    var buttons = [{ Name: "view_cancel", CssClass: "cancel", Title: gridModel.Cancel }];
    if (gridModel.AuthorizedToEdit && !cannotEditForMasterDetail) {
        buttons.unshift({ Name: "view_edit", CssClass: "edit", Title: gridModel.Edit });
    }
    var containerId = "default-view-popup-div";
    var bodyHTML = "";
    $(".windowGridViewInformation").remove();
    var div = $('<div class="windowGridViewInformation"></div>');
    createAndOpenCustomWindow(div, bodyHTML, containerId, title, buttons, view.ShowTwoColumns);
    updateViewWithNewElement(view, item);
    //if (div.parent().find(".default-popup-view-icon").length == 0) {
    //    div.parent().find("div.k-window-titlebar.k-header").prepend("<span class='maestro-popup-icon default-popup-view-icon'></span>");
    //}
    div.find("#view_cancel").off('click').on('click', function () {
        div.data("kendo-window").close();
    });

    div.find("#view_cancel").addClass("secondary-action");
    div.find("#view_edit").addClass("primary-action");

}

function updateViewWithNewElement(view, item) {
    var detailNO = localStorage.getItem("CurrentDetalNO");
    var _gridModel = ($("#grid"));
    var _grid = getGridObject();
    if (_grid.IsFromMasterDetail)
        _gridModel = $(".grid-detail-" + _grid.DetailName);


    var template = kendo.template($("#DefaultViewTemplate").html());
    $("#default-view-popup-div .customPopupView-body").html(template({ GridModel: _grid, View: view, Item: item, ViewNavigation: true }));


    if (view.ViewHandler != "") {
        var handlerFanction = getFunctionDelegate(view.ViewHandler);
        if (handlerFanction != null) {
            var handlerData = { sender: _gridModel.data('kendoGrid'), container: $(".windowGridViewInformation"), model: item };
            handlerFanction(handlerData);
        }
    }

    $("#view_edit").off('click').on('click', function () {
        $(".windowGridViewInformation").data("kendo-window").close();
        if (_grid.IsFromMasterDetail)
            updateDetail(item.Id, detailNO);
        else
            update(item.Id);
    });


    $(".next-element-button").off('click').on('click', function () {
        var newItem = getNextElement(item.Id);
        updateViewWithNewElement(view, newItem);
    });

    $(".previous-element-button").off('click').on('click', function () {
        var newItem = getPreviousElement(item.Id);
        updateViewWithNewElement(view, newItem);
    });

    $(".next-page-button").off('click').on('click', function () {
        view.ShowFirstElement = true;
        moveNextPage();

    });

    $(".previous-page-button").off('click').on('click', function () {
        view.ShowLastElement = true;
        movePreviousPage();
    });
}
function getGridObject() {
    var detailNO = localStorage.getItem("CurrentDetalNO");
    var _grid = window.gridModel;
    if (detailNO != null) {
        var detailmodels = _grid.DetailModels;
        for (_Model in detailmodels) {
            _grid = detailmodels[_Model];
            if (parseInt(_grid.DetailNO) == parseInt(detailNO)) {
                break;
            }
        }
    }
    return _grid;
}
function getGridModel() {
    var _gridModel = ($("#grid"));
    var _grid = getGridObject();
    if(_grid.IsFromMasterDetail)
        _gridModel = $(".grid-detail-" + _grid.DetailName);
    return _gridModel;
}
function moveNextPage() {
    //var grid = $("#" + window.gridModel.Name).data("kendoGrid");
    var _gridModel = getGridModel();
    var dataSource = _gridModel.data("kendoGrid").dataSource;
    dataSource.page(getCurrentPageNumber(dataSource) + 1);
}
function movePreviousPage() {
    var _gridModel = getGridModel();
    var dataSource = _gridModel.data("kendoGrid").dataSource;
    dataSource.page(getCurrentPageNumber(dataSource) - 1);
}
function getCurrentPageNumber(dataSource) {
    //var dataSource = _grid.dataSource;
    return dataSource.page();
}
function getTotalItems(_grid) {
    var dataSource = _grid.dataSource;
    return dataSource.total();
}
function getPageSize(_grid) {
    var dataSource = _grid.dataSource;
    return dataSource.pageSize();
}
function getTotalPageCount(_grid) {
    return Math.ceil(getTotalItems(_grid) / getPageSize(_grid));
}
function canShowNextElementButton(elementId, _grid) {
    var grid = $("#" + window.gridModel.Name).data("kendoGrid");
    if (_grid.IsFromMasterDetail)
        grid = $(".grid-detail-" + _grid.DetailName).data("kendoGrid");
    var dataSource = grid.dataSource;
    var length = dataSource._data.length;
    if (length == 0)
        return false;
    return dataSource._data[length - 1].Id != elementId;
}
function canShowPreviousElementButton(elementId, _grid) {
    var grid = $("#" + window.gridModel.Name).data("kendoGrid");
    if (_grid.IsFromMasterDetail)
        grid = $(".grid-detail-" + _grid.DetailName).data("kendoGrid");
    var dataSource = grid.dataSource;
    if (dataSource._data.length == 0)
        return false;
    return dataSource._data[0].Id != elementId;
}
function canShowNextPageButton(elementId, _grid) {
    var grid = $("#" + window.gridModel.Name).data("kendoGrid");
    if (_grid.IsFromMasterDetail)
        grid = $(".grid-detail-" + _grid.DetailName).data("kendoGrid");
    return getCurrentPageNumber(grid.dataSource) != getTotalPageCount(grid);
}
function canShowPreviousPageButton(elementId, _grid) {
    var grid = $("#" + window.gridModel.Name).data("kendoGrid");
    if (_grid.IsFromMasterDetail)
        grid = $(".grid-detail-" + _grid.DetailName).data("kendoGrid");
    return getCurrentPageNumber(grid.dataSource) != 1;
}
function getNextElement(currentElementId) {
    var _gridModel = getGridModel();
    var dataSource = _gridModel.data("kendoGrid").dataSource;
    var data = dataSource._data;
    for (var i = 0; i < data.length; i++) {
        if (data[i].Id == currentElementId) {
            return data[i + 1];
        }
    }
    return null;
}
function getPreviousElement(currentElementId) {
    var _gridModel = getGridModel();
    var dataSource = _gridModel.data("kendoGrid").dataSource;
    var data = dataSource._data;
    for (var i = 0; i < data.length; i++) {
        if (data[i].Id == currentElementId) {
            return data[i - 1];
        }
    }
    return null;
}
function getFirstElement() {
    var grid = $("#" + window.gridModel.Name).data("kendoGrid");
    return grid.dataSource.at(0);

}
function getLastElement() {
    var grid = $("#" + window.gridModel.Name).data("kendoGrid");
    var dataSource = grid.dataSource;
    var data = dataSource._data;
    if (data.length == 0)
        return null;
    return data[data.length - 1];
}

function getControlOrderClass(index, view) {
    if (!view.ShowTwoColumns)
        return "";
    if (index % 2 == 0)
        return "first-column";
    return "second-column";

}

function getStringValue(gridModel, column, item) {
    
    var field = getFieldByName(gridModel.SchemaFields, column.FieldName);

    if (column.Type == "DropDown") {
        if (item[column.FieldName] != null && item[column.FieldName].Name != null) {
            return item[column.FieldName].Name;
        } else {
            return "";
        }
    } else if (field.Type == "date") {
        var _column = getColumnByName(gridModel.Views[0].Columns, column.FieldName);
        if (item[_column.FieldName] != null) {
            if (_column.IsDateTime)
                return toStringForDateTime(item[_column.FieldName]);
            else if (_column.IsTime)
                return toStringForTime(item[_column.FieldName]);
            return ToStringForDate(item[_column.FieldName]);
        } else {
            return "";
        }
    } else if (field.Type == "boolean") {
        if (item[column.FieldName] != null) {
            var status = '';
            if (item[column.FieldName]) {
                status = ' checked="checked" ';
            }
            return '<input type="checkbox"' + status + ' disabled="disabled">';
        } else {
            return "";
        }
    } else if (column.Type == "AutoComplete") {
        if (item[column.FieldName] != null && item[column.FieldName].Name != null) {
            return item[column.FieldName].Name;
        } else {
            return "";
        }
    }
    else {
        if (item[column.FieldName] != null) {
            return item[column.FieldName];
        } else {
            return "";
        }
    }
}
function getStringValueForFile(gridModel, column, item) {
    var t1 = item[column.FieldName];
    if (t1 == null)
        return "";
    var t2 = t1.split('_');
    var downloadFileName = "";
    for (var i = 3; i < t2.length; i++) {
        downloadFileName += t2[i];
    }
    return '<a href="#" onclick=\'downloadFile("' + column.FieldName + '","' + gridModel.TypeFullName + '","' + item[column.FieldName] + '","' + downloadFileName + '")\'>' + downloadFileName + '</a>';


    //return '<a href="' + window.applicationpath+ 'Upload/DownloadFile?fieldName=' + column.FieldName +
    //                                                                '&typeName=' + gridModel.TypeFullName +
    //                                                                '&fileName=' + item[column.FieldName] +
    //                                                                '">' + item[column.FieldName] + '</a>';
}
function addRequiredStar(fieldName) {
    $("label[for='" + fieldName + "']").append('<span class="equired-star">*</span>');
}
function removeRequiredStar(fieldName) {
    $("label[for='" + fieldName + "']").find('.equired-star').remove();
}

function downloadFile(fieldName, typeName, fileName, downloadFileName) {
    window.location = window.applicationpath + 'Upload/DownloadFile?fieldName=' + fieldName + '&typeName=' + typeName + '&fileName=' + fileName + '&downloadFileName=' + downloadFileName;
}

function showValidationMesageOnControl(messageText, fieldName, gridModel) {
    var t = kendo.template($('#TooltipInvalidMessageTemplate').html())({ message: messageText });
    var affectedControl = getControlByFieldName(fieldName, gridModel);
    if (affectedControl != null)
        affectedControl.after(t);
    else {
        if (window.gridModel != null)
            ShowMessageBox(window.gridModel.ResError, messageText, "k-icon w-b-error", [{ Title: window.gridModel.ResOk, ClassName: "k-icon k-update" }]);
        if (gridModel != null)
            ShowMessageBox(gridModel.ResError, messageText, "k-icon w-b-error", [{ Title: gridModel.ResOk, ClassName: "k-icon k-update" }]);
    }

}