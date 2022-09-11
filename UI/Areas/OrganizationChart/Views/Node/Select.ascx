<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.OrgChart.ValueObjects.Node>" %>
<fieldset class="ParentFieldset">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="width: 25%; vertical-align: top">
                <%: Html.DisplayFor(model => model.Id)%>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Code) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.Code, new { @readonly = true, @class = "SingleLine" })%>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Type) %>
                </div>
                <div class="editor-field">
                    <%: Html.DisplayFor(model => model.Type.Name)%>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Name) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(model => model.Name, new { @readonly = true, @class = "SingleLine" })%>
                </div>
            </td>
            <td style="width: 25%; vertical-align: top">
                <% if (ViewData["RootNodeSelected"] != null && ViewData["RootNodeSelected"].ToString().ToLower() == "false")
                   { %>
                <div class="editor-label-required">
                    <%: Html.LabelFor(model => model.Positions) %>
                </div>
                <%: Html.ActionLink(Resources.Shared.Buttons.Function.GoToPositions, "GoToPositions", "Node")%>
                <%  } %>
            </td>
        </tr>
    </table>
</fieldset>
