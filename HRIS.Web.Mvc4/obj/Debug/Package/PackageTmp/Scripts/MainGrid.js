//After Apply Master Detail Feature

function generateGrid(data) {
    document.cookie = "filter-Count=" + 0;
    document.cookie = "filter-values=" + "";
    localStorage.setItem("offsetTop", 0);
    localStorage.setItem("idRow", 0);
    var filteredFields = [];
    var view = getViewById(data.Views, data.CurrentViewId);
  
    var template = "";
    if (view.EditorTemplate != "") {
        template = kendo.template($('#' + view.EditorTemplate).html());
    }

    var height = $(window).height() - 250;
    if (requestInformation.NavigationInfo.Next.length == 0)
        height += 108;
    if (height < 380) {
        height = 380;
    }

    var actionListTemplate = kendo.template($("#ActionListTemplate").html());
    var gridObj = new Grid(data);
    $("#" + data.Name).html("").kendoGrid({
        dataSource: gridObj.GetMainDataSource(filteredFields),
        change: function (e) {
            localStorage.removeItem("CurrentDetalNO");
            updateRibbonState(e);
        },
        dataBound: function (e) {
            updateRibbonState(e);
            if (e.sender.dataSource.view().length == 0) {
                var colspan = e.sender.thead.find("th").length;
                //insert empty row with colspan equal to the table header th count
                var emptyRow = "<tr><td colspan='" + colspan + "'></td></tr>";
                e.sender.tbody.html(emptyRow);
            }

            if (view.ShowFirstElement == true) {

                view.ShowFirstElement = null;
                var newItem = getFirstElement();
                updateViewWithNewElement(view, newItem);

            } else if (view.ShowLastElement == true) {
                view.ShowLastElement = null;
                var newItem = getLastElement();
                updateViewWithNewElement(view, newItem);

            }

            var lastStep = window.requestInformation.NavigationInfo.Previous[window.requestInformation.NavigationInfo.Previous.length - 1];
            if (lastStep.FromBreadcrumb && lastStep.RowId != 0) {
                lastStep.FromBreadcrumb = false;

                e.sender.select(getRowById(lastStep.RowId));
            }

            e.sender.tbody.off('dblclick').on('dblclick', 'tr', function (element) {
                var dataItem = getItemByUid(element.currentTarget.getAttribute("data-uid"));
                //if (element.ctrlKey) {
                //    update(dataItem.Id);
                //} else {
                    showInformation(dataItem.Id);
                //}
            });
            if (view.DataBoundHandler != "") {
                var dataBoundHandlerFanction = getFunctionDelegate(view.DataBoundHandler);
                if (dataBoundHandlerFanction != null)
                    dataBoundHandlerFanction(e);
            }
            e.sender.tbody.off('click').on('click', 'tr', function (element) {
                
                var dataItem = getItemByUid(element.currentTarget.getAttribute("data-uid"));
                var offsetParent = $(this).parent().offset().top;
                var offseChild = $(this).offset().top;
                localStorage.setItem("offsetTop", -(offsetParent - offseChild));
                localStorage.setItem("idRow", dataItem.Id);
            });
            initializeViewSelector(data);
            //this line is written to solve kendo 'rtl' bug.
            $(".k-grid-content").scrollLeft($(".k-grid-content").scrollLeft() - 1);
            if ($("body").hasClass("local-rtl")) {
                $(".k-grid-content").scrollLeft($(".k-grid-content").scrollLeft() + 1000);
            }
            var grid = $("#grid").data("kendoGrid");
            if (getDataItemById(+localStorage.getItem("idRow")) != undefined) {
                
                var row = grid.table.find("[data-uid=" + getDataItemById(+localStorage.getItem("idRow")).uid + "]");
                var scrollContentOffset = this.element.find("tbody").offset().top;
                var selectContentOffset = localStorage.getItem("offsetTop");
                var distance = selectContentOffset - scrollContentOffset;
                var grid = $("#grid").data("kendoGrid");
                this.element.find(".k-grid-content").animate({
                    scrollTop: distance
                }, 400);
                grid.select(row);
            }
        },
        selectable: "row",
        filterable: gridObj.FilterableMessage,
        sortable: {
            mode: "multiple"
        },
        navigatable: true,
        pageable: gridObj.PageableMessage,
        toolbar: getToolbarCommands(data.ToolbarCommands),
        columns: gridObj.GetMainColumns(),
        height: height,
        filterMenuInit: function (e) {
            //
            //var column = getColumnByName(view.Columns, e.field);
            //if (column.IsTime) {
            //    var popup = $(e.container[0]).data("kendoPopup");
            //    popup.bind("close", function (e) {
            //        if (scroll) {
            //            e.preventDefault(); //prevent popup closing                    
            //        }
            //    });
            //}
        },
        editable: {
            confirmation: data.Confirmation,
            mode: view.EditorMode,
            template: template,
            window: {
                title: data.EditWindowTitle
            }
        },
        
        edit: function (e) {

            var show = 1;
            var msg = "";
            var _title = "Title";
            e.container.parent().hide();
            if (e.model.Id == 0) {
                Souccar.ajax(window.applicationpath + "Crud/BeforeCreate", { requestInformation: window.requestInformation, viewModelTypeFullName: gridObj.GridModel.ViewModelTypeFullName }, function (data) {
                    if (data.Data == false) {
                        show = 0;
                        msg = data.message;
                    }
                });
            }
            

            if (show == 0) {
           
                for (var j = 0; j < data.SchemaFields.length; j++) {
                    var field = data.SchemaFields[j];
                    e.container.find("label[for='" + field.Name + "']").parents(".k-edit-label").next().remove();
                    e.container.find("label[for='" + field.Name + "']").parents(".k-edit-label").remove();
                }
                Souccar.ajax(window.applicationpath + "Crud/GetTitleOfBeforeCreatePopup", { requestInformation: window.requestInformation }, function (data) {
                    if (data != null) {
                        _title=data.Title;
                    }
                });
                e.sender.clearSelection();
                e.container.find(".k-grid-update").hide();
                e.container.find(".k-grid-cancel").hide();
                e.container.parent().css({ left: ($(window).width() - e.container.parent().width()) / 2 });
                e.container.parent().css({ top: ($(window).height() - e.container.parent().height()) / 2 });
                e.container.parent().find(".k-window-title").html(_title);
                e.container.find('.k-edit-form-container').html("<div style='text-align:center;'>" + msg + "</div>");
                e.container.parent().show();
                            }
            if (show != 0) {

                e.container.parent().hide();

                ////Remove first column: Actions
                e.container.find('.k-edit-label:eq(0)').remove();
                e.container.find('.k-edit-field:eq(0)').remove();
                addGridNotification(data, e);
                if (view.ShowTwoColumns) {
                    e.container.parent().addClass('default-window-edit-two-column');
                } else {
                    e.container.parent().addClass('default-window-edit-one-column');
                }
                //Remove read only columns

                for (var j = 0; j < data.SchemaFields.length; j++) {
                    var field = data.SchemaFields[j];
                    if (!field.Editable) {
                        e.container.find("label[for='" + field.Name + "']").parents(".k-edit-label").next().remove();
                        e.container.find("label[for='" + field.Name + "']").parents(".k-edit-label").remove();
                    }
                }
                gridObj.AddFieldSets(e);
                gridObj.ShowTwoColumns(e);
                gridObj.AddRequiredStyle(e);
                gridObj.PreventCommaInNumericTextBox(e);
                //   addFieldsets(e);
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
                e.container.find(".k-grid-cancel").addClass("secondary-action");
                
              
               // $(".k-popup-edit-form.k-window-content.k-content").append($(".k-edit-buttons.k-state-default:not(.not-default-button)"));
                e.container.append(e.container.find(".k-edit-buttons.k-state-default:not(.not-default-button)"));
                var master=window.requestInformation.NavigationInfo.Previous[window.requestInformation.NavigationInfo.Previous.length - 1];
                if (e.model.Id == 0 && !master.IsMasterContainsDetailsWithSameInterface) {
                    $('<a class="k-button k-button-icontext k-grid-update k-grid-save-and-new" ><span class="k-icon k-update"></span>' + gridObj.GridModel.SaveAndNew + '</a>').insertAfter(".k-grid-update");
                    $('<a class="k-button k-button-icontext k-grid-update k-grid-save-and-Copy" ><span class="k-icon k-update"></span>' + gridObj.GridModel.SaveAndCopy + '</a>').insertBefore(".k-grid-cancel");
                    
                    $(".k-button.k-button-icontext.k-grid-update.k-grid-save-and-Copy").off('click').on('click', function () {
                      
                        $(".k-button.k-button-icontext.k-grid-update.k-grid-save-and-Copy").data('clicked', true);
                       
                        window.localStorage.setItem ("notsave", true);
                    });


                    $(".k-button.k-button-icontext.k-grid-update.k-grid-save-and-new").off('click').on('click', function () {

                        $(".k-button.k-button-icontext.k-grid-update.k-grid-save-and-new").data('clicked', true);

                        window.localStorage.setItem("notsave", true);
                    });
                    e.container.parent().find(".k-window-title").html(data.Add + " " + data.EntityTitle);
                } else if (e.model.Id != 0 && !master.IsMasterContainsDetailsWithSameInterface) {

                    $('<a class="k-button k-button-icontext k-grid-update k-grid-edit-new" ><span class="k-icon k-update"></span>' + gridObj.GridModel.SaveAndNew + '</a>').insertAfter(".k-grid-update");
                    $('<a class="k-button k-button-icontext k-grid-update k-grid-copy" ><span class="k-icon k-update"></span>' + gridObj.GridModel.SaveAndCopy + '</a>').insertBefore(".k-grid-cancel");

                    $(".k-button.k-button-icontext.k-grid-update.k-grid-edit-new").off('click').on('click', function () {

                        $(".k-button.k-button-icontext.k-grid-update.k-grid-edit-new").data('clicked', true);

                        window.localStorage.setItem("notsave", true);
                    });
                    $(".k-button.k-button-icontext.k-grid-update.k-grid-copy").off('click').on('click', function () {

                        $(".k-button.k-button-icontext.k-grid-update.k-grid-copy").data('clicked', true);

                        window.localStorage.setItem("notsave", true);
                    });

                    e.container.parent().find(".k-window-title").html(data.Edit + " " + data.EntityTitle);
                }

                e.container.parent().css({ left: ($(window).width() - e.container.parent().width()) / 2 });
                e.container.parent().css({ top: ($(window).height() - e.container.parent().height()) / 2 });

                e.container.parent().show();
                if (e.model.Id == 0)
                    gridObj.PreventDefaultValueInDateTimeFields(e);
                else
                    gridObj.PreventStandardDateValueInTimeFields(e);
                updateEditFormAsTabs(e);


            }
        },
        dataBinding: function (e) {
             
            //this is the key to keeping the popup open
           // e.preventDefault();
        },
        cancel: function (e) {
            e.sender.clearSelection();
            $("#" + data.Name).data("kendoGrid").dataSource.read();
        }
    });



    var grid = $("#" + data.Name).data("kendoGrid");
    for (var i = 0; i < filteredFields.length; i++) {
        grid.thead.find("th[data-field='" + filteredFields[i] + "'] a:eq(0)").addClass('k-state-active');
    }

    initializeToolbarCommands(data.ToolbarCommands, data.Name);
    /*if (requestInformation.NavigationInfo.Next.length != 0) {
        $("#viewSelector").before("<span class='grid-resizer-control resizer-min-ico'></span>");
        var gridContent = $("#" + data.Name).find(".k-grid-content");
        $(".grid-resizer-control").off("click").on("click", function () {
            $("#Ribbon").slideToggle(200);
            if ($(this).hasClass("resizer-min-ico")) {
                gridContent.animate({ height: gridContent.height() + 108 }, 300);
            } else {
                gridContent.animate({ height: gridContent.height() - 108 }, 100);
            }

            $(this).toggleClass("resizer-min-ico");
            $(this).toggleClass("resizer-max-ico");
        });

    }*/
}

