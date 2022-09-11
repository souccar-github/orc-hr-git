<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.ProjectManagment.ValueObjects.EvaluatedPhase>" %>
<%@ Import Namespace="HRIS.Domain.ProjectManagment.ValueObjects" %>
<%@ Import Namespace="UI.Helpers.Views" %>
<table width="100%">
    <% foreach (var item in (IEnumerable<EvaluatedPhase>)ViewData["ValueObjectsList"])
       { %>
    <tr style="border-width: 11; border-color: Black; border-spacing: 11">
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Name) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Name)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Team) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Team.Name)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.TeamRole) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.TeamRole.Role.Name)%>
            </div>
        </td>
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Description) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Description, new ReadOnlyTextBox(true, "MultiLine"))%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.StartDate) %>
            </div>
            <div class="display-field">
                <%:Html.Encode(item.StartDate.ToShortDateString())%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.EndDate) %>
            </div>
            <div class="display-field">
                <%:Html.Encode(item.EndDate.ToShortDateString())%>
            </div>
        </td>
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Status) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Status.Name)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.CompletionPercentage) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.CompletionPercentage)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.TotalPhaseRate)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.TotalPhaseRate)%>
            </div>
        </td>
        <td align="right">
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <input type="button" value="<%: Resources.Shared.Buttons.Function.Edit %>" onclick="ShowEditUserControl()"
                class="EditButton" />
            <script type="text/javascript">
                function ShowEditUserControl() {
                    $('#addValueObjectArea').load('<%:Url.Action("JsonEdit", "EvaluatedPhase", new { })%>');
                }
            </script>
            <% } %>
        </td>
    </tr>
    <% } %>
</table>
