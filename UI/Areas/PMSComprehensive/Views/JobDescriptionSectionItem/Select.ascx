<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.PMS.ValueObjects.Implementation.JobDescription.JobDescriptionSectionItem>" %>
<%@ Import Namespace="HRIS.Domain.PMS.ValueObjects.Implementation.JobDescription" %>

<table width="100%">
    <% foreach (var item in (IEnumerable<JobDescriptionSectionItem>)ViewData["ValueObjectsList"])
       { %>
    <tr>
        <td style="width: 33.3%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.JobTitle)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.JobTitle)%>
            </div>
        </td>
        <td style="width: 33.3%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Weigth)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Weigth)%>
            </div>
        </td>
        <td style="width: 33.3%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Rate)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Rate)%>
            </div>
        </td>
        
        <td align="right">
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <input type="button" value="Edit" onclick="ShowEditUserControl()" class="EditButton" />
            <script type="text/javascript">
                function ShowEditUserControl() {
                    $('#addValueObjectArea').load('<%:Url.Action("JsonEdit", "JobDescriptionSectionItem", new { })%>');
                }
            </script>
            <% } %>
        </td>
    </tr>
    <% } %>
</table>

