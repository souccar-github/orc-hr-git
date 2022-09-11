<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.OrgChart.ValueObjects.AssignedGrade.PositionGrade>" %>
<%@ Import Namespace="UI.Areas.OrganizationChart.Helpers" %>
<% using (Ajax.BeginForm("Save", "PositionGrade", new AjaxOptions() { OnComplete = "JsonAdd_OnComplete" }))
   {%>
<script type="text/javascript">

    function JsonAdd_OnComplete(context) {
        var JsonAdd = context.get_response().get_object();
        if (JsonAdd.Success) {
            location.reload();
        }
        else {
            $("#result").html(JsonAdd.PartialViewHtml);
            Toggle("edit");
        }
    }
</script>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend">
        <%:Resources.Areas.OrgChart.ValueObjects.AssignedGrade.PositionGrade.PositionGradeModel.GradeTitle %></legend>
    <table border="0" cellpadding="0" cellspacing="0">
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
                <%: Html.ValidationMessageFor(model => model.GroupSalaryScale) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.SalaryScale) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.MinPay) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.MidPointPay) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.MaxPay) %>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td colspan="2">
                <input type="image" value="<%:Resources.Shared.Buttons.Function.Save %>" src="<%= Url.Content("~/Content/Ribbon/Icons/48/37.png") %>"
                    title="<%:Resources.Shared.Buttons.Function.Save %>" alt="<%:Resources.Shared.Buttons.Function.Save %>"
                    height="24" width="24" />
            </td>
            <%: Html.HiddenFor(model => model.Id) %>
        </tr>
        <tr>
            <td style="width: 100%; vertical-align: top" colspan="2">
                <table width="80%">
                    <tr>
                        <td style="width: 33%; vertical-align: top">
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.Name) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.TextBoxFor(model => model.Name, new { @readonly = true, @class = "SingleLine" })%>
                            </div>
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.Step) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.TextBoxFor(model => model.Step, new { @readonly = true, @class = "SingleLine" })%>
                            </div>
                        </td>
                        <td style="width: 33%; vertical-align: top">
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.GroupSalaryScale) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().ComboBoxFor(model => model.GroupSalaryScale.Id)
                                      .AutoFill(true)
                                      .BindTo(DropDownListHelpers.ListOfGroupSalaryScale)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                      .Filterable(filtering => filtering.FilterMode(AutoCompleteFilterMode.StartsWith))
                                      .HighlightFirstMatch(true)
                                %>
                            </div>
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.SalaryScale) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.TextBoxFor(model => model.SalaryScale) %>
                            </div>
                        </td>
                        <td style="width: 33%; vertical-align: top;">
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.MinPay) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().NumericTextBox().Name("MinPay").MinValue(0).MaxValue(float.MaxValue)%>
                            </div>
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.MidPointPay) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().NumericTextBox().Name("MidPointPay").MinValue(0).MaxValue(float.MaxValue)%>
                            </div>
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.MaxPay) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().NumericTextBox().Name("MaxPay").MinValue(0).MaxValue(float.MaxValue)%>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <img onclick="cancel()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/90.png") %>"
                    title="<%:Resources.Shared.Buttons.Function.Cancel %>" alt="<%:Resources.Shared.Buttons.Function.Cancel %>"
                    height="24" width="24" />
                <script type="text/javascript">
                    function cancel() {
                        $('#result').load('<%: Url.Action("Index", "PositionGrade") %>');
                    }
                </script>
            </td>
            <td style="width: 10%; vertical-align: top">
                <input type="image" value="<%:Resources.Shared.Buttons.Function.Save %>" src="<%= Url.Content("~/Content/Ribbon/Icons/48/37.png") %>"
                    title="<%:Resources.Shared.Buttons.Function.Save %>" alt="<%:Resources.Shared.Buttons.Function.Save %>"
                    height="24" width="24" />
            </td>
        </tr>
    </table>
</fieldset>
<% } %>