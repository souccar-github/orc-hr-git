<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.JobDesc.ValueObjects.ComputerSkill>" %>
<%@ Import Namespace="UI.Areas.JobDesc.Helpers" %>
<%@ Import Namespace="UI.Areas.Personnel.Helpers" %>
<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Type) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Level) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Weight) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Required) %>
        </td>
    </tr>
</table>
<fieldset class="ParentFieldset">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <%: Html.HiddenFor(model => model.Id) %>
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Type) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.Type.Id)
                                      .BindTo(UI.Areas.JobDesc.Helpers.DropDownListHelpers.ListOfComputerSkillType)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                    %>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Level) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.Level.Id)
                                      .BindTo(UI.Areas.Personnel.Helpers.PersonnelDropDownListHelpers.ListOfLevels)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                    %>                  
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Required) %>
                </div>
                <div class="editor-field">
                    <%: Html.CheckBoxFor(model => model.Required) %>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Weight) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().NumericTextBox().Name("Weight").MinValue(0).MaxValue(100)%>                    
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
