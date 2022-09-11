<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.JobDesc.ValueObjects.Role>" %>
<%@ Import Namespace="UI.Areas.JobDesc.Helpers" %>
<table>
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
            <%: Html.ValidationMessageFor(model => model.Priority) %>
        </td>
    </tr>
    <tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.JobTitle) %>
        </td>
    </tr>
    <%--<tr>
        <td>
            <%: Html.ValidationMessageFor(model => model.JobRole) %>
        </td>
    </tr>--%>
</table>
<fieldset class="ParentFieldset">
    <table width="100%">
        <tr>
            <td style="width: 33.3%; vertical-align: top">
                <%: Html.HiddenFor(model => model.Id) %>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Name) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.Name) %>
                </div>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Weight) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().PercentTextBox().Name("Weight").MinValue(0).MaxValue(100)%>
                </div>
            </td>
            <td style="width: 33.3%; vertical-align: top">
                <fieldset class="ParentFieldset">
                    <legend><%: Resources.Areas.JobDesc.ValueObjects.Role.RoleModel.DelegatedToLegendTitle %></legend>
                    <div class="editor-label-required">
                        <%: Html.LabelFor(model => model.JobTitle) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.Telerik().DropDownListFor(model => model.JobTitle.Id)
                                      
                                      .BindTo(UI.Areas.OrganizationChart.Helpers.DropDownListHelpers.ListOfJobTitle)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                     
                        %>
                    </div>
                    <%--<div class="editor-label-required">
                        <%: Html.LabelFor(model => model.JobRole) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.Telerik().DropDownListFor(model => model.JobRole.Id)
                                     
                                       .BindTo(UI.Areas.OrganizationChart.Helpers.DropDownListHelpers.ListOfJobRole)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                      
                        %>
                    </div>--%>
                </fieldset>
            </td>
            <td style="width: 33.3%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Priority) %>
                </div>
                <div class="editor-field">
                    <%: Html.Telerik().DropDownListFor(model => model.Priority.Id)
                                      
                                       .BindTo(DropDownListHelpers.ListOfPriorities)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                      
                    %>
                </div>
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.Summary) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextAreaFor(model => model.Summary)%>
                </div>
            </td>
        </tr>
    </table>
</fieldset>
<table width="100%">
    <tr>
        <td style="width: 100%; vertical-align: top" align="right">
            <input type="image" value="Save" src="<%= Url.Content("~/Content/Ribbon/Icons/48/37.png") %>"
                title="<%: Resources.Shared.Buttons.Function.Save %>" alt="<%: Resources.Shared.Buttons.Function.Save %>" height="24" width="24" align="middle" />
        </td>
    </tr>
</table>
