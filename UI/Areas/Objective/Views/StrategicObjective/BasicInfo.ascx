<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Objectives.RootEntities.StrategicObjective>" %>
<%@ Import Namespace="UI.Helpers.Views" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%:Resources.Areas.Objective.Entities.OrganizationalObjective.OrganizationalObjectiveModel.OrganizationalObjectiveNo %> (<%: Html.DisplayFor(model => model.Id)%>)
        Details</legend>
    <table width="100%" style="vertical-align: middle">
        <tr>
            <td>
                <input type="button" value="<%:Resources.Shared.Buttons.Function.Cancel  %>" onclick="CancelButton()" class="CancelButton" />
            </td>
            <td style="width: 50%; vertical-align: top">
                <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
                   {%>
                <input type="button" value="<%:Resources.Shared.Buttons.Function.Edit  %>" onclick="ShowEditUserControl()" class="EditButton" />
                <% } %>
                <script type="text/javascript">
                    function ShowEditUserControl() {
                        $('#result').load('<%: Url.Action("Edit", "StrategicObjective", new {id=Model.Id}) %>');
                    }
                </script>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <fieldset style="height: auto">
                    <legend>Defenition</legend>
                    <table>
                        <tr>
                            <td style="vertical-align: top;">
                                <div class="display-label">
                                    <%: Html.LabelFor(model => model.Name) %>
                                </div>
                                <div class="display-field">
                                    <%: Html.TextBoxFor(model => model.Name, new ReadOnlyTextBox(true, "SingleLine"))%>
                                </div>
                            </td>
                            <td style="vertical-align: top;">
                                <div class="display-label">
                                    <%: Html.LabelFor(model => model.Dimension) %>
                                </div>
                                <div class="display-field">
                                    <%:Html.TextBoxFor(model => model.Dimension.Name, new ReadOnlyTextBox(true, "SingleLine"))%>
                                </div>
                            </td>
                            <td style="vertical-align: top;">
                                <div class="editor-label">
                                    <%: Html.LabelFor(model => model.Description) %>
                                </div>
                                <div class="editor-field">
                                    <%: Html.TextAreaFor(model => model.Name, new ReadOnlyTextBox(true, "MultiLine"))%>
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
                    <legend>Planning</legend>
                    <table>
                        <tr>
                            <td style="vertical-align: top;">
                                <div class="display-label">
                                    <%: Html.LabelFor(model=>model.Period) %>
                                </div>
                                <div class="display-field">
                                    <%: Html.TextBoxFor(model => model.Period, new ReadOnlyTextBox(true, "SingleLine"))%>
                                </div>
                            </td>
                            <td style="vertical-align: top;">
                                <div class="display-label">
                                    <%: Html.LabelFor(model=>model.FromYear) %>
                                </div>
                                <div class="display-field">
                                    <%: Html.TextBoxFor(model => model.FromYear, new ReadOnlyTextBox(true, "SingleLine"))%>
                                </div>
                            </td>
                            <td style="vertical-align: top;">
                                <div class="display-label">
                                    <%: Html.LabelFor(model=>model.ToYear) %>
                                </div>
                                <div class="display-field">
                                    <%: Html.TextBoxFor(model => model.ToYear, new ReadOnlyTextBox(true, "SingleLine"))%>
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
                                <div class="display-label">
                                    <%: Html.LabelFor(model=>model.DoesNotMeet) %>
                                </div>
                                <div class="display-field">
                                    <%: Html.TextAreaFor(model => model.DoesNotMeet, new ReadOnlyTextBox(true, "MultiLine"))%>
                                </div>
                            </td>
                            <td style="width: 30%; vertical-align: top;">
                                <div class="display-label">
                                    <%: Html.LabelFor(model=>model.Meet) %>
                                </div>
                                <div class="display-field">
                                    <%: Html.TextAreaFor(model => model.Meet, new ReadOnlyTextBox(true, "MultiLine"))%>
                                </div>
                            </td>
                            <td style="vertical-align: top;">
                                <div class="display-label">
                                    <%: Html.LabelFor(model=>model.Above) %>
                                </div>
                                <div class="editor-field">
                                    <%: Html.TextAreaFor(model => model.Above, new ReadOnlyTextBox(true, "MultiLine"))%>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
    </table>
</fieldset>
