<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Objectives.RootEntities.StrategicObjective>" %>
<%@ Import Namespace="UI.Areas.Objective.Helpers" %>
<% using (Ajax.BeginForm("JsonEdit", "StrategicObjective", new AjaxOptions { OnComplete = "JsonEdit_OnComplete" }))
   {%>
<fieldset class="ParentFieldset" style="height: auto">
    <legend class="ParentLegend"><%:Resources.Areas.Objective.Entities.OrganizationalObjective.OrganizationalObjectiveModel.OrganizationalObjectiveNo %> (<%: Html.DisplayFor(model => model.Id)%>)
        Details</legend>
    <script type="text/javascript">
        function JsonEdit_OnComplete(context) {

            var jsonEdit = context.get_response().get_object();

            if (jsonEdit.Success) {
                $.ajax({
                    url: '<%: Url.Action("SaveTabIndex", "StrategicObjective")%>/', type: "POST",
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
    <table>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Name) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Description) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Period) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.FromYear) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.ToYear) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Dimension) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.DoesNotMeet) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Meet) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Above) %>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td style="width: 10%; vertical-align: top" colspan="2">
                <input type="submit" value="<%:Resources.Shared.Buttons.Function.Save %>" onclick="DisableSaveButton()" />
                <%: Html.HiddenFor(model => model.Id) %>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <fieldset style="height: auto">
                    <legend>Defenition</legend>
                    <table>
                        <tr>
                            <td style="vertical-align: top;">
                                <div class="editor-label-required">
                                    <%: Html.LabelFor(model => model.Name) %>
                                </div>
                                <div class="editor-field">
                                    <%: Html.TextBoxFor(model => model.Name)%>
                                </div>
                            </td>
                            <td style="vertical-align: top;">
                                <div class="editor-label-required">
                                    <%: Html.LabelFor(model => model.Dimension) %>
                                </div>
                                <div class="editor-field">
                                    <%:Html.Telerik().DropDownListFor(model => model.Dimension.Id)
                                .BindTo(DropDownListHelpers.ListOfDimensions)
                              .HtmlAttributes(new {style = string.Format("width:{0}px", 200)})%>
                                </div>
                            </td>
                            <td style="vertical-align: top;">
                                <div class="editor-label">
                                    <%: Html.LabelFor(model => model.Description) %>
                                </div>
                                <div class="editor-field">
                                    <%:Html.TextAreaFor(model => model.Description)%>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
        </tr>
        <tr>
            <td colspan="2">
                <fieldset style="height: auto">
                    <legend>Planning</legend>
                    <table>
                        <tr>
                            <td style="vertical-align: top;">
                                <div class="editor-label-required">
                                    <%: Html.LabelFor(model=>model.Period) %>
                                </div>
                                <div class="editor-field">
                                    <%:Html.Telerik().DropDownListFor(model => model.Period)
                                .BindTo(DropDownListHelpers.ListOfObjectivePeriods)
                              .HtmlAttributes(new {style = string.Format("width:{0}px", 200)})%>
                                </div>
                            </td>
                            <td style="vertical-align: top;">
                                <div class="editor-label-required">
                                    <%: Html.LabelFor(model=>model.FromYear) %>
                                </div>
                                <div class="editor-field">
                                    <%: Html.Telerik().IntegerTextBoxFor(model => model.FromYear).NumberGroupSize(0)%>
                                </div>
                            </td>
                            <td style="vertical-align: top;">
                                <div class="editor-label-required">
                                    <%: Html.LabelFor(model=>model.ToYear) %>
                                </div>
                                <div class="editor-field">
                                    <%: Html.Telerik().IntegerTextBoxFor(model => model.ToYear).NumberGroupSize(0)%>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <fieldset style="height: auto">
                    <legend>Evaluation Criteria</legend>
                    <table>
                        <tr>
                            <td style="width: 30%; vertical-align: top;">
                                <div class="editor-label-required">
                                    <%: Html.LabelFor(model=>model.DoesNotMeet) %>
                                </div>
                                <div class="editor-field">
                                    <%:Html.TextAreaFor(model => model.DoesNotMeet)%>
                                </div>
                            </td>
                            <td style="width: 30%; vertical-align: top;">
                                <div class="editor-label-required">
                                    <%: Html.LabelFor(model=>model.Meet) %>
                                </div>
                                <div class="editor-field">
                                    <%:Html.TextAreaFor(model => model.Meet)%>
                                </div>
                            </td>
                            <td style="vertical-align: top;">
                                <div class="editor-label-required">
                                    <%: Html.LabelFor(model=>model.Above) %>
                                </div>
                                <div class="editor-field">
                                    <%:Html.TextAreaFor(model => model.Above)%>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
        <tr>
            <td>
                <input type="button" value="<%:Resources.Shared.Buttons.Function.Cancel  %>" onclick="cancel()" class="CancelButton" />
                <script type="text/javascript">
                    function cancel() {
                        $('#result').load('<%: Url.Action("PartialMasterInfo", "StrategicObjective") %>');
                    }
                </script>
            </td>
            <td style="width: 10%; vertical-align: top">
                <input type="submit" value="<%:Resources.Shared.Buttons.Function.Save  %>" onclick="DisableSaveButton()" />
            </td>
        </tr>
    </table>
</fieldset>
<% } %>
