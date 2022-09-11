<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.PMS.RootEntities.Appraisal>" %>
<fieldset>
    <legend>Appraisal</legend>
    <table style="width: 100%; border: 2">
<%--        <tr>
            <td style="width: 50%">
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Date) %>
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.Date) %>
                </div>
            </td>
            <td style="width: 50%;">
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Position) %>
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.Position)%>
                </div>
            </td>
        </tr>
        <tr style="height: 20px">
            <td style="width: 50%;">
                <div class="display-label">
                    <%: Html.LabelFor(model => model.EmployeeGrade) %>
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.EmployeeGrade)%>
                </div>
            </td>
            <td style="width: 50%;">
                <div class="display-label">
                    <%: Html.LabelFor(model => model.AppraiserJobTitle) %>
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.AppraiserJobTitle)%>
                </div>
            </td>
        </tr>--%>
    </table>
    <%--    <%: Html.HiddenFor(model => model.Id) %>
            <div class="editor-label">
            <%: Html.LabelFor(model => model.FinalSubmit) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.FinalSubmit) %>
            <%: Html.ValidationMessageFor(model => model.FinalSubmit) %>
        </div>
    <div class="display-label">
        <%: Html.LabelFor(model => model.EmployeeOrganizationalLevel) %>
    </div>
    <div class="display-field">
        <%: Html.EditorFor(model => model.EmployeeOrganizationalLevel) %>
    </div>--%>
</fieldset>
