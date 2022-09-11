<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Objectives.ValueObjects.Evaluation>" %>
<%@ Import Namespace="HRIS.Domain.Objectives.ValueObjects" %>
<%@ Import Namespace="UI.Helpers.Views" %>
<table width="100%">
    <% foreach (var item in (IEnumerable<Evaluation>)ViewData["ValueObjectsList"])
       { %>
    <tr>
        <td style="width: 33.3%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Evaluator)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Evaluator.FirstName)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Position)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Position.JobTitle.Name)%>
            </div>
        </td>
        <td style="width: 33.3%; vertical-align: top">
            <div class="display-label">
                <%:Html.LabelFor(model => model.Date)%>
            </div>
            <div class="display-field">
                <%:Html.Encode(String.Format("{0:MM/dd/yyyy}", item.Date))%>
            </div>
            <div class="display-label">
                <%:Html.LabelFor(model => model.Quarter)%>
            </div>
            <div class="display-field">
                <%:Html.TextBoxFor(model => item.Quarter, new ReadOnlyTextBox(true, "SingleLine"))%>
            </div>
        </td>
        <td style="width: 33.3%; vertical-align: top">
            <div class="display-label">
                <%:Html.LabelFor(model => model.TotalEvaluationRate)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.TotalEvaluationRate)%>%
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Comment) %>
            </div>
            <div class="display-field" id="original">
                <%: Html.TextAreaFor(model => item.Comment, new ReadOnlyTextBox(true, "MultiLine"))%>
            </div>
        </td>
        <td align="right">
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <%: Ajax.ActionLink(Resources.Shared.Buttons.Function.Edit, "JsonEdit", "Evaluation", new { }, new AjaxOptions { OnComplete = "JsonEdit_OnComplete" })%>
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
