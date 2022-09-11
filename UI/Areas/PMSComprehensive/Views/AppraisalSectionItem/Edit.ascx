<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.PMS.ValueObjects.TemplateSectionItem>" %>
<%@ Import Namespace="UI.Areas.PMSComprehensive.Helpers" %>
<table>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Name) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Weight) %>
        </td>
    </tr>      
</table>
<fieldset class="ParentFieldset">
    <table width="100%">
        <tr>
            <%: Html.HiddenFor(model => model.Id) %>
            <td style="width: 33%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Name) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model=>model.Name)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Weight) %>
                </div> 
                <div class="editor-field">
                    <%: Html.Telerik().NumericTextBox().Name("Weight").MinValue(0).MaxValue(float.MaxValue)%>
                </div>
            </td>
            <td style="width: 33%; vertical-align: top">
                <div class="editor-label">
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
        <td style="width: 100%; vertical-align: top" align="right">
            <input type="submit" value=<%:Resources.Shared.Buttons.Function.Save %> onclick="DisableSaveButton()" />
        </td>
    </tr>
</table>
