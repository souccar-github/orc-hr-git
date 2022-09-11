function toggleRibbon(e) {
    var selected = $.map(e.sender.select(), function (item) {
        return $(item).text();
    });

    if (selected.length == 0 && !($(".switch_bar").is(':hidden'))) {
        $(".switch_bar").slideToggle("slow", "swing");
    } else if (selected.length > 0 && $(".switch_bar").is(':hidden')) {
        $(".switch_bar").slideToggle("slow", "swing");
    }
}

function updateRibbonState(e) {
 
    var selected = $.map(e.sender.select(), function (item) {
        return $(item).text();
    });

    if (selected.length == 0) {
        $(".group li a").addClass('disabled_ribbon_item');
    } else {
        $(".group li a").removeClass('disabled_ribbon_item');
        var previous=requestInformation.NavigationInfo.Previous;
        previous[previous.length - 1].RowId = getItemByUid(e.sender.select().attr("data-uid")).Id;
    }
}

function detailClick(typeName, name, title) {
 
    var grid = $("#grid").data("kendoGrid");
    var item = grid.dataItem(grid.select());
    window.requestInformation.NavigationInfo.Previous[window.requestInformation.NavigationInfo.Previous.length - 1].RowId = item.Id;
    window.requestInformation.NavigationInfo.Previous[window.requestInformation.NavigationInfo.Previous.length - 1].StepInformation = item.NameForDropdown;
    window.requestInformation.NavigationInfo.Previous[window.requestInformation.NavigationInfo.Previous.length - 1].Filter = grid.dataSource.filter();
    window.requestInformation.NavigationInfo.Previous[window.requestInformation.NavigationInfo.Previous.length - 1].Sort = grid.dataSource.sort();
    window.requestInformation.NavigationInfo.Previous[window.requestInformation.NavigationInfo.Previous.length - 1].PageNumber = grid.dataSource.page();
    window.requestInformation.NavigationInfo.Previous[window.requestInformation.NavigationInfo.Previous.length - 1].PageSize = grid.dataSource.pageSize();
    window.requestInformation.NavigationInfo.Previous[window.requestInformation.NavigationInfo.Previous.length - 1].SkipElement = grid.dataSource.take();
    window.requestInformation.NavigationInfo.Previous[window.requestInformation.NavigationInfo.Previous.length] = { Name: name, Title: title, TypeName: typeName, PageSize: grid.dataSource.pageSize()};
    changeLocation();
    if ($("#requestdivisionWindow").data("kendo-window")!=null)
    $("#requestdivisionWindow").data("kendo-window").close();
}

function getRibbonItemsByGroupId(ribbonItems, groupOrder) {
    var idx = 0;
    var specificRibbonItems = [];
    
    for (var i = 0; i < ribbonItems.length; i++) {
        if (ribbonItems[i].GroupOrder > groupOrder) {
            break;
        } else if (ribbonItems[i].GroupOrder == groupOrder) {
            specificRibbonItems[idx] = ribbonItems[i];
            idx++;
        }
    }
    
    return specificRibbonItems;
}

function getRibbonDetailsForGroup(details,groupName) {
    var result = [];
    for (var i = 0; i < details.length; i++) {
        if (details[i].GroupName == groupName)
            result.push(details[i]);
    }
    return result;
}


function getRibbonGroupsName(e) {
    var result = [];
    for (var i = 0; i < e.length; i++) {
        if (!IsInSet(result, e[i].GroupName))
            result.push(e[i].GroupName);
    }
    return result;
}
function IsInSet(array, item) {
    for (var i = 0; i < array.length; i++) {
        if (array[i] == item)
            return true;
    }
    return false;
}
function getRibbonHtml(requestInformation) {
    if (requestInformation.NavigationInfo.Next != null && requestInformation.NavigationInfo.Next.length > 0) {
        var resultHtml = "<div class='switch_bar'><ul id='Ribbon'>";
        var groupsName = getRibbonGroupsName(requestInformation.NavigationInfo.Next);
        for (var i = 0; i < groupsName.length; i++) {
            var groupRibbonItems = getRibbonDetailsForGroup(requestInformation.NavigationInfo.Next, groupsName[i]);
            if (groupRibbonItems.length == 0) {
                break;
            }
            var groupName = "";
            var groupLi = "<li>";
            var groupUl = "<ul class='group'>";
            for (var j = 0; j < groupRibbonItems.length; j++) {
                var ribbonItem = groupRibbonItems[j];

                groupUl += "<li class='" + ribbonItem.CSSClass + "'>" +
                                "<a href='#' onclick=\"detailClick('" + ribbonItem.TypeName + "','" + ribbonItem.Name + "','" + ribbonItem.Title + "')\">" +
                                    "<span class='stats_icon " + ribbonItem.ImageClass + "'></span>" +
                                    "<span class='label'>" + ribbonItem.Title + "</span>" +
                                "</a>" +
                            "</li>";
            }
            groupUl += "</ul>";
            groupLi += groupUl;

            groupLi += "<div class='group_name'>" + ribbonItem.GroupName + "</div>";

            groupLi += "</li>";
            resultHtml += groupLi;
        }

        resultHtml += "</ul></div>";
        return resultHtml;
    }
    return "";
}

