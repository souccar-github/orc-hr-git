<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Objectives.Entities.SharedWith>" %>
<%@ Import Namespace="HRIS.Domain.Objectives.Entities" %>
<%@ Import Namespace="HRIS.Domain.Objectives.ValueObjects" %>
<%
    using (Ajax.BeginForm("Save", "SharedWith", new AjaxOptions { OnComplete = "JsonAdd_OnComplete" }))
    {%>
<script type="text/javascript">
    function JsonAdd_OnComplete(context) {
        var jsonAdd = context.get_response().get_object();
        if (jsonAdd.Success) {
            location.reload();
        } else {
            $("#ValueObjectsList").html(jsonAdd.PartialViewHtml);
            Toggle("add");
        }
    }
</script>
<table width="100%">
    <tr>
        <td>
            <input type="button" value="<%:Resources.Shared.Buttons.Function.Cancel  %>" onclick="CancelButton()" class="CancelButton" />
        </td>
    </tr>
</table>
<div id="addValueObjectArea" style="width: auto">
    <%
        if (ViewData["ValueObjectsList"] != null & ((IEnumerable<SharedWith>)ViewData["ValueObjectsList"]).Count() == 0)
        {
            if (Model != null)
            {
                Html.RenderPartial("Edit");
            }
            else
            {
                Html.RenderAction("Load");
            }
        }
        else
        {
            if (Model != null)
            {
                Html.RenderPartial("Edit");
            }
        }
    %>
</div>
<%
    }%>
<%
    if (ViewData["ValueObjectsList"] != null)
    {
        Html.RenderPartial("Select");
    }%>
<input id="toggleStatus" style="width: 0; border-width: 0; background-color: White;
    visibility: hidden" />
<input id="toggleFiredBy" style="width: 0; border-width: 0; background-color: White;
    visibility: hidden" />
<script type="text/javascript">

    function add() {
        $('#addValueObjectArea').load('<%:Url.Action("Load", "SharedWith")%>');

        Toggle("add");
    }

</script>
