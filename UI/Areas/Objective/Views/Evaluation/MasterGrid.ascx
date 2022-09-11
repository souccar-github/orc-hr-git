<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="HRIS.Domain.Objectives.ValueObjects" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%: Resources.Areas.Objective.ValueObjects.Evaluation.EvaluationModel.EvaluationDetails %></legend>
    <table width="100%">
        <tr>
            <td>
                <%
                    Html.Telerik().Grid<Evaluation>("evaluations")
                        .Name("Grid")
                        .DataKeys(k => k.Add(o => o.Id))
                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Evaluation"))
                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                        .Columns(c =>
                        {
                            c.Bound(o => o.Id).Title(Resources.Shared.Model.Fields.Id).Width(1).Groupable(false).Filterable(false).Sortable(false);
                            c.Bound(o => o.Evaluator.FirstName).Title( Resources.Areas.Objective.ValueObjects.Evaluation.EvaluationModel.Evaluator);
                            c.Bound(o => o.Position.JobTitle.Name).Title(Resources.Areas.Objective.ValueObjects.Evaluation.EvaluationModel.Position);
                            c.Bound(o => o.Date).Format("{0:MM/dd/yyyy}");
                            c.Bound(o => o.Quarter);
                            c.Bound(o => o.TotalEvaluationRate).Title(Resources.Areas.Objective.ValueObjects.Evaluation.EvaluationModel.TotalEvaluationRate);
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
                                                          // 1stnd Item In Tab
                                                          items.Add().Text(Resources.Areas.Objective.ValueObjects.Evaluation.EvaluationModel.EvaluatedObjectiveStep).Content(() =>
                                                          {
                %>
                <%:Html.Telerik().Grid(e.EvaluatedObjectiveSteps)
                                        .Name("EvaluatedObjectiveSteps" + e.Id)
                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "EvaluatedObjectiveStep"))
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Title(Resources.Shared.Model.Fields.Id).Width(1).Groupable(false).Filterable(false).Sortable(false);
                                            columns.Bound(o => o.Number);
                                            columns.Bound(o => o.Description);
                                            columns.Bound(o => o.Owner.FirstName).Title(Resources.Areas.Objective.ValueObjects.EvaluatedObjectiveStep.EvaluatedObjectiveStepModel.Owner);
                                            columns.Bound(o => o.Status.Name).Title(Resources.Areas.Objective.ValueObjects.EvaluatedObjectiveStep.EvaluatedObjectiveStepModel.Status);
                                            columns.Bound(o => o.EvaluationRate).Title(Resources.Areas.Objective.ValueObjects.EvaluatedObjectiveStep.EvaluatedObjectiveStepModel.EvaluationRate); ;
                                            columns.Command(s =>
                                            {
                                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                {
                                                    s.Delete().ButtonType(GridButtonType.Image);
                                                }
                                            }).Width(1);   
                                        })
                                        .ClientEvents(events => events.OnRowSelect("evaluatedObjectiveStepsRowSelected"))  
                                        .Selectable()                                  
                                        .Pageable(pager => pager.PageSize(3))
                                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
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
                        url: '<%=Url.Action("SaveTabIndexSecondLevel", "BasicObjective")%>/', type: "POST",
                        data: { selectedIndex: item.index() }
                    });
                }

                function SetMasterRecordValue(e) {

                    var x = e.masterRow.cells[1].innerHTML;

                    $('#result').fadeOut('fast');

                    $('#result').load('<%: Url.Action("Index", "Evaluation") %>', { selectedSubRowId: x.toString() }, function () {
                        $('#result').fadeIn('slow');
                    });
                }

                function loadPartialView(e) {

                    $('> .t-hierarchy-cell > .t-icon', e.row).click();

                    $('#result').fadeOut('fast');
                }

                function evaluatedObjectiveStepsRowSelected(e) {

                    $('> .t-hierarchy-cell > .t-icon', e.row).click();

                    $('#result').fadeOut('fast');

                    var x = e.row.cells[0].innerHTML;

                    var url = '<%: Url.Action("Index", "EvaluatedObjectiveStep", new { selectedSubRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('slow');
                    });
                }

                function OnDetailViewExpandEvaluatedObjectiveSteps(e) {

                    var x = e.masterRow.cells[0].innerHTML;

                    var url = '<%: Url.Action("Index", "EvaluatedObjectiveStep", new { selectedSubRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('slow');
                    });
                }
            </script>
        </tr>
    </table>
</fieldset>
