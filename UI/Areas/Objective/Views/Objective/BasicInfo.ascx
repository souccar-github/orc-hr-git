<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Objectives.RootEntities.Objective>" %>
<%@ Import Namespace="UI.Helpers.Views" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"> <%: Resources.Areas.Objective.Entities.BasicObjective.BasicObjectiveModel.ObjectiveNO %>(<%:Html.DisplayFor(model => model.Id)%>)
        Details</legend>
    <table width="100%" style="vertical-align: middle">
        <tr>
            <td>
                <input type="button" value="Cancel" onclick="CancelButton()" class="CancelButton" />
            </td>
            <td style="width: 50%; vertical-align: top">
                <%
                    if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
                    {%>
                <input type="button" value="<%: Resources.Shared.Buttons.Function.Edit %>" onclick="ShowEditUserControl()" class="EditButton" />
                <%
                    }%>
                <script type="text/javascript">
                    function ShowEditUserControl() {
                        $('#result').load('<%:Url.Action("Edit", "Objective", new {id = Model.Id})%>');
                    }
                </script>
            </td>
        </tr>
        <tr>
            <td style="width: 100%; vertical-align: top">
                <table width="100%">
                    <tr>
                        <td style="width: 25%; vertical-align: top;">
                            <div class="display-label">
                                <%:Html.LabelFor(model => model.CreatedBy)%>
                            </div>
                            <div class="display-field">
                                <%:Html.TextBoxFor(model => model.CreatedBy, new ReadOnlyTextBox(true, "SingleLine"))%>
                            </div>
                                                            <div class="display-label">
                                <%:Html.LabelFor(model => model.Owner)%>
                            </div>
                            <div class="display-field">
                                <%:Html.TextBoxFor(model => model.Owner.JobTitle.Name, new ReadOnlyTextBox(true, "SingleLine"))%>
                            </div>
                        </td>
                        <td style="width: 25%; vertical-align: top;">
                            <div class="display-label">
                                <%:Html.LabelFor(model => model.CreationDate)%>
                            </div>
                            <div class="display-field">
                                <%:Html.Encode(String.Format("{0:MM/dd/yyyy}", Model.CreationDate))%>
                            </div>
                            <div class="display-label">
                                <%:Html.LabelFor(model => model.Description)%>
                            </div>
                            <div class="display-field">
                                <%:Html.TextAreaFor(model => model.Description, new ReadOnlyTextBox(true, "MultiLine"))%>
                            </div>
                                                        <div class="display-label">
                                <%:Html.LabelFor(model => model.Type)%>
                            </div>
                            <div class="display-field">
                                <%:Html.TextBoxFor(model => model.Type, new ReadOnlyTextBox(true, "SingleLine"))%>
                            </div>
                                                        <div class="display-label">
                                <%:Html.LabelFor(model => model.Priority)%>
                            </div>
                            <div class="display-field">
                                <%:Html.TextBoxFor(model => model.Priority, new ReadOnlyTextBox(true, "SingleLine"))%>
                            </div>
                        </td>
                        <td style="width: 25%; vertical-align: top;">
                            <div class="display-label">
                                <%:Html.LabelFor(model => model.Name)%>
                            </div>
                            <div class="display-field">
                                <%:Html.TextBoxFor(model => model.Name, new ReadOnlyTextBox(true, "SingleLine"))%>
                            </div>
                            <div class="display-label">
                                <%:Html.LabelFor(model => model.PlannedStartingDate)%>
                            </div>
                            <div class="display-field">
                                <%:Html.Encode(String.Format("{0:MM/dd/yyyy}", Model.PlannedStartingDate))%>
                            </div>
                            <div class="display-label">
                                <%:Html.LabelFor(model => model.PlannedClosingDate)%>
                            </div>
                            <div class="display-field">
                                <%:Html.Encode(String.Format("{0:MM/dd/yyyy}", Model.PlannedClosingDate))%>
                            </div>

                        </td>
                        <td style="width: 25%; vertical-align: top;">
                            <div class="display-label">
                                <%:Html.LabelFor(model => model.Type)%>
                            </div>
                            <div class="display-field">
                                <%:Html.TextBoxFor(model => model.Type, new ReadOnlyTextBox(true, "SingleLine"))%>
                            </div>
                            <div class="display-label">
                                <%:Html.LabelFor(model => model.Weight)%>
                            </div>
                            <div class="display-field">
                                <%:Html.TextBoxFor(model => model.Weight, new ReadOnlyTextBox(true, "SingleLine"))%>
                            </div>

                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</fieldset>
