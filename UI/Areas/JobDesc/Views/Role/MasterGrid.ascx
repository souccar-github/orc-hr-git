<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="HRIS.Domain.JobDesc.ValueObjects" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%:Resources.Areas.JobDesc.ValueObjects.Role.RoleModel.RoleMasterGridTitle %></legend>
    <table width="100%">
        <tr>
            <td>
                <%
                    Html.Telerik().Grid<Role>("roles")
                        .Name("Grid")
                        .DataKeys(k => k.Add(o => o.Id))
                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Role"))
                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                        .Columns(c =>
                        {
                            c.Bound(o => o.Id).Width(1).Groupable(false).Filterable(false).Sortable(false);
                            c.Bound(o => o.Name);
                            c.Bound(o => o.Priority.Name);
                            //c.Bound(o => o.JobRole.Name).Title(Resources.Areas.JobDesc.ValueObjects.Role.RoleModel.JobRole);
                            c.Bound(o => o.Summary);
                            c.Command(s =>
                                            {
                                                s.Select().ButtonType(GridButtonType.Image).HtmlAttributes(new { @class = "MasterGridSelect" });
                                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                {
                                                    s.Delete().ButtonType(GridButtonType.Image);
                                                }
                                            }).Width("7%");
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
                                                          // 1st Item In Tab
                                                          items.Add().Text(Resources.Areas.JobDesc.ValueObjects.Role.RoleModel.ResponsibilityMasterGridTitle).Content(() =>
                                    {
                %>
                <table>
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="AddResponsibility()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title="<%: Resources.Shared.Buttons.Function.Add %>" alt="<%: Resources.Shared.Buttons.Function.Add %>" height="24" width="24" align="middle" />
                            <script type="text/javascript">
                                function AddResponsibility() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "Responsibility") %>', function () {
                                        $('#result').fadeIn('fast');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <% Html.Telerik().Grid(e.Responsibilities)
                                         .Name("Responsibilities" + e.Id)
                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Responsibility"))
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Groupable(false).Filterable(false).Sortable(false);
                                            columns.Bound(o => o.Description).Width("70%");
                                            columns.Bound(o => o.Priority.Name).Width("15%");
                                            columns.Bound(o => o.Weight).Width("15%");
                                            columns.Command(s =>
                                            {
                                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                {
                                                    s.Delete().ButtonType(GridButtonType.Image);
                                                }
                                            }).Width(1);
                                        })
                                        .ClientEvents(events => events
                                            .OnRowSelect("responsibilitiesRowSelected")
                                            .OnDetailViewExpand("OnDetailViewExpandResponsibilities"))
                                            .Selectable()
                                            .RowAction(row =>
                                            {
                                                if (ViewData["ResponsibilitySelectedRow"] != null)
                                                    if (row.DataItem.Id == (int)ViewData["ResponsibilitySelectedRow"])
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
                                        .Pageable(pager => pager.PageSize(3))
                                         .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                        .DetailView(kpiDetailsView => kpiDetailsView.Template(c =>
                                    {                            
                %>
                <fieldset class="ParentFieldset">
                    <legend class="ParentLegend"><%: Resources.Areas.JobDesc.ValueObjects.Responsibility.ResponsibilityModel.ResponsibilityKpiTitle %></legend>
                    <table>
                        <tr>
                            <td>
                                <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                                   {%>
                                <img alt="<%: Resources.Shared.Buttons.Function.Add %>" onclick="AddresponsibilitKPI()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png")%>"
                                    height="24" width="24" />
                                <script type="text/javascript">
                                    function AddresponsibilitKPI() {
                                        $('#result').fadeOut('fast');
                                        $('#result').load('<%: Url.Action("Index", "ResponsibilityKpi")%>', function () {
                                            $('#result').fadeIn('fast');
                                        });
                                    }
                                </script>
                                <% } %>
                            </td>
                        </tr>
                    </table>
                    <%:
                                                Html.Telerik().Grid(c.ResponsibilityKpis)
                                               .Name("ResponsibilityKpis" + c.Id)
                                               .DataKeys(s => s.Add(x => x.Id))
                                               .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "ResponsibilityKpi"))
                                               .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                                .Columns(columns =>
                                                {
                                                    columns.Bound(o => o.Id).Width(1).Groupable(false).Filterable(false).Sortable(false);
                                                    columns.Bound(o => o.Value);
                                                    columns.Bound(o => o.Description);
                                                    columns.Command(s =>
                                                    {
                                                        if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                        {
                                                            s.Delete().ButtonType(GridButtonType.Image);
                                                        }
                                                    }).Width(1); 
                                                })
                                                .ClientEvents(events => events.OnRowSelect("ResponsibilityKpisRowSelected"))
                                                .Selectable()
                                                .Pageable(pager => pager.PageSize(3))
                    %>
                    <%
                                    })).Render();
                                        
                                        
                                                                          
                                                                             
                    %>
                </fieldset>
                <%
                                    });

                                                          // 2nd Item In Tab
                                                          items.Add().Text(Resources.Areas.JobDesc.ValueObjects.Role.RoleModel.RoleKpiMasterGridTitle).Content(() =>
                                                          {
                %>
                <table>
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="AddRoleKpi()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title="<%: Resources.Shared.Buttons.Function.Add %>" alt="<%: Resources.Shared.Buttons.Function.Add %>" height="24" width="24" align="middle" />
                            <script type="text/javascript">
                                function AddRoleKpi() {
                                    $('#result').fadeOut('fast');
                                    $('#result').load('<%: Url.Action("Index", "RoleKpi") %>', function () {
                                        $('#result').fadeIn('fast');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%:Html.Telerik().Grid(e.RoleKpis)
                                        .Name("RoleKpis" + e.Id)
                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "RoleKpi"))
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Groupable(false).Filterable(false).Sortable(false);
                                            columns.Bound(o => o.Value);
                                            columns.Command(s =>
                                            {
                                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                {
                                                    s.Delete().ButtonType(GridButtonType.Image);
                                                }
                                            }).Width(1);   
                                        })
                                        .ClientEvents(events => events.OnRowSelect("roleKpisRowSelected"))  
                                        .Selectable()                                  
                                        .Pageable(pager => pager.PageSize(3))
                                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                %>
                <%
                                                          });
                                                          // 3rd Item In Tab
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
                        url: '<%=Url.Action("SaveTabIndexSecondLevel", "JobDescEntity")%>/', type: "POST",
                        data: { selectedIndex: item.index() }
                    });
                }

                function SetMasterRecordValue(e) {


                    $('#result').fadeOut('fast');
                    var grid = $('#Grid').data('tGrid');
                    var indexOfIdColumn = GetGridColumnIndexByName(grid, "Id", e);
                    var idValue = e.masterRow.cells[indexOfIdColumn].innerHTML;
                    alert(idValue);
                    
                    var x = e.masterRow.cells[1].innerHTML;

                    $('#result').fadeOut('fast');

                    $('#result').load('<%: Url.Action("Index", "Role") %>', { selectedSubRowId: x.toString() }, function () {
                        $('#result').fadeIn('slow');
                    });
                }

                function loadPartialView(e) {

                    $('> .t-hierarchy-cell > .t-icon', e.row).click();

                    $('#result').fadeOut('fast');
                }

                function responsibilitiesRowSelected(e) {

                    $('> .t-hierarchy-cell > .t-icon', e.row).click();

                    $('#result').fadeOut('fast');
                    var grid = $('#Grid').data('tGrid');
                    var indexOfIdColumn = GetGridColumnIndexByName(grid, "Id", e);
                    var idValue = e.row.cells[indexOfIdColumn].innerHTML;
                    alert(idValue);

                    var x = e.row.cells[1].innerHTML;

                    var url = '<%: Url.Action("Index", "Responsibility", new { selectedSubRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('slow');
                    });
                }

                function ResponsibilityKpisRowSelected(e) {

                    $('#result').fadeOut('fast');
                    var grid = $('#Grid').data('tGrid');
                    var indexOfIdColumn = GetGridColumnIndexByName(grid, "Id", e);
                    var idValue = e.row.cells[indexOfIdColumn].innerHTML;
                    alert(idValue);

                    //$(window).scrollTop($(window).height() - $(window).scrollTop());

                    var x = e.row.cells[0].innerHTML;

                    var url = '<%: Url.Action("Index", "ResponsibilityKpi", new { selectedSubRowId = "Value1" }) %>'; //, selectedWorkingCondition = "Value2" }) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('slow');
                    });
                }

                function roleKpisRowSelected(e) {

                    $('#result').fadeOut('fast');

                    var x = e.row.cells[0].innerHTML;

                    var url = '<%: Url.Action("Index", "RoleKpi", new { selectedSubRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('slow');
                    });
                }

                function OnDetailViewExpandResponsibilities(e) {

                    var x = e.masterRow.cells[1].innerHTML;

                    var url = '<%: Url.Action("Index", "Responsibility", new { selectedSubRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('slow');
                    });
                }
            </script>
        </tr>
    </table>
</fieldset>
