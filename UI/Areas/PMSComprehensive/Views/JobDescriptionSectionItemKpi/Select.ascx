<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.PMS.ValueObjects.Implementation.JobDescription.JobDescriptionSectionItemKpi>" %>
<%@ Import Namespace="HRIS.Domain.PMS.ValueObjects.Implementation.JobDescription" %>
<%@ Import Namespace="UI.Helpers.Views" %>

<table width="100%">
    <% foreach (var item in (IEnumerable<JobDescriptionSectionItemKpi>)ViewData["ValueObjectsList"])
       { %>
    <tr>
        <td style="width: 50%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Value)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Value)%>
            </div>
        </td>
        <td style="width: 33.3%; vertical-align: top">
            <div class="display-label">
                <%:Html.LabelFor(model => model.Description)%>
            </div>
            <div class="display-field">
                <%:Html.TextAreaFor(model => item.Description, new ReadOnlyTextBox(true, "MultiLine"))%>
            </div>
        </td>
        
        <td align="right">
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <input type="button" value="Edit" onclick="ShowEditUserControl()" class="EditButton" />
            <script type="text/javascript">
                function ShowEditUserControl() {
                    $('#addValueObjectArea').load('<%:Url.Action("JsonEdit", "JobDescriptionSectionItemKpi", new { })%>');
                }
            </script>
            <% } %>
        </td>
    </tr>
    <% } %>
</table>

