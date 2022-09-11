<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Personnel.ValueObjects.Contact>" %>
<table width="100%">
    <% foreach (var item in (IEnumerable<HRIS.Domain.Personnel.ValueObjects.Contact>)ViewData["ValueObjectsList"])
       { %>
    <tr>
        <td style="width: 33%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.FirstContact) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.FirstContact, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.SecondContact) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.SecondContact, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Fax) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.Fax, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.POBox) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.POBox, new { @readonly = true, @class = "SingleLine" })%>
            </div>
        </td>
        <td style="width: 33%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.PrimaryEMail) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.PrimaryEMail, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.SecondaryEMail) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.SecondaryEMail, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Address) %>
            </div>
            <div class="display-field">
                <%: Html.TextAreaFor(model => item.Address, new { @readonly = true, @class = "MultiLine" })%>
            </div>
        </td>
        <td style="width: 33%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.WebSite) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.WebSite, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Twitter) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.Twitter, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Facebook) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.Facebook, new { @readonly = true, @class = "SingleLine" })%>
            </div>
        </td>
        <td>
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <%: Ajax.ActionLink(Resources.Shared.Buttons.Function.Edit, "JsonEdit", "Contact", new { Id = item.Id }, new AjaxOptions { OnComplete = "JsonEdit_OnComplete" })%>
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
