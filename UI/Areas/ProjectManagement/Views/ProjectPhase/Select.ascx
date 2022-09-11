<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.ProjectManagment.ValueObjects.ProjectPhase>" %>
<%@ Import Namespace="HRIS.Domain.ProjectManagment.ValueObjects" %>
<%@ Import Namespace="UI.Helpers.Views" %>

<table width="100%">
    <% foreach (var item in (IEnumerable<ProjectPhase>)ViewData["ValueObjectsList"])
       { %>
    <tr>
        <td style="width: 33.3%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Name)%>
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
            <div class="display-label">
                <%: Html.LabelFor(model => model.TeamMember) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.TeamMember.Employee.FirstName)%>
            </div>
        </td>
        <td style="width: 33.3%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Description) %>
            </div>
            <div class="display-field" id="original">
                <%: Html.TextAreaFor(model => item.Description, new ReadOnlyTextBox(true, "MultiLine"))%>
            </div>
            <div class="display-label">
                <%:Html.LabelFor(model => model.StartDate)%>
            </div>
            <div class="display-field">
                <%:Html.Encode(String.Format("{0:MM/dd/yyyy}", item.StartDate))%>
            </div>
            <div class="display-label">
                <%:Html.LabelFor(model => model.EndDate)%>
            </div>
            <div class="display-field">
                <%:Html.Encode(String.Format("{0:MM/dd/yyyy}", item.EndDate))%>
            </div>
        </td>
        <td style="width: 33.3%; vertical-align: top">
            <div class="display-label">
                <%:Html.LabelFor(model => model.Status)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Status.Name)%>
            </div>
            <div class="display-label">
                <%:Html.LabelFor(model => model.CompletionPercentage)%>
            </div>
            <div class="display-field">
                <%:Html.TextBoxFor(model => item.CompletionPercentage, new ReadOnlyTextBox(true, "SingleLine"))%>
            </div>
        </td>
        <td align="right">
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <input type="button" value="<%: Resources.Shared.Buttons.Function.Edit %>" onclick="ShowEditUserControl()" class="EditButton" />
            <script type="text/javascript">
                function ShowEditUserControl() {
                    $('#addValueObjectArea').load('<%:Url.Action("JsonEdit", "ProjectPhase", new { })%>');
                }
            </script>
            <% } %>
        </td>
    </tr>
    <% } %>
</table>

