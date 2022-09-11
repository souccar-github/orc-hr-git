<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Personnel.ValueObjects.Spouse>" %>
<table width="100%">
    <% foreach (var item in (IEnumerable<HRIS.Domain.Personnel.ValueObjects.Spouse>)ViewData["ValueObjectsList"])
       { %>
    <tr>
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.FirstName)%>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.FirstName, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.LastName)%>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.LastName, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.PlaceOfBirth)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.PlaceOfBirth.Name)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.DateOfBirth)%>
            </div>
            <div class="display-field">
                <%: Html.Encode(String.Format("{0:MM/dd/yyyy}", item.DateOfBirth))%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Nationality)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Nationality.Name)%>
            </div>
        </td>
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.ResidencyNo)%>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.ResidencyNo, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.ResidencyExpiryDate)%>
            </div>
            <div class="display-field">
                <%: Html.Encode(String.Format("{0:MM/dd/yyyy}", item.ResidencyExpiryDate))%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.PassportNo)%></div>
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
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.FirstContactNumber) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.FirstContactNumber, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.SecondContactNumber) %></div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.SecondContactNumber, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.EMail) %></div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.EMail, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.WorkEMail) %></div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.WorkEMail, new { @readonly = true, @class = "SingleLine" })%>
            </div>
        </td>
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.CompanyName) %></div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.CompanyName, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.JobTitle) %></div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.JobTitle, new { @readonly = true, @class = "SingleLine" })%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.WorkAddress) %>
            </div>
            <div class="display-field">
                <%: Html.TextAreaFor(model => item.WorkAddress, new { @readonly = true, @class = "MultiLine" })%>
            </div>
        </td>
        <td>
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <%: Ajax.ActionLink(Resources.Shared.Buttons.Function.Edit, "JsonEdit", "Spouse", new { Id = item.Id }, new AjaxOptions { OnComplete = "JsonEdit_OnComplete" })%>
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
