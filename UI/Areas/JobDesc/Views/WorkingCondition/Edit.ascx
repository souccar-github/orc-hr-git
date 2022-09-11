<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.JobDesc.ValueObjects.WorkingCondition>" %>
<%@ Import Namespace="UI.Areas.JobDesc.Helpers" %>


<table border="0" cellpadding="0" cellspacing="0">    
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.InternalRelationships) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.ExternalRelationships) %>
        </td>
    </tr>    
</table>
<fieldset class="ParentFieldset">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <%: Html.HiddenFor(model => model.Id) %>
            <td style="width: 50%; vertical-align: top">                
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.InternalRelationships) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextAreaFor(model => model.InternalRelationships)%>
                </div>
                </td>
                <td style="width: 50%; vertical-align: top">  
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.ExternalRelationships) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextAreaFor(model => model.ExternalRelationships)%>
                </div>                   
            </td>
        </tr>
    </table>
</fieldset>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td style="width: 100%; vertical-align: top" align="right">
            <input type="image" value="Save" src="<%= Url.Content("~/Content/Ribbon/Icons/48/37.png") %>"
                title="<%: Resources.Shared.Buttons.Function.Save %>" alt="<%: Resources.Shared.Buttons.Function.Save %>" height="24" width="24" align="middle" />
        </td>
    </tr>
</table>
