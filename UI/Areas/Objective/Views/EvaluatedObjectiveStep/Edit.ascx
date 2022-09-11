<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Objectives.ValueObjects.EvaluatedObjectiveStep>" %>
<%@ Import Namespace="UI.Helpers.Views" %>
<table>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.EvaluationRate)%>
        </td>
    </tr>
</table>
<fieldset class="ParentFieldset">
    <table width="100%">
        <tr>
            <td style="width: 20%; vertical-align: top">
                <%: Html.HiddenFor(model => model.Id) %>
                <%: Html.HiddenFor(model => model.Evaluation.Id) %>
                <%: Html.HiddenFor(model => model.Owner.Id) %>
                <%: Html.HiddenFor(model => model.Status.Id) %>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Number) %>
                </div>
                <div class="display-field">
                    <%: Html.TextBoxFor(model => model.Number, new ReadOnlyTextBox(true, "SingleLineInputSelectMode"))%>
                </div>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Description) %>
                </div>
                <div class="display-field">
                    <%: Html.TextBoxFor(model => model.Description, new ReadOnlyTextBox(true, "MultiLine"))%>
                </div>
            </td>
            <td style="width: 20%; vertical-align: top">
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Owner) %>
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.Owner.FirstName)%>
                </div>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Status) %>
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.Status.Name)%>
                </div>
            </td>
            <td style="width: 20%; vertical-align: top">
                <div class="display-label">
                    <%: Html.LabelFor(model => model.PlannedStartingDate) %>
                </div>
                <div class="display-field">
                    <%: Html.TextBoxFor(model => model.PlannedStartingDate, new ReadOnlyTextBox(true, "MultiLine"))%>
                </div>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.PlannedClosingDate) %>
                </div>
                <div class="display-field">
                    <%: Html.TextBoxFor(model => model.PlannedClosingDate, new ReadOnlyTextBox(true, "MultiLine"))%>
                </div>
            </td>
            <td style="width: 20%; vertical-align: top">
                <div class="display-label">
                    <%: Html.LabelFor(model => model.ActualStartingDate) %>
                </div>
                <div class="display-field">
                    <%: Html.TextBoxFor(model => model.ActualStartingDate, new ReadOnlyTextBox(true, "MultiLine"))%>
                </div>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.ActualClosingDate) %>
                </div>
                <div class="display-field">
                    <%: Html.TextBoxFor(model => model.ActualClosingDate, new ReadOnlyTextBox(true, "MultiLine"))%>
                </div>
            </td>
            <td style="width: 20%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.EvaluationRate) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().PercentTextBox().Name("EvaluationRate").MinValue(0).MaxValue(100)%>
                </div>
            </td>
        </tr>
    </table>
</fieldset>
<table width="100%">
    <tr>
        <td style="width: 10%; vertical-align: top">
            <input type="submit" value="<%:Resources.Shared.Buttons.Function.Save %>" onclick="DisableSaveButton()" />
        </td>
    </tr>
</table>
