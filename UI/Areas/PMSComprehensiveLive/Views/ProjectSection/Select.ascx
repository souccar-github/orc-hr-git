<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Model.PMS.ValueObjects.Implementation.Project.ProjectSection>" %>

<%@ Import Namespace="Model.PMS.ValueObjects.Implementation.Project" %>


<table width="100%">
    <% foreach (var item in (IEnumerable<ProjectSection>)ViewData["ValueObjectsList"])
       { %>
    <tr>
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Name)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Name)%>
            </div>
        </td>
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Weight)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Weight)%>
            </div>
        </td>
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.TotalRate)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.TotalRate)%>
            </div>
        </td>
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.FinalSubmit)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.FinalSubmit)%>
            </div>
        </td>
        
        
        <td align="right">
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <input type="button" value="Edit" onclick="ShowEditUserControl()" class="EditButton" />
            <script type="text/javascript">
                function ShowEditUserControl() {
                    $('#addValueObjectArea').load('<%:Url.Action("JsonEdit", "ProjectSection", new { })%>');
                }
            </script>
            <% } %>
        </td>
    </tr>
    <% } %>
</table>

