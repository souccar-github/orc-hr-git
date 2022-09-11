//----------------------------------PMS Props----------------------------------
var OrgLevelNamespcae = "HRIS.Domain.OrganizationChart.Indexes.OrganizationalLevel";
var GradeUrl = 'WorkflowTree/GetGradeList';
var JobTitleUrl = 'WorkflowTree/GetJobTitleList';
var JobDescriptionUrl = 'WorkflowTree/GetJobDescriptionList';
var PostitionUrl = 'WorkflowTree/GetPositionList';
//--------------------------------------------------------------------------------

//---------------------------Apply workflows Props----------------------------
var ApplyUrl = 'WorkflowTree/PhaseConfigurationApply';
var ApplyAllUrl = 'WorkflowTree/PhaseConfigurationApplyAll';
var WorkflowTreeUrl = 'WorkflowTree/PhaseConfigurationWorkflowTree';
var WorkflowDeleteNodeUrl = 'WorkflowTree/PhaseConfigurationDeleteWorkflow';
var ItemId = '';
//--------------------------------------------------------------------------------

//-----------------Workflow Manage-----------------

function createTemplateWorkflowWindow(itemId, applyUrl, applyAllUrl, workflowTreeUrl) {
    ApplyUrl = applyUrl;
    ApplyAllUrl = applyAllUrl;
    WorkflowTreeUrl = workflowTreeUrl;
    ItemId = itemId;
    createTemplateWorkflowWindow(itemId);
}//Customized URL

