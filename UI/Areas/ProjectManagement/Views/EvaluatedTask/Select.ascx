<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.ProjectManagment.ValueObjects.EvaluatedTask>" %>
<%@ Import Namespace="HRIS.Domain.ProjectManagment.ValueObjects" %>
<%@ Import Namespace="UI.Helpers.Views" %>
<table width="100%">
    <% foreach (var item in (IEnumerable<EvaluatedTask>)ViewData["ValueObjectsList"])
       { %>
    <tr style="border-width: 11; border-color: Black; border-spacing: 11">
        <td style="width: 20%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Description) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Description, new ReadOnlyTextBox(true, "MultiLine"))%>
            </div>
            <div class="display-label"> 
                <%: Html.LabelFor(model => model.Weight) %> 
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Weight)%>
            </div>
        </td>
        <td style="width: 20%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.TaskKpi)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.TaskKpi, new ReadOnlyTextBox(true, "MultiLine"))%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Team)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Team.Name)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.TeamRole)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.TeamRole.Role.Name)%>
            </div>
        </td>
        <td style="width: 20%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.DeadLine) %>
            </div>
            <div class="display-field">
                <%:Html.Encode(item.DeadLine.ToShortDateString())%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.ActualClosingDate) %>
            </div>
            <div class="display-field">
                <%:Html.Encode(item.ActualClosingDate.ToShortDateString())%>
            </div>
        </td>
        <td style="width: 20%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Status) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Status.Name)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Constraints) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Constraints, new ReadOnlyTextBox(true, "MultiLine"))%>
            </div>
        </td>
        <td style="width: 20%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Rate) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Rate)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Remarks)%>
            </div>
            <div class="display-field">
               <%: Html.DisplayFor(model => item.Remarks, new ReadOnlyTextBox(true, "MultiLine"))%>
            </div>
        </td>
        <td align="right">
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <input type="button" value="<%: Resources.Shared.Buttons.Function.Edit %>" onclick="ShowEditUserControl()" class="EditButton" />
            <script type="text/javascript">
                function ShowEditUserControl() {
                    $('#addValueObjectArea').load('<%:Url.Action("JsonEdit", "EvaluatedTask", new { })%>');
                }
            </script>
            <% } %>
        </td>
    </tr>
    <% } %>
</table>
