<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.ProjectManagment.Entities.Project>" %>
<%@ Import Namespace="UI.Helpers.Views" %>

<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%: string.Format(Resources.Areas.ProjectManagment.Entities.Project.ProjectModel.BasicInfoTitle.ToLower(), Model.Id)%></legend>
    <table width="100%" style="vertical-align: middle">
        <tr>
            <td>
                <input type="button" value="<%: Resources.Shared.Buttons.Function.Cancel %>" onclick="CancelButton()" class="CancelButton" />
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
                        $('#result').load('<%:Url.Action("Edit", "Project", new {id = Model.Id})%>');
                    }
                </script>
            </td>
        </tr>
        <tr>
            <td style="width: 100%; vertical-align: top">
                <table width="100%">
                    <tr>
                        <td style="width: 20%; vertical-align: top;">
                            <div class="display-label">
                                <%:Html.LabelFor(model => model.Code)%>
                            </div>
                            <div class="display-field">
                                <%:Html.TextBoxFor(model => model.Code, new ReadOnlyTextBox(true, "SingleLine"))%>
                            </div>
                            <div class="display-label">
                                <%:Html.LabelFor(model => model.Name)%>
                            </div>
                            <div class="display-field">
                                <%:Html.TextBoxFor(model => model.Name, new ReadOnlyTextBox(true, "SingleLine"))%>
                            </div>
                        </td>
                        <td style="width: 20%; vertical-align: top;">
                            <div class="display-label">
                                <%:Html.LabelFor(model => model.Type)%>
                            </div>
                            <div class="display-field">
                                <%:Html.TextBoxFor(model => model.Type.Name, new ReadOnlyTextBox(true, "SingleLine"))%>
                            </div>
                            <div class="display-label">
                                <%:Html.LabelFor(model => model.Owner)%>
                            </div>
                            <div class="display-field">
                                <%:Html.DisplayFor(model => model.Owner.FirstName)%>
                                <%:Html.DisplayFor(model => model.Owner.LastName)%>
                            </div>
                        </td>
                        <td style="width: 20%; vertical-align: top;">
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
                        <td style="width: 20%; vertical-align: top;">
                            <div class="display-label">
                                <%:Html.LabelFor(model => model.ActualStartingDate)%>
                            </div>
                            <div class="display-field">
                                <%:Html.Encode(String.Format("{0:MM/dd/yyyy}", Model.ActualStartingDate))%>
                            </div>
                            <div class="display-label">
                                <%:Html.LabelFor(model => model.ActualClosingDate)%>
                            </div>
                            <div class="display-field">
                                <%:Html.Encode(String.Format("{0:MM/dd/yyyy}", Model.ActualClosingDate))%>
                            </div>
                        </td>
                        <td style="width: 20%; vertical-align: top;">
                            <div class="display-label">
                                <%:Html.LabelFor(model => model.Description)%>
                            </div>
                            <div class="display-field">
                                <%:Html.TextAreaFor(model => model.Description, new ReadOnlyTextBox(true, "MultiLine"))%>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</fieldset>
