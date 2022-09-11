<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.PMS.ValueObjects.Implementation.Objective.ObjectiveSection>" %>
<%@ Import Namespace="UI.Areas.PMSComprehensive.Helpers" %>
<table>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Name) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.FinalSubmit) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Weight) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.TotalRate) %>
        </td>
    </tr>
</table>
<fieldset class="ParentFieldset">
    <table width="100%">
        <tr>
            <td style="width: 33.3%; vertical-align: top">
                <%: Html.HiddenFor(model => model.Id) %>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Name) %>
                </div>
                <div class="display-field">
                    <%: Html.TextBoxFor(model => model.Name, new { @readonly = true, @class = "SingleLine" })%>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.FinalSubmit)%>
                </div>
                <div class="editor-field">
                    <%: Html.CheckBoxFor(model => model.FinalSubmit)%>
                </div>
            </td>
            <td style="width: 33.3%; vertical-align: top">
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Weight) %>
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.Weight)%>
                </div>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.TotalRate) %>
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.TotalRate)%>
                </div>
            </td>
        </tr>
    </table>
</fieldset>
<table width="100%">
    <tr>
        <td style="width: 100%; vertical-align: top" align="right">
            <input type="submit" value="Save" onclick="DisableSaveButton()" />
        </td>
    </tr>
</table>
