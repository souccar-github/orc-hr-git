<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<UI.Areas.Services.DTO.ViewModels.DelegationViewModel>" %>
<% using (Ajax.BeginForm("Delegate", "Delegation", new AjaxOptions { OnComplete = "Json_OnComplete" }))
   {%>
<script type="text/javascript">
    function Json_OnComplete(context) {
        var json = context.get_response().get_object();
        if (json.Success) {
            alert(json.Message);
            var url = '<%:Url.Action("Index", "Delegation")%>';
            window.location.replace(url);
        } else {
            $("#divDelegationService").html(json.PartialViewHtml);
            document.getElementById("hiddenRoles").value = JSON.stringify(json.Roles);
            document.getElementById("hiddenAuthorities").value = JSON.stringify(json.Authorities);
            document.getElementById("hiddenAssignedEmployees").value = JSON.stringify(json.AssignedEmployees);
        }
    }
</script>
<table>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Delegation.Reason)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Delegation.From)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Delegation.To)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Delegation.Appraisable)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Delegation.Position)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Roles)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Authorities)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.AssignedEmployees)%>
        </td>
    </tr>
<%--    <tr>
        <td>
            <%: Html.ValidationMessage(Souccar.Core.DomainErrors.InternalError.ToString())%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessage(Souccar.Core.DomainErrors.SecurityError.ToString())%>
        </td>
    </tr>--%>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.EmployeeId)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessage("PositionDelegated")%>
        </td>
    </tr>
</table>
<fieldset class="ParentFieldset">
    <legend>
        <%: Resources.Areas.Services.Delegation.Messages.DelegationServiceTitle%></legend>
    <table width="100%">
        <tr>
            <td style="width: 20%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Delegation.Reason)%>
                </div>
                <div class="editor-field">
                    <%:Html.TextAreaFor(model => model.Delegation.Reason)%>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.Delegation.Comment)%>
                </div>
                <div class="editor-field">
                    <%:Html.TextAreaFor(model => model.Delegation.Comment)%>
                </div>
            </td>
            <td style="width: 20%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Delegation.From)%>
                </div>
                <div class="editor-field">
                    <%:Html.Telerik().DatePickerFor(model => model.Delegation.From).Value(Model.Delegation.From).Min(DateTime.MinValue)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Delegation.To)%>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DatePickerFor(model => model.Delegation.To).Value(Model.Delegation.To).Min(DateTime.MinValue)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.CheckBoxFor(model => model.Delegation.Appraisable)%>
                    <%: Html.LabelFor(model => model.Delegation.Appraisable)%>
                </div>
            </td>
            <td style="width: 20%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Delegation.Position)%>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().ComboBoxFor(m => m.Delegation.Position.Id)
                            .BindTo(UI.Areas.OrganizationChart.Helpers.DropDownListHelpers.ListOfPositions)
                            .Filterable(filtering => filtering.FilterMode(AutoCompleteFilterMode.Contains))
                                          .ClientEvents(events => events.OnChange("PositionChanged"))
                    %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.EmployeeId)%>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().ComboBoxFor(m => m.EmployeeId)
                            .BindTo(UI.Areas.Personnel.Helpers.PersonnelDropDownListHelpers.ListOfEmployees)
                            .Filterable(filtering => filtering.FilterMode(AutoCompleteFilterMode.Contains))
                    %>
                    <input type="button" value="+" onclick="BindEmployeeToSelectedRolesAndAuthorities()" />
                </div>
            </td>
            <td style="width: 20%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Resources.Areas.Services.Delegation.Messages.RolesTitle%>
                </div>
                <div id="divRoles">
                    <% Html.RenderPartial("Roles", Model.Roles); %>
                </div>
            </td>
            <td style="width: 20%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Resources.Areas.Services.Delegation.Messages.AuthoritiesTitle%>
                </div>
                <div id="divAuthorities">
                    <% Html.RenderPartial("Authorities", Model.Authorities); %>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div class="editor-label-required">
                    <%: Resources.Areas.Services.Delegation.Messages.AssignedEmployeesTitle%>
                </div>
                <div id="divAssignedEmployees">
                    <% Html.RenderPartial("AssignedEmployee", Model.AssignedEmployees); %>
                </div>
            </td>
        </tr>
        <tr>
            <td style="width: 20%; vertical-align: top">
                <input type="submit" value="<%: Resources.Areas.Services.Delegation.Buttons.Delegate %>" />
            </td>
        </tr>
    </table>
    <%:Html.Hidden("hiddenRoles")%>
    <%:Html.Hidden("hiddenAuthorities")%>
    <%:Html.Hidden("hiddenAssignedEmployees")%>
