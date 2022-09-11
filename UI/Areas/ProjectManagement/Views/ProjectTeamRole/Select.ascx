<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.ProjectManagment.ValueObjects.ProjectTeamRole>" %>
<%@ Import Namespace="UI.Helpers.Views" %>
<table width="100%">
    <% foreach (var item in (IEnumerable<HRIS.Domain.ProjectManagment.ValueObjects.ProjectTeamRole>)ViewData["ValueObjectsList"])
       { %>
    <tr>
        <td style="width: 33.33%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Role) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Role.Name)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.ParentRole) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.ParentRole.Name)%>
            </div>
        </td>
        <td style="width: 33.33%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Weight) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Weight)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Count) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Count)%>
            </div>
        </td>
        <td style="width: 33.33%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.IndirectProjectTeam) %>
            </div>
            <div class="display-field">
                <% if (item.IndirectProjectTeam != null)
                   { %>
                <%: Html.DisplayFor(model => item.IndirectProjectTeam.Name)%>
                <% }
                   else
                   { %>
                    <p>Empty</p>
                <% } %>
            </div>
                        <div class="display-label">
                <%: Html.LabelFor(model => model.IndirectProjectTeamRole) %>
            </div>
            <div class="display-field">
                <% if (item.IndirectProjectTeamRole != null)
                   { %>
                <%: Html.DisplayFor(model => item.IndirectProjectTeamRole.Role.Name)%>
                <% }
                   else
                   { %>
                   <p>Empty</p>
                <% } %>
            </div>
                        <div class="display-label">
                <%: Html.LabelFor(model => model.IndirectTeamMember) %>
            </div>
            <div class="display-field">
                <% if (item.IndirectTeamMember != null)
                   { %>
                <%: Html.DisplayFor(model => item.IndirectTeamMember.Employee.FirstName)%>
                <% }
                   else
                   { %>
                   <p>Empty</p>
                <% } %>
            </div>
        </td>
        <td align="right">
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <input type="button" value="<%: Resources.Shared.Buttons.Function.Edit %>" onclick="ShowEditUserControl()" class="EditButton" />
            <script type="text/javascript">
                function ShowEditUserControl() {
                    $('#addValueObjectArea').load('<%:Url.Action("JsonEdit", "ProjectTeamRole", new { })%>');
                }
            </script>
            <% } %>
        </td>
    </tr>
    <% } %>
</table>
