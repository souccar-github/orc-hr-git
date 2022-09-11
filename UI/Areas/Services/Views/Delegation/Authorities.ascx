<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<UI.Areas.Services.DTO.ViewModels.AuthorityViewModel>>" %>
<div style="height: 200px; overflow: auto">
    <% if (Model.Count > 0)
       {%>
    <div class="editor-label-required">
        <%: Html.CheckBox("CheckAllAuthorities", new { onclick = "AllAuthoritiesChecked()" })%>
        <%: Resources.Areas.Services.Delegation.Messages.SelectAllAuthoritiesCheckBox%>
    </div>
    <% for (int i = 0; i < Model.Count(); i++)
       {%>
    <%: Html.CheckBox("chkAuthorities_" + Model[i].Id, Model[i].Checked, new { onchange = "AuthorityChecked(checked," + Model[i].Id + ");" })%>
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
    function AllAuthoritiesChecked() 
    {
        if ($('#CheckAllAuthorities').attr('checked') == true) 
        {
            <% foreach (var item in Model)
            {%>
                $("#chkAuthorities_"+<%:item.Id %>).attr('checked', true);
                AuthorityChecked(true,<%:item.Id %>);
           <%}%>
        }
        else 
        {
            <% foreach (var item in Model)
            {%>
                $("#chkAuthorities_"+<%:item.Id %>).attr('checked', false);
                AuthorityChecked(false,<%:item.Id %>);
            <% } %>
        }
    }
    

    function AuthorityChecked(checkState,authorityId) {
    var data = JSON.parse(document.getElementById("hiddenAuthorities").value);
    for(var i = 0; i < data.length; i++)
        {
            if(data[i].Id==authorityId)
            {
                data[i].Checked = checkState;
            }
        }
    document.getElementById("hiddenAuthorities").value = JSON.stringify(data);
    }
</script>
