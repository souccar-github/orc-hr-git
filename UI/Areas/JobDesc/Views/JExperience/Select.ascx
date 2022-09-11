<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.JobDesc.ValueObjects.JExperience>" %>
<%@ Import Namespace="HRIS.Domain.JobDesc.ValueObjects" %>
<%@ Import Namespace="Infrastructure.Validation" %>
<div style="color: Maroon; font-size: smaller;">
    <%
        if (ViewData["ExpiredRules"] != null)
        {
            foreach (BrokenBusinessRule brokenBusinessRule in ViewData["ExpiredRules"] as IList<BrokenBusinessRule>)
            {%>
    <%:Html.DisplayTextFor(model => brokenBusinessRule.Rule)%>
    <br />
    <%
            }
        }%>
    <br />
</div>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <% foreach (var item in (IEnumerable<JExperience>)ViewData["ValueObjectsList"])
       { %>
    <tr style="border-width: 11; border-color: Black; border-spacing: 11">
        <td style="width: 20%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Industry) %>
            </div>
            <div class="display-field">
                <%: Html.TextBoxFor(model => item.Industry, new { @readonly = true, @class = "SingleLine" })%>
            </div>
        </td>
        <td style="width: 20%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.CareerLevel) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.CareerLevel.Name) %>
            </div>
        </td>
        <td style="width: 20%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Weight) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Weight) %>
            </div>
        </td>
        <td style="width: 20%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Required) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Required) %>
            </div>
        </td>
        <td style="width: 20%; vertical-align: top">
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <%: Ajax.ActionLink(Resources.Shared.Buttons.Function.Edit, "JsonEdit", "JExperience", new { }, new AjaxOptions { OnComplete = "JsonEdit_OnComplete" })%>
            <% } %>
        </td>
    </tr>
    <% } %>
</table>
<script type="text/javascript">

    function JsonEdit_OnComplete(context) {

        var JsonEdit = context.get_response().get_object();
        if (JsonEdit.Success) {
            $("#addValueObjectArea").html(JsonEdit.PartialViewHtml);
            Toggle("edit");
        }
        else {
            $("#ValueObjectsList").html(JsonAdd.PartialViewHtml);
            $("#addValueObjectArea").slideToggle("fast");
        }
    };

</script>
