<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Model.PMS.ValueObjects.Implementation.Project.ProjectSection>" %>

                                   //Project Section
                                   items.Add().Text("Project Section").Content(() =>
                                                                                 {
                %>
                <% Html.Telerik().Grid(e.ProjectSections)
                           .Name("ProjectPhasesGrid" + e.Id)
                           .DataKeys(s => s.Add(x => x.Id))
                           .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Title("No").Groupable(false).Filterable(
                                                false).Sortable(false);
                                            columns.Bound(o => o.Name);
                                            columns.Bound(o => o.Weight);
                                            columns.Bound(o => o.TotalRate);
                                            columns.Bound(o => o.Id).Title("")
                                                   .Format(Html.ActionLink("Details", "MasterIndex", "ProjectSection", new { id = "{0}" }, null).ToHtmlString())
                                                   .Encoded(false)
                                                   .Width(1).Sortable(false).Filterable(false);
                                        })
                           .Selectable()
                           .ClientEvents(events => events.OnRowSelect("ProjectSectionRowSelected"))
                           .Pageable(pager => pager.PageSize(5))
                           .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                                      .DetailView(projectSectionItemsDetailsView => projectSectionItemsDetailsView.Template(c =>
                                    {
                                        Html.Telerik().TabStrip()
                                                     .Name("TabStrip_" + c.Id)
                                                     .Effects(fx => fx.Opacity())
                                                     .SelectedIndex(0)
                                                     .Items(projectSectionItems =>
                                                     {
                                                         // 1st Item In Tab
                                                         projectSectionItems.Add().Text("Project Section Item").Content(() =>
                                   {
                                       
                %>
                <%
                                       
                                       Html.Telerik().Grid(c.ProjectSectionItems)
                                      .Name("ProjectSectionItems" + c.Id)
                                      .DataKeys(s => s.Add(x => x.Id))
                                      .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "ProjectSectionItem"))

                              .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                               .Columns(columns =>
                               {
                                   columns.Bound(o => o.Id).Title("No").Width(1).Groupable(false).Filterable(false).Sortable(false);
                                   columns.Bound(o => o.TaskKpi);
                                   columns.Bound(o => o.Rate);
                               })
                              .ClientEvents(events => events.OnRowSelect("ProjectSectionItemRowSelected"))
                               .Selectable()
                               .Pageable(pager => pager.PageSize(3))
                               .Render();
                                   });
                                                     }).Render();
                %>
                <%
                                    })).Render();
                %>
                <%
                                                                                 }).Visible(true);

                                   //End Project Section    