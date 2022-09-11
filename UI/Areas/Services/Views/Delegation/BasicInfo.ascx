<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<UI.Areas.Services.DTO.ViewModels.DelegationViewModel>" %>
<%@ Import Namespace="UI.Helpers.Views" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend">
        <%:String.Format(Resources.Areas.Services.Delegation.Messages.DelegationTitleWithNO,Html.DisplayFor(model => model.Delegation.Id))%>
    </legend>
    <table width="100%" style="vertical-align: middle">
        <tr>
            <td>
                <input type="button" value="<%:Resources.Shared.Buttons.Function.Cancel %>" onclick="CancelButton()"
                    class="CancelButton" />
            </td>
            <td style="width: 50%; vertical-align: top" align="right">
                <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
                   {%>
                <input type="button" value="<%:Resources.Shared.Buttons.Function.Edit %>" onclick="ShowEditUserControl()"
                    class="EditButton" />
                <% } %>
                <script type="text/javascript">
                    function ShowEditUserControl() {
                        $('#result').load('<%: Url.Action("Edit", "Delegation", new {id=Model.Delegation.Id}) %>');
                    }
                </script>
            </td>
        </tr>
        <tr>
            <td style="width: 33%; vertical-align: top">
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Delegation.Reason)%>
                </div>
                <div class="display-field">
                    <%: Html.TextAreaFor(model => model.Delegation.Reason, new ReadOnlyTextBox(true, "MultiLine"))%>
                </div>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Delegation.Comment)%>
                </div>
                <div class="display-field">
                    <%: Html.TextAreaFor(model => model.Delegation.Comment, new ReadOnlyTextBox(true, "MultiLine"))%>
                </div>
            </td>
            <td style="width: 33%; vertical-align: top">
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Delegation.From)%>
                </div>
                <div class="display-field">
                    <%: Html.Encode(String.Format("{0:MM/dd/yyyy}", Model.Delegation.From))%>
                </div>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Delegation.To)%>
                </div>
                <div class="display-field">
                    <%: Html.Encode(String.Format("{0:MM/dd/yyyy}", Model.Delegation.To))%>
                </div>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Delegation.Appraisable)%>
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.Delegation.Appraisable)%>
                </div>
            </td>
            <td style="width: 34%; vertical-align: top">
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Delegation.Position)%>
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.Delegation.Position.Name)%>
                </div>
            <% foreach (var item in Model.AssignedEmployees)
               {%>
            <table style="border: 1px solid black;">
                <tr style="border: 1px solid black;">
                    <td colspan="2" style="border: 1px solid black; padding: 3px;">
                        <%:Html.DisplayFor(x=>item.Employee.FullName) %>
                    </td>
                </tr>
                <tr style="border: 1px solid black;">
                    <td style="border: 1px solid black; padding: 3px;">
                        <%: Resources.Areas.Services.Delegation.Messages.RolesColumnTitle%>
                    </td>
                    <td style="border: 1px solid black; padding: 3px;">
                        <%: Resources.Areas.Services.Delegation.Messages.AuthoritiesColumnTitle%>
                    </td>
                </tr>
                <tr style="border: 1px solid black; padding: 3px;">
                    <td style="border: 1px solid black; padding: 3px;">
                        <% foreach (var role in item.Roles)
                           {%>
                           <%:Html.DisplayFor(x=>role.Checked) %>
                        <%:Html.DisplayFor(x=>role.Name) %>
                        <br />
                        <%}%>
                    </td>
                    <td style="border: 1px solid black; padding: 3px;">
                        <% foreach (var authority in item.Authorities)
                           {%>
                           <%:Html.DisplayFor(x=>authority.Checked) %>
                        <%:Html.DisplayFor(x=>authority.Name) %>
                        <br />
                        <%}%>
                    </td>
                </tr>
            </table>
            <%}%>
            </td>
        </tr>
    </table>
</fieldset>
