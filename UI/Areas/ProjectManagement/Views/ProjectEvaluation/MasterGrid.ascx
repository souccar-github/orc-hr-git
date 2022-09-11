<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="HRIS.Domain.ProjectManagment.ValueObjects" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%: Resources.Areas.ProjectManagment.ValueObjects.ProjectEvaluation.ProjectEvaluationModel.ProjectEvaluationMasterGridTitle %></legend>
    <table width="100%">
        <tr>
            <td>
                <%
                    Html.Telerik().Grid<ProjectEvaluation>("evaluations")
                        .Name("Grid")
                        .DataKeys(k => k.Add(o => o.Id))
                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "ProjectEvaluation"))
                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                        .Columns(c =>
                        {
                            c.Bound(o => o.Id).Width(1).Groupable(false).Filterable(false).Sortable(false);
                            c.Bound(o => o.Evaluator.FirstName);
                            c.Bound(o => o.EvaluatorProjectRole).Title(Resources.Areas.ProjectManagment.ValueObjects.ProjectEvaluation.ProjectEvaluationModel.EvaluatorProjectRole);
                            c.Bound(o => o.Date); 
                            c.Bound(o => o.Quarter); 
                            c.Bound(o => o.CompletionPercentage); 
                            c.Bound(o => o.TotalProjectRate);
                            c.Bound(o => o.Status.Name);
                            c.Command(s =>
                            {
                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                {
                                    s.Delete().ButtonType(GridButtonType.Image);
                                }
                            }).Width(1);
                        })

                        .DetailView(detailView => detailView.Template(e =>
                            {
                %>
                <% Html.Telerik().TabStrip()
                                                      .Name("TabStrip_" + e.Id)
                                                      .Effects(fx => fx.Opacity())
                                                      .SelectedIndex(0)
                                                      .Items(items =>
                                                      {
                                                          // 1stnd Item In Tab
                                                          items.Add().Text(Resources.Areas.ProjectManagment.ValueObjects.ProjectEvaluation.ProjectEvaluationModel.EvaluatedPhasePluralTitle).Content(() =>
                                                          {
                %>
                <% Html.Telerik().Grid(e.EvaluatedPhases)
                                        .Name("EvaluatedPhase" + e.Id)
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "EvaluatedPhase"))
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Groupable(false).Filterable(false).Sortable(false);
                                            columns.Bound(o => o.Name);
                                            columns.Bound(o => o.Team.Name);
                                            columns.Bound(o => o.TeamRole.Role.Name);
                                            columns.Bound(o => o.StartDate);
                                            columns.Bound(o => o.EndDate);
                                            columns.Bound(o => o.Status.Name);
                                            columns.Bound(o => o.CompletionPercentage);
                                            columns.Bound(o => o.TotalPhaseRate);
                                            columns.Command(s =>
                                            {
                                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                {
                                                    s.Delete().ButtonType(GridButtonType.Image);
                                                }
                                            }).Width(1);
                                        })
                                        .RowAction(row =>
                                        {
                                            if (ViewData["EvaluatedPhaseSelectedRow"] != null)
                                                if (row.DataItem.Id == (int)ViewData["EvaluatedPhaseSelectedRow"])
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
                                        .ClientEvents(events => {
                                            events.OnRowSelect("EvaluatedPhaseRowSelected");
                                            events.OnDetailViewExpand("EvaluatedPhaseExpandCollapse");
                                            //events.OnDetailViewCollapse("EvaluatedPhaseRowSelected"); 
                                        })
                                        .Selectable()
                                        .Pageable(pager => pager.PageSize(3))
                                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                        .DetailView(EvaluatedTaskDetailsView => EvaluatedTaskDetailsView.Template(c =>
                                        {
                %>
                <% Html.Telerik().TabStrip()
                                                      .Name("TabStrip_" + c.Id)
                                                      .Effects(fx => fx.Opacity())
                                                      .SelectedIndex(0)
                                                      .Items(item =>
                                                      {
                                                          // 1stnd Item In Tab
                                                          item.Add().Text(Resources.Areas.ProjectManagment.ValueObjects.ProjectEvaluation.ProjectEvaluationModel.EvaluatedTaskPluralTitle).Content(() =>
                                                          {
                %>
                <%: Html.Telerik().Grid(c.EvaluatedTasks)
                                                            .Name("EvaluatedTask" + c.Id)
                                                            .DataKeys(s => s.Add(x => x.Id))
                                                            .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "EvaluatedTask"))
                                                            .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                                            .Columns(columns =>
                                                            {
                                                                columns.Bound(o => o.Id).Width(1).Groupable(false).Sortable(false).Filterable(false);
                                                                columns.Bound(o => o.Weight);
                                                                columns.Bound(o => o.Team.Name);
                                                                columns.Bound(o => o.TeamRole.Role.Name);
                                                                columns.Bound(o => o.DeadLine);
                                                                columns.Bound(o => o.ActualClosingDate);
                                                                columns.Bound(o => o.Status.Name).Title(Resources.Areas.ProjectManagment.ValueObjects.EvaluatedTask.EvaluatedTaskModel.Status);
                                                                columns.Bound(o => o.Rate);
                                                                columns.Command(s =>
                                                                {
                                                                    if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                                    {
                                                                        s.Delete().ButtonType(GridButtonType.Image);
                                                                    }
                                                                }).Width(1);
                                                            })
                                                            .Selectable()
                                                            .ClientEvents(events => events.OnRowSelect("EvaluatedTaskRowSelected"))
               
                %>
                <%
                                                          });
                                                      }).Render();
                                        })).Render();                                                           
                %>
                <%
                                                          });

                                                      })
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
        </tr>
    </table>
</fieldset>
<script type="text/javascript">
    function SetMasterRecordValue(e) {

        var x = e.masterRow.cells[1].innerHTML;

        $('#result').fadeOut('fast');

        $('#result').load('<%: Url.Action("Index", "ProjectEvaluation") %>', { selectedSubRowId: x.toString() }, function () {
            $('#result').fadeIn('slow');
        });
    }

    function loadPartialView(e) {

        $('> .t-hierarchy-cell > .t-icon', e.row).click();

        $('#result').fadeOut('fast');
    }

    function EvaluatedPhaseExpandCollapse(e) {
        var x = e.masterRow.cells[1].innerHTML;

        $('#result').fadeOut('fast');

        $('#result').load('<%: Url.Action("Index", "EvaluatedPhase") %>', { selectedSubRowId: x.toString() }, function () {
            $('#result').fadeIn('slow');
        });
    }

    function EvaluatedPhaseRowSelected(e) {

        $('> .t-hierarchy-cell > .t-icon', e.row).click();

        $('#result').fadeOut('fast');

        var x = e.row.cells[1].innerHTML;

        var url = '<%: Url.Action("Index", "EvaluatedPhase", new { selectedSubRowId = "Value1"}) %>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('slow');
        });
    }

    function EvaluatedTaskRowSelected(e) {

        $('> .t-hierarchy-cell > .t-icon', e.row).click();

        $('#result').fadeOut('fast');

        var x = e.row.cells[0].innerHTML;

        var url = '<%: Url.Action("Index", "EvaluatedTask", new { selectedSubRowId = "Value1"}) %>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('slow');
        });
    }        
</script>
