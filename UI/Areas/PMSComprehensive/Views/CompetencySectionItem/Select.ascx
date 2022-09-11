<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.PMS.ValueObjects.Implementation.Competency.CompetencySectionItem>" %>
<%@ Import Namespace="HRIS.Domain.PMS.ValueObjects.Implementation.Competency" %>
<%@ Import Namespace="UI.Helpers.Views" %>


<table width="100%">
    <% foreach (var item in (IEnumerable<CompetencySectionItem>)ViewData["ValueObjectsList"])
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
                <%: Html.LabelFor(model => model.Type)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Type)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Level)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Level)%>
            </div>
        </td>
        <td style="width: 33.3%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Weight)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Weight)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Rate)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Rate)%>
            </div>
            <div class="display-label">
                <%:Html.LabelFor(model => model.Comment)%>
            </div>
            <div class="display-field">
                <%:Html.TextAreaFor(model => item.Comment, new ReadOnlyTextBox(true, "MultiLine"))%>
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
                    $('#addValueObjectArea').load('<%:Url.Action("JsonEdit", "CompetencySectionItem", new { })%>');
                }
            </script>
            <% } %>
        </td>
    </tr>
    <% } %>
</table>

