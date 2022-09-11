<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.Objectives.Entities.SharedWith>" %>

<table>

    <tr>
        <td>
            <%:Html.ValidationMessageFor(model => model.Percentage)%>
        </td>
    </tr>
</table>
<fieldset class="ParentFieldset">
    <table width="100%" style="vertical-align: top">
        <tr>
            <td>

            <%: Html.HiddenFor(model => model.Id) %>
                <div class="editor-label-required">
                    <%:Html.LabelFor(model => model.Percentage)%>
                </div>
                <div class="editor-field">
                    <%:Html.Telerik().PercentTextBox().Name("Percentage").MinValue(0).MaxValue(100)%>
                </div>
                <div class="editor-label-required">
                    <%:Html.LabelFor(model => model.Position)%>
                </div>
                <div id="nodePositions">
                    <fieldset style="height: auto; width: 300px">

                        <%:Html.Telerik().ComboBoxFor(model => model.Position.Id)                                 
                                  .HtmlAttributes(new {style = string.Format("width:{0}px", 300)})                                  

                                .BindTo( UI.Areas.OrganizationChart.Helpers.DropDownListHelpers.ListOfPositions)
                        %>



                    </fieldset>
                </div>
            </td>
        </tr>
    </table>
</fieldset>
<table width="100%">
    <tr>
        <td style="width: 100%; vertical-align: top">
            <input type="submit" value="<%:Resources.Shared.Buttons.Function.Save  %>" onclick="DisableSaveButton()" />
        </td>
    </tr>
</table>
