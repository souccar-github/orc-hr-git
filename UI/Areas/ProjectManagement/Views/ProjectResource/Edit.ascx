<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.ProjectManagment.ValueObjects.ProjectResource>" %>
<%@ Import Namespace="UI.Areas.ProjectManagement.Helpers" %>
<table>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Name)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Type)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Status)%>
        </td> 
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Constraints)%>
        </td>
    </tr>    
</table>
<fieldset class="ParentFieldset">
    <table width="100%">
        <tr>
            <td style="width: 50%; vertical-align: top">
                <%: Html.HiddenFor(model => model.Id) %>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Name) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.Name) %>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.Description)%>
                </div>
                <div class="editor-field">
                    <%: Html.TextAreaFor(model => model.Description)%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Type) %>
                </div>
                <div class="editor-field">
                    <%:Html.Telerik().DropDownListFor(model => model.Type.Id)
                              .BindTo(DropDownListHelpers.ListOfProjectResourceType)
                              .HtmlAttributes(new {style = string.Format("width:{0}px", 200)})%>
                </div>
                
            </td>
            <td style="width: 50%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Status) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.Status.Id)
                            .BindTo(DropDownListHelpers.ListOfResourceStatus)
                            .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })                                     
                    %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Constraints)%>
                </div>
                <div class="editor-field">
                    <%: Html.TextAreaFor(model => model.Constraints)%>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.Comments)%>
                </div>
                <div class="editor-field">
                    <%: Html.TextAreaFor(model => model.Comments)%>
                </div>
            </td>
        </tr>
    </table>
</fieldset>
<table width="100%">
    <tr>
        <td style="width: 100%; vertical-align: top" align="right">
            <input type="submit" value="<%: Resources.Shared.Buttons.Function.Save %>" onclick="DisableSaveButton()" />
        </td>
    </tr>
</table>
