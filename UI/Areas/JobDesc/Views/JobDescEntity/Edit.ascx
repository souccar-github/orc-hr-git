<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.JobDesc.Entities.JobDescription>" %>
<%@ Import Namespace="UI.Areas.OrganizationChart.Helpers" %>
<% using (Ajax.BeginForm("JsonEdit", "JobDescEntity", new AjaxOptions { OnComplete = "JsonEdit_OnComplete" }))
   {%>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%: string.Format(Resources.Areas.JobDesc.Entities.JobDescription.JobDescriptionModel.BasicInfoTitle.ToLower(), Model.Id)%></legend>
    <script type="text/javascript">
        function JsonEdit_OnComplete(context) {
            var jsonEdit = context.get_response().get_object();
            if (jsonEdit.Success) {
                $.ajax({
                    url: '<%: Url.Action("SaveTabIndex", "JobDescEntity")%>/', type: "POST",
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
            <td style="width: 10%; vertical-align: top" align="right" colspan="2">
                <input type="image" value="Save" src="<%= Url.Content("~/Content/Ribbon/Icons/48/37.png") %>"
                    title="<%: Resources.Shared.Buttons.Function.Save %>" alt="<%: Resources.Shared.Buttons.Function.Save %>" height="24" width="24" align="middle" />
                <%: Html.HiddenFor(model => model.Id) %>
            </td>
        </tr>
        <tr>
            <td style="width: 100%; vertical-align: top" colspan="2">
                <table border="0" cellpadding="0" cellspacing="0" width="80%">
                    <tr>
                        <%--<td style="width: 33%; vertical-align: top">
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.JobRole) %>
                            </div>
                            <div class="editor-field">
                                <div class="editor-field">
                                    <%: Html.Telerik().DropDownListFor(model => model.JobRole.Id)
                                      .BindTo(DropDownListHelpers.ListOfJobRole)
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                    %>
                                </div>
                            </div>
                        </td>--%>
                        <td style="width: 33%; vertical-align: top">
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.JobTitle) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.Telerik().DropDownListFor(model => model.JobTitle.Id)
                                      .BindTo(DropDownListHelpers.ListOfJobDescJobTitles(Model.Id))
                                      .HtmlAttributes(new { style = string.Format("width:{0}px", 200) })
                                %>
                            </div>
                        </td>
                        <td style="width: 33%; vertical-align: top;">
                            <div class="editor-label-required">
                                <%: Html.LabelFor(model => model.Summary) %>
                            </div>
                            <div class="editor-field">
                                <%: Html.TextAreaFor(model => model.Summary)%>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="left">
                <img onclick="cancel()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/90.png") %>"
                    title="<%: Resources.Shared.Buttons.Function.Cancel %>" alt="<%: Resources.Shared.Buttons.Function.Cancel %>" height="24" width="24" align="middle" />
                <script type="text/javascript">
                    function cancel() {
                        $('#result').load('<%: Url.Action("PartialMasterInfo", "JobDescEntity") %>');
                    }
                </script>
            </td>
            <td style="width: 10%; vertical-align: top" align="right">
                <input type="image" value="Save" src="<%= Url.Content("~/Content/Ribbon/Icons/48/37.png") %>"
                    title="<%: Resources.Shared.Buttons.Function.Save %>" alt="<%: Resources.Shared.Buttons.Function.Save %>" height="24" width="24" align="middle" />
            </td>
        </tr>
    </table>
</fieldset>
<% } %>
