<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.PMS.Entities.AppraisalPhase>" %>
<%@ Import Namespace="UI.Areas.PMSComprehensive.Helpers" %>
<%
    using (Html.BeginForm("JsonInsert", "AppraisalPhase"))
    {%>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend">
        <%: Resources.Areas.PMS.Entities.AppraisalPhase.AppraisalPhaseModel.CreatePageTitle %></legend>
    <table style="margin-left: 1px">
        <tr>
            <td>
                <%:Html.ValidationMessageFor(model => model.StartDate)%>
            </td>
        </tr>
        <tr>
            <td>
                <%:Html.ValidationMessageFor(model => model.Id)%>
            </td>
        </tr>
        <tr>
            <td>
                <%:Html.ValidationMessageFor(model => model.FromDate)%>
            </td>
        </tr>
        <tr>
            <td>
                <%:Html.ValidationMessageFor(model => model.ToDate)%>
            </td>
        </tr>
        <tr>
            <td>
                <%:Html.ValidationMessageFor(model => model.WithSelfAssessment)%>
            </td>
        </tr>
        <tr>
            <td>
                <%:Html.ValidationMessageFor(model => model.WithSecondLevelSuperior)%>
            </td>
        </tr>
        <tr>
            <td>
                <%:Html.ValidationMessageFor(model => model.TopLevelGrade)%>
            </td>
        </tr>
        <tr>
            <td>
                <%:Html.ValidationMessageFor(model => model.SelfAssessmentWeight)%>
            </td>
        </tr>
        <tr>
            <td>
                <%:Html.ValidationMessageFor(model => model.DirectManagerWeight)%>
            </td>
        </tr>
        <tr>
            <td>
                <%:Html.ValidationMessageFor(model => model.IndirectManagerWeight)%>
            </td>
        </tr>
        <tr>
            <td>
                <%:Html.ValidationMessageFor(model => model.GapThreshold)%>
            </td>
        </tr>
        <tr>
            <td>
                <%:Html.ValidationMessageFor(model => model.SkillThreshold)%>
            </td>
        </tr>
        <tr>
            <td>
                <%:Html.ValidationMessageFor(model => model.FullMarkThreshold)%>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td colspan="2">
                <input type="submit" value="<%: Resources.Shared.Buttons.Function.Save %>" onclick=" DisableSaveButton(); " />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <%:Html.HiddenFor(model => model.Id)%>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table width="100%">
                    <tr>
                        <td style="width: 25%; vertical-align: top;">
                            <div class="editor-label">
                                <%: Html.LabelFor(model => model.StartDate) %>
                            </div>
                            <div class="editor-field">
                                <%:Html.Telerik().DatePickerFor(model => model.StartDate).Value(Model.StartDate.Date).Min(DateTime.MinValue)%>
                            </div>
                            <div class="editor-label">
                                <%: Html.LabelFor(model => model.FromDate) %>
                            </div>
                            <div class="editor-field">
                                <%:Html.Telerik().DatePickerFor(model => model.FromDate).Value(Model.FromDate.Date).Min(DateTime.MinValue)%>
                            </div>
                            <div class="editor-label">
                                <%: Html.LabelFor(model => model.ToDate) %>
                            </div>
                            <div class="editor-field">
                                <%:Html.Telerik().DatePickerFor(model => model.ToDate).Value(Model.ToDate.Date).Min(DateTime.MinValue)%>
                            </div>
                            <div class="editor-label-required">
                                <%:Html.LabelFor(model => model.Period)%>
                            </div>
                            <div class="editor-field">
                                <div id="ProjectTypeIndex">
                                    <%:Html.Telerik().DropDownListFor(model => model.Period.Id)
                                .BindTo(DropDownListHelpers.ListOfAppraisalPeriod)
                                .HtmlAttributes(new {style = string.Format("width:{0}px", 200)})
                                    %></div>
                            </div>
                        </td>
                        <td style="width: 25%; vertical-align: top;">
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.WithSelfAssessment) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.CheckBoxFor(model => model.WithSelfAssessment) %>
                            </div>
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.WithSecondLevelSuperior) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.CheckBoxFor(model => model.WithSecondLevelSuperior, new { onclick = "CheckWithSecondLevelSuperiorFunc();" })%>
                            </div>
                            <div id="divSelectTopLevelGrade">
                                <div class="editor-label-required">
                                    <%:Html.LabelFor(model => model.TopLevelGrade)%>
                                </div>
                                <div class="editor-field">
                                    <div id="Div1">
                                        <%:Html.Telerik().DropDownListFor(model => model.TopLevelGrade.Id)
                                                    .BindTo(UI.Areas.OrganizationChart.Helpers.DropDownListHelpers.ListOfGrades)
                                    .HtmlAttributes(new {style = string.Format("width:{0}px", 200)})
                                        %></div>
                                </div>
                            </div>
                        </td>
                        <td style="width: 25%; vertical-align: top;">
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.SelfAssessmentWeight) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().PercentTextBox().Name("SelfAssessmentWeight").MinValue(0).MaxValue(100).Value(0)%>
                            </div>
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.DirectManagerWeight) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().PercentTextBox().Name("DirectManagerWeight").MinValue(0).MaxValue(100).Value(0)%>
                            </div>
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.IndirectManagerWeight) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().PercentTextBox().Name("IndirectManagerWeight").MinValue(0).MaxValue(100).Value(0)%>
                            </div>
                        </td>
                        <td style="width: 25%; vertical-align: top;">
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.GapThreshold) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().NumericTextBoxFor(model => model.GapThreshold).MinValue(1).MaxValue(5).Value(1).IncrementStep(0.25f)%>
                            </div>
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.SkillThreshold) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().NumericTextBoxFor(model => model.SkillThreshold).MinValue(1).MaxValue(5).Value(1).IncrementStep(0.25f)%>
                            </div>
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.FullMarkThreshold) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().NumericTextBoxFor(model => model.FullMarkThreshold).MinValue(1).MaxValue(5).Value(1).IncrementStep(0.25f)%>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <a href="<%:Url.Action("Index", "AppraisalPhase")%>">
                    <input type="button" value="<%: Resources.Shared.Buttons.Function.Cancel %>" />
                </a>
            </td>
            <td style="width: 10%; vertical-align: top">
                <input type="submit" value="<%: Resources.Shared.Buttons.Function.Save %>" onclick=" DisableSaveButton(); " />
            </td>
        </tr>
    </table>
</fieldset>
<%
    }%>


<script type="text/javascript">

    $(document).ready(function () {
        CheckWithSecondLevelSuperiorFunc();
    });


    function CheckWithSecondLevelSuperiorFunc() {
        if ($('#WithSecondLevelSuperior').attr('checked') == true) {
            $("#divSelectTopLevelGrade").show();
        }
        else {
            $("#divSelectTopLevelGrade").hide();

        }
    }
    
</script>
