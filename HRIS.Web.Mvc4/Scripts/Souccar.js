
function Souccar() {

    //Start Of-------------------------------Global Function-----------------------------

    this.getItemByPropName = function (list, propName, value) {
        for (var i = 0; i < list.length; i++) {
            if (list[i][propName] == value)
                return list[i];
        }
        return null;
    };
    this.getMaxValueByPropName = function (list, propName) {
        if (list == null || list.length == 0) {
            return null;
        }
        var max = list[0][propName];
        for (var i = 0; i < list.length; i++) {
            if (list[i][propName] > max)
                max= list[i][propName];
        }
        return max;
    };
    this.getMinValueByPropName = function (list, propName) {
        if (list == null || list.length == 0) {
            return null;
        }
        var min = list[0][propName];
        for (var i = 0; i < list.length; i++) {
            if (list[i][propName] <min)
                min = list[i][propName];
        }
        return max;
    };
    this.clone = function (source, destination) {
        for (var i in source) {
            destination[i] = source[i];
        }
        return destination;
    };
    this.cloneBasedOnDestination = function (source, destination) {
        for (var i in destination) {
            destination[i] = source[i];
        }
        return destination;
    };
    this.cloneBasedOnSource = function (source, destination) {
        for (var i in source) {
            destination[i] = source[i];
        }
        return destination;
    };
    this.replaceNullWithEmptyString = function (object) {
        var result = {};
        for (var i in object) {
            result[i] = object[i];
            if (result[i] == null || result[i]=="null") {
                result[i] = "";
            } 
        }
        return result;
    };
    this.findInList = function (list, element) {
        for (var i = 0; i < list.length;i++) {
            list[i] = element;
            return true;
        }
        return false;
    };

    this.ajax = function (url,jsonParm,successFunction,errorFunction) {
        $.ajax({         
            url: url,
            type: "POST",
            data: JSON.stringify(jsonParm),
            async :false,
            contentType: 'application/json',
            success: successFunction,
            error: errorFunction
        });
        return false;
    };

    this.toStringForDate = function (date) {
        if (!(date instanceof Date)) {
            var milli = date.toString().replace(/\/Date\((-?\d+)\)\//, '$1');
            if (new Date(parseInt(milli)).getFullYear() == 1970)
                return kendo.toString(new Date(milli), "dd/MM/yyyy");
            return kendo.toString(new Date(parseInt(milli)), "dd/MM/yyyy");
        }
        return kendo.toString(date, "dd/MM/yyyy");
    };
    this.toStringForTime = function (date) {
        if (!(date instanceof Date)) {
            var milli = date.toString().replace(/\/Date\((-?\d+)\)\//, '$1');
            if (new Date(parseInt(milli)).getFullYear() == 1970)
                return kendo.toString(new Date(milli), "hh:mm tt");
            return kendo.toString(new Date(parseInt(milli)), "hh:mm tt");
        }
        return kendo.toString(date, "hh:mm tt");
    };
    this.toStringForDateTime = function (date) {
        if (!(date instanceof Date)) {
            var milli = date.toString().replace(/\/Date\((-?\d+)\)\//, '$1');
            if (new Date(parseInt(milli)).getFullYear() == 1970)
                return kendo.toString(new Date(milli), "dd/MM/yyyy hh:mm tt");
            return kendo.toString(new Date(parseInt(milli)), "dd/MM/yyyy hh:mm tt");
        }
        return kendo.toString(date, ":dd/MM/yyyy hh:mm tt");
    };
    this.showValidationMesageOnControl = function (messageText, fieldName) {
        //var t = kendo.template($('#TooltipInvalidMessageTemplate').html())({ message: messageText });
        //var affectedControl = getControlByFieldName(fieldName, gridModel);
        //if (affectedControl != null)
        //    affectedControl.after(t);
        //else {
        //    if (window.gridModel != null)
        //        ShowMessageBox(window.gridModel.ResError, messageText, "k-icon w-b-error", [{ Title: window.gridModel.ResOk, ClassName: "k-icon k-update" }]);
        //    if (gridModel != null)
        //        ShowMessageBox(gridModel.ResError, messageText, "k-icon w-b-error", [{ Title: gridModel.ResOk, ClassName: "k-icon k-update" }]);
        //}
    }
    //End Of-------------------------------Global Function-----------------------------

    //Start Of-------------------------------Message Box-----------------------------

    this.showInfoMessage = function (title, message, buttomTitle) {
        var commands = [{ Title: buttomTitle, ClassName: "k-icon k-update" }];
        ShowMessageBox(title, message, "k-icon w-b-info", commands);
    };
    
    this.showErrorMessage = function (title, message, buttomTitle) {
        var commands = [{ Title: buttomTitle, ClassName: "k-icon k-update" }];
        ShowMessageBox(title, message, "k-icon w-b-error", commands);
    };

    this.showWarningMessage = function (title, message, buttomTitle) {
        var commands = [{ Title: buttomTitle, ClassName: "k-icon k-update" }];
        ShowMessageBox(title, message, "k-icon w-b-warning", commands);
    };

    this.showQuestionMessage = function (title, message, buttomTitle) {
        var commands = [{ Title: buttomTitle, ClassName: "k-icon k-update" }];
        ShowMessageBox(title, message, "k-icon w-b-question", commands);
    };

    //End Of-------------------------------Message Box-----------------------------

}

window.Souccar = new Souccar();
