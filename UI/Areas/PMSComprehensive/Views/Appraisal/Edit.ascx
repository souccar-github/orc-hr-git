<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.PMS.Entities.Appraisal>" %>
<%@ Import Namespace="UI.Areas.PMSComprehensive.Helpers" %>
<% using (Ajax.BeginForm("JsonEdit", "Appraisal", new AjaxOptions { OnComplete = "JsonEdit_OnComplete" }))
   {%>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend">Appraisal No. (<%: Html.DisplayFor(model => model.Id)%>)
        Details</legend>
    <script type="text/javascript">

        function JsonEdit_OnComplete(context) {
            var jsonEdit = context.get_response().get_object();
            if (jsonEdit.Success) {
                $.ajax({
                    url: '<%: Url.Action("SaveTabIndex", "Appraisal")%>/', type: "POST",
                    data: { selectedIndex: 100 },
                    success: function () {
                        location.reload();
                    }
                });
            } else {
                $("#result").html(jsonEdit.PartialViewHtml);
            }
        }
    </script>
    <table style="margin-left: 1px">
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Type) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Period) %>
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.ValidationMessageFor(model => model.Date) %>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <tr>
                <td style="width: 50%; vertical-align: top;">
                    <div class="editor-label">
                        <%: Html.LabelFor(model => model.Period) %>
                    </div>
                    <div class="editor-field">
                        <%:Html.Telerik().DropDownListFor(model => model.Period.Id)
                                .BindTo(DropDownListHelpers.ListOfAppraisalPeriod)
                                .HtmlAttributes(new {style = string.Format("width:{0}px", 200)})
                        %>
                    </div>
                    <div class="editor-label">
                        <%: Html.LabelFor(model => model.Type) %>
                    </div>
                    <div class="editor-field">
                        <%:Html.Telerik().DropDownListFor(model => model.Type.Id)
                                .BindTo(DropDownListHelpers.ListOfAppraisalType)
                                .HtmlAttributes(new {style = string.Format("width:{0}px", 200)})
                        %>
                    </div>
                    <div class="editor-label">
                        <%: Html.LabelFor(model => model.Date) %>
                    </div>
                    <div class="editor-field">
                        <%:Html.Telerik().DatePickerFor(model => model.Date).Value(Model.Date.Date).Min(DateTime.MinValue)%>
                    </div>
                    <div class="editor-label">
                        <%: Html.LabelFor(model => model.FinalSubmit) %>
                    </div>
                    <div class="editor-field">
                        <%: Html.EditorFor(model => model.FinalSubmit) %>
                    </div>
                </td>
             <%--   <td style="width: 50%; vertical-align: top;">
                    <div class="editor-label">
                        <%: Html.LabelFor(model => model.Position) %>
                    </div>
                    <div class="display-field">
                        <%: Html.DisplayFor(model => model.Position) %>
                    </div>
                    <div class="editor-label">
                        <%: Html.LabelFor(model => model.EmployeeOrganizationalLevel) %>
                    </div>
                    <div class="display-field">
                        <%: Html.DisplayFor(model => model.EmployeeOrganizationalLevel) %>
                    </div>
                    <div class="editor-label">
                        <%: Html.LabelFor(model => model.EmployeeGrade) %>
                    </div>
                    <div class="display-field">
                        <%: Html.DisplayFor(model => model.EmployeeGrade) %>
                    </div>
                    <div class="editor-label">
                        <%: Html.LabelFor(model => model.AppraiserJobTitle) %>
                    </div>
                    <div class="display-field">
                        <%: Html.DisplayFor(model => model.AppraiserJobTitle) %>
                    </div>
                </td>--%>
            </tr>
        </tr>
        <tr>
            <td>
                <input type="button" value="Cancel" onclick="cancel()" class="CancelButton" />
                <script type="text/javascript">
                    function cancel() {
                        $('#result').load('<%: Url.Action("PartialMasterInfo", "Appraisal") %>');
                    }
                </script>
            </td>
            <td style="width: 10%; vertical-align: top" align="right">
                <input type="submit" value="Save" onclick="DisableSaveButton()" />
            </td>
        </tr>
    </table>
</fieldset>
<% } %>
