//**********************Action plan Props**********************
var InitEmployeesUrl = 'ActionPlan/Employees';
var InitEvaluationPhasesUrl = 'ActionPlan/EvaluationPeriods';
var InitApprovalPhasesUrl = 'ActionPlan/ApprovalPeriods';
var InitEvaluationObjectivesUrl = 'ActionPlan/EvaluationWorkflowObjectives';
var InitApprovalObjectivesUrl = 'ActionPlan/ApprovalWorkflowObjectives';
var InitTrackingWindowDataUrl = 'ActionPlan/ViewActionPlanTracking';
var InitEvaluationWindowContentUrl = 'ActionPlan/ObjectiveActionsInfo';

var OnEvaluationSaveUrl = 'ActionPlan/EvaluateObjective';
var OnActionPlanTrackingUpdateUrl = 'ActionPlan/TrackActionPlan';
var OnActionPlanClosingUrl = 'ActionPlan/CloseActionPlan';
var GetObjectiveWorkflowIdUrl = 'ActionPlan/GetObjectiveWorkflowId';
var ApproveWorkflowUrl = 'ActionPlan/ApproveWorkflow';
//******************************************************************

///**********************Reused Methods**********************

function DrawGrid(data, fields, cols, isObjectiveView) {
    
    $("#Grid").kendoGrid({
        dataSource: {
            data: data,
            schema: {
                model: {
                    fields: fields
                }
            },
            pageSize: 20
        },
        selectable: "row",
        height: 430,
        scrollable: true,
        sortable: true,
        change: isObjectiveView ? onGridChange : null,
        filterable: true,
        pageable: {
            pageSizes: [10, 20, 50, 100, 500]
        },
        columns: cols
    });
}

//**************************************************************

//**********************Action plan Evauation**********************//

function initEvaluationControls() {
    
    initEvaluationPeriods(true);
}

//----------------------Employees & Evaluation Period functions----------------

var selectedPhaseId;

function initEvaluationPeriods(isEvaluation) {

    $.ajax({
        url: isEvaluation ? InitEvaluationPhasesUrl : InitApprovalPhasesUrl,
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            if ($(data.Data).size() != 0) {
                $("#PhasesSelect").kendoDropDownList({
                    dataTextField: "Name",
                    dataValueField: "Id",
                    dataSource: data.Data,
                    index: 0,
                    select: isEvaluation ? onEvaluationPeriodSelect : onApprovalPeriodSelect
                });
                //get selected evaluation period value.
                var phaseDropdown = $("#PhasesSelect").data("kendoDropDownList");
                selectedPhaseId = phaseDropdown.value();

                if (!isEvaluation && selectedPhaseId) //Approval Phase call
                    initObjectivesGrid(isEvaluation,0);   
                else
                    initEmployees();
                
            } else {
                $("#PhasesTr").hide();
                $("#EmployeesTr").hide();
                $("#Phases").hide();
            }
        }
    });
}

function onEvaluationPeriodSelect(e) {

    var dataItem = this.dataItem(e.item.index());
    selectedPhaseId = dataItem.Id;
    if (selectedPhaseId)
        initObjectivesGrid(true,0);
}

function initEmployees() {
    
        $.ajax({
            url: InitEmployeesUrl,
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            success: function (data) {

                if ($(data.Data).size() != 0) {
                    
                    $("#EmployeesSelect").kendoDropDownList({
                        dataTextField: "Name",
                        dataValueField: "Id",
                        dataSource: setFirstEmptyItem(data.Data),
                        index: 0,
                        select: onEmployeeSelect
                    });
                    var employeesDropdownlist = $("#EmployeesSelect").data("kendoDropDownList");
                    var empId = employeesDropdownlist.value();
                    initObjectivesGrid(true, empId);
                } else {
                    $("#EmployeesTr").hide(); //No employees exist.
                    if (selectedPhaseId)
                        initObjectivesGrid(true,0);//When the current is not a manager empId should to be 0.
                }
            }
        });
}

//push an empty item at the first of a JSON array.
//Need refactoring and marge with Workflow.js.
//function setFirstNewEmptyItem(data) {

//    var emptyItem = { Id: 0, Name: '' };
//    if (data.length == 0)
//        return data;

//    var temp = data;
//    data = new Array();
//    data.push(emptyItem);
//    for (var i = 0; i < temp.length; i++)
//        data.push(temp[i]);
//    return data;
//}

