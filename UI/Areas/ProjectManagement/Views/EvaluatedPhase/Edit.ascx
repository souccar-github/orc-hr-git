<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.ProjectManagment.ValueObjects.EvaluatedPhase>" %>
<%@ Import Namespace="UI.Helpers.Views" %>
<table>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.CompletionPercentage)%>
        </td>
    </tr>
</table>
<fieldset class="ParentFieldset">
    <table width="100%">
        <tr>
            <td style="width: 25%; vertical-align: top">
                <%: Html.HiddenFor(model => model.Id) %>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Name) %>
                </div>
                <div class="display-field">
                    <%: Html.TextBoxFor(model => model.Name, new ReadOnlyTextBox(true, "SingleLine"))%>
                </div>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Team)%>
                </div>
                <div class="display-field">
                    <%: Html.TextBoxFor(model => model.Team.Name, new ReadOnlyTextBox(true, "SingleLine"))%>
                </div>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.TeamRole)%>
                </div>
                <div class="display-field">
                    <%: Html.TextBoxFor(model => model.TeamRole.Role.Name, new ReadOnlyTextBox(true, "SingleLine"))%>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Description) %>
                </div>
                <div class="display-field">
                    <%: Html.TextAreaFor(model => model.Description, new ReadOnlyTextBox(true, "MultiLine"))%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.StartDate) %>
                </div>
                <div class="editor-field">
                    <%: Html.HiddenFor(model => model.StartDate)%>
                    <%:Html.Encode(Model.StartDate.ToShortDateString())%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.EndDate) %>
                </div>
                <div class="editor-field">
                    <%: Html.HiddenFor(model => model.EndDate)%>
                    <%:Html.Encode(Model.EndDate.ToShortDateString())%>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Status) %>
                </div>
                <div class="display-field">
                    <%: Html.HiddenFor(model => model.Status.Id)%>
                    <%: Html.TextBoxFor(model => model.Status.Name, new ReadOnlyTextBox(true, "SingleLine"))%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.CompletionPercentage) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().PercentTextBox().Name("CompletionPercentage").MinValue(0).MaxValue(100)%>
                </div>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.TotalPhaseRate) %>
                </div>
                <div class="display-field">
                    <%: Html.TextBoxFor(model => model.TotalPhaseRate, new ReadOnlyTextBox(true, "SingleLine"))%>
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
