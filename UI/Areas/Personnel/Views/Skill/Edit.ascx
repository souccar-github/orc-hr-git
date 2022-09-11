<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Personnel.ValueObjects.Skill>" %>
<%@ Import Namespace="UI.Areas.Personnel.Helpers" %>
<table>
    <tr>
        <td>
            <%= Html.ValidationMessageFor(model => model.Name)%>
        </td>
    </tr>
    <tr>
        <td>
            <%= Html.ValidationMessageFor(model => model.Level)%>
        </td>
    </tr>
    <tr>
        <td>
            <%= Html.ValidationMessageFor(model => model.Description) %>
        </td>
    </tr>
    <tr>
        <td>
            <%= Html.ValidationMessageFor(model => model.Comments) %>
        </td>
    </tr>
</table>
<br />
<fieldset class="ParentFieldset">
    <table width="100%">
        <tr>
            <td style="width: 25%; vertical-align: top">
                <%= Html.HiddenFor(model => model.Id) %>
                <div class="editor-label-required">
                    <%= Html.LabelFor(model => model.Name) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.Name.Id)
                                      
                                      .BindTo(PersonnelDropDownListHelpers.ListOfSkillTypes)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                    %>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label-required">
                    <%= Html.LabelFor(model => model.Level) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.Level.Id)
                                      
                                      .BindTo(PersonnelDropDownListHelpers.ListOfLevels)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                    %>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label">
                    <%= Html.LabelFor(model => model.Description) %>
                </div>
                <div class="editor-field">
                    <%= Html.TextAreaFor(model => model.Description)%>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label">
                    <%= Html.LabelFor(model => model.Comments) %>
                </div>
                <div class="editor-field">
                    <%= Html.TextAreaFor(model => model.Comments) %>
                </div>
            </td>
        </tr>
    </table>
</fieldset>
<table>
    <tr>
        <td>
            <input type="submit" value="<%: Resources.Shared.Buttons.Function.Save %>" onclick=" DisableSaveButton(); " />
        </td>
    </tr>
</table>
