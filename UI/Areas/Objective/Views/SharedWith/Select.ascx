<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Objectives.Entities.SharedWith>" %>
<%@ Import Namespace="HRIS.Domain.Objectives.Entities" %>
<%@ Import Namespace="HRIS.Domain.Objectives.ValueObjects" %>
<%@ Import Namespace="UI.Helpers.Views" %>
<table width="100%">
    <%
        foreach (var item in (IEnumerable<SharedWith>)ViewData["ValueObjectsList"])
        {%>
    <tr style="border-width: 11; border-color: Black; border-spacing: 11">
        <td style="width: 25%; vertical-align: top">

        </td>
        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%:Html.LabelFor(model => model.Position)%>
            </div>
            <div class="display-field">
                <%:Html.TextBoxFor(model => item.Position.JobTitle.Name, new ReadOnlyTextBox(true, "SingleLine"))%>
            </div>
        </td>

        <td style="width: 25%; vertical-align: top">
            <div class="display-label">
                <%:Html.LabelFor(model => model.Percentage)%>
            </div>
            <div class="display-field">
                <%:Html.TextBoxFor(model => item.Percentage, new ReadOnlyTextBox(true, "SingleLine"))%>
            </div>
        </td>
        <td>
            <%
            if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
            {%>
            <%:Ajax.ActionLink(Resources.Shared.Buttons.Function.Edit, "JsonEdit", "SharedWith", new {},
                                                  new AjaxOptions {OnComplete = "JsonEdit_OnComplete"})%>
            <%
            }%>
        </td>
    </tr>
    <%
        }%>
</table>
<script type="text/javascript">

    function JsonEdit_OnComplete(context) {

        var jsonEdit = context.get_response().get_object();
        if (jsonEdit.Success) {
            $("#addValueObjectArea").html(jsonEdit.PartialViewHtml);
            Toggle("edit");
        } else {
            $("#ValueObjectsList").html(window.jsonEdit.PartialViewHtml);
            $("#addValueObjectArea").slideToggle("fast");
        }
    }
    
</script>
