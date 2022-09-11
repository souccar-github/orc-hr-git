<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.JobDesc.Entities.JobDescription>" %>
<%@ Import Namespace="UI.Areas.OrganizationChart.Helpers" %>
<% using (Html.BeginForm("JsonInsert", "JobDescEntity"))
   {%>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%: Resources.Areas.JobDesc.Entities.JobDescription.JobDescriptionModel.CreatePageTitle %></legend>
    <table border="0" cellpadding="0" cellspacing="0" style="margin-left: 1px">
        <%--<tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.JobRole) %>
            </td>
        </tr>--%>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.JobTitle) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Summary) %>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td colspan="2" align="right">
                <input type="image" value="Save" src="<%= Url.Content("~/Content/Ribbon/Icons/48/37.png") %>"
                    title="<%: Resources.Shared.Buttons.Function.Save %>" alt="<%: Resources.Shared.Buttons.Function.Save %>" height="24" width="24" align="middle" />
            </td>
            <%: Html.HiddenFor(model => model.Id) %>
           <%-- <%: Html.HiddenFor(model => model.Specification.SingleOrDefault().Id) %>--%>
        </tr>
        <tr>
            <td colspan="2">
                <table border="0" cellpadding="0" cellspacing="0" width="80%">
                    <tr>
                        <td style="width: 33%; vertical-align: top;">
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.Summary) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.TextAreaFor(model => model.Summary)%>
                            </div>
                        </td>
                        <td style="width: 33%; vertical-align: top">
                            <%--<div class="editor-label-required">
                                <%: Html.LabelFor(model => model.JobRole) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().DropDownListFor(model => model.JobRole.Id)
                                      .BindTo(DropDownListHelpers.ListOfJobRole)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                %>
                            </div>--%>
                            <br />
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.JobTitle) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().DropDownListFor(model => model.JobTitle.Id)
                                             .BindTo(DropDownListHelpers.ListOfJobDescJobTitles(0))
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                %>
                            </div>
                        </td>
                        <td style="width: 33%; vertical-align: top">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="left">
                <a href="<%: Url.Action("Index", "JobDescEntity") %>">
                    <img src="<%: Url.Content("~/Content/Ribbon/Icons/48/90.png") %>" title="<%: Resources.Shared.Buttons.Function.BackToMainPage %>"
                        alt="<%: Resources.Shared.Buttons.Function.BackToMainPage %>" height="24" width="24" align="middle" />
                </a>
            </td>
            <td style="width: 10%; vertical-align: top" align="right">
                <input type="image" value="Save" src="<%= Url.Content("~/Content/Ribbon/Icons/48/37.png") %>"
                    title="<%: Resources.Shared.Buttons.Function.Save %>" alt="<%: Resources.Shared.Buttons.Function.Save %>" height="24" width="24" align="middle" />
            </td>
        </tr>
    </table>
</fieldset>
<% } %>
