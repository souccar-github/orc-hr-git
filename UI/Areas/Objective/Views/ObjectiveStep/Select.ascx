<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Objectives.ValueObjects.ObjectiveStep>" %>
<%@ Import Namespace="UI.Helpers.Views" %>
<table width="100%">
    <% foreach (var item in (IEnumerable<HRIS.Domain.Objectives.ValueObjects.ObjectiveStep>)ViewData["ValueObjectsList"])
       { %>
    <tr>
        <td style="width: 33.3%; vertical-align: top">
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
            <div class="display-label">
                <%: Html.LabelFor(model => model.Owner) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Owner.FirstName)%>
                <%: Html.DisplayFor(model => item.Owner.LastName)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Status) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Status.Name)%>
            </div>
        </td>
        <td style="width: 33.3%; vertical-align: top">
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
        <td style="width: 33.3%; vertical-align: top">
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
            <div class="display-label">
                <%: Html.LabelFor(model => model.OutComes) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.OutComes, new ReadOnlyTextBox(true, "MultiLine"))%>
            </div>
        </td>
        <td>
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
<%--            <%: Ajax.ActionLink(Resources.Shared.Buttons.Function.Edit , "JsonEdit", "ObjectiveStep", new { Id = item.Id }, new AjaxOptions { OnComplete = "JsonEdit_OnComplete" })%>--%>

            <input type="button" value="<%:Resources.Shared.Buttons.Function.Edit  %>" onclick="ShowEditUserControl()" class="EditButton" />
            <script type="text/javascript">
                function ShowEditUserControl() {
                    $('#addValueObjectArea').load('<%:Url.Action("JsonEdit", "ObjectiveStep", new { })%>');
                    Toggle("edit");
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
            $("#ValueObjectsList").html(jsonEdit.PartialViewHtml);
            $("#addValueObjectArea").slideToggle("fast");
        }
    };

</script>
