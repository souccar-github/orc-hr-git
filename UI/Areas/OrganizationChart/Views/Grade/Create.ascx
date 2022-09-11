<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.OrgChart.Entities.Grade>" %>
<%@ Import Namespace="UI.Areas.OrganizationChart.Helpers" %>

<% using (Html.BeginForm("JsonInsert", "Grade"))
   {%>

<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%: Resources.Areas.OrgChart.Entities.Grade.GradeModel.InsertGradeBasicDetailsTitle %></legend>
    <table border="0" cellpadding="0" cellspacing="0">
         <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Level) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Name) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Step) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.MinSalary) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.MidPointSalary) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.MaxSalary) %>
            </td>
        </tr>
    </table>
    <table width="100%" style="vertical-align: top">
        <tr>
            <td style="width: 50%; vertical-align: top">
                <div class="editor-label">
                    <%: Html.HiddenFor(model => model.Id) %>
                </div>
            </td>
            <td style="width: 50%; vertical-align: top" align="right">
                <input type="image" value="Save" src="<%= Url.Content("~/Content/Ribbon/Icons/48/37.png") %>"
                    title=<%: Resources.Shared.Buttons.Function.Save %> alt=<%: Resources.Shared.Buttons.Function.Save %> height="24" width="24" align="middle" />
            </td>
        </tr>
        <tr>
            <td style="width: 100%; vertical-align: top" colspan="2">
                <table border="0" cellpadding="0" cellspacing="0" width="80%">
                    <tr>
                          <td style="width: 33%; vertical-align: top">
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.Level) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().DropDownListFor(model => model.Level.Id)
                           .BindTo(DropDownListHelpers.ListOfOrganizationalLevel)
                           .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                %>
                            </div>
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.Name) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().DropDownListFor(model => model.Name.Id)
                                      .BindTo(DropDownListHelpers.ListOfGradeName)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                %>
                            </div>
                             <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.Step) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().DropDownListFor(model => model.Step.Id)
                                      .BindTo(DropDownListHelpers.ListOfGradeStepName)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                %>
                            </div>
                           
                        </td>
                        <td style="width: 33%; vertical-align: top;">
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.MinSalary) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().NumericTextBox().Name("MinSalary").MinValue(0).MaxValue(float.MaxValue)%>
                            </div>
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.MidPointSalary) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().NumericTextBox().Name("MidPointSalary").MinValue(0).MaxValue(float.MaxValue)%>
                            </div>
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.MaxSalary) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().NumericTextBox().Name("MaxSalary").MinValue(0).MaxValue(float.MaxValue)%>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="left">
                <a href="<%: Url.Action("Index", "Grade") %>">
                    <img src="<%: Url.Content("~/Content/Ribbon/Icons/48/90.png") %>" title=<%: Resources.Shared.Buttons.Function.BackToMainPage %>
                        alt=<%: Resources.Shared.Buttons.Function.BackToMainPage %> height="24" width="24" align="middle" />
                </a>
            </td>
            <td style="width: 10%; vertical-align: top" align="right">
                <input type="image" value="Save" src="<%= Url.Content("~/Content/Ribbon/Icons/48/37.png") %>"
                    title=<%: Resources.Shared.Buttons.Function.Save %> alt=<%: Resources.Shared.Buttons.Function.Save %> height="24" width="24" align="middle" />
            </td>
        </tr>
    </table>
</fieldset>
<% } %>