</fieldset>
<script type="text/javascript">

    function PositionChanged() {
        var comboBox = $("#Delegation_Position_Id").data("tComboBox");
        var positionComboBoxId = comboBox.value();

        $.ajax({
            url: '<%:Url.Action("LoadRoles", "Delegation", new { area = "Services"})%>/',
            type: "POST",
            data: { positionId: positionComboBoxId },
            success: function (result) {
                if (result.Success) {
                    document.getElementById("hiddenRoles").value = JSON.stringify(result.Data);
                    $('#divRoles').html(result.PartialViewHtml);
                    $('#divRoles').fadeIn('fast');
                }
            }
        });
        $.ajax({
            url: '<%:Url.Action("LoadAuthorities", "Delegation", new { area = "Services"})%>/',
            type: "POST",
            data: { positionId: positionComboBoxId },
            success: function (result) {
                if (result.Success) {
                    document.getElementById("hiddenAuthorities").value = JSON.stringify(result.Data);
                    $('#divAuthorities').html(result.PartialViewHtml);
                    $('#divAuthorities').fadeIn('fast');
                }
            }
        });
        document.getElementById("hiddenAssignedEmployees").value = "";
        jQuery('#divAssignedEmployees').html('');
    }

    function DeleteAssignedRole(employeeId, roleId, divId) {
        $("#" + divId).remove();
        EnableRole(roleId);
        DeleteFromAssignedEmployee(employeeId, 0, roleId);
    }

    function DeleteAssignedAuthority(employeeId, authorityId, divId) {
        $("#" + divId).remove();
        EnableAuthority(authorityId);
        DeleteFromAssignedEmployee(employeeId, authorityId, 0);
    }

    function DeleteFromAssignedEmployee(employeeId, authorityId, roleId) {

        var tempHiddenAssignedEmployees = JSON.parse(document.getElementById("hiddenAssignedEmployees").value);
        if (roleId > 0) {
            for (var i = 0; i < tempHiddenAssignedEmployees.length; i++) {
                if (tempHiddenAssignedEmployees[i].Employee.Id == employeeId) {
                    for (var z = 0; z < tempHiddenAssignedEmployees[i].Roles.length; z++) {
                        if (tempHiddenAssignedEmployees[i].Roles[z].Id == roleId) {
                            tempHiddenAssignedEmployees[i].Roles.splice(z, 1);
                            break;
                        }
                    }
                }
            }
        }
        else
            if (authorityId > 0) {
                for (var y = 0; y < tempHiddenAssignedEmployees.length; y++) {
                    if (tempHiddenAssignedEmployees[y].Employee.Id == employeeId) {
                        for (var a = 0; a < tempHiddenAssignedEmployees[y].Authorities.length; a++) {
                            if (tempHiddenAssignedEmployees[y].Authorities[a].Id == authorityId) {
                                tempHiddenAssignedEmployees[y].Authorities.splice(a, 1);
                                break;
                            }
                        }
                    }
                }
            }

        document.getElementById("hiddenAssignedEmployees").value = JSON.stringify(tempHiddenAssignedEmployees);
        DeleteEmptyEmployeeDiv(employeeId);
    }

    function EnableRole(roleId) {
        $("#chkRoles_" + roleId).removeAttr("disabled");
    }

    function EnableAuthority(authorityId) {
        $("#chkAuthorities_" + authorityId).removeAttr("disabled");
    }

    function BindEmployeeToSelectedRolesAndAuthorities() {
        var comboBox1 = $("#Delegation_Position_Id").data("tComboBox");
        var positionComboBoxId = comboBox1.value();
        if (positionComboBoxId == "") {
            alert('<%: Resources.Areas.Services.Delegation.Messages.SelectPositionMsg%>');
            return;
        }

        var comboBox = $("#EmployeeId").data("tComboBox");
        var employeeComboBoxId = comboBox.value();
        if (employeeComboBoxId == "") {
            alert('<%: Resources.Areas.Services.Delegation.Messages.SelectEmployeeMsg%>');
            return;
        }

        var roles = new Array();
        var authorities = new Array();

        var hiddenAuthoritiesData;
        if (document.getElementById("hiddenAuthorities").value != "") {
            hiddenAuthoritiesData = JSON.parse(document.getElementById("hiddenAuthorities").value);
            for (var i = 0; i < hiddenAuthoritiesData.length; i++) {
                if ($("#chkAuthorities_" + hiddenAuthoritiesData[i].Id).attr('checked') == true && $("#chkAuthorities_" + hiddenAuthoritiesData[i].Id).attr('disabled') == false) {
                    authorities.push(hiddenAuthoritiesData[i].Id);
                    $("#chkAuthorities_" + hiddenAuthoritiesData[i].Id).attr("disabled", "disabled");
                }
            }
        }

        var hiddenRolesData;
        if (document.getElementById("hiddenRoles").value != "") {
            hiddenRolesData = JSON.parse(document.getElementById("hiddenRoles").value);
            for (var z = 0; z < hiddenRolesData.length; z++) {
                if ($("#chkRoles_" + hiddenRolesData[z].Id).attr('checked') == true && $("#chkRoles_" + hiddenRolesData[z].Id).attr('disabled') == false) {
                    roles.push(hiddenRolesData[z].Id);
                    $("#chkRoles_" + hiddenRolesData[z].Id).attr("disabled", "disabled");
                }
            }
        }

        if (roles.length == 0 && authorities.length == 0) {
            alert('<%: Resources.Areas.Services.Delegation.Messages.SelectOneAuthorityOrRole%>');
            return;
        }

        var hiddenAssignedEmployeesData = document.getElementById("hiddenAssignedEmployees").value;

        $.ajax({
            url: '<%:Url.Action("BindEmployeeToSelectedRolesAndAuthorities", "Delegation", new { area = "Services"})%>/',
            type: "POST",
            traditional: true,
            data: { employeeId: employeeComboBoxId, positionId: positionComboBoxId, roleIds: roles, authorityIds: authorities,
                hiddenAssignedEmployeesData: hiddenAssignedEmployeesData
            },
            success: function (result) {
                if (result.Success) {
                    document.getElementById("hiddenAssignedEmployees").value = JSON.stringify(result.Data);
                    $('#divAssignedEmployees').html(result.PartialViewHtml);
                    $('#divAssignedEmployees').fadeIn('fast');
                }
            }
        });
    }

    function DeleteEmptyEmployeeDiv(employeeId) {
        var hiddenAssignedEmployeesData = JSON.parse(document.getElementById("hiddenAssignedEmployees").value);
        if (hiddenAssignedEmployeesData.length > 0) {
            for (var i = 0; i < hiddenAssignedEmployeesData.length; i++) {
                if (hiddenAssignedEmployeesData[i].Employee.Id == employeeId) {
                    if (hiddenAssignedEmployeesData[i].Roles.length > 0) {
                        return;
                    }
                    else {
                        if (hiddenAssignedEmployeesData[i].Authorities.length > 0) {
                            return;
                        }
                    }
                    hiddenAssignedEmployeesData.splice(i, 1);
                    $("#divEmployeeInfo_" + employeeId).html('');
                    document.getElementById("hiddenAssignedEmployees").value = JSON.stringify(hiddenAssignedEmployeesData);
                }
            }
        }
    }

</script>
<% } %>