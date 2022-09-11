<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Personnel.ValueObjects.Child>" %>
<%@ Import Namespace="HRIS.Domain.Personnel.ValueObjects" %>
<table width="100%">
    <% foreach (var item in (IEnumerable<Child>)ViewData["ValueObjectsList"])
       { %>
    <tr>
        <td style="width: 25%; vertical-align: top">
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
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Nationality) %>
            </div>
            <div class="editor-field">
                <%: Html.DisplayFor(model => item.Nationality.Name)%>
            </div>
        </td>
        <td style="width: 25%; vertical-align: top">
            <div class="editor-label">
                <%: Html.LabelFor(model => model.PlaceOfBirth) %>
            </div>
            <div class="editor-field">
                <%: Html.DisplayFor(model => item.PlaceOfBirth.Name)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.DateOfBirth) %>
            </div>
            <div class="display-field">
                <%: Html.Encode(String.Format("{0:MM/dd/yyyy}", item.DateOfBirth))%>
            </div>
        </td>
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.ResidencyNo) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.ResidencyNo, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.ResidencyExpiryDate) %>
            </div>
            <div class="display-field">
                <%: Html.Encode(String.Format("{0:MM/dd/yyyy}", item.ResidencyExpiryDate))%>
            </div>
        </td>
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.PassportNo) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.PassportNo, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.PassportExpiryDate) %>
            </div>
            <div class="display-field">
                <%: Html.Encode(String.Format("{0:MM/dd/yyyy}", item.PassportExpiryDate))%>
            </div>
        </td>
        <td>
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <%: Ajax.ActionLink(Resources.Shared.Buttons.Function.Edit, "JsonEdit", "Children", new { item.Id }, new AjaxOptions { OnComplete = "JsonEdit_OnComplete" })%>
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
