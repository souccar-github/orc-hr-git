<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.ProjectManagment.ValueObjects.ProjectTeamRole>" %>
<%@ Import Namespace="UI.Areas.ProjectManagement.Helpers" %>
<script type="text/javascript">

    function GetRelevantTeams() {

        var comboBox = $("#IndirectProjectTeam_Id").data("tComboBox");
        var id = comboBox.value();

        $.ajax({
            url: '<%: Url.Action("GetProjectTeamRoles","ProjectTeamRole") %>/', type: "POST",
            data: { id: id },
            success: function (result) {
                if (result.Success) {
                    $('#relevantRoles').html(result.PartialViewHtml1);
                    $('#relevantTeams').html(result.PartialViewHtml2);
                    $('#relevantTeams').show();
                    $('#relevantRoles').show();

                }
            }
        });
    }

    function GetRelevantMembers() {

        var comboBox = $("#IndirectProjectTeamRole_Id").data("tComboBox");
        var id = comboBox.value();
        $.ajax({
            url: '<%: Url.Action("GetTeamMembers","ProjectTeamRole") %>/', type: "POST",
            data: { id: id },
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
            <%: Html.ValidationMessageFor(model => model.Role) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.ParentRole) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Weight) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Count) %>
        </td>
    </tr>
</table>
<fieldset class="ParentFieldset">
    <input type="hidden" id="SelectedNodeID" />
    <input type="hidden" id="SelectedNodeCode" />
    <table width="100%">
        <tr>
            <%: Html.HiddenFor(model => model.Id) %>
            <td style="width: 33.33%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Role) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.Role.Id)
                            .BindTo(DropDownListHelpers.ListOfProjectRoleName)
                            .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                     
                    %>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.ParentRole) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.ParentRole.Id)
                            .BindTo(DropDownListHelpers.ListOfProjectRoleName)
                            .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                     
                    %>
                </div>
            </td>
            <td style="width: 33.33%; vertical-align: top">
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.Weight) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().NumericTextBox().Name("Weight").MinValue(1).MaxValue(100) %>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.Count) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().IntegerTextBox().Name("Count").MinValue(1)%>
                </div>
            </td>
            <td style="width: 33.33%; vertical-align: top">
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.IndirectProjectTeam)%>
                </div>
                <div>
                    <%: Html.Telerik().ComboBoxFor(model => model.IndirectProjectTeam.Id)
                                      .BindTo(DropDownListHelpers.ListOfProjectTeams(int.Parse(TempData["projectId"].ToString())))
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                      .ClientEvents(e => e.OnClose("GetRelevantTeams"))
                                      
                    %>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.IndirectProjectTeamRole)%>
                </div>
                <div id="relevantRoles">
                    <% if (Model.IsTransient())
                       { %>
                    <%:Html.Telerik().ComboBoxFor(model => model.IndirectProjectTeamRole.Id)
                                   .BindTo(DropDownListHelpers.ListOfProjectTeamRoles(0,Model.Id))
                                   .HtmlAttributes(new { style = string.Format("width:{0}px", 300)})
                                   .ClientEvents(e => e.OnClose("GetRelevantMembers"))           
                    %>
                    <% }
                       else
                       { %>
                                           <%:Html.Telerik().ComboBoxFor(model => model.IndirectProjectTeamRole.Id)
                                           .BindTo(DropDownListHelpers.ListOfProjectTeamRoles(Model.IndirectProjectTeam ==null? 0:Model.IndirectProjectTeam.Id,Model.Id))
                                   .HtmlAttributes(new { style = string.Format("width:{0}px", 300)})
                                   .ClientEvents(e => e.OnClose("GetRelevantMembers"))           
                    %>
                    <% } %>
                </div>
                <div class="editor-label">
                    Indirect Team Member
                </div>
                <div id="relevantTeams">
                    <% if (Model.IsTransient())
                       { %>
                    <%:Html.Telerik().ComboBoxFor(model => model.IndirectTeamMember.Id)                             
                                  .BindTo(DropDownListHelpers.ListOfTeamMembers(0))
                                  .HtmlAttributes(new {style = string.Format("width:{0}px", 300)})
                                 
                    %>
                    <% }
                       else
                       { %>
                                           <%:Html.Telerik().ComboBoxFor(model => model.IndirectTeamMember.Id)
                                          .BindTo(DropDownListHelpers.ListOfTeamMembers(Model.IndirectProjectTeamRole == null ? 0 : Model.IndirectProjectTeamRole.Id))
                                  .HtmlAttributes(new {style = string.Format("width:{0}px", 300)})
                                 
                    %>
                    <% } %>
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
