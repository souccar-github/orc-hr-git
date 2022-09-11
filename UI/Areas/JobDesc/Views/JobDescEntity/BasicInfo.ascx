<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.JobDesc.Entities.JobDescription>" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%: string.Format(Resources.Areas.JobDesc.Entities.JobDescription.JobDescriptionModel.BasicInfoTitle.ToLower(), Model.Id)%></legend>
    <table width="100%" style="vertical-align: middle">
        <tr>            
            <td align="left">
                <img onclick="cancel()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/90.png") %>"
                    title="<%: Resources.Shared.Buttons.Function.Cancel %>" alt="<%: Resources.Shared.Buttons.Function.Cancel %>" height="24" width="24" align="middle" />
                <script type="text/javascript">
                    function cancel() {
                        $('#result').fadeOut('slow', function () {
                            $('#result').empty();
                        });
                    }
                </script>
            </td>
            <td style="width: 50%; vertical-align: top" align="right">
                <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
                   {%>
                <input type="image" value="Edit Info" onclick="ShowEditUserControl()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/39.png") %>"
                    title="<%: Resources.Shared.Buttons.Function.Edit %>" alt="<%: Resources.Shared.Buttons.Function.Edit %>" height="24" width="24" align="middle" />
                <% } %>
                <script type="text/javascript">
                    function ShowEditUserControl() {
                        $('#result').load('<%: Url.Action("Edit", "JobDescEntity", new {id=Model.Id}) %>');
                    }
                </script>
            </td>
        </tr>
        <tr>
            <td style="width: 100%; vertical-align: top">
                <table border="0" cellpadding="0" width="100%">
                    <tr>
                        <%--<td style="width: 33%; vertical-align: top">
                            <div class="display-label">
                                <%: Html.LabelFor(model => model.JobRole) %>
                            </div>
                            <div class="display-field">
                                <%: Html.DisplayFor(model => model.JobRole.Name)%>
                            </div>
                        </td>--%>
                        <td style="width: 33%; vertical-align: top">
                            <div class="display-label">
                                <%: Html.LabelFor(model => model.JobTitle) %>
                            </div>
                            <div class="display-field">
                                <%: Html.DisplayFor(model => model.JobTitle.Name)%>
                            </div>
                        </td>
                        <td style="width: 33%; vertical-align: top">
                            <div class="display-label">
                                <%: Html.LabelFor(model => model.Summary) %>
                            </div>
                            <div class="display-field">
                                <%: Html.TextAreaFor(model => model.Summary, new { @readonly = true, @class = "MultiLine" })%>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</fieldset>