function getToolbarCommands(toolbar) {
    var idx = 0;
    var result = [];

    for (var i = 0; i < toolbar.length; i++) {
        var item = toolbar[i];
        //if (item.Name == "create") {
        //    alert("hello");
        //}
        if (item.Additional == true) {
            continue;
        }

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
    return result;
}

function updateEditFormAsTabs(e) {
    e.model.dirty = true;
    var detailmodels = this.gridModel.DetailModels;
    for (_Model in detailmodels) {
        _gridModel = detailmodels[_Model];
        _gridModel.DetailNO = 0;
    }
    localStorage.removeItem("DetalNO");
    localStorage.setItem("DetalNO", 0);
    var template = kendo.template($("#GridTabsTemplate").html());
    e.container.find('.k-edit-form-container').append(template({}));
    e.container.find('.grid-tab-container-general').append(e.container.find('.controls-container'));
    e.container.find(".grid-taps-container").kendoTabStrip({
        animation: {
            close: {
                duration: 100,
                effects: "fadeOut"
            },
            open: {
                duration: 100,
                effects: "fadeIn"
            }

        }
    });
    $('.grid-tab-title').off('click').on('click', function () {

         

        var tab = $(this);
        var detailName = tab.attr('data-detail-name');
        if (e.model.Id == 0) {
            if (tab.hasClass('detail-loaded-add'))
                return;
            tab.addClass('detail-loaded-add');
        }
        else {

            if (tab.hasClass('detail-loaded-edit'))
                return;
            tab.addClass('detail-loaded-edit');
        }
        var detail = Souccar.getItemByPropName(requestInformation.NavigationInfo.Next, 'Name', detailName);
        window.requestInformation.NavigationInfo.Previous[window.requestInformation.NavigationInfo.Previous.length] = { Name: detail.Name, Title: detail.Title, TypeName: detail.TypeName, IsFromMasterDetail: true };
        Souccar.ajax(window.applicationpath + "Crud", window.requestInformation, function (gridModel) {
            var view = getViewById(gridModel.gridModel.Views, gridModel.gridModel.CurrentViewId);
            gridModel.gridModel.DetailName = detailName;
            var _detailNo = parseInt(localStorage.getItem("DetalNO")) + 1;
            gridModel.gridModel.DetailNO = _detailNo;
            localStorage.setItem("DetalNO", _detailNo);
            //gridModel.gridModel.TypeFullNameViewModel = ViewModelTypeFullName
            if (e.model.Id == 0)
                gridModel.gridModel.LocalData = [];
            else
                Souccar.ajax(window.applicationpath + view.ReadUrl, { requestInformation: window.requestInformation, viewModelTypeFullName: gridModel.gridModel.ViewModelTypeFullName,pageSize:500 }, function (data) {
                     
                    gridModel.gridModel.LocalData = data;
                });
            window.requestInformation.NavigationInfo.Previous.pop();
            var gridDetail = e.container.find('.grid-detail-' + detailName)
             
            grnerateGridDetail(e, gridDetail, gridModel.gridModel);


        }, function () { });
    });
}