function onEmployeeSelect(e) {
    
    var dataItem = this.dataItem(e.item.index());
    var empId = dataItem.Id;
    //if (empId) {
        if (selectedPhaseId)
            initObjectivesGrid(true,empId);
    //}
}

//----------------------Grid functions----------------------
var selectedObjectiveId;
var selectedObjectiveData;

function initObjectivesGrid(isEvaluation,empId) {

    var fields = {
        Id: { type: "number" },
        Code: { type: "string" },
        Name: { type: "string" },
        Type: { type: "string" },
        Weight: { type: "number" },
        CreatedDate: { type: "date" }
    };

    var columns = [
        {
            command: [ {
                name: isEvaluation?"Evaluate":"Approve",
                click: function (e) {
                    selectedObjectiveId = 0;
                    //Get current row selected.
                    e.preventDefault();
                    var grid = $("#Grid").data("kendoGrid");
                    var tr = $(e.target).closest("tr");
                    var rowIndex = $("tr", grid.tbody).index(tr);
                    var row = grid.dataItem("tr:eq(" + (rowIndex + 1/*because the row counting is begin from 1 in this method.*/) + ")");
                    if (!row)
                        row = grid.dataItem("tr:eq(" + (rowIndex/*because the row counting is begin from 1 in this method.*/) + ")");
                    /////////////////
                    selectedObjectiveId = row.Id;
                    if (isEvaluation) {
                        
                        initEvaluationWindowContent();
                        if ($(selectedObjectiveData.Actions.ActionsData).size() > 0) //to address no action plans exist case.
                            openEvaluationWindow(true);
                        else
                            alert("You can't evaluate objective with no action plans...!");
                    }
                    else {
                        openEvaluationWindow(false);
                    }
                }
            }]
        },
        //{ field: "Id", title: "Id", width: "130px" },
        { field: "Code", title: "Code", width: "130px" },
        { field: "Name", title: "Name", width: "130px" },
        { field: "Type", title: "Type", width: "130px" },
        { field: "Weight", title: "Weight", width: "130px" },
        {  field: "CreatedDate", title: "Created Date",
            width: "130px", format: "{0:dd/MM/yyyy}"
        }
    ];

    var evaluationParams = {
        empId: empId,
        evaluationPeriodId: selectedPhaseId
    };

    var approvalParams = {
        phasePeriodId: selectedPhaseId
    };

    $.ajax({
        url: isEvaluation?InitEvaluationObjectivesUrl:InitApprovalObjectivesUrl,
        type: "POST",
        dataType: 'json',
        data: isEvaluation ? JSON.stringify(evaluationParams) : JSON.stringify(approvalParams),
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
                DrawGrid(data.Data, fields, columns, true);
        }
    });

}

function onGridChange(arg) {

    //var grid = this;
    //grid.select().each(function() {
    //    var dataItem = grid.dataItem($(this));
    //    alert(selectedObjectiveId);
    //    selectedObjectiveId = dataItem.Id;
    //});
}

//////////////////
function afterReadWorkflow(e) {

    content = "";
    var template = kendo.template($("#ActionsTemplate").html());
    content = template({ data: selectedObjectiveData });
    $("#workflow_data").html(content);
    applyEvaluationLayoutStyles(selectedObjectiveData);
}

function afterAcceptWorkflow(e) {
    onEvaluationSave();
}

function approveWorkflow(status) {
    
    $.ajax({
        url: ApproveWorkflowUrl,
        type: "POST",
        dataType: 'json',
        data: JSON.stringify({
            workflowId: selectedWorkflowId,
            status: status
        }),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            alert(data.Data);
        }
    });


}

function afterReject(e) {
    approveWorkflow("Reject");
}

function afterPending(e) {
    approveWorkflow("Pending");
}
//////////////////

