<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="HRIS.Domain.ProjectManagment.ValueObjects" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%: Resources.Areas.ProjectManagment.ValueObjects.ProjectPhase.ProjectPhaseModel.ProjectPhaseMasterGridTitle %></legend>
    <table width="100%">
        <tr>
            <td>
                <%
                    Html.Telerik().Grid<ProjectPhase>("projectPhases")
                        .Name("Grid")
                        .DataKeys(k => k.Add(o => o.Id))
                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "ProjectPhase"))
                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                        .Columns(c =>
                        {
                            c.Bound(o => o.Id).Width(1).Groupable(false).Filterable(false).Sortable(false);
                            c.Bound(o => o.Name);
                            c.Bound(o => o.StartDate);
                            c.Bound(o => o.EndDate);
                            c.Bound(o => o.Status.Name);
                            c.Bound(o => o.CompletionPercentage);
                            c.Command(s =>
                                            {
                                                //s.Select().ButtonType(GridButtonType.Image).HtmlAttributes(new { @class = "MasterGridSelect" });
                                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                {
                                                    s.Delete().ButtonType(GridButtonType.Image);
                                                }
                                            }).Width(1);
                        })

                        .DetailView(detailView => detailView.Template(e => // Set the server template
                            {
                %>
                <% Html.Telerik().TabStrip()
                                                      .Name("TabStrip_" + e.Id)
                                                      .Effects(fx => fx.Opacity())
                                                      .SelectedIndex(Session["SelectedTabIndexSecondLevel"] != null ? int.Parse(Session["SelectedTabIndexSecondLevel"].ToString()) : 0)
                                                      .Items(items =>
                                                      {
                                                          //Phase Task
                                                          items.Add().Text(Resources.Areas.ProjectManagment.ValueObjects.ProjectPhase.ProjectPhaseModel.PhaseTaskPluralTitle).Content(() =>
                                                                 {
                %>
                <%Html.Telerik().Grid(e.Tasks)
                           .Name("PhaseTasksGrid" + e.Id)
                           .DataKeys(s => s.Add(x => x.Id))
                           .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "PhaseTask"))
                           .ToolBar(toolBar => toolBar.Template(() =>
                            { 
                %>
                <table class="GridToolBar">
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridAddPhaseTask()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title="<%: Resources.Shared.Buttons.Function.Add %>" alt="<%: Resources.Shared.Buttons.Function.Add %>" height="24" width="24" align="middle" />
                            <script type="text/javascript">
                                function GridAddPhaseTask() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%:Url.Action("Index", "PhaseTask")%>', function () {
                                        $('#result').fadeIn('fast');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <% 
                            }))
                           .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Groupable(false).Filterable(
                                                false).Sortable(false);
                                            columns.Bound(o => o.ActualClosingDate);
                                            columns.Bound(o => o.Status.Name);
                                            columns.Command(s =>
                                                                {
                                                                    if (ViewData["CanDelete"] != null &&
                                                                        (bool)ViewData["CanDelete"])
                                                                    {
                                                                        s.Delete().ButtonType(GridButtonType.Image);
                                                                    }
                                                                }).Width(1);
                                        })
                           .Selectable()
                           .ClientEvents(events => events.OnRowSelect("PhaseTasksServiceRowSelected"))
                           .Pageable(pager => pager.PageSize(5))
                           .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending())).Render();
                %>
                <%
                                                                 });

                                                      })
                                        .ClientEvents(events => events.OnSelect("tabStripSelect"))
                                        .Render();
                %>
                <%
                            }))

                                            .RowAction(row =>
                                            {
                                                if (ViewData["SelectedRow"] != null)
                                                    if (row.DataItem.Id == (int)ViewData["SelectedRow"])
                                                    {
                                                        {
                                                            row.DetailRow.Expanded = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        row.DetailRow.Expanded = false;
                                                    }
                                            })
                                            .ClientEvents(builder =>
                                            {
                                                builder.OnRowSelect("loadPartialView");
                                                builder.OnDetailViewExpand("SetMasterRecordValue");
                                                builder.OnDetailViewCollapse("SetMasterRecordValue");
                                            })
                                            .Pageable(p => p.PageSize(5).PageTo((int)ViewData["PageTo"]))
                                            .Sortable()
                                            .Filterable()
                                            .Selectable()
                                            .Render();
                %>
            </td>
            <script type="text/javascript">

                function tabStripSelect(e) {

                    var item = $(e.item);

                    $('#result').fadeOut();

                    $.ajax({
                        url: '<%=Url.Action("SaveTabIndexSecondLevel", "Project")%>/', type: "POST",
                        data: { selectedIndex: item.index() }
                    });
                }

                function SetMasterRecordValue(e) {

                    var x = e.masterRow.cells[1].innerHTML;

                    $('#result').fadeOut('fast');

                    $('#result').load('<%: Url.Action("Index", "ProjectPhase") %>', { selectedSubRowId: x.toString() }, function () {
                        $('#result').fadeIn('slow');
                    });
                }

                function loadPartialView(e) {

                    $('> .t-hierarchy-cell > .t-icon', e.row).click();

                    $('#result').fadeOut('fast');
                }

                function PhaseTasksServiceRowSelected(e) {

                    $('> .t-hierarchy-cell > .t-icon', e.row).click();

                    $('#result').fadeOut('fast');

                    var x = e.row.cells[0].innerHTML;

                    var url = '<%: Url.Action("Index", "PhaseTask", new { selectedSubRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('slow');
                    });
                }

                
            </script>
        </tr>
    </table>
</fieldset>
