<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="HRIS.Domain.Objectives.RootEntities" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%: Resources.Areas.Objective.Entities.OrganizationalObjective.OrganizationalObjectiveModel.OrganizationalObjectivBasicInfo %></legend>
    <table width="100%">
        <tr>
            <td style="width: 90%">
                <table width="100%">
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <a href="<%: Url.Action("Insert", "StrategicObjective") %>">
                                <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>" title='<%:Resources.Shared.Buttons.Function.Add %>'
                                    alt="<%:Resources.Shared.Buttons.Function.Add %>" height="36" width="36" align="middle" />
                            </a>
                            <% } %>
                            <%:
                                    Html.Telerik().Grid<StrategicObjective>("organizationalObjectives")
                                        .Name("Grid")
                                        .DataKeys(k => k.Add(o => o.Id))
                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "StrategicObjective"))
                                        .Filterable()
                                        .Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                        .Columns(c =>
                                        {
                                            c.Bound(o => o.Id).Title(Resources.Shared.Model.Fields.Id).Width(1).Groupable(false).Filterable(false).Sortable(false);
                                            c.Bound(o => o.Code.Code);
                                            c.Bound(o => o.Name);
                                            c.Bound(o => o.Dimension.Name).Title("Dimension");
                                            c.Bound(o => o.Period);
                                            c.Bound(o => o.FromYear);
                                            c.Bound(o => o.ToYear);
                                            c.Command(s =>
                                                            {
                                                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                                {
                                                                    s.Delete().ButtonType(GridButtonType.Image);
                                                                }
                                                            }).Width("1%");
                                        })
                                        .ClientEvents(builder => builder.OnRowSelect("loadPartialView"))
                                        .Pageable(p => p.PageSize(5).PageTo((int)ViewData["PageTo"]))
                                        .Selectable()
                            %>
                        </td>
                    </tr>
                </table>
            </td>
            <script type="text/javascript">

                function tabStripSelect(e) {

                    var item = $(e.item);

                    $('#result').fadeOut('fast');

                    $.ajax({
                        url: '<%: Url.Action("SaveTabIndex", "StrategicObjective")%>/', type: "POST",
                        data: { selectedIndex: item.index() }
                    });
                }

                function SetMasterRecordValue(e) {

                    var x = e.masterRow.cells[1].innerHTML;

                    $('#result').load('<%: Url.Action("PartialMasterInfo", "StrategicObjective") %>', { selectedRowId: x }, function () {
                        $('#result').fadeIn('fast');

                        loadRibbon();
                    });
                }

                function loadPartialView(e) {

                    //$('> .t-hierarchy-cell > .t-icon', e.row).click();

                    $('#result').fadeOut('fast');

                    var x = e.row.cells[0].innerHTML;

                    $('#result').load('<%: Url.Action("PartialMasterInfo", "StrategicObjective") %>', { selectedRowId: x }, function () {
                        $('#result').fadeIn('fast');

                        loadRibbon();
                    });
                }

                function loadRibbon() {
                    $('#ObjectiveFunctionsArea').load('<%: Url.Action("GetFunctionsPartial", "ObjectiveModule") %>');
                }
            </script>
        </tr>
    </table>
</fieldset>
