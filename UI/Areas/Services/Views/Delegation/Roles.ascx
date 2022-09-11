<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<UI.Areas.Services.DTO.ViewModels.RoleViewModel>>" %>
<div style="height: 200px; overflow: auto">
    <% if (Model.Count > 0)
       {%>
    <div class="editor-label-required">
        <%: Html.CheckBox("CheckAllRoles", new { onclick = "AllRolesChecked()" })%>
        <%: Resources.Areas.Services.Delegation.Messages.SelectAllRolesCheckBox%>
    </div>
    <% for (int i = 0; i < Model.Count(); i++)
       {%>
    <%: Html.CheckBox("chkRoles_" + Model[i].Id, Model[i].Checked, new { onchange = "RoleChecked(checked," + Model[i].Id + ");" })%>
    <%: Html.DisplayFor(x => Model[i].Id)%>-
    <%: Html.DisplayFor(x => Model[i].Name)%>
    <br />
    <%}%>
    <%}
       else
       {%>
    <%: Resources.Areas.Services.Delegation.Messages.PleaseSelectPositionMsg%>
    <%} %>
</div>
<script type="text/javascript">
    function AllRolesChecked() {
        if ($('#CheckAllRoles').attr('checked') == true) 
        {
        <% foreach (var item in Model)
            {%>
                $("#chkRoles_"+<%:item.Id %>).attr('checked', true);
                RoleChecked(true,<%:item.Id %>);
           <%}%>
        }
        else 
        {
            <% foreach (var item in Model)
            {%>
                $("#chkRoles_"+<%:item.Id %>).attr('checked', false);
                RoleChecked(false,<%:item.Id %>);
            <% } %>
        }
    }

    function RoleChecked(checkState,roleId) {
    var data = JSON.parse(document.getElementById("hiddenRoles").value);
        for(var i = 0; i < data.length; i++)
        {
            if(data[i].Id==roleId)
            {
                data[i].Checked = checkState;
            }
        }
    document.getElementById("hiddenRoles").value = JSON.stringify(data);
    }
</script>