function openEvaluationWindow(isEvaluation) {

    //get selected evaluation period value.
    var phaseDropdown = $("#PhasesSelect").data("kendoDropDownList");
    selectedPhaseId = phaseDropdown.value();
    //Get the selected objective workflow (selectedWorkflow).
    getObjectiveWorkflowId(selectedObjectiveId, selectedPhaseId, isEvaluation);
    
    if (isEvaluation) {//Evaluation phase
        if (selectedWorkflowId) {

            createWorkflow($("#EvaluationWorkflow"), selectedWorkflowId, afterReadWorkflow,afterAcceptWorkflow,afterReject,afterPending);
        }
    }
    else {//Approval phase
        if (selectedWorkflowId) {
            
            createWorkflow($("#ApprovalWorkflow"), selectedWorkflowId);
        }
    }


    ////--------Window properties--------
    //var title = "Evaluate actions";
    //var buttons = [
    //         { Name: "Evaluate", CssClass: "update", Title: 'Save' },
    //         { Name: "Cancel", CssClass: "cancel", Title: 'Cancel' }
    //                      ];
    //var containerId = "ActionPlansWindow"; 
    ////------------------------------------
    //createAndOpenCustomWindow($("#ActionPlansWindow"), content, containerId, title, buttons, false);
    ////On window cancel.
    //$("#Cancel").off('click').on('click', function () {
    //    $("#ActionPlansWindow").data("kendo-window").close();
    //});
    ////On window evaluation save.
    //$("#Evaluate").off('click').on('click', onEvaluationSave);var phaseDropdown

}

//------------------------------------------------------------//

//----------------------Evaluation window functions----------------------

var content = "";

function initEvaluationWindowContent() {

    //var phaseDropdown = $("#PhasesSelect").data("kendoDropDownList");
    //var evaluationPeriodId = phaseDropdown.value();

    $.ajax({
        url: InitEvaluationWindowContentUrl,
        type: "POST",
        dataType: 'json',
        data: JSON.stringify({
            objectiveId: selectedObjectiveId,
            evaluationPeriodId: selectedPhaseId
        }),
        contentType: 'application/json; charset=utf-8',
        async:false,
        success: function (data) {
            selectedObjectiveData = data.Data;
        }
    });
    
}

function applyEvaluationLayoutStyles(data) {

    //Evaluation percentage layout styles.
    for (var i = 0; i < $(data.Actions.ActionsData).size(); i++) {

        $('#EvaluationPercentage' + data.Actions.ActionsData[i].Id).kendoNumericTextBox({
            format: "# \\%",
            value: 0,
            min: 0,
            max: 100,
            step: 0.1,
            enable: true,
            change: calculateYourAverageEvaluation,
            spin: calculateYourAverageEvaluation
        });
    }
    $("#EvaluateButton").click(onEvaluationSave);
    //$("#EvaluateButton").kendoButton();
    
        //{
    //    click: onEvaluationSave
    //});

}

//------------------------------------------------------------//

//--------------------Evaluation object----------------------//

function Evaluation(actionId, percentage, notes) {
    this.ActionId = actionId;
    this.Percentage = percentage;
    this.Notes = notes;
}

//------------------------------------------------------------//

function calculateYourAverageEvaluation() {

    var sum = 0;
    var size = $(selectedObjectiveData.Actions.ActionsData).size();
    for (var i = 0; i < size; i++) {

        sum = parseInt($('#EvaluationPercentage' + selectedObjectiveData.Actions.ActionsData[i].Id).val()) + sum;
    }
    $('#YourEvaluationAveragePercentage').text((sum / size).toString() + '%');
}

function onEvaluationSave() {
     
    var evaluationData = [];
    
        //var evaluationData =new Array();
        for (var i = 0; i < $(selectedObjectiveData.Actions.ActionsData).size(); i++) {

            var actionId = selectedObjectiveData.Actions.ActionsData[i].Id;
            var percentage = $("#EvaluationPercentage" + actionId).val();
            var notes = $("#EvaluationNotes" + actionId).val();
            var evaluation = new Evaluation(actionId, percentage, notes);
            evaluationData.push(evaluation);
        }

        $.ajax({
            url: OnEvaluationSaveUrl,
            type: "POST",
            dataType: 'json',
            data: JSON.stringify({
                objectiveId: selectedObjectiveId,
                evaluationPeriodId: selectedPhaseId,
                evaluationData: evaluationData,
                workflowId: selectedWorkflowId,
                description: $("#NewStepDescription").val()
            }),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                //refresh objective evaluation content.
                initEvaluationWindowContent();
                //recreate the workflow area.
                createWorkflow($("#EvaluationWorkflow"), selectedWorkflowId, afterReadWorkflow, afterAcceptWorkflow);
                
                
            }
        });
}

//******************************************************************//

//**********************Action plan tracking & closing**********************//

var ItemId;
var ActualStartDate;
var ActualClosingDate;

