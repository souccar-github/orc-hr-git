<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.ProjectManagment.ValueObjects.ProjectResource>" %>
<%@ Import Namespace="UI.Helpers.Views" %>
<table width="100%">
    <% foreach (var item in (IEnumerable<HRIS.Domain.ProjectManagment.ValueObjects.ProjectResource>)ViewData["ValueObjectsList"])
       { %>
    <tr>
        <td style="width: 33.3%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Id) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Id)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Name) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.Name, new ReadOnlyTextBox(true, "SingleLine"))%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Description) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Description, new ReadOnlyTextBox(true, "MultiLine"))%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Type) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Type.Name)%>
            </div>
        </td>
        <td style="width: 33.3%; vertical-align: top">
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
            <div class="display-label">
                <%: Html.LabelFor(model => model.Comments) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Comments, new ReadOnlyTextBox(true, "MultiLine"))%>
            </div>
        </td>
        <td align="right">
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <input type="button" value="<%: Resources.Shared.Buttons.Function.Edit %>" onclick="ShowEditUserControl()" class="EditButton" />
            <script type="text/javascript">
                function ShowEditUserControl() {
                    $('#addValueObjectArea').load('<%:Url.Action("JsonEdit", "ProjectResource", new { })%>');
                }
            </script>
            <% } %>
        </td>
    </tr>
    <% } %>
</table>
