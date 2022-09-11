<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Objectives.ValueObjects.ObjectiveStep>" %>
<%@ Import Namespace="UI.Areas.Objective.Helpers" %>
<table>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Description)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Owner)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Status)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.PlannedStartingDate)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.PlannedClosingDate)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.ActualStartingDate)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.ActualClosingDate)%>
        </td>
    </tr>
</table>
<fieldset class="ParentFieldset">
    <table width="100%">
        <tr>
            <td style="width: 33.3%; vertical-align: top">
                <%: Html.HiddenFor(model => model.Id) %>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Description) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextAreaFor(model => model.Description) %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Owner) %>
                </div>
                <div class="editor-field">
                    <%:Html.Telerik().DropDownListFor(model => model.Owner.Id)
                                .BindTo(DropDownListHelpers.ListOfPossibleStepOwners(Model.BasicObjective.Id))
                              .HtmlAttributes(new {style = string.Format("width:{0}px", 200)})%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Status) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.Status.Id)
                            .BindTo(DropDownListHelpers.ListOfStepStatus)
                            .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })                                     
                    %>
                </div>
            </td>
            <td style="width: 33.3%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.PlannedStartingDate) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DatePickerFor(model => model.PlannedStartingDate).Value(Model.PlannedStartingDate).Min(DateTime.MinValue) %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.PlannedClosingDate) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DatePickerFor(model => model.PlannedClosingDate).Value(Model.PlannedClosingDate).Min(DateTime.MinValue) %>
                </div>
            </td>
            <td style="width: 33.3%; vertical-align: top">
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.ActualStartingDate) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DatePickerFor(model => model.ActualStartingDate).Value(Model.ActualStartingDate).Min(DateTime.MinValue)%>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.ActualClosingDate) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DatePickerFor(model => model.ActualClosingDate).Value(Model.ActualClosingDate).Min(DateTime.MinValue)%>
                </div>
                 <div class="editor-label">
                    <%: Html.LabelFor(model => model.OutComes) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextAreaFor(model => model.OutComes)%>
                </div>
            </td>
        </tr>
    </table>
</fieldset>
<table width="100%">
    <tr>
        <td style="width: 100%; vertical-align: top" align="right">
            <input type="submit" value="<%:Resources.Shared.Buttons.Function.Save  %>" onclick="DisableSaveButton()" />
        </td>
    </tr>
</table>
