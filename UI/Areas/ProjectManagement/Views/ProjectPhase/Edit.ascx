<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.ProjectManagment.ValueObjects.ProjectPhase>" %>
<%@ Import Namespace="UI.Areas.ProjectManagement.Helpers" %>
<script type="text/javascript">

    function GetRelevantTeams() {

        var comboBox = $("#Team_Id").data("tComboBox");
        var id = comboBox.value();

        $.ajax({
            url: '<%: Url.Action("GetProjectTeamRoles","ProjectPhase") %>/', type: "POST",
            data: { projectTeamId: id },
            success: function (result) {
                if (result.Success) {
                    $('#relevantRoles').html(result.PartialViewHtml1);
                    $('#relevantTeams').html(result.PartialViewHtml2);
                    $('#relevantRoles').show();
                    $('#relevantTeams').show();
                }
            }
        });
    }

    function GetRelevantMembers() {

        var comboBox = $("#TeamRole_Id").data("tComboBox");
        var id = comboBox.value();

        $.ajax({
            url: '<%: Url.Action("GetTeamMembers","ProjectPhase") %>/', type: "POST",
            data: { projectTeamRoleId: id },
            success: function (result) {
                if (result.Success) {
                    $('#relevantTeams').html(result.PartialViewHtml);
                    $('#relevantTeams').show();
                }
            }
        });
    }    
            
</script>
<table>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Name) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Team)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.TeamRole) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.TeamMember) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Description) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.StartDate) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.EndDate) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Status) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.CompletionPercentage) %>
        </td>
    </tr>
</table>
<fieldset class="ParentFieldset">
    <table width="100%">
        <tr>
            <td style="width: 33.3%; vertical-align: top">
                <%: Html.HiddenFor(model => model.Id) %>
                <div class="editor-label-required">
                    <%:Html.LabelFor(model => model.Name)%>
                </div>
                <div class="editor-field">
                    <%:Html.EditorFor(model => model.Name)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Team)%>
                </div>
                <div>
                    <%: Html.Telerik().ComboBoxFor(model => model.Team.Id)
                                      .BindTo(DropDownListHelpers.ListOfProjectTeams(int.Parse(TempData["projectId"].ToString())))
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                      .ClientEvents(e => e.OnClose("GetRelevantTeams"))
                                      
                    %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.TeamRole)%>
                </div>
                <div id="relevantRoles">
                    <% if (Model.IsTransient())
                       { %>
                    <%:Html.Telerik().ComboBoxFor(model => model.TeamRole.Id)
                                   .BindTo(DropDownListHelpers.ListOfProjectTeamRoles(0))
                                   .HtmlAttributes(new { style = string.Format("width:{0}px", 300)})
                                   .ClientEvents(e => e.OnClose("GetRelevantMembers"))           
                    %>
                    <% }
                       else
                       { %>
                    <%:Html.Telerik().ComboBoxFor(model => model.TeamRole.Id)
                                  .BindTo(DropDownListHelpers.ListOfProjectTeamRoles(Model.Team == null ? 0 : Model.Team.Id))
                                   .HtmlAttributes(new { style = string.Format("width:{0}px", 300)})
                                   .ClientEvents(e => e.OnClose("GetRelevantMembers"))           
                    %>
                    <% } %>
                </div>
                <div class="editor-label-required">
                    <%:Html.LabelFor(model => model.TeamMember)%>
                </div>
                <div id="relevantTeams">
                    <% if (Model.IsTransient())
                       { %>
                    <%:Html.Telerik().ComboBoxFor(model => model.TeamMember.Id)                             
                                  .BindTo(DropDownListHelpers.ListOfTeamMembers(0))
                                  .HtmlAttributes(new {style = string.Format("width:{0}px", 300)})
                                 
                    %>
                    <% }
                       else
                       { %>
                    <%:Html.Telerik().ComboBoxFor(model => model.TeamMember.Id)
                                  .BindTo(DropDownListHelpers.ListOfTeamMembers(Model.TeamRole == null ? 0 : Model.TeamRole.Id))
                                  .HtmlAttributes(new {style = string.Format("width:{0}px", 300)})
                                 
                    %>
                    <% } %>
                </div>
            </td>
            <td style="width: 33.3%; vertical-align: top">
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.Description) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextAreaFor(model => model.Description)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.StartDate) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DatePickerFor(model => model.StartDate).Value(Model.StartDate.Date).Min(DateTime.MinValue)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.EndDate) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DatePickerFor(model => model.EndDate).Value(Model.EndDate.Date).Min(DateTime.MinValue)%>
                </div>
            </td>
            <td style="width: 33.3%; vertical-align: top">
                <div class="editor-label-required">
                    <%:Html.LabelFor(model => model.Status)%>
                </div>
                <div class="editor-field">
                    <%:Html.Telerik().DropDownListFor(model => model.Status.Id)
                    .BindTo(DropDownListHelpers.ListOfPhaseStatus)
                    .HtmlAttributes(new {style = string.Format("width:{0}px", 200)})
                    %>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.CompletionPercentage) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().PercentTextBox().Name("CompletionPercentage").MinValue(0).MaxValue(100)%>
                </div>
            </td>
        </tr>
    </table>
</fieldset>
<table width="100%">
    <tr>
        <td style="width: 10%; vertical-align: top">
            <input type="submit" value="<%: Resources.Shared.Buttons.Function.Save %>" onclick="DisableSaveButton()" />
        </td>
    </tr>
</table>
