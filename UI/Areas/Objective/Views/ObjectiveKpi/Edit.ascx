<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Objectives.Entities.ObjectiveKpi>" %>
<%@ Import Namespace="UI.Areas.Objective.Helpers" %>
<table>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Type)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Value)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Weight)%>
        </td>
    </tr>
        <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Id)%>
        </td>
    </tr>
        <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Description)%>
        </td>
    </tr>
</table>
<fieldset class="ParentFieldset">
    <table width="100%">
        <tr>
            <td style="width: 33%; vertical-align: top">
                <%: Html.HiddenFor(model => model.Id) %>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Type) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.Type.Id)
                            .BindTo(DropDownListHelpers.ListOfObjectiveKpiType)
                            .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                     
                    %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Value)%>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().IntegerTextBoxFor(model => model.Value).MinValue(0).MaxValue(100)%>
                </div>
            </td>
            <td style="width: 33%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Weight)%>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().NumericTextBoxFor(model => model.Weight)%>
                </div>
            </td>
            <td style="width: 33%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Description)%>
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
            <input type="submit" value="<%:Resources.Shared.Buttons.Function.Save  %>" onclick="DisableSaveButton()" />
        </td>
    </tr>
</table>
