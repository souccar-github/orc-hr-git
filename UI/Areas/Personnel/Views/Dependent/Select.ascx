<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Personnel.ValueObjects.Dependent>" %>
<%@ Import Namespace="HRIS.Domain.Personnel.ValueObjects" %>
<table width="100%">
    <% foreach (var item in (IEnumerable<Dependent>)ViewData["ValueObjectsList"])
       { %>
    <tr>
        <td style="width: 33%; vertical-align: top">
            <%: Html.HiddenFor(model => model.Id) %>
            <div class="display-label">
                <%: Html.LabelFor(model => model.FirstName) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.FirstName, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.LastName) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.LastName, new { @readonly = true, @class = "SingleLine" })%>
            </div>
        </td>
        <td style="width: 33%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.PlaceOfBirth) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.PlaceOfBirth.Name)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.DateOfBirth) %>
            </div>
            <div class="display-field">
                <%: Html.Encode(String.Format("{0:MM/dd/yyyy}", item.DateOfBirth))%>
            </div>
        </td>
        <td style="width: 33%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Nationality) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Nationality.Name)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.ContactNumber) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.ContactNumber, new { @readonly = true, @class = "SingleLine" })%>
            </div>
        </td>
        <td>
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <%: Ajax.ActionLink(Resources.Shared.Buttons.Function.Edit, "JsonEdit", "Dependent", new { Id = item.Id }, new AjaxOptions { OnComplete = "JsonEdit_OnComplete" })%>
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
    };

</script>
