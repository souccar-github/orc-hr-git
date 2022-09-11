<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Personnel.ValueObjects.Language>" %>
<%@ Import Namespace="UI.Areas.Personnel.Helpers" %>
<table>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Name) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Reading) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Writing) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Listening) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Speaking) %>
        </td>
    </tr>
</table>
<br />
<fieldset class="ParentFieldset">
    <table width="100%">
        <tr>
            <td style="width: 20%; vertical-align: top">
                <div class="editor-label">
                    <%: Html.HiddenFor(model => model.Id) %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Name) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.Name) %>
                </div>
            </td>
            <td style="width: 20%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Reading) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.Reading.Id)
                                      
                                      .BindTo(PersonnelDropDownListHelpers.ListOfLevels)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                    %>
                </div>
            </td>
            <td style="width: 20%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Writing) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.Writing.Id)
                                      
                                      .BindTo(PersonnelDropDownListHelpers.ListOfLevels)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                    %>
                </div>
            </td>
            <td style="width: 20%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Listening) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.Listening.Id)
                                      
                                      .BindTo(PersonnelDropDownListHelpers.ListOfLevels)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                    %>
                </div>
            </td>
            <td style="width: 20%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Speaking) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.Speaking.Id)
                                      
                                      .BindTo(PersonnelDropDownListHelpers.ListOfLevels)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                    %>
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
