<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.PMS.ValueObjects.Implementation.Objective.ObjectiveSection>" %>
<%@ Import Namespace="HRIS.Domain.PMS.ValueObjects.Implementation.Objective" %>
<table width="100%">
    <% foreach (var item in (IEnumerable<ObjectiveSection>)ViewData["ValueObjectsList"])
       { %>
    <tr style="border-width: 11; border-color: Black; border-spacing: 11">
        <td style="width: 33%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Name) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.Name, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.FinalSubmit) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.FinalSubmit) %>
            </div>            
        </td>
        <td style="width: 33%; vertical-align: top">            
            <div class="display-label">
                <%: Html.LabelFor(model => model.Weight) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Weight) %>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.TotalRate) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.TotalRate)%>
            </div>
        </td>
        <td>
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            
            <input type="button" value="Edit" onclick="ShowEditUserControl()" class="EditButton" />
            <script type="text/javascript">
                function ShowEditUserControl() {
                    $('#addValueObjectArea').load('<%:Url.Action("JsonEdit", "ObjectiveSection", new { })%>');
                    Toggle("edit");
                }
            </script>

            <% } %>
        </td>
    </tr>
    <% } %>
</table>
