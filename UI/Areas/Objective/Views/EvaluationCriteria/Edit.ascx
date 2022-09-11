<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Objectives.ValueObjects.EvaluationCriteria>" %>
<table style="margin-left: 1px">
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Below)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Meet)%>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Above)%>
        </td>
    </tr>
</table>
<fieldset class="ParentFieldset">
    <table width="100%">
        <tr>
            <td style="width: 33%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Below)%>
                </div>
                <div class="editor-field">
                    <%: Html.TextAreaFor(model => model.Below)%>
                </div>
            </td>
            <td style="width: 33%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Meet)%>
                </div>
                <div class="editor-field">
                    <%: Html.TextAreaFor(model => model.Meet)%>
                </div>
            </td>
            <td style="width: 33%; vertical-align: top">
                <%: Html.HiddenFor(model => model.Id) %>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Above)%>
                </div>
                <div class="editor-field">
                    <%: Html.TextAreaFor(model => model.Above)%>
                </div>
            </td>
        </tr>
    </table>
</fieldset>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td style="width: 10%; vertical-align: top" align="right">
            <input type="submit" value="<%:Resources.Shared.Buttons.Function.Save %>" onclick="DisableSaveButton()" />
        </td>
    </tr>
</table>
