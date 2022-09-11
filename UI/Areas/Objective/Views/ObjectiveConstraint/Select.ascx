<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Objectives.ValueObjects.ObjectiveConstraint>" %>
<%@ Import Namespace="HRIS.Domain.Objectives.ValueObjects" %>
<%@ Import Namespace="UI.Helpers.Views" %>
<table width="100%">
    <% foreach (var item in (IEnumerable<ObjectiveConstraint>)ViewData["ValueObjectsList"])
       { %>
    <tr style="border-width: 11; border-color: Black; border-spacing: 11">
        <td style="width: 100%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Description) %>
            </div>
            <div class="display-field" id="original">
                <%: Html.TextAreaFor(model => item.Description, new ReadOnlyTextBox(true, "MultiLine"))%>
            </div>
        </td>
        <td align="right">
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <%: Ajax.ActionLink(Resources.Shared.Buttons.Function.Edit, "JsonEdit", "ObjectiveConstraint", new { }, new AjaxOptions { OnComplete = "JsonEdit_OnComplete" })%>
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
            $("#ValueObjectsList").html(jsonEdit.PartialViewHtml);
            $("#addValueObjectArea").slideToggle("fast");
        }
    };

</script>
