<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.ProjectManagment.Entities.Project>" %>
<%@ Import Namespace="UI.Areas.ProjectManagement.Helpers" %>

<% using (Ajax.BeginForm("JsonEdit", "Project", new AjaxOptions { OnComplete = "JsonEdit_OnComplete" }))
   {%>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%: string.Format(Resources.Areas.ProjectManagment.Entities.Project.ProjectModel.BasicInfoTitle.ToLower(), Model.Id)%></legend>
    <script type="text/javascript">

        function JsonEdit_OnComplete(context) {
            var jsonEdit = context.get_response().get_object();
            if (jsonEdit.Success) {
                $.ajax({
                    url: '<%: Url.Action("SaveTabIndex", "Project")%>/', type: "POST",
                    data: { selectedIndex: 100 },
                    success: function () {
                        location.reload();
                    }
                });
            } else {
                $("#result").html(jsonEdit.PartialViewHtml);
            }
        }
    </script>
    <%--<% if (Session["ProjectRelatedPosition"] != null)
       { %>
    <input type="hidden" id="position" value="<%: Session["ProjectRelatedPosition"].ToString() %>" />
    <% } %>--%>
    <table style="margin-left: 1px">
        <tr>
            <td>
                <%:Html.ValidationMessageFor(model => model.Code)%>
            </td>
        </tr>
        <tr>
            <td>
                <%:Html.ValidationMessageFor(model => model.Id)%>
            </td>
        </tr>
        <tr>
            <td>
                <%:Html.ValidationMessageFor(model => model.Name)%>
            </td>
        </tr>
        <tr>
            <td>
                <%:Html.ValidationMessageFor(model => model.Type)%>
            </td>
        </tr>
        <tr>
            <td>
                <%:Html.ValidationMessageFor(model => model.Owner)%>
            </td>
        </tr>
        <tr>
            <td>
                <%:Html.ValidationMessageFor(model => model.PlannedStartingDate)%>
            </td>
        </tr>
        <tr>
            <td>
                <%:Html.ValidationMessageFor(model => model.PlannedClosingDate)%>
            </td>
        </tr>
        <tr>
            <td>
                <%:Html.ValidationMessageFor(model => model.ActualStartingDate)%>
            </td>
        </tr>
        <tr>
            <td>
                <%:Html.ValidationMessageFor(model => model.ActualClosingDate)%>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td style="width: 10%; vertical-align: top" colspan="2">
                <input type="submit" value="<%: Resources.Shared.Buttons.Function.Save %>" onclick="DisableSaveButton()" />
            </td>]
             <td>
                <%:Html.HiddenFor(model => model.Id)%>
                <% Html.RenderPartial("NodeChooser"); %>           
            </td>
        </tr>
        <tr>
            <td style="width: 100%; vertical-align: top" colspan="2">
                <table width="80%">
                    <tr>
                        <td style="width: 20%; vertical-align: top;">
                            <div class="editor-label-required">
                                <%:Html.LabelFor(model => model.Code)%>
                            </div>
                            <div class="editor-field">
                                <%:Html.EditorFor(model => model.Code)%>
                            </div>
                            <div class="editor-label-required">
                                <%:Html.LabelFor(model => model.Name)%>
                            </div>
                            <div class="editor-field">
                                <%:Html.EditorFor(model => model.Name)%>
                            </div>
                        </td>
                        <td style="width: 20%; vertical-align: top;">
                            <div class="editor-label-required">
                                <%:Html.LabelFor(model => model.Type)%>
                            </div>
                            <div class="editor-field">
                                <%:Html.Telerik().DropDownListFor(model => model.Type.Id)
                                .BindTo(DropDownListHelpers.ListOfProjectType)
                                .HtmlAttributes(new {style = string.Format("width:{0}px", 200)})
                                %>
                            </div>
                        </td>
                        <td style="width: 20%; vertical-align: top;">
                            <div class="editor-label-required">
                                <%:Html.LabelFor(model => model.PlannedStartingDate)%>
                            </div>
                            <div class="editor-field">
                                <%:
            Html.Telerik().DatePickerFor(model => model.PlannedStartingDate).Value(Model.PlannedStartingDate.Date).Min(
                DateTime.MinValue)%>
                            </div>
                            <div class="editor-label-required">
                                <%:Html.LabelFor(model => model.PlannedClosingDate)%>
                            </div>
                            <div class="editor-field">
                                <%:
            Html.Telerik().DatePickerFor(model => model.PlannedClosingDate).Value(Model.PlannedClosingDate.Date).Min(
                DateTime.MinValue)%>
                            </div>
                        </td>
                        <td style="width: 20%; vertical-align: top;">
                            <div class="editor-label">
                                <%:Html.LabelFor(model => model.ActualStartingDate)%>
                            </div>
                            <div class="editor-field">
                                <%:
            Html.Telerik().DatePickerFor(model => model.ActualStartingDate).Value(Model.ActualStartingDate.Date).Min(
                DateTime.MinValue)%>
                            </div>
                            <div class="editor-label">
                                <%:Html.LabelFor(model => model.ActualClosingDate)%>
                            </div>
                            <div class="editor-field">
                                <%:
            Html.Telerik().DatePickerFor(model => model.ActualClosingDate).Value(Model.ActualClosingDate.Date).Min(
                DateTime.MinValue)%>
                            </div>
                        </td>
                        <td style="width: 20%; vertical-align: top;">
                            <div class="editor-label">
                                <%:Html.LabelFor(model => model.Description)%>
                            </div>
                            <div class="editor-field">
                                <%:Html.TextAreaFor(model => model.Description)%>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <input type="button" value="<%: Resources.Shared.Buttons.Function.Cancel %>" onclick="cancel()" class="CancelButton" />
                <script type="text/javascript">
                    function cancel() {
                        $('#result').load('<%: Url.Action("PartialMasterInfo", "Project") %>');
                    }
                </script>
            </td>
            <td style="width: 10%; vertical-align: top" align="right">
                <input type="submit" value="<%: Resources.Shared.Buttons.Function.Save %>" onclick="DisableSaveButton()" />
            </td>
        </tr>
    </table>
</fieldset>
<% } %>