//-----------------Ammar alziebak-----------------
function createTemplateWorkflowWindow(itemId) {

    ItemId = itemId;

    var templateWorkflowData = [
        { Name: "OrganizationalLevel", Title: "Organizational Level", Type: "Index", TypeName: OrgLevelNamespcae, Creatable: false, IsRequired: false },
        { Name: "Grade", Title: "Grade", Type: "Dropdown", DataSource: "GradeDataSource" },
        { Name: "JobTitle", Title: "Job Title", Type: "Dropdown", DataSource: "JobTitleDataSource" },
        { Name: "JobDescription", Title: "Job Description", Type: "Dropdown", DataSource: "JobDescriptionDataSource" },
        { Name: "Position", Title: "Position", Type: "Dropdown", DataSource: "PositionDataSource" },
        { Name: "StepCount", Title: "Step Count", Type: "Number" }
    ];

    var templateWorkDatabind = kendo.observable();
    templateWorkDatabind.OrganizationalLevel = -1;
    templateWorkDatabind.Grade = 0;
    templateWorkDatabind.JobTitle = 0;
    templateWorkDatabind.JobDescription = 0;
    templateWorkDatabind.Position = 0;
    templateWorkDatabind.StepCount = 1;
    
    var title = "Objective Workflow";
    var buttons = [
            { Name: "WorkflowWindowCancel", CssClass: "cancel", Title: "Cancel" }
    ];
    var containerId = "container_div";
    var isTwoColumns = false;

    createAndOpenCustomWindow($("#EditWorkflowWindow"), "<div class='WorkflowWindow'><fieldset><legend>Workflow configuration</legend></fieldset></div>", containerId, title, buttons, isTwoColumns);
    createCustomEditForm($("#EditWorkflowWindow .WorkflowWindow"), { Fields: templateWorkflowData, Key: "add_Workflow" }, templateWorkDatabind);

    //Handle the cancel button.
    $("#WorkflowWindowCancel").off('click').on('click', function () {

        var dialog = $("#EditWorkflowWindow").data("kendoWindow");
        dialog.close();
        //$("#WorkflowTreeWindow .tree-window").data("kendoWindow").close();
    });

    $("[Name='OrganizationalLevel']").data("kendoDropDownList").bind("change", onChange);
    $("[Name='Grade']").data("kendoDropDownList").bind("change", onChange);
    $("[Name='JobTitle']").data("kendoDropDownList").bind("change", onChange);
    $("[Name='JobDescription']").data("kendoDropDownList").bind("change", onChange);

    var organizationalLevelApplyButton = $('<a class="k-button " id="organizationalLevelApplyButton"><span class="k-icon k-update"></span>Applay</a>');
    $("[data-container-for='OrganizationalLevel']").after(organizationalLevelApplyButton);

    var gradeApplyButton = $('<a class="k-button " id="gradeApplyButton"><span class="k-icon k-update"></span>Applay</a>');
    $("[data-container-for='Grade']").after(gradeApplyButton);

    var jobTitleApplyButton = $('<a class="k-button " id="jobTitleApplyButton"><span class="k-icon k-update"></span>Applay</a>');
    $("[data-container-for='JobTitle']").after(jobTitleApplyButton);

    var jobDescriptionApplyButton = $('<a class="k-button " id="jobDescriptionApplyButton"><span class="k-icon k-update"></span>Applay</a>');
    $("[data-container-for='JobDescription']").after(jobDescriptionApplyButton);

    var positionApplyButton = $('<a class="k-button " id="positionApplyButton"><span class="k-icon k-update"></span>Applay</a>');
    $("[data-container-for='Position']").after(positionApplyButton);

    var allApplyButton = $('<a class="k-button " id="allApplyButton"><span class="k-icon k-update"></span>Applay all</a>');
    $("[data-container-for='StepCount']").after(allApplyButton);

    //Open view window
    var viewWorkflowTreeButton = $('<a class="k-button " id="viewWorkflowTreeButton"><span class="k-icon k-update"></span>Workflow tree</a>');
    $("#allApplyButton").after(viewWorkflowTreeButton);

    $("#organizationalLevelApplyButton").off('click').on('click', function () {

        var applyViewModel = kendo.observable({
            Id: $("#OrganizationalLevel_add_Workflow").data("kendoDropDownList").value(),
            dropDownName: "OrganizationalLevel",
            stepCount: $("#StepCount_add_Workflow").val(),
            per: "OrganizationalLevel"
        });
        applyButton(applyViewModel);
    });

    $("#gradeApplyButton").off('click').on('click', function () {

        var applyViewModel = kendo.observable({
            Id: $("#Grade_add_Workflow").data("kendoDropDownList").value(),
            dropDownName: "Grade",
            stepCount: $("#StepCount_add_Workflow").val(),
            per: "Grade"
        });
        applyButton(applyViewModel);
    });

    $("#jobTitleApplyButton").off('click').on('click', function () {

        var applyViewModel = kendo.observable({
            Id: $("#JobTitle_add_Workflow").data("kendoDropDownList").value(),
            dropDownName: "JobTitle",
            stepCount: $("#StepCount_add_Workflow").val(),
            per: "JobTitle"
        });
        applyButton(applyViewModel);
    });

    $("#jobDescriptionApplyButton").off('click').on('click', function () {

        var applyViewModel = kendo.observable({
            Id: $("#JobDescription_add_Workflow").data("kendoDropDownList").value(),
            dropDownName: "JobDescription",
            stepCount: $("#StepCount_add_Workflow").val(),
            per: "JobDescription"
        });
        applyButton(applyViewModel);
    });

    $("#positionApplyButton").off('click').on('click', function () {

        var applyViewModel = kendo.observable({
            Id: $("#Position_add_Workflow").data("kendoDropDownList").value(),
            dropDownName: "Position",
            stepCount: $("#StepCount_add_Workflow").val(),
            per: "Position"
        });
        applyButton(applyViewModel);
    });

    $("#allApplyButton").off('click').on('click', function () {

        applyAllWorkflowSave();
    });

    $("#viewWorkflowTreeButton").off('click').on('click', function () {

        createTemplateWorkflowTreeWindow();
    });

}//Default URL (Objective)
//-----------------****************-----------------

//push an empty item at the first of a JSON array.
function setFirstEmptyItem(data) {

    var emptyItem = { Id: 0, Name: '' };
    if (data.length == 0)
        return data;

    var temp = data;
    data = new Array();
    data.push(emptyItem);
    for (var i = 0; i < temp.length; i++)
        data.push(temp[i]);
    return data;
}

