<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.JobDesc.ValueObjects.JExperience>" %>
<%@ Import Namespace="UI.Areas.JobDesc.Helpers" %>
<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.Industry) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.CareerLevel) %>
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
                    <%: Html.LabelFor(model => model.Industry) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.Industry)%>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.CareerLevel) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.CareerLevel.Id)
                                      .BindTo(DropDownListHelpers.ListOfCareerLevel)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                    %>
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
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Required) %>
                </div>
                <div class="editor-field">
                    <%: Html.CheckBoxFor(model => model.Required) %>
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
