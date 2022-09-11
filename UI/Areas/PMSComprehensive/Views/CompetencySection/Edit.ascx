<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.PMS.ValueObjects.Implementation.Competency.CompetencySection>" %>
<table>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.FinalSubmit) %>
        </td>
    </tr>
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
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.TotalRate) %>
        </td>
    </tr>
</table>
<fieldset class="ParentFieldset">
    <table width="100%">
        <tr>
            <td style="width: 25%; vertical-align: top">
                <%: Html.HiddenFor(model => model.Id) %>
                <%: Html.HiddenFor(model => model.Name) %>
                <%: Html.HiddenFor(model => model.Weight) %>
                <%: Html.HiddenFor(model => model.TotalRate) %>

                <div class="display-label">
                    <%: Html.LabelFor(model => model.Name) %>
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.Name)%>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Weight) %>
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.Weight)%>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="display-label">
                    <%: Html.LabelFor(model => model.TotalRate) %>
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.TotalRate)%>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.FinalSubmit)%>
                </div>
                <div class="editor-field">
                    <%: Html.CheckBoxFor(model => model.FinalSubmit)%>
                </div>
            </td>
        </tr>
    </table>
</fieldset>
<table width="100%">
    <tr>
        <td style="width: 10%; vertical-align: top">
            <input type="submit" value="Save" onclick="DisableSaveButton()" />
        </td>
    </tr>
</table>