function onChange() {

    var objectName = this.element.attr("name");
    var id = this.value();
    var url = '';
    var jsonParam = {};

    switch (objectName) {

        case 'OrganizationalLevel':
            url = GradeUrl;
            jsonParam = { organizationalLevelId: id };
            break;

        case 'Grade':
            url = JobTitleUrl;
            //url = JobDescriptionUrl;//Temperory
            jsonParam = { gradeId: id };
            break;

        case 'JobTitle':
            url = JobDescriptionUrl;
            jsonParam = { jobTitleId: id };
            break;

        case 'JobDescription':
            url = PostitionUrl;
            jsonParam = { jobDescriptionId: id };
            break;
    }

    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(jsonParam),
        contentType: 'application/json',
        success: function (data) {

            switch (objectName) {
                case 'OrganizationalLevel':
                    var gradeData = new kendo.data.DataSource(
                        {
                            data: setFirstEmptyItem(data.result)
                        }
                    );
                    $("#Grade_add_Workflow").data("kendoDropDownList").setDataSource(gradeData);
                    //if (data.result.length == 0) {//clear dropdowns items.
                        $("#Grade_add_Workflow").data("kendoDropDownList").text('');
                        $("#Grade_add_Workflow").data("kendoDropDownList").value('');

                        $("#JobTitle_add_Workflow").data("kendoDropDownList").setDataSource({});
                        $("#JobTitle_add_Workflow").data("kendoDropDownList").text('');
                        $("#JobTitle_add_Workflow").data("kendoDropDownList").value('');

                        $("#JobDescription_add_Workflow").data("kendoDropDownList").setDataSource({});
                        $("#JobDescription_add_Workflow").data("kendoDropDownList").text('');
                        $("#JobDescription_add_Workflow").data("kendoDropDownList").value('');

                        $("#Position_add_Workflow").data("kendoDropDownList").setDataSource({});
                        $("#Position_add_Workflow").data("kendoDropDownList").text('');
                        $("#Position_add_Workflow").data("kendoDropDownList").value('');
                    //}
                    //else//select an empty item.
                        $("#Grade_add_Workflow").data("kendoDropDownList").select(0);
                    break;

                case 'Grade':
                    var jobTitleData = new kendo.data.DataSource(
                        {
                            data: setFirstEmptyItem(data.result)
                        }
                    );
                    $("#JobTitle_add_Workflow").data("kendoDropDownList").setDataSource(jobTitleData);
                    //if (data.result.length == 0) {//clear dropdowns items.
                        $("#JobTitle_add_Workflow").data("kendoDropDownList").text('');
                        $("#JobTitle_add_Workflow").data("kendoDropDownList").value('');

                        $("#JobDescription_add_Workflow").data("kendoDropDownList").setDataSource({});
                        $("#JobDescription_add_Workflow").data("kendoDropDownList").text('');
                        $("#JobDescription_add_Workflow").data("kendoDropDownList").value('');

                        $("#Position_add_Workflow").data("kendoDropDownList").setDataSource({});
                        $("#Position_add_Workflow").data("kendoDropDownList").text('');
                        $("#Position_add_Workflow").data("kendoDropDownList").value('');
                    //}
                    //else//select an empty item.
                        $("#JobTitle_add_Workflow").data("kendoDropDownList").select(0);
                    break;

                case 'JobTitle':
                    var jobDescriptionData = new kendo.data.DataSource(
                        {
                            data: setFirstEmptyItem(data.result)
                        }
                    );
                    $("#JobDescription_add_Workflow").data("kendoDropDownList").setDataSource(jobDescriptionData);
                    //if (data.result.length == 0) {//clear dropdowns items.
                        $("#JobDescription_add_Workflow").data("kendoDropDownList").text('');
                        $("#JobDescription_add_Workflow").data("kendoDropDownList").value('');

                        $("#Position_add_Workflow").data("kendoDropDownList").setDataSource({});
                        $("#Position_add_Workflow").data("kendoDropDownList").text('');
                        $("#Position_add_Workflow").data("kendoDropDownList").value('');
                    //}
                    //else//select an empty item.
                        $("#JobDescription_add_Workflow").data("kendoDropDownList").select(0);
                    break;

                case 'JobDescription':
                    var positionData = new kendo.data.DataSource(
                        {
                            data: setFirstEmptyItem(data.result)
                        }
                    );
                    $("#Position_add_Workflow").data("kendoDropDownList").setDataSource(positionData);
                    //if (data.result.length == 0) {//clear dropdown items.
                        $("#Position_add_Workflow").data("kendoDropDownList").text('');
                        $("#Position_add_Workflow").data("kendoDropDownList").value('');
                    //}
                    //else//select an empty item.
                        $("#Position_add_Workflow").data("kendoDropDownList").select(0);
                    break;
            }
        }
    });
}

