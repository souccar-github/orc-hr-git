<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Objectives.ValueObjects.ObjectiveConstraint>" %>
<table>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Description) %>
        </td>
    </tr>    
</table>
<fieldset class="ParentFieldset">
    <table width="100%">
        <tr>
            <td style="width: 100%; vertical-align: top">
                <%: Html.HiddenFor(model => model.Id) %>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Description) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextAreaFor(model => model.Description)%>
                </div>                
            </td>
        </tr> 
    </table>
</fieldset>
<table width="100%">
    <tr>
        <td style="width: 100%; vertical-align: top">
            <input type="submit" value="<%:Resources.Shared.Buttons.Function.Save  %>" onclick="DisableSaveButton()" />
        </td>
    </tr>
</table>
