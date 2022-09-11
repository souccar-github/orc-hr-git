<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="HRIS.Domain.Objectives.RootEntities" %>
<%@ Import namespace="Souccar.Core.Extensions" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%: Resources.Areas.Objective.Entities.BasicObjective.BasicObjectiveModel.ObjectiveList %></legend>
    <table width="100%">
        <tr>
            <td>
                <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                   {%>
                <a href="<%:Url.Action("Insert", "Objective")%>">
                    <input type="button" value="<%: Resources.Shared.Buttons.Function.Add %>" />
                </a>
                <br />
                <br />
                <% } %>
                <%

                    Html.Telerik().Grid<Objective>("objectives")
                        .Name("Grid")
                        .DataKeys(k => k.Add(o => o.Id))
                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Objective"))
                        .Filterable()
                        .Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                        .Columns(c =>
                                     {
                                         c.Bound(o => o.Id).Title(Resources.Shared.Model.Fields.Id).Width(1).Groupable(false).Filterable(false).
                                             Sortable(false);
                                         c.Bound(o => o.Name).Width(15);
                                         c.Bound(o => o.Code.Code);                                         
                                         c.Bound(o => o.Weight);
                                       //  c.Bound(o => o.Priority.GetDescription());//.Title(Resources.Areas.JobDesc.Indexes.Priority.PriorityModel.Name);
                                         c.Bound(o => o.Owner.Name);//.Title(Resources.Areas.Objective.Entities.BasicObjective.BasicObjectiveModel.Employee);
                                        
                                         c.Command(s =>
                                                       {
                                                           //s.Select().ButtonType(GridButtonType.Image);
                                                           if (ViewData["CanDelete"] != null &&
                                                               (bool)ViewData["CanDelete"])
                                                           {
                                                               s.Delete().ButtonType(GridButtonType.Image);
                                                           }
                                                       }).Width(1);
                                     }).DetailView(detailView => detailView.Template(e =>
                                                                                         {
                %>
                <% Html.Telerik().TabStrip()
                            .Name("TabStrip_" + e.Id)
                            .Effects(fx => fx.Opacity())
                            .SelectedIndex(Session["SelectedTabIndex"] != null ? int.Parse(Session["SelectedTabIndex"].ToString()) : 111)
                            .Items(items =>
                                       {
                                                                                                         


                   //ObjectiveKpi
                                           items.Add().Text("Objective Kpi").Content(() =>
                                                                 {
                %>
                <table>
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="button" value="<%: Resources.Shared.Buttons.Function.Add %>" onclick=" GridAddObjectiveKpi(); " />
                            <script type="text/javascript">
                                function GridAddObjectiveKpi() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%:Url.Action("Index", "ObjectiveKpi")%>', function () {
                                        $('#result').fadeIn('fast');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%:Html.Telerik().Grid(e.Kpis)
                           .Name("ObjectiveKpi" + e.Id)
                           .DataKeys(s => s.Add(x => x.Id))
                           .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "ObjectiveKpi"))
                           .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Groupable(false).Filterable(
                                                false).Sortable(false);
                                            columns.Bound(o => o.Type.Name);
                                            columns.Bound(o => o.Value);
                                            columns.Bound(o => o.Weight);
                                            columns.Command(s =>
                                                                {
                                                                    if (ViewData["CanDelete"] != null &&
                                                                        (bool) ViewData["CanDelete"])
                                                                    {
                                                                        s.Delete().ButtonType(GridButtonType.Image);
                                                                    }
                                                                }).Width(1);
                                        })
                           .Selectable()
                           .ClientEvents(events => events.OnRowSelect("ObjectiveKpiServiceRowSelected"))
                           .Pageable(pager => pager.PageSize(5))
                           .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                %>
                <%
                                                                 });
                   //End ObjectiveKpi


                   //SharedWith
                   items.Add().Text("SharedWith").Content(() =>
                                                               {
                %>
                                <table>
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="button" value="<%: Resources.Shared.Buttons.Function.Add %>" onclick=" GridAddSharedWith(); " />
                            <script type="text/javascript">
                                function GridAddSharedWith() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%:Url.Action("Index", "SharedWith")%>', function () {
                                        $('#result').fadeIn('fast');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%:Html.Telerik().Grid(e.SharedWiths)
                           .Name("SharedWithGrid" + e.Id)
                           .DataKeys(s => s.Add(x => x.Id))
                           .Filterable()
                           .Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                           .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "SharedWith"))
                           .Columns(c =>
                                        {
                                            c.Bound(o => o.Id).Title(Resources.Shared.Model.Fields.Id).Width(1).Groupable(false).Filterable(false).
                                                Sortable(false);
                                            c.Bound(o => o.Position.Node.Code);
                                            c.Bound(o => o.Position.Code);
                                           // c.Bound(o => o..FirstName).Title(Resources.Areas.Objective.Entities.BasicObjective.BasicObjectiveModel.Employee);
                                            c.Bound(o => o.Percentage);
                                            c.Command(s =>
                                                          {
                                                              if (ViewData["CanDelete"] != null &&
                                                                  (bool) ViewData["CanDelete"])
                                                              {
                                                                  s.Delete().ButtonType(GridButtonType.Image);
                                                              }
                                                          }).Width(1);
                                        })
                           .Selectable()
                           .ClientEvents(events => events.OnRowSelect("SharedWithServiceRowSelected"))
                           .Pageable(pager => pager.PageSize(5))
                %>
                <%
                                                               });
                   //End SharedWith    
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
                       .Pageable(p => p.PageSize(1).PageTo((int)ViewData["PageTo"]))
                       .Selectable()
                       .Render();

                %>
                <script type="text/javascript">

                    function tabStripSelect(e) {

                        var item = $(e.item);

                        $('#result').fadeOut('fast');

                        $.ajax({
                            url: '<%:Url.Action("SaveTabIndex", "Objective")%>/',
                            type: "POST",
                            data: { selectedIndex: item.index() }
                        });
                    }


                    function SetMasterRecordValue(e) {

                        var x = e.masterRow.cells[1].innerHTML;


                        $('#result').load('<%:Url.Action("PartialMasterInfo", "Objective")%>', { selectedRowId: x }, function () {
                            $('#result').fadeIn('fast');

                            loadRibbon();
                        });
                    }


                    function loadPartialView(e) {

                        $('> .t-hierarchy-cell > .t-icon', e.row).click();

                        $('#result').fadeOut('fast');
                    }

                    function loadRibbon() {

                        $('#ObjectiveFunctionsArea').load('<%:Url.Action("GetFunctionsPartial", "ObjectiveModule")%>');
                    }

                  

              
                    function ObjectiveKpiServiceRowSelected(e) {

                        $('#result').fadeOut('fast');

                        var x = e.row.cells[0].innerHTML;

                        var url = '<%:Url.Action("Index", "ObjectiveKpi", new {selectedSubRowId = "Value1"})%>';
                        url = url.replace("Value1", x);

                        $('#result').load(url, function () {
                            $('#result').fadeIn('fast');
                        });
                    }


                    function SharedWithServiceRowSelected(e) {

                     
                        $('#result').fadeOut('fast');

                        var x = e.row.cells[0].innerHTML;

                        var url = '<%:Url.Action("Index", "SharedWith", new {selectedSubRowId = "Value1"})%>';
                        url = url.replace("Value1", x);

                        $('#result').load(url, function () {
                            $('#result').fadeIn('fast');
                        });
                    }

                </script>
            </td>
        </tr>
    </table>
</fieldset>
