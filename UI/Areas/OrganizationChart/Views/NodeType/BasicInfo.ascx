<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.OrgChart.Indexes.NodeType>" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%:Resources.Areas.OrgChart.Indexes.NodeType.NodeTypeModel.BasicInfoNodeTypesTitle %></legend>
    <table width="100%" style="vertical-align: middle">
        <table width="80%" style="vertical-align: middle">
            <tr>
                <td style="width: 50%; vertical-align: top" colspan="4">
                    <div class="display-label">
                    </div>
                </td>
                <td style="width: 50%; vertical-align: top" align="right">
                    <% if (ViewData["CanUpdate"] != null && (bool)ViewData["CanUpdate"])
                       {%>
                    <input type="image" value=<%:Resources.Shared.Buttons.Function.Edit %> onclick="ShowEditUserControl()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/39.png") %>"
                        title=<%:Resources.Shared.Buttons.Function.Edit %> alt=<%:Resources.Shared.Buttons.Function.Edit %> height="24" width="24" align="middle" />
                    <% } %>
                    <script type="text/javascript">
                        function ShowEditUserControl() {
                            $('#result').load('<%: Url.Action("Edit", "NodeType", new {id=Model.Id}) %>');
                        }
                    </script>
                </td>
            </tr>
            <tr>
                <td style="width: 50%; vertical-align: top">
                    <div class="display-label">
                        <%: Html.LabelFor(model => model.NodeOrder) %>
                    </div>
                    <div class="display-field">
                        <%: Html.DisplayFor(model => model.NodeOrder)%>
                    </div>
                </td>
                <td style="width: 50%; vertical-align: top">
                    <div class="display-label">
                        <%: Html.LabelFor(model => model.Name) %>
                    </div>
                    <div class="display-field">
                        <%: Html.DisplayFor(model => model.Name)%>
                    </div>
                </td>
            </tr>
        </table>
    </table>
</fieldset>
