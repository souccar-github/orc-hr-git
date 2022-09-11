<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Objectives.ValueObjects.EvaluationCriteria>" %>
<%@ Import Namespace="UI.Helpers.Views" %>
<table width="100%">
    <% foreach (var item in (IEnumerable<HRIS.Domain.Objectives.ValueObjects.EvaluationCriteria>)ViewData["ValueObjectsList"])
       { %>
    <tr>
        <td style="width: 33%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Below) %>
            </div>
            <div class="display-field">
                <%: Html.TextAreaFor(model => item.Below, new ReadOnlyTextBox(true, "MultiLine"))%>
            </div>
        </td>
        <td style="width: 33%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Meet) %>
            </div>
            <div class="display-field">
                <%: Html.TextAreaFor(model => item.Meet, new ReadOnlyTextBox(true, "MultiLine"))%>
            </div>
        </td>
        <td style="width: 33%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Above) %>
            </div>
            <div class="display-field">
                <%: Html.TextAreaFor(model => item.Above, new ReadOnlyTextBox(true, "MultiLine"))%>
            </div>
        </td>
        <td>
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <%: Ajax.ActionLink(Resources.Shared.Buttons.Function.Edit , "JsonEdit", "EvaluationCriteria", new { Id = item.Id }, new AjaxOptions { OnComplete = "JsonEdit_OnComplete" })%>
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
