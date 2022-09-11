<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.ProjectManagment.ValueObjects.TeamMember>" %>
<%@ Import Namespace="HRIS.Domain.ProjectManagment.ValueObjects" %>
<%@ Import Namespace="UI.Helpers.Views" %>
<table width="100%">
    <%
        foreach (var item in (IEnumerable<TeamMember>)ViewData["ValueObjectsList"])
        {%>
    <tr style="border-width: 11; border-color: Black; border-spacing: 11">
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%:Html.LabelFor(model => model.Node)%>
            </div>
            <div class="display-field">
                <%:Html.TextBoxFor(model => item.Position.Node.Name, new ReadOnlyTextBox(true, "SingleLine"))%>
            </div>
        </td>
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%:Html.LabelFor(model => model.Position)%>
            </div>
            <div class="display-field">
                <%:Html.TextBoxFor(model => item.Position.JobTitle.Name, new ReadOnlyTextBox(true, "SingleLine"))%>
            </div>
        </td>
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%:Html.LabelFor(model => model.Employee)%>
            </div>
            <div class="display-field">
                <%:Html.TextBoxFor(model => item.Employee.FirstName, new ReadOnlyTextBox(true, "SingleLine"))%>
            </div>
        </td>
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%:Html.LabelFor(model => model.IsCross)%>
            </div>
            <div class="display-field">
                <%:Html.TextBoxFor(model => item.IsCross, new ReadOnlyTextBox(true, "SingleLine"))%>
            </div>
            <div class="display-label">
                <%:Html.LabelFor(model => model.IsEvaluator)%>
            </div>
            <div class="display-field">
                <%:Html.TextBoxFor(model => item.IsEvaluator, new ReadOnlyTextBox(true, "SingleLine"))%>
            </div>
        </td>
        <td>
            <%
            if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
            {%>
           
     <input type="button" value="<%: Resources.Shared.Buttons.Function.Edit %>" onclick="ShowEditUserControl()" class="EditButton" />
            <script type="text/javascript">
                function ShowEditUserControl() {
                    $('#addValueObjectArea').load('<%:Url.Action("JsonEdit", "TeamMember", new { })%>');
                }
            </script>

            <%
            }%>
        </td>
    </tr>
    <%
        }%>
</table>

