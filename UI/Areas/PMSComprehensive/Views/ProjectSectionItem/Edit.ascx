<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.PMS.ValueObjects.Implementation.Project.ProjectSectionItem>" %>

<table>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Weight) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Rate) %>
        </td>
    </tr>
        <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Comment) %>
        </td>
    </tr>
</table>
<fieldset class="ParentFieldset">
    <table width="100%">
        <tr>
            <td style="width: 33.3%; vertical-align: top">
                <%: Html.HiddenFor(model => model.Id) %>
            </td>
            <td style="width: 33.3%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Weight) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().PercentTextBox().Name("Weight").MinValue(0).MaxValue(100)%>
                </div>
            </td>
            <td style="width: 33.3%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Rate) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().PercentTextBox().Name("Rate").MinValue(0).MaxValue(100)%>
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



        <div class="editor-label">
            <%: Html.LabelFor(model => model.Phase) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Phase) %>
            <%: Html.ValidationMessageFor(model => model.Phase) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Role) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Role) %>
            <%: Html.ValidationMessageFor(model => model.Role) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.TaskDescription) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.TaskDescription) %>
            <%: Html.ValidationMessageFor(model => model.TaskDescription) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Weight) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Weight) %>
            <%: Html.ValidationMessageFor(model => model.Weight) %>
        </div>



