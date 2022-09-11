<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.OrgChart.ValueObjects.AssignedGrade.PositionGrade>" %>
<%@ Import Namespace="Infrastructure.Validation" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend">
        <%:Resources.Areas.OrgChart.ValueObjects.AssignedGrade.PositionGrade.PositionGradeModel.GradeTitle %></legend>
    <div style="color: Maroon; font-size: smaller;">
        <%
            if (ViewData["ExpiredRules"] != null)
            {
                foreach (BrokenBusinessRule brokenBusinessRule in (IList<BrokenBusinessRule>)ViewData["ExpiredRules"])
                {%>
        <%BrokenBusinessRule rule = brokenBusinessRule;%><%:Html.DisplayTextFor(model => rule.Rule)%>
        <br />
        <%
                }
            }%>
        <br />
    </div>
    <table width="100%">
        <tr>
            <td>
                <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
                   {%>
                <input type="image" value=<%:Resources.Shared.Buttons.Function.Edit %> onclick="ShowEditUserControl()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/39.png") %>"
                    title=<%:Resources.Shared.Buttons.Function.Edit %> alt=<%:Resources.Shared.Buttons.Function.Edit %> height="24" width="24" />
                <% } %>
                <script type="text/javascript">
                    function ShowEditUserControl() {
                        $('#result').load('<%: Url.Action("Edit", "PositionGrade") %>');
                    }
                </script>
            </td>
        </tr>
        <tr>
            <td style="width: 100%; vertical-align: top" colspan="3">
                <fieldset>
                    <table width="80%">
                        <tr>
                            <td style="width: 33%; vertical-align: top">
                                <div class="display-label">
                                    <%: Html.LabelFor(model => model.Name) %>
                                </div>
                                <div class="display-field">
                                    <%: Html.TextBoxFor(model => model.Name, new { @readonly = true, @class = "SingleLine" })%>
                                </div>
                                <div class="display-label">
                                    <%: Html.LabelFor(model => model.Step) %>
                                </div>
                                <div class="display-field">
                                    <%: Html.TextBoxFor(model => model.Step, new { @readonly = true, @class = "SingleLine" })%>
                                </div>
                            </td>
                            <td style="width: 33%; vertical-align: top">
                                <div class="display-label">
                                    <%: Html.LabelFor(model => model.GroupSalaryScale) %>
                                </div>
                                <div class="display-field">
                                    <%: Html.DisplayFor(model => model.GroupSalaryScale.Name)%>
                                </div>
                                <div class="display-label">
                                    <%: Html.LabelFor(model => model.SalaryScale) %>
                                </div>
                                <div class="display-field">
                                    <%: Html.TextBoxFor(model => model.SalaryScale, new { @readonly = true, @class = "SingleLine" })%>
                                </div>
                            </td>
                            <td style="width: 33%; vertical-align: top;">
                                <div class="display-label">
                                    <%: Html.LabelFor(model => model.MinPay) %>
                                </div>
                                <div class="display-field">
                                    <%: Html.DisplayTextFor(model => model.MinPay)%>
                                </div>
                                <div class="display-label">
                                    <%: Html.LabelFor(model => model.MidPointPay) %>
                                </div>
                                <div class="display-field">
                                    <%: Html.DisplayTextFor(model => model.MidPointPay)%>
                                </div>
                                <div class="display-label">
                                    <%: Html.LabelFor(model => model.MaxPay) %>
                                </div>
                                <div class="display-field">
                                    <%: Html.DisplayTextFor(model => model.MaxPay)%>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
    </table>
</fieldset>
