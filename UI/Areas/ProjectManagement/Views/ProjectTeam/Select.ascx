<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.ProjectManagment.ValueObjects.ProjectTeam>" %>
<%@ Import Namespace="HRIS.Domain.ProjectManagment.ValueObjects" %>
<%@ Import Namespace="UI.Helpers.Views" %>
<table width="100%">
    <% foreach (var item in (IEnumerable<ProjectTeam>)ViewData["ValueObjectsList"])
       { %>
    <tr>
        <td style="width: 100%; vertical-align: top">
            <div class="display-label">
                <%:Html.LabelFor(model => model.Name)%>
            </div>
            <div class="display-field">
                <%:Html.TextBoxFor(model => item.Name, new ReadOnlyTextBox(true, "SingleLine"))%>
            </div>
        </td>
        <td align="right">
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>

               <input type="button" value="<%: Resources.Shared.Buttons.Function.Edit %>" onclick="ShowEditUserControl()" class="EditButton" />
            <script type="text/javascript">
                function ShowEditUserControl() {
                    $('#addValueObjectArea').load('<%:Url.Action("JsonEdit", "ProjectTeam", new { })%>');
                }
            </script>
            <% } %>
        </td>
    </tr>
    <% } %>
</table>
<script type="text/javascript">

    function JsonEdit_OnComplete(context) {

        var jsonEdit = context.get_response().get_object();
        if (jsonEdit.Success) {
            $("#addValueObjectArea").html(jsonEdit.PartialViewHtml);
            Toggle("edit");
        }
        else {
            $("#ValueObjectsList").html(window.jsonEdit.PartialViewHtml);
            $("#addValueObjectArea").slideToggle("fast");
        }
    };

</script>