function applyButton(applyViewModel) {
    
    if (applyViewModel.Id) {
        
        $.ajax({
            url: ApplyUrl,
            type: "POST",
            data: JSON.stringify({ model: applyViewModel, Id: ItemId }),
            contentType: 'application/json',
            success: function(data) {
                if (data.Success)
                    alert("Applying succeed");
                else
                    alert("Applying failed");
            }
        });
    }
}

function applyAllWorkflowSave() {
    
        var viewModel = kendo.observable({
            stepCount: $("#StepCount_add_Workflow").val(),
            per: "OrganizationalLevel"
        });

        $.ajax({
            url: ApplyAllUrl,
            type: "POST",
            data: JSON.stringify({ model: viewModel, Id: ItemId }),
            contentType: 'application/json',
            success: function(data) {
                if (data.Success)
                    alert("Applying all succeed");
                else
                    alert("Applying all failed");
            }
        });
}

//------------------------------------------------------

//-----------------Workflow Tree---------------------

var treeData;
var selectedNodeItem;

function createTemplateWorkflowTreeWindow() {

    $("#WorkflowTreeWindow").html(' <div class="tree-window"> </div>');
    //--------Window properties--------
    var title = "Workflow tree";
    var buttons = [
             { Name: "Update", CssClass: "update", Title: 'Delete' },
             { Name: "WorkflowTreeWindowCancel", CssClass: "cancel", Title: 'Cancel' }
    ];
    //------------------------------------  
    
    
    createAndOpenCustomWindow($("#WorkflowTreeWindow .tree-window"), '<div class="obj-tree"></div>', "soso", title, buttons, false);
    
    initWorkflowTreeWindowContent(false);


    //On window close.
    var dialog = $(".tree-window").data("kendoWindow");
    dialog.bind("close", onDialogClose);
    function onDialogClose(e) {
        $(".tree-window").data("kendoWindow").destroy();
    }
    //On window cancel.
    $("#WorkflowTreeWindowCancel").off('click').on('click', function () {
        $(".tree-window").data("kendoWindow").destroy();
    });

    //On window delete node.
    $("#Update").off('click').on('click', onDeleteNode);
}

function initWorkflowTreeWindowContent(isRefresh) {

    var selectedId = ItemId;

    if (selectedId) {

        $.ajax({
            url: WorkflowTreeUrl,
            type: "POST",
            dataType: 'json',
            async: false,
            data: JSON.stringify({
                Id: selectedId
            }),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {

                var result = data.Data;
                
                if (isRefresh) {
                    
                    var tv = $("#soso .obj-tree").data("kendoTreeView");

                    tv.setDataSource(new kendo.data.HierarchicalDataSource({
                        data: result,
                        schema: {
                            model: {
                                children: "Items"
                            }
                        }
                    }));
                    
                }
                else {
                    
                    treeData = new kendo.data.HierarchicalDataSource({
                        data: result,
                        schema: {
                            model: {
                                children: "Items"
                            }
                        }
                    });
                    
                    $("#soso .obj-tree").kendoTreeView({
                        dataSource: treeData,
                        dataTextField: ["Name", "Name"],
                        dataValueField: ["Id", "Id"],
                        select: onNodeSelect
                    });
                    
                }
                
            }
        });
    }
}

function onDeleteNode() {

    if (selectedNodeItem) {

        $.ajax({
            url: WorkflowDeleteNodeUrl,
            type: "POST",
            dataType: 'json',
            async: false,
            data: JSON.stringify({
                nodeId: selectedNodeItem.Id,
                levelNumber: selectedNodeItem.LevelNumber,
                Id:ItemId
            }),
            contentType: 'application/json; charset=utf-8',

            success: function (data) {
                {
                    if (data.RowAffected == -1)//Prevent configuration operation.
                        alert("You can't delete node in the configuration related with phase");
                    else {
                        alert(data.RowAffected + ' Rows affected');
                        if (data.RowAffected > 0) {
                            //Refresh overall tree.
                            initWorkflowTreeWindowContent(true);
                            selectedNodeItem = null;
                        }
                    }
                }
            }
        });
    }
}

function onNodeSelect(e) {

    var tv = $('#soso .obj-tree').data('kendoTreeView');
    var item = tv.dataItem(e.node);
    selectedNodeItem = item;
}

//-------------------------------------------------------

