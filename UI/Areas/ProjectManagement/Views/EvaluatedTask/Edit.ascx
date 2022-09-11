<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.ProjectManagment.ValueObjects.EvaluatedTask>" %>
<%@ Import Namespace="UI.Helpers.Views" %>
<table>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Rate)%>
        </td>
    </tr>
</table>
<fieldset class="ParentFieldset">
    <table width="100%">
        <tr>
            <td style="width: 20%; vertical-align: top">
                <%: Html.HiddenFor(model => model.Id) %> 
                <div class="display-label"> 
                    <%: Html.LabelFor(model => model.Description) %>
                </div>
                <div class="display-field">
                    <%: Html.TextAreaFor(model => model.Description, new UI.Helpers.Views.ReadOnlyTextBox(true, "MultiLine"))%>
                </div>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Weight) %>
                </div>
                <div class="display-field">
                    <%: Html.TextBoxFor(model => model.Weight, new UI.Helpers.Views.ReadOnlyTextBox(true, "SingleLine"))%>
                </div>
            </td>
            <td style="width: 20%; vertical-align: top">
                <div class="display-label">
                    <%: Html.LabelFor(model => model.TaskKpi) %>
                </div>
                <div class="display-field">
                    <%: Html.TextAreaFor(model => model.TaskKpi, new UI.Helpers.Views.ReadOnlyTextBox(true, "MultiLine"))%>
                </div>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Team)%>
                </div>
                <div class="display-field">
                    <%: Html.TextBoxFor(model => model.Team.Name, new UI.Helpers.Views.ReadOnlyTextBox(true, "SingleLine"))%>
                </div>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.TeamRole)%>
                </div>
                <div class="display-field">
                    <%: Html.TextBoxFor(model => model.TeamRole.Role.Name, new UI.Helpers.Views.ReadOnlyTextBox(true, "SingleLine"))%>
                </div>
            </td>
            <td style="width: 20%; vertical-align: top">
                <div class="display-label">
                    <%: Html.LabelFor(model => model.DeadLine) %>
                </div>
                <div class="display-field">
                    <%: Html.HiddenFor(model => model.DeadLine)%>
                    <%:Html.Encode(Model.DeadLine.ToShortDateString())%>
                </div>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.ActualClosingDate) %>
                </div>
                <div class="display-field">
                    <%: Html.HiddenFor(model => model.ActualClosingDate)%>
                    <%:Html.Encode(Model.ActualClosingDate.ToShortDateString())%>
                </div>
            </td>
            <td style="width: 20%; vertical-align: top">
                <div class="display-label">
                    <%: Html.HiddenFor(model => model.Status.Id)%>
                    <%: Html.LabelFor(model => model.Status) %>
                </div>
                <div class="display-field">
                    <%: Html.TextBoxFor(model => model.Status.Name, new UI.Helpers.Views.ReadOnlyTextBox(true, "SingleLine"))%>
                </div>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Constraints) %>
                </div>
                <div class="display-field">
                    <%: Html.TextAreaFor(model => model.Constraints, new UI.Helpers.Views.ReadOnlyTextBox(true, "MultiLine"))%>
                </div>
            </td>
            <td style="width: 20%; vertical-align: top">
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Rate) %>
                </div>
                <div class="display-field">
                    <%: Html.Telerik().PercentTextBox().Name("Rate").MinValue(0).MaxValue(100)%>
                </div>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Remarks) %>
                </div>
                <div class="display-field">
                    <%: Html.TextAreaFor(model => model.Remarks, new UI.Helpers.Views.ReadOnlyTextBox(true, "MultiLine"))%>
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
