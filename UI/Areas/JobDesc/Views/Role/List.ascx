<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.JobDesc.ValueObjects.Role>" %>
<%@ Import Namespace="HRIS.Domain.JobDesc.ValueObjects" %>
<% using (Ajax.BeginForm("Save", "Role", new AjaxOptions { OnComplete = "JsonAdd_OnComplete" }))
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
        <td align="left">
            <img onclick="cancel()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/90.png") %>"
                title="<%: Resources.Shared.Buttons.Function.Cancel %>" alt="<%: Resources.Shared.Buttons.Function.Cancel %>" height="24" width="24" align="middle" />
            <script type="text/javascript">
                function cancel() {
                    $('#result').fadeOut('slow', function () {
                        $('#result').empty();
                    });
                }
            </script>
        </td>
        <td align="right">
            <%: Html.ActionLink(Resources.Shared.Buttons.Function.GoToDetails , "GoToRoles", "JobDescEntity")%>
        </td>
    </tr>
</table>
<div id="addValueObjectArea" style="width: auto">
    <% 
       if (ViewData["ValueObjectsList"] != null & ((IEnumerable<Role>)ViewData["ValueObjectsList"]).Count() == 0)
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
<% } %>
<% if (ViewData["ValueObjectsList"] != null)
   {
       Html.RenderPartial("Select");
   } %>
<input id="toggleStatus" style="width: 0; border-width: 0; background-color: White;
    visibility: hidden" />
<input id="toggleFiredBy" style="width: 0; border-width: 0; background-color: White;
    visibility: hidden" />
<script type="text/javascript">

    function add() {
        $('#addValueObjectArea').load('<%: Url.Action("Load", "Role") %>');

        Toggle("add");
    }

</script>
