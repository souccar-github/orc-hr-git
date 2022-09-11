<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Personnel.ValueObjects.Education>" %>
<%@ Import Namespace="HRIS.Domain.Personnel.ValueObjects" %>
<% using (Ajax.BeginForm("Save", "Education", new AjaxOptions { OnComplete = "JsonAdd_OnComplete" }))
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
<table>
    <tr>
        <td>
            <input onclick="cancel()" type="button" value="<%: Resources.Shared.Buttons.Function.Cancel %>" />
            <script type="text/javascript">
                function cancel() {
                    $('#result').fadeOut('fast', function () {
                        $('#result').empty();
                    });
                }
            </script>
        </td>
    </tr>
</table>
<div id="addValueObjectArea" style="width: auto">
    <% 
       if (ViewData["ValueObjectsList"] != null & ((IEnumerable<Education>)ViewData["ValueObjectsList"]).Count() == 0)
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
        $('#addValueObjectArea').load('<%: Url.Action("Load", "Education") %>');

        Toggle("add");
    }
</script>
