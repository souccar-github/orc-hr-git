<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.JobDesc.ValueObjects.JLanguage>" %>
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
    <% foreach (var item in (IEnumerable<JLanguage>)ViewData["ValueObjectsList"])
       { %>
    <tr style="border-width: 11; border-color: Black; border-spacing: 11">
        <td style="width: 33.3%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Name) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model =>item.Name.Name)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Required) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Required) %>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Weight) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Weight)%>
            </div>
        </td>
        <td style="width: 33.3%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Reading) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model =>item.Reading.Name)%>
            </div>
            <div class="display-label-required">
                <%: Html.LabelFor(model => model.Writing) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Writing.Name)%>
            </div>
        </td>
        <td style="width: 33.3%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Speaking) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Speaking.Name) %>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Listening) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Listening.Name) %>
            </div>
        </td>
        <td align="right">
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <%: Ajax.ActionLink(Resources.Shared.Buttons.Function.Edit, "JsonEdit", "JLanguage", new { Id = item.Id }, new AjaxOptions { OnComplete = "JsonEdit_OnComplete" })%>
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
