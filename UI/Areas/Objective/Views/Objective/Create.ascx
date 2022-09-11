<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Objectives.RootEntities.Objective>" %>
<%@ Import Namespace="HRIS.Domain.Objectives.Enums" %>
<%@ Import Namespace="UI.Areas.Objective.Helpers" %>
<%@ Import Namespace="UI.Helpers.Views" %>
<%
    using (Html.BeginForm("JsonInsert", "Objective"))
    {%>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%:Resources.Areas.Objective.Entities.BasicObjective.BasicObjectiveModel.BasicInformation%></legend>
    <table style="margin-left: 1px">

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
                <%:Html.ValidationMessageFor(model => model.Priority)%>
            </td>
        </tr>
        <tr>
            <td>
                <%:Html.ValidationMessageFor(model => model.Weight)%>
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

    </table>

    <table width="100%">

        <tr>
            <td colspan="2">
                <table width="100%">
                    <tr>
                        <td style="width: 25%; vertical-align: top;">
                            <div class="editor-label-required">
                                <%:Html.LabelFor(model => model.CreationDate)%>
                            </div>
                            <div class="editor-field">
                                <%:
    Html.Telerik().DatePickerFor(model => model.CreationDate).Value(Model.CreationDate.Date).Value(
        DateTime.Today)%>
                            </div>
                            <div class="editor-label-required">
                                <%:Html.HiddenFor(model => model.Owner.Id)%>
                                <%:Html.LabelFor(model => model.Owner)%>
                            </div>
                            <div id="nodePositions">
                            <fieldset style="height: auto; width: 300px">

                                <%:Html.Telerik().ComboBoxFor(model => model.Owner)
                           .HtmlAttributes(new {style = string.Format("width:{0}px", 300)})
                           .BindTo(UI.Areas.OrganizationChart.Helpers.DropDownListHelpers.ListOfPositions) %>
           
                            </fieldset>
                            </div>
                            <div class="editor-label-required">
                                <%:Html.LabelFor(model => model.Type)%>
                            </div>
                            <div class="editor-field">
                               
                                <%:Html.Telerik().ComboBoxFor(model => model.Type)
                           .HtmlAttributes(new {style = string.Format("width:{0}px", 300)})
                           .BindTo(DropDownListHelpers.ListOfObjectiveTypes())
    %>
                            </div>
                        </td>
                        <td style="width: 25%; vertical-align: top;">
                            <div class="editor-label-required">
                                <%:Html.LabelFor(model => model.Name)%>
                            </div>
                            <div class="editor-field">
                                <%:Html.EditorFor(model => model.Name)%>
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

                        </div>
                        </td>
                        <td style="width: 25%; vertical-align: top;">

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
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <a href="<%:Url.Action("Index", "Objective")%>">
                    <input type="button" value="<%:Resources.Shared.Buttons.Function.Cancel%>" />
                </a>
            </td>
            <td style="width: 10%; vertical-align: top">
                <input type="submit" value="<%:Resources.Shared.Buttons.Function.Add%>" onclick=" DisableSaveButton(); " />
            </td>
        </tr>
    </table>
</fieldset>
<%
    }%>