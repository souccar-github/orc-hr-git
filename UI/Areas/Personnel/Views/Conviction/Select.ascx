<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Personnel.ValueObjects.Conviction>" %>
<%@ Import Namespace="HRIS.Domain.Personnel.ValueObjects" %>
<table width="100%">
    <% foreach (var item in (IEnumerable<Conviction>)ViewData["ValueObjectsList"])
       { %>
    <tr style="border-width: 11; border-color: Black; border-spacing: 11">
        <td style="width: 33.3%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Number) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.Number, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Rule) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Rule.Name) %>
            </div>
        </td>
        <td style="width: 33.3%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.ConvictionDate) %>
            </div>
            <div class="display-field">
                <%: Html.Encode(String.Format("{0:MM/dd/yyyy}", item.ConvictionDate))%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.ReleaseDate) %>
            </div>
            <div class="display-field">
                <%: Html.Encode(String.Format("{0:MM/dd/yyyy}", item.ReleaseDate))%>
            </div>
        </td>
        <td style="width: 33.3%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Reason) %>
            </div>
            <div class="display-field">
                <%: Html.TextAreaFor(model => item.Reason, new { @readonly = true, @class = "MultiLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Notes) %>
            </div>
            <div class="display-field">
                <%: Html.TextAreaFor(model => item.Notes, new { @readonly = true, @class = "MultiLine" })%>
            </div>
        </td>
        <td>
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <%: Ajax.ActionLink(Resources.Shared.Buttons.Function.Edit, "JsonEdit", "Conviction", new { Id = item.Id }, new AjaxOptions { OnComplete = "JsonEdit_OnComplete" })%>
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
