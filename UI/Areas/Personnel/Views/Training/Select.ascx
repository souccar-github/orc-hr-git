<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Personnel.ValueObjects.Training>" %>
<%@ Import Namespace="HRIS.Domain.Personnel.ValueObjects" %>
<table width="100%">
    <% foreach (var item in (IEnumerable<Training>)ViewData["ValueObjectsList"])
       { %>
    <tr>
        <td style="width: 27%; vertical-align: top">
            <%: Html.HiddenFor(model => model.Id) %>
            <div class="display-label">
                <%: Html.LabelFor(model => model.CourseName)%>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.CourseName, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.CourseDuration)%>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.CourseDuration, new { @readonly = true, @class = "SingleLine" })%>
                / Hours
            </div>
        </td>
        <td style="width: 23%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.CertificateIssuanceDate)%>
            </div>
            <div class="display-field">
                <%: Html.Encode(String.Format("{0:MM/dd/yyyy}", item.CertificateIssuanceDate))%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Status) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Status.Name)%>
            </div>
        </td>
        <td style="width: 27%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.TrainingCenter)%>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.TrainingCenter, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.TrainingCenterLocation)%>
            </div>
            <div class="display-field">
                <%: Html.TextAreaFor(model => item.TrainingCenterLocation, new { @readonly = true, @class = "MultiLine" })%>
            </div>
        </td>
        <td style="width: 23%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Notes)%>
            </div>
            <div class="display-field">
                <%: Html.TextAreaFor(model => item.Notes, new { @readonly = true, @class = "MultiLine" })%>
            </div>
        </td>
        <td>
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <%: Ajax.ActionLink(Resources.Shared.Buttons.Function.Edit, "JsonEdit", "Training", new { Id = item.Id }, new AjaxOptions { OnComplete = "JsonEdit_OnComplete" })%>
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
    }

</script>
