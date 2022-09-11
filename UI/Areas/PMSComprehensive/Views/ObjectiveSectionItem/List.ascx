<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.PMS.ValueObjects.Implementation.Objective.ObjectiveSectionItem>" %>
<%@ Import Namespace="HRIS.Domain.PMS.ValueObjects.Implementation.Objective" %>
<% using (Ajax.BeginForm("Save", "ObjectiveSectionItem", new AjaxOptions { OnComplete = "JsonAdd_OnComplete" }))
   {%>

<script type="text/javascript">
    function JsonAdd_OnComplete(context) {
        var JsonAdd = context.get_response().get_object();
        if (JsonAdd.Success) {
            location.reload();
        } else {
            $("#ValueObjectsList").html(JsonAdd.PartialViewHtml);
            Toggle("add");
        }
    }
</script>

<table width="100%">
    <tr>
        <td> 
            <input type="button" value="Cancel" onclick="CancelButton()" class="CancelButton" />
        </td>
    </tr>
</table>

<div id="addValueObjectArea" style="width: auto">   
    <% 
       if (ViewData["ValueObjectsList"] != null & ((IEnumerable<ObjectiveSectionItem>)ViewData["ValueObjectsList"]).Count() == 0)
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
        $('#addValueObjectArea').load('<%: Url.Action("Load", "ObjectiveSectionItem") %>');

        Toggle("add");
    }
</script>