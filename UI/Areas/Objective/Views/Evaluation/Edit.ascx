<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Objectives.ValueObjects.Evaluation>" %>
<%@ Import Namespace="UI.Helpers.Views" %>
<table>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Evaluator) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Position) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Date) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Quarter) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.TotalEvaluationRate)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Comment) %>
        </td>
    </tr>
</table>
<fieldset class="ParentFieldset">
    <table width="100%">
        <tr>
            <td style="width: 33.3%; vertical-align: top">
                <%: Html.HiddenFor(model => model.Id) %>
                <%: Html.HiddenFor(model => model.Evaluator.Id)%>
                <%: Html.HiddenFor(model => model.Position.Id)%>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Evaluator) %>
                </div>
                <div class="editor-field">
                    <%: Html.DisplayFor(model => model.Evaluator.FirstName)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Position) %>
                </div>
                <div class="editor-field">
                    <%: Html.DisplayFor(model => model.Position.JobTitle.Name)%>
                </div>
            </td>
            <td style="width: 33.3%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Date) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DatePickerFor(model => model.Date).Value(Model.Date.Date).Min(DateTime.MinValue)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Quarter) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().IntegerTextBox().Name("Quarter").MinValue(1).MaxValue(4)%>
                </div>
            </td>
            <td style="width: 33.3%; vertical-align: top">
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.TotalEvaluationRate) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.TotalEvaluationRate, new ReadOnlyTextBox(true, "SingleLine"))%>%
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.Comment) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextAreaFor(model => model.Comment)%>
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