function registerGridKeydownEvent() {
    if (true) {
        $("#" + window.gridModel.Name).keyup(function (e) {
            if (window.gridModel != null && !window.gridModel.TypeFullName.includes("Index")) {
                var grid = $("#" + window.gridModel.Name).data("kendoGrid");

                var dataSource = grid.dataSource;
                var filter_count = parseInt(getCookie("filter-Count"));
                var filter_values = getCookie("filter-values");
                var item = grid.dataItem(grid.select());
             $(".k-grid-cancel").click();
                if (grid.dataSource.filter() && filter_count != undefined && (filter_count < grid.dataSource._filter.filters.length
                    || filter_values != dataSource._filter.filters.map(c => c.value).join('-').toString()
                    )) {
                    document.cookie = "filter-Count=" + dataSource._filter.filters.length;
                    document.cookie = "filter-values=" + dataSource._filter.filters.map(c => c.value).join('-').toString();
                }
                else if (item != null && e.which == 13) {
                    showInformation(item.Id);
                    e.preventDefault();
                }
            }
        });
    }
    else if ($("#" + window.gridModel.Name) != null) {
        $("#" + window.gridModel.Name).keydown(function (e) {
            var item;
            var grid = $("#" + window.gridModel.Name).data("kendoGrid");

            if ((e.which == 69) && (e.ctrlKey) && (window.gridModel.AuthorizedToEdit)) { //Ctrl + e
                item = grid.dataItem(grid.select());

                if (item != null) {
                    update(item.Id);
                    e.preventDefault();
                }
            }
            else if ((((e.which == 68) && (e.ctrlKey)) || (e.which == 46)) && (window.gridModel.AuthorizedToDelete)) { // Ctrl + d, Or, Delete
                item = grid.dataItem(grid.select());

                if (item != null) {
                    destroy(item.Id);
                    e.preventDefault();
                }
            }
            else if ((e.which == 65) && (e.ctrlKey) && (window.gridModel.AuthorizedToAdd)) { // Ctrl + n
                grid.addRow();
                e.preventDefault();
            }
            else if ((e.which == 65) && (e.altKey) && (window.gridModel.AuthorizedToAdd)) { // Ctrl + n
                grid.addRow();
                e.preventDefault();
            }
            else if (((e.which == 82) && (e.ctrlKey)) || (e.which == 116)) { // Ctrl + r, Or, F5
                grid.refresh();
                e.preventDefault();
            }
        });
        $("#" + window.gridModel.Name).keyup(function (e) {
            if (window.gridModel != null && !window.gridModel.TypeFullName.includes("Index")) {
                var grid = $("#" + window.gridModel.Name).data("kendoGrid");

                var dataSource = grid.dataSource;
                var filter_count = parseInt(getCookie("filter-Count"));
                var filter_values = getCookie("filter-values");
                var item = grid.dataItem(grid.select());
                $(".k-grid-cancel").click();
                if (grid.dataSource.filter() && filter_count != undefined && (filter_count < grid.dataSource._filter.filters.length
                    || filter_values != dataSource._filter.filters.map(c => c.value).join('-').toString()
                    )) {
                    document.cookie = "filter-Count=" + dataSource._filter.filters.length;
                    document.cookie = "filter-values=" + dataSource._filter.filters.map(c => c.value).join('-').toString();
                }
                else if (item != null && e.which == 13) {
                    showInformation(item.Id);
                    e.preventDefault();
                }
            }

        });
    }
}
function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}
function registerGridMouseEvent(gridName) {
    if (true) {

    }
    else{
        var grid = $("#" + gridName);
    grid.click(function (e) {
        if (grid.data("kendoGrid").tbody.find('.dropdown.open').length > 0) {
            grid.data("kendoGrid").tbody.find('.actionlist_menu').remove();
    }
    });
    }
    
}
