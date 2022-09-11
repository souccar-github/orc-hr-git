<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.OrgChart.Entities.Grade>" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%: string.Format(Resources.Areas.OrgChart.Entities.Grade.GradeModel.BasicDetailsTitle.ToLower(), Model.Name.Name)%>
                </legend>
    <table width="100%" style="vertical-align: middle">
        <tr>
            <td style="width: 50%; vertical-align: top">
                <div class="display-label">
                </div>
            </td>
            <td style="width: 50%; vertical-align: top">
                <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
                   {%>
                <input type="image" value="Edit Info" onclick="ShowEditUserControl()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/39.png") %>"
                    title=<%: Resources.Shared.Buttons.Function.Edit %> alt=<%: Resources.Shared.Buttons.Function.Edit %> height="24" width="24" />
                <% } %>
                <script type="text/javascript">
                    function ShowEditUserControl() {
                        $('#result').load('<%: Url.Action("Edit", "Grade", new {id=Model.Id}) %>');
                    }
                </script>
            </td>
        </tr>
        <tr>
            <td style="width: 100%; vertical-align: top" colspan="2">
                <table width="80%">
                    <tr>
                        <td style="width: 33%; vertical-align: top">
                         <div class="display-label">
                                <%: Html.LabelFor(model => model.Level) %>
                            </div>
                            <div class="display-field">
                                <%: Html.TextBoxFor(model => model.Level.Name, new { @readonly = true, @class = "SingleLine" })%>
                            </div>
                            <div class="display-label">
                                <%: Html.LabelFor(model => model.Name) %>
                            </div>
                            <div class="display-field">
                                <%: Html.TextBoxFor(model => model.Name.Name, new { @readonly = true, @class = "SingleLine" })%>
                            </div>
                            <div class="display-label">
                                <%: Html.LabelFor(model => model.Step) %>
                            </div>
                            <div class="display-field">
                                <%: Html.TextBoxFor(model => model.Step.Name, new { @readonly = true, @class = "SingleLine" })%>
                            </div>
                        </td>
                        <td style="width: 33%; vertical-align: top;">
                            <div class="display-label">
                                <%: Html.LabelFor(model => model.MinSalary) %>
                            </div>
                            <div class="display-field">
                                <%: Html.DisplayTextFor(model => model.MinSalary)%>
                            </div>
                            <div class="display-label">
                                <%: Html.LabelFor(model => model.MidPointSalary) %>
                            </div>
                            <div class="display-field">
                                <%: Html.DisplayTextFor(model => model.MidPointSalary)%>
                            </div>
                            <div class="display-label">
                                <%: Html.LabelFor(model => model.MaxSalary) %>
                            </div>
                            <div class="display-field">
                                <%: Html.DisplayTextFor(model => model.MaxSalary)%>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>       
    </table>
</fieldset>
