<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.ProjectManagment.ValueObjects.ProjectEvaluation>" %>
<table>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Date) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Quarter)%>
        </td>
    </tr>
    <tr> 
        <td>
            <%: Html.ValidationMessageFor(model => model.Status) %>
        </td>
    </tr>
</table>
<fieldset class="ParentFieldset">
    <table width="100%">
        <tr>
            <td style="width: 33.3%; vertical-align: top">
                <%: Html.HiddenFor(model => model.Id) %>
                <%: Html.HiddenFor(model => model.Evaluator.Id)%>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Evaluator) %>
                </div>
                <div class="editor-field">
                    <%:Html.TextBoxFor(model => model.Evaluator.FirstName, new UI.Helpers.Views.ReadOnlyTextBox(true, "SingleLine"))%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.EvaluatorProjectRole) %>
                </div>
                <div class="editor-field">
                    <%:Html.TextBoxFor(model => model.EvaluatorProjectRole, new UI.Helpers.Views.ReadOnlyTextBox(true, "SingleLine"))%>
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
                    <%: Html.LabelFor(model => model.CompletionPercentage) %>
                </div>
                <div class="editor-field">
                    <%:Html.TextBoxFor(model => model.CompletionPercentage, new UI.Helpers.Views.ReadOnlyTextBox(true, "SingleLine"))%>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.TotalProjectRate) %>
                </div>
                <div class="editor-field">
                    <%:Html.TextBoxFor(model => model.TotalProjectRate, new UI.Helpers.Views.ReadOnlyTextBox(true, "SingleLine"))%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Status) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.Status.Id)
                            .BindTo(UI.Areas.ProjectManagement.Helpers.DropDownListHelpers.ListOfPhaseStatus)
                            .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                     
                    %>
                </div>
            </td>
        </tr>
    </table>
</fieldset>
<table width="100%">
    <tr>
        <td style="width: 10%; vertical-align: top">
            <input type="submit" value="<%: Resources.Shared.Buttons.Function.Save %>" onclick="DisableSaveButton()" />
        </td>
    </tr>
</table>