function createTrackingWindow(itemId) {

    ItemId = itemId;

    var templateTrackingData = [
       { Name: "ActualStartingDate", Title: "Actual Starting Date", Type: "Date",  IsRequired: true },
       { Name: "ActualClosingDate", Title: "Actual Closing Date", Type: "Date", IsRequired: false }
    ];
    
    initTrackingWindowData();
    
    var templateTrackingDatabind = kendo.observable();
    templateTrackingDatabind.ActualStartingDate = new Date(ActualStartDate);
    templateTrackingDatabind.ActualClosingDate = new Date(ActualClosingDate);
    
    var title = "Action plan tracking";
    var buttons = [
            { Name: "cancel", CssClass: "cancel", Title: "Cancel" },
            { Name: "update", CssClass: "update", Title: "Update" }
    ];
    var containerId = "container_div";
    var isTwoColumns = false;

    createAndOpenCustomWindow($("#ActionPlanTrackingWindow"), "<div class='ActionPlanTrackingWindow'><fieldset><legend>Action Plan Tracking</legend></fieldset></div>", containerId, title, buttons, isTwoColumns);
    createCustomEditForm($("#ActionPlanTrackingWindow .ActionPlanTrackingWindow"), { Fields: templateTrackingData, Key: "track_ActionPlan" }, templateTrackingDatabind);

    $("#cancel").off('click').on('click', function () {
        $("#ActionPlanTrackingWindow").data("kendo-window").close();
    });
    //On window evaluation save.
    $("#update").off('click').on('click', onActionPlanTrackingUpdate);
}

function initTrackingWindowData() {
    
    $.ajax({
        url: InitTrackingWindowDataUrl,
        type: "POST",
        dataType: 'json',
        data: JSON.stringify({
            Id: ItemId
        }),
        async:false,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            ActualStartDate = data.Data.actualStartDate;
            ActualClosingDate = data.Data.actualClosingDate;
        }
    });
}

function  onActionPlanTrackingUpdate() {
    
    var startDate = $("#ActualStartingDate_track_ActionPlan").data("kendoDatePicker").value();
    var closingDate = $("#ActualClosingDate_track_ActionPlan").data("kendoDatePicker").value();
    if (kendo.parseDate(startDate) > kendo.parseDate(closingDate))
        alert("Actual Start date should to be less or equal than Actual Closing date");
    else
        $.ajax({
            url: OnActionPlanTrackingUpdateUrl,
            type: "POST",
            dataType: 'json',
            async: false,
            data: JSON.stringify({
                Id: ItemId,
                actualStartDate: startDate,
                actualClosingDate: closingDate
            }),
            contentType: 'application/json; charset=utf-8',
            success: function(data) {
                alert(data.success);
                //refresh the grid view.
                $("#grid").data("kendoGrid").dataSource.fetch();
            }
        });
}

function closeActionPlan(itemId) {
    
    ItemId = itemId;
    var result = confirm("are you sure to close this action plan?");
    if (result) {
        $.ajax({
            url: OnActionPlanClosingUrl,
            type: "POST",
            async: false,
            dataType: 'json',
            data: JSON.stringify({
                Id: ItemId
            }),
            contentType: 'application/json; charset=utf-8',
            success: function(data) {
                alert(data.success);
                //refresh the grid view.
                $("#grid").data("kendoGrid").dataSource.fetch();
            }
        });
    }
}

//**************************************************************************//

//**********************Action plan approval**********************//

function initApprovalControls() {
    
    initEvaluationPeriods(false);

}

function onApprovalPeriodSelect(e) {

    var dataItem = this.dataItem(e.item.index());
    selectedPhaseId = dataItem.Id;
    if (selectedPhaseId)
        initObjectivesGrid(false,0);
}

//***************************************************************//

//**********************Workflow Operations**********************//

var selectedWorkflowId;

function getObjectiveWorkflowId(objectiveId, phaseId, isEvaluation) {
    
    $.ajax({
        url: GetObjectiveWorkflowIdUrl,
        type: "POST",
        dataType: 'json',
        data: JSON.stringify({
            objectiveId: selectedObjectiveId,
            phaseId:phaseId,
            isEvaluation:isEvaluation
        }),
        contentType: 'application/json; charset=utf-8',
        async: false,
        success: function (data) {
            selectedWorkflowId = data.Data;
        }
    });

}

//***************************************************************//