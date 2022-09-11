<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="HRIS.Domain.Services" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend">
        <%:Resources.Areas.Services.Delegation.Messages.DelegationServiceTitle %></legend>
    <table width="100%">
        <tr>
            <td>
                <% Html.Telerik().Grid<Delegation>("Delegations")
                        .Name("Grid")
                        .DataKeys(k => k.Add(o => o.Id))
                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Delegation"))
                        .ToolBar(toolBar => toolBar.Template(() =>
                        { 
                %>
                <table class="GridToolBar">
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <a href="<%:Url.Action("Insert", "Delegation")%>">
                                <input type="button" value="<%:Resources.Shared.Buttons.Function.Add %>" />
                            </a>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <% 
                        }))
                        .Filterable()
                        .Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                        .Columns(c =>
                        {
                            c.Bound(o => o.Id).Title("No").Width(1).Groupable(false).Filterable(false).Sortable(false);
                            c.Bound(o => o.From);
                            c.Bound(o => o.To);
                            c.Bound(o => o.Appraisable);
                            c.Bound(o => o.Position.Name).Title(Resources.Areas.Services.Delegation.DelegationModel.Position);
                            c.Command(s =>
                                            {

                                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                {
                                                    s.Delete().ButtonType(GridButtonType.Image);
                                                }
                                            }).Width(1);
                        })
                        .RowAction(row =>
                        {
                            if (ViewData["SelectedRow"] != null)
                                row.Selected = row.DataItem.Id == (int)ViewData["SelectedRow"];
                        })
                        .ClientEvents(builder => builder.OnRowSelect("loadPartialView"))
                        .Pageable(p => p.PageSize(5).PageTo((int)ViewData["PageTo"]))
                        .Selectable()
                        .Render();
                %>
            </td>
            <script type="text/javascript">
                function loadPartialView(e) {
                    var x = e.row.cells[0].innerHTML;

                    $('#result').load('<%: Url.Action("PartialMasterInfo", "Delegation") %>', { selectedRowId: x }, function () {
                        $('#result').fadeIn('fast');
                    });
                }
            </script>
        </tr>
    </table>
</fieldset>
