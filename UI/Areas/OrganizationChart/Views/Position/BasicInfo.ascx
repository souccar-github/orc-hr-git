<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<UI.Areas.OrganizationChart.DTO.ViewModels.PositionViewModel>" %>
<%@ Import Namespace="Google.ProtocolBuffers.Collections" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend">
        <%: string.Format(Resources.Areas.OrgChart.ValueObjects.Position.PositionModel.PositionDetailsTitle, Model.Position.Code)%>
    </legend>
    <table width="100%" style="vertical-align: middle">
        <tr>
            <td style="width: 50%; vertical-align: top" colspan="4">
                <div class="display-label">
                </div>
            </td>
            <td style="width: 50%; vertical-align: top" align="right">
                <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
                   {%>
                <input type="image" value="<%: Resources.Shared.Buttons.Function.Edit %>" onclick="ShowEditUserControl()"
                    src="<%= Url.Content("~/Content/Ribbon/Icons/48/39.png") %>" title="<%: Resources.Shared.Buttons.Function.Edit %>"
                    alt="<%: Resources.Shared.Buttons.Function.Edit %>" height="24" width="24" align="middle" />
                <% } %>
                <script type="text/javascript">
                    function ShowEditUserControl() {
                        $('#result').load('<%: Url.Action("Edit", "Position") %>');
                    }
                </script>
            </td>
        </tr>
        <tr>
            <td colspan="5">
                <table width="100%" style="vertical-align: middle">
                    <tr>
                        <td style="width: 25%; vertical-align: top">
                            <%: Html.HiddenFor(model => model.Position.Id)%>
                            <div class="display-label">
                                <%: Html.LabelFor(model => model.Position.Code)%>
                            </div>
                            <div class="display-field">
                                <%: Html.DisplayFor(model => model.Position.Code)%>
                            </div>
                            <div class="display-label">
                                <%: Html.LabelFor(model => model.Position.JobTitle)%>
                            </div>
                            <div class="display-field">
                                <%: Html.DisplayFor(model => model.Position.JobTitle.Name)%>
                            </div>
                            <div class="display-label">
                                <%: Html.LabelFor(model => model.Position.Grades)%>
                            </div>
                            <div class="display-field">
                                <%: Html.DisplayFor(model => model.Position.ActiveGrade.Name)%>
                            </div>
                        </td>
                        <td style="width: 25%; vertical-align: top">
                            <div class="display-label">
                                <%: Html.LabelFor(model => model.Position.Type)%>
                            </div>
                            <div class="display-field">
                                <%: Html.DisplayFor(model => model.Position.Type.Name)%>
                            </div>
                            <div class="display-label">
                                <%: Html.LabelFor(model => model.Position.Level)%>
                            </div>
                            <div class="display-field">
                                <%: Html.DisplayFor(model => model.Position.Level.Name)%>
                            </div>
                            <div class="display-label">
                               <%: Html.LabelFor(model => model.Position.PositionReportings)%>
                            </div>
                            <div class="display-field">
                                <ul>
                                <li>
                                <%:Resources.Areas.OrgChart.ValueObjects.PositionReporting.PositionReportingModel.ParentPosition%>
                                
                                <%:Resources.Areas.OrgChart.ValueObjects.PositionReporting.PositionReportingModel.FromDate%>
                                
                                <%:Resources.Areas.OrgChart.ValueObjects.PositionReporting.PositionReportingModel.ExpireDate%>
                               
                                <%:Resources.Areas.OrgChart.ValueObjects.PositionReporting.PositionReportingModel.IsPrimary%>
                                </li>
                               
                                <% foreach (var positionReporting in Model.Position.PositionReportings)
                                   {%>
                                   <li>
                                    <%: Html.DisplayFor(x => positionReporting.ParentPosition.Name)%>
                                     <%:Html.Encode(String.Format("{0:MM/dd/yyyy}", positionReporting.FromDate))%>
                                     <%:Html.Encode(String.Format("{0:MM/dd/yyyy}", positionReporting.ExpireDate))%>
                                   
                                    <%: Html.DisplayFor(x => positionReporting.IsPrimary)%>         
                                    </li>
                                
                                <%}%>
                                 </ul>
                            </div>
                        </td>
                        <td style="width: 25%; vertical-align: top">
                            <div class="display-label">
                                <%: Html.LabelFor(model => model.Position.CostCenter)%>
                            </div>
                            <div class="display-field">
                                <%: Html.DisplayFor(model => model.Position.CostCenter.Name)%>
                            </div>
                            <div class="display-label">
                                <%: Html.LabelFor(model => model.Position.Budget)%>
                            </div>
                            <div class="display-field">
                                <%: Html.DisplayFor(model => model.Position.Budget)%>
                            </div>
                            <div class="display-label">
                                <%: Html.LabelFor(model => model.Position.WorkingHours)%>
                            </div>
                            <div class="display-field">
                                <%: Html.DisplayFor(model => model.Position.WorkingHours)%>
                            </div>
                        </td>
                        <td style="width: 25%; vertical-align: top">
                            <div class="display-label">
                                <div class="display-label">
                                    <%: Html.LabelFor(model => model.Position.Per)%>
                                </div>
                                <div class="display-field">
                                    <%: Html.DisplayFor(model => model.Position.Per.Name)%>
                                </div>
                                <%: Html.LabelFor(model => model.Position.DisabilityStatus)%>
                            </div>
                            <div class="display-field">
                                <%: Html.DisplayFor(model => model.Position.DisabilityStatus.Name)%>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</fieldset>
