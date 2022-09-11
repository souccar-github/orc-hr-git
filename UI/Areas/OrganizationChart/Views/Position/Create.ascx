<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<UI.Areas.OrganizationChart.DTO.ViewModels.PositionViewModel>" %>

<%@ Import Namespace="Souccar.Core" %>
<%@ Import Namespace="UI.Areas.OrganizationChart.Helpers" %>
<% using (Ajax.BeginForm("JsonInsert", "Position", new AjaxOptions { OnComplete = "JsonAdd_OnComplete" }))
   {%>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend">
        <%:Resources.Areas.OrgChart.ValueObjects.Position.PositionModel.PositionBasicInformationTitle %></legend>
    <script type="text/javascript">

        function JsonAdd_OnComplete(context) {

            var JsonAdd = context.get_response().get_object();
            if (JsonAdd.Success) {
                $("#result").html(JsonAdd.PartialViewHtml);
                location.reload();
            } else {
                $("#result").html(JsonAdd.PartialViewHtml);
            }
        }
    </script>
    <table border="0" cellpadding="0" cellspacing="0" style="margin-left: 1px">
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.GradeId) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Position.Status) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Position.Code) %>
                 <%: Html.ValidationMessage("DupLicateCode")%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Position.Type)%>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Position.Level) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Position.JobTitle) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Position.PositionReportings) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Position.CostCenter) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Position.Budget) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Position.WorkingHours) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Position.Per) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Position.DisabilityStatus) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessage(DomainErrors.InternalError.ToString())%>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td colspan="4" align="right">
                <input type="image" value="<%: Resources.Shared.Buttons.Function.Save %>" src="<%= Url.Content("~/Content/Ribbon/Icons/48/37.png") %>"
                    title="<%: Resources.Shared.Buttons.Function.Save %>" alt="<%: Resources.Shared.Buttons.Function.Save %>"
                    height="24" width="24" align="middle" />
            </td>
        </tr>
        <tr>
            <td colspan="5" style="width: 100%">
                <table width="100%" style="vertical-align: middle">
                    <tr>
                        <td style="width: 25%; vertical-align: top">
                            <%: Html.HiddenFor(model => model.Position.Id) %>
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.Position.Code) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.TextBoxFor(model => model.Position.Code)%>
                            </div>
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.Position.JobTitle)%>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().DropDownListFor(model => model.Position.JobTitle.Id)
                                                    .BindTo(DropDownListHelpers.ListOfJobTitle)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                %>
                            </div>
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.Position.Grades)%>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().DropDownListFor(model => model.GradeId)
                                          .BindTo(DropDownListHelpers.ListOfGradeSystems)
                                          .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                %>
                            </div>
                        </td>
                        <td style="width: 25%; vertical-align: top">
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.Position.Type) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().DropDownListFor(model => model.Position.Type.Id)
                                      .BindTo(DropDownListHelpers.ListOfPositionType)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                %>
                            </div>
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.Position.Level) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().DropDownListFor(model => model.Position.Level.Id)
                                      .BindTo(DropDownListHelpers.ListOfPositionLevel)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                %>
                            </div>
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.Position.PositionReportings)%>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().DropDownListFor(model => model.ParentPositionId)
                                                                  .BindTo(DropDownListHelpers.ListOfPositionsOfParentNode(int.Parse(ViewData["NodeID"].ToString())))
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                %>
                            </div>
                        </td>
                        <td style="width: 25%; vertical-align: top">
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.Position.CostCenter)%>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().DropDownListFor(model => model.Position.CostCenter.Id)
                                      .BindTo(DropDownListHelpers.ListOfCostCenter)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                %>
                            </div>
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.Position.Budget)%>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().NumericTextBoxFor(model => model.Position.Budget).MinValue(0).MaxValue(float.MaxValue)%>
                            </div>
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.Position.WorkingHours)%>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().NumericTextBoxFor(model => model.Position.WorkingHours).MinValue(1).MaxValue(float.MaxValue)%>
                            </div>
                        </td>
                        <td style="width: 25%; vertical-align: top">
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.Position.Per)%>
                            </div>
                            
                            <div class="editor-field">
                                <%: Html.Telerik().DropDownListFor(model => model.Position.Per.Id)
                                      .BindTo(DropDownListHelpers.ListOfTimeIntervals)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                %>
                            </div>
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.Position.DisabilityStatus)%>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().DropDownListFor(model => model.Position.DisabilityStatus.Id)
                                      .BindTo(DropDownListHelpers.ListOfDisabilityStatus)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                %>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="left">
                <a href="<%: Url.Action("Index", "Position") %>">
                    <img src="<%: Url.Content("~/Content/Ribbon/Icons/48/90.png") %>" title="<%: Resources.Shared.Buttons.Function.BackToMainPage %>"
                        alt="<%: Resources.Shared.Buttons.Function.BackToMainPage %>" height="24" width="24"
                        align="middle" />
                </a>
            </td>
            <td style="width: 10%; vertical-align: top" align="right">
                <input type="image" value="<%: Resources.Shared.Buttons.Function.Save %>" src="<%= Url.Content("~/Content/Ribbon/Icons/48/37.png") %>"
                    title="<%: Resources.Shared.Buttons.Function.Save %>" alt="<%: Resources.Shared.Buttons.Function.Save %>"
                    height="24" width="24" align="middle" />
            </td>
        </tr>
    </table>
</fieldset>
<% } %>
