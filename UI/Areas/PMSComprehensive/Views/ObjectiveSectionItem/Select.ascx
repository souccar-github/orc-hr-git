<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.PMS.ValueObjects.Implementation.Objective.ObjectiveSectionItem>" %>
<%@ Import Namespace="HRIS.Domain.PMS.ValueObjects.Implementation.Objective" %>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <% foreach (var item in (IEnumerable<ObjectiveSectionItem>)ViewData["ValueObjectsList"])
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
                <%: Html.LabelFor(model => model.IsShared)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.IsShared)%>
            </div>
        </td>
        <td style="width: 25%; vertical-align: top">
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
                <%: Html.LabelFor(model => model.SharedWithPercentage)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.SharedWithPercentage)%>
            </div>
        </td>
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Comment) %>
            </div>
            <div class="display-field">
                <%: Html.TextAreaFor(model => item.Comment, new { @readonly = true, @class = "MultiLine", @disapled = true })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Description) %>
            </div>
            <div class="display-field">
                <%: Html.TextAreaFor(model => item.Description, new { @readonly = true, @class = "MultiLine", @disapled = true })%>
            </div>
        </td>
        <td>
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <input type="button" value="Edit" onclick="ShowEditUserControl()" class="EditButton" />
            <script type="text/javascript">
                function ShowEditUserControl() {
                    $('#addValueObjectArea').load('<%:Url.Action("JsonEdit", "ObjectiveSectionItem", new { })%>');
                    Toggle("edit");
                }
            </script>
            <% } %>
        </td>
    </tr>
    <% } %>
</table>
