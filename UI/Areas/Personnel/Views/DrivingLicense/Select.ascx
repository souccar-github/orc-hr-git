<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Personnel.ValueObjects.DrivingLicense>" %>
<table width="100%">
    <% foreach (var item in (IEnumerable<HRIS.Domain.Personnel.ValueObjects.DrivingLicense>)ViewData["ValueObjectsList"])
       { %>
    <tr>
        <td style="width: 33.3%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Number) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.Number, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Type) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Type.Name)%>
            </div>
        </td>
        <td style="width: 33.3%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.PlaceOfIssuance) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.PlaceOfIssuance.Name)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.LegalCondition) %>
            </div>
            <div class="display-field">
                <%: Html.TextAreaFor(model => item.LegalCondition, new { @readonly = true, @class = "MultiLine" })%>
            </div>
        </td>
        <td style="width: 33.3%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.IssuanceDate) %>
            </div>
            <div class="display-field">
                <%: Html.Encode(String.Format("{0:MM/dd/yyyy}", item.IssuanceDate))%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.ExpiryDate) %>
            </div>
            <div class="display-field">
                <%: Html.Encode(String.Format("{0:MM/dd/yyyy}", item.ExpiryDate))%>
            </div>
        </td>
        <td>
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <%: Ajax.ActionLink(Resources.Shared.Buttons.Function.Edit, "JsonEdit", "DrivingLicense", new { Id = item.Id }, new AjaxOptions { OnComplete = "JsonEdit_OnComplete" })%>
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
