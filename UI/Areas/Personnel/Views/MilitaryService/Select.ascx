<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Personnel.ValueObjects.MilitaryService>" %>
<table width="100%">
    <% foreach (var item in (IEnumerable<HRIS.Domain.Personnel.ValueObjects.MilitaryService>)ViewData["ValueObjectsList"])
       { %>
    <tr>
        <td colspan="2" style="width: 100%; vertical-align: top">
            <table width="100%">
                <tr>
                    <td style="width: 20%; vertical-align: top">
                        <%: Html.HiddenFor(model => model.Id) %>
                        <div class="display-label">
                            <%: Html.LabelFor(model => model.MilitiryServiceNo) %>
                        </div>
                        <div class="display-field">
                            <%: Html.TextBoxFor(model => item.MilitiryServiceNo, new { @readonly = true, @class = "SingleLine" })%>
                        </div>
                        <div class="display-label">
                            <%: Html.LabelFor(model => model.ServiceStartDate) %>
                        </div>
                        <div class="display-field">
                            <%: Html.Encode(String.Format("{0:MM/dd/yyyy}", item.ServiceStartDate))%>
                        </div>
                        <div class="display-label">
                            <%: Html.LabelFor(model => model.Employee.MilitaryStatus) %>
                        </div>
                        <div class="display-field">
                            <%: Html.DisplayFor(model => item.Employee.MilitaryStatus.Name)%>
                        </div>
                    </td>
                    <td style="width: 20%; vertical-align: top">
                        <div class="display-label">
                            <%: Html.Label("Duration M/D") %>
                        </div>
                        <div class="display-label">
                            <%: Html.LabelFor(model => model.Months) %>
                        </div>
                        <div class="display-field">
                            <%: Html.DisplayFor(model => item.Months)%>
                        </div>
                        <div class="display-label">
                            <%: Html.LabelFor(model => model.Days) %>
                        </div>
                        <div class="display-field">
                            <%: Html.DisplayFor(model => item.Days)%>
                        </div>
                    </td>
                    <td style="width: 20%; vertical-align: top">
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
                        <%: Ajax.ActionLink(Resources.Shared.Buttons.Function.Edit, "JsonEdit", "MilitaryService", new { Id = item.Id }, new AjaxOptions { OnComplete = "JsonEdit_OnComplete" })%>
                        <% } %>
                    </td>
                </tr>
            </table>
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
