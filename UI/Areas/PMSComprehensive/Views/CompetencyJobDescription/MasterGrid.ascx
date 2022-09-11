<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="HRIS.Domain.PMS.ValueObjects.Implementation.Competency" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend">Competency Job Description Details</legend>
    <table width="100%">
        <tr>
            <td>
                <%
                    Html.Telerik().Grid<CompetencyJobDescription>("competencyJobDescriptions")
                        .Name("Grid")
                        .DataKeys(k => k.Add(o => o.Id))
                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "CompetencyJobDescription"))
                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                        .Columns(c =>
                        {
                            c.Bound(o => o.Id).Title("No").Width(1).Groupable(false).Filterable(false).Sortable(false);
                            c.Bound(o => o.JobTitle);
                            c.Bound(o => o.Weight);
                            c.Bound(o => o.Rate);
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
                                                          //Competency Section Item
                                                          items.Add().Text("Competency Section Item").Content(() =>
                                                                 {
                %>
                <%Html.Telerik().Grid(e.CompetencySectionItems)
                           .Name("CompetencySectionItemsGrid" + e.Id)
                           .DataKeys(s => s.Add(x => x.Id))
                           .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "CompetencySectionItem"))
                           .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Title("No").Groupable(false).Filterable(
                                                false).Sortable(false);
                                            columns.Bound(o => o.Name);
                                            columns.Bound(o => o.Type);
                                            columns.Bound(o => o.Level);
                                            columns.Bound(o => o.Weight);
                                            columns.Bound(o => o.Rate);
                                        })
                           .Selectable()
                           .ClientEvents(events => events.OnRowSelect("CompetencySectionItemRowSelected"))
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
                        url: '<%=Url.Action("SaveTabIndexSecondLevel", "Appraisal")%>/', type: "POST",
                        data: { selectedIndex: item.index() }
                    });
                }

                function SetMasterRecordValue(e) {

                    var x = e.masterRow.cells[1].innerHTML;

                    $('#result').fadeOut('fast');

                    $('#result').load('<%: Url.Action("Index", "CompetencyJobDescription") %>', { selectedSubRowId: x.toString() }, function () {
                        $('#result').fadeIn('slow');
                    });
                }

                function loadPartialView(e) {

                    $('> .t-hierarchy-cell > .t-icon', e.row).click();

                    $('#result').fadeOut('fast');
                }

                function CompetencySectionItemRowSelected(e) {

                    $('> .t-hierarchy-cell > .t-icon', e.row).click();

                    $('#result').fadeOut('fast');

                    var x = e.row.cells[0].innerHTML;

                    var url = '<%: Url.Action("Index", "CompetencySectionItem", new { selectedSubRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('slow');
                    });
                }

                
            </script>
        </tr>
    </table>
</fieldset>
