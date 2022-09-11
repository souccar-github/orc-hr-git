<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<UI.Areas.OrganizationChart.DTO.ViewModels.PositionViewModel>" %>
<%@ Import Namespace="Infrastructure.Utilities" %>
<%@ Import Namespace="UI.Areas.OrganizationChart.Helpers" %>
<% using (Ajax.BeginForm("JsonEdit", "Position", new AjaxOptions { OnComplete = "JsonEdit_OnComplete" }))
   {%>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend">
        <%: string.Format(Resources.Areas.OrgChart.ValueObjects.Position.PositionModel.PositionDetailsTitle, Html.DisplayTextFor(model => model.Position.Code))%>
    </legend>
    <script type="text/javascript">
        function JsonEdit_OnComplete(context) {

//            var jsonEdit = context.get_response().get_object();
//           // alert(jsonEdit.PartialViewHtml);
//            if (jsonEdit.Success) {
//                $.ajax({
//                    url: '<%=Url.Action("SaveTabIndex", "Position")%>/', type: "POST",
//                    data: { selectedIndex: 100 },
//                    success: function () {
//                        location.reload();

//                    }
//                    
//                });
//            }
//            else {

//                //alert(jsonEdit.PartialViewHtml);
//                $("#ErrorDivResult").text(jsonEdit.Msg);
            //            }

            var jsonEdit = context.get_response().get_object();
            if (jsonEdit.Success) {
                $("#result").html(jsonEdit.PartialViewHtml);
                location.reload();
            } else {
                $("#result").html(jsonEdit.PartialViewHtml);
            }
        }
    </script>
    <table style="margin-left: 1px">
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Position.Grades) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Position.Code)%>
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
                <%: Html.ValidationMessageFor(model => model.Position.Per.Name) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Position.DisabilityStatus) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessage("DupLicateReportingPosition")%>
               
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td style="width: 10%; vertical-align: top" align="right" colspan="3">
                <input type="image" value="<%: Resources.Shared.Buttons.Function.Save %>" src="<%= Url.Content("~/Content/Ribbon/Icons/48/37.png") %>"
                    title="<%: Resources.Shared.Buttons.Function.Save %>" alt="<%: Resources.Shared.Buttons.Function.Save %>"
                    height="24" width="24" align="middle" />
                <%: Html.HiddenFor(model => model.Position.Id)%>
            </td>
        </tr>
        <tr>
            <td colspan="5" style="width: 100%">
                <fieldset class="ParentFieldset">
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
                                    <% if (Model.GradeId == 0)
                                       {%>
                                    <%: Html.Telerik().DropDownListFor(model => model.GradeId)
                                                                                     .BindTo(DropDownListHelpers.ListOfGrades)
                                          .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                    %>
                                    <%}
                                       else
                                       {%>
                                    <%: Html.HiddenFor(model => model.GradeId)%>
                                    <% if (Model.Position.Grades.Count > 0)
                                       {%>
                                    <%: Html.Label(Model.Position.ActiveGrade.Name)%>
                                    <%}
                                       }%>
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
                                                           .BindTo(DropDownListHelpers.ListOfPositionsOfParentNode(Model.NodeId))
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
                </fieldset>
            </td>
        </tr>
        <tr>
            <td align="left">
                <img onclick="cancel()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/90.png") %>"
                    title="<%: Resources.Shared.Buttons.Function.Cancel %>" alt="<%: Resources.Shared.Buttons.Function.Cancel %>"
                    height="24" width="24" align="middle" />
                <script type="text/javascript">
                    function cancel() {
                        $('#result').load('<%: Url.Action("PartialInfo", "Position") %>');
                    }
                </script>
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
