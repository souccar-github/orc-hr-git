<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Objectives.RootEntities.Objective>" %>
<%@ Import Namespace="HRIS.Domain.Objectives.Enums" %>
<%@ Import Namespace="UI.Areas.Objective.Helpers" %>
<%@ Import Namespace="UI.Helpers.Views" %>
<% using (Ajax.BeginForm("JsonEdit", "Objective", new AjaxOptions {OnComplete = "JsonEdit_OnComplete"}))
   {%>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%:Resources.Areas.Objective.Entities.BasicObjective.BasicObjectiveModel.ObjectiveNO%> (<%:Html.DisplayFor(model => model.Id)%>)
        Details</legend>
    <script type="text/javascript">

        function JsonEdit_OnComplete(context) {
            var jsonEdit = context.get_response().get_object();
            if (jsonEdit.Success) {
                $.ajax({
                        url: '<%:Url.Action("SaveTabIndex", "Objective")%>/',
                        type: "POST",
                        data: { selectedIndex: 100 },
                        success: function() {
                            location.reload();
                        }
                    });
            } else {
                $("#result").html(jsonEdit.PartialViewHtml);
            }
        }
    </script>
    <% if (Session["RelatedPosition"] != null)
{ %>
    <input type="hidden" id="position" value="<%:Session["RelatedPosition"].ToString()%>" />
    <% } %>
    <table style="margin-left: 1px">
        <tr>
            <td>
                <%:Html.ValidationMessageFor(model => model.Name)%>
            </td>
        </tr>
        <tr>
            <td>
                <%:Html.ValidationMessageFor(model => model.Id)%>
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
                <%:Html.ValidationMessageFor(model => model.Priority)%>
            </td>
        </tr>

        <tr>
            <td>
                <%:Html.ValidationMessageFor(model => model.Weight)%>
            </td>
        </tr>
    </table>
    <table width="100%">

        <tr>
            <td style="width: 100%; vertical-align: top" colspan="2">
                <table width="80%">
                    <tr>
                        <td style="width: 20%; vertical-align: top;">
                            <div class="editor-label">
                                <%:Html.LabelFor(model => model.CreationDate)%>
                            </div>
                            <div class="editor-field">
                                <%:Html.Telerik().DatePickerFor(model => model.CreationDate).Value(Model.CreationDate.Date).Value(
    DateTime.Today)%>
                            </div>


                            <div class="editor-label">
                                <%:Html.LabelFor(model => model.Owner)%>
                            </div>
                            <div class="editor-field">
                                <%:Html.Telerik().ComboBoxFor(model => model.Owner.Id)
                           .HtmlAttributes(new {style = string.Format("width:{0}px", 300)})
                           .BindTo(UI.Areas.OrganizationChart.Helpers.DropDownListHelpers.ListOfPositions)
    %>
                            </div>
                            
                            <div class="editor-label-required">
                                <%:Html.LabelFor(model => model.Type)%>
                            </div>
                            <div class="editor-field">
                               
                                <%:Html.Telerik().ComboBoxFor(model => model.Type)
                           .HtmlAttributes(new {style = string.Format("width:{0}px", 300)})
                           .BindTo(DropDownListHelpers.ListOfObjectiveTypes())
                           .SelectedIndex((int)System.Enum.Parse(typeof(ObjectiveType),Model.Type.ToString()) )
    %>
                            </div>
 
                        </td>
                        <td style="width: 20%; vertical-align: top;">
                            <div class="editor-label-required">
                                <%:Html.LabelFor(model => model.Name)%>
                            </div>
                            <div class="editor-field">
                                <%:Html.EditorFor(model => model.Name
                           )%>
                            </div>

                            <div class="editor-label">
                                <%:Html.LabelFor(model => model.Description)%>
                            </div>
                            <div class="editor-field">
                                <%:Html.TextAreaFor(model => model.Description)%>
                            </div>

                            <div class="editor-label-required">
                                <%:Html.LabelFor(model => model.Weight)%>
                            </div>
                            <div class="editor-field">
                                <%:Html.Telerik().NumericTextBoxFor(model => model.Weight).MinValue(0).MaxValue(100)%>
                            </div>
                        </td>
                        <td style="width: 20%; vertical-align: top;">

                            <div class="editor-label-required">
                                <%:Html.LabelFor(model => model.PlannedStartingDate)%>
                            </div>
                            <div class="editor-field">
                                <%:Html.Telerik().DatePickerFor(model => model.PlannedStartingDate).Value(
    Model.PlannedStartingDate.Date).Min(DateTime.MinValue)%>
                            </div>
                            <div class="editor-label-required">
                                <%:Html.LabelFor(model => model.PlannedClosingDate)%>
                            </div>
                            <div class="editor-field">
                                <%:Html.Telerik().DatePickerFor(model => model.PlannedClosingDate).Value(
    Model.PlannedClosingDate.Date).Min(DateTime.MinValue)%>
                            </div>

                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <input type="button" value="<%:Resources.Shared.Buttons.Function.Cancel%>" onclick=" cancel() " class="CancelButton" />
                <script type="text/javascript">
                    function cancel() {
                        $('#result').load('<%:Url.Action("PartialMasterInfo", "Objective")%>');
                    }
                </script>
            </td>
            <td style="width: 10%; vertical-align: top" align="right">
                <input type="submit" value="<%:Resources.Shared.Buttons.Function.Save%>" onclick=" DisableSaveButton() " />
            </td>
        </tr>
    </table>
</fieldset>
<% } %>