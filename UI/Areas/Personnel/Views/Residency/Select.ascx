<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Personnel.ValueObjects.Residency>" %>
<%@ Import Namespace="HRIS.Domain.Personnel.ValueObjects" %>
<table width="100%">
    <% foreach (var item in (IEnumerable<Residency>)ViewData["ValueObjectsList"])
       { %>
    <tr style="border-width: 11; border-color: Black; border-spacing: 11">
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.No) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => item.No, new { @readonly = true, @class = "SingleLine" })%></div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.FirstName) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.FirstName, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.MiddleName) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.MiddleName, new { @readonly = true, @class = "SingleLine" })%>
            </div>
        </td>
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.LastName) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.LastName, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.MotherName) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.MotherName, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Nationality) %>
            </div>
            <div class="editor-field">
                <%: Html.DisplayFor(model => item.Nationality.Name)%>
            </div>
        </td>
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Type) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Type.Name)%>
            </div>
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
        <td style="width: 25%; vertical-align: top">
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Tel) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.Tel, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Address) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.Address, new { @readonly = true, @class = "MultiLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Notes) %>
            </div>
            <div class="editor-field">
                <%: Html.TextAreaFor(model => item.Notes, new { @readonly = true, @class = "MultiLine" })%>
            </div>
        </td>
        <td>
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <%: Ajax.ActionLink(Resources.Shared.Buttons.Function.Edit, "JsonEdit", "Residency", new { Id = item.Id }, new AjaxOptions { OnComplete = "JsonEdit_OnComplete" })%>
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
