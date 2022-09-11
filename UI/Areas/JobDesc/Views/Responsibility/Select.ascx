<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.JobDesc.ValueObjects.Responsibility>" %>
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
    <% foreach (var item in (IEnumerable<Responsibility>)ViewData["ValueObjectsList"])
       { %>
    <tr style="border-width: 11; border-color: Black; border-spacing: 11">        
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Priority) %>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Priority.Name)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Weight)%>
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => item.Weight)%>
            </div>
        </td>
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%: Html.LabelFor(model => model.Description) %>
            </div>
            <div class="display-field">
                <%: Html.TextAreaFor(model => item.Description, new { @readonly = true, @class = "MultiLine", @disapled = true })%>
            </div>
        </td>
        <td style="width: 25%; vertical-align: top">
            <fieldset class="ParentFieldset">
                <legend><%: Resources.Areas.JobDesc.ValueObjects.Role.RoleModel.DelegatedToLegendTitle %></legend>
                <%--<div class="display-label">
                    <%: Html.LabelFor(model => model.JobRole) %>
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => item.JobRole.Name)%>
                </div>--%>
                <br />
                <div class="display-label">
                    <%: Html.LabelFor(model => model.JobTitle) %>
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => item.JobTitle.Name)%>
                </div>
            </fieldset>
        </td>
        <td align="right">
            <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
               {%>
            <%: Ajax.ActionLink(Resources.Shared.Buttons.Function.Edit, "JsonEdit", "Responsibility", new { Id = item.Id }, new AjaxOptions { OnComplete = "JsonEdit_OnComplete" })%>
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
