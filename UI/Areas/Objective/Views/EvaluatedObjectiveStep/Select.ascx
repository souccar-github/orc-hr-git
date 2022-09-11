<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Objectives.ValueObjects.EvaluatedObjectiveStep>" %>
<%@ Import Namespace="HRIS.Domain.Objectives.ValueObjects" %>
<%@ Import Namespace="UI.Helpers.Views" %>
<table width="100%">
    <% foreach (var item in (IEnumerable<EvaluatedObjectiveStep>)ViewData["ValueObjectsList"])
       { %>
    <tr style="border-width: 11; border-color: Black; border-spacing: 11">
        <td style="width: 20%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Number) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Number)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Description) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Description, new ReadOnlyTextBox(true, "MultiLine"))%>
            </div>
        </td>
        <td style="width: 20%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Owner) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Owner.FirstName)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Status) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Status.Name)%>
            </div>
        </td>
        <td style="width: 20%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.PlannedStartingDate) %>
            </div>
            <div class="display-field">
                <%:Html.Encode(String.Format("{0:MM/dd/yyyy}", item.PlannedStartingDate))%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.PlannedClosingDate) %>
            </div>
            <div class="display-field">
                <%:Html.Encode(String.Format("{0:MM/dd/yyyy}", item.PlannedClosingDate))%>
            </div>
        </td>
        <td style="width: 20%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.ActualStartingDate) %>
            </div>
            <div class="display-field">
                <%:Html.Encode(String.Format("{0:MM/dd/yyyy}", item.ActualStartingDate))%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.ActualClosingDate) %>
            </div>
            <div class="display-field">
                <%:Html.Encode(String.Format("{0:MM/dd/yyyy}", item.ActualClosingDate))%>
            </div>
        </td>
        <td style="width: 20%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.EvaluationRate) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.EvaluationRate)%>%
            </div>
        </td>
        <td align="right">
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <%: Ajax.ActionLink(Resources.Shared.Buttons.Function.Edit, "JsonEdit", "EvaluatedObjectiveStep", new { Id = item.Id }, new AjaxOptions { OnComplete = "JsonEdit_OnComplete" })%>
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
