<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="HRIS.Domain.JobDesc.Entities" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend">
        <%:Resources.Areas.JobDesc.Entities.JobDescription.JobDescriptionModel.JobDescMasterGridTitle %></legend>
    <table width="100%">
        <tr>
            <td>
                <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                   {%>
                <a href="<%: Url.Action("Insert", "JobDescEntity") %>">
                    <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>" title='<%: Resources.Shared.Buttons.Function.Add %>'
                        alt="<%: Resources.Shared.Buttons.Function.Add %>" height="36" width="36" align="middle" />
                </a>
                <% } %>
                <%
                    Html.Telerik().Grid<JobDescription>("jobDescriptions")
                        .Name("Grid")
                        .DataKeys(k => k.Add(o => o.Id))
                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "JobDescEntity"))
                        .Filterable()
                        .Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                        .Columns(c =>
                        {
                            c.Bound(o => o.Id).Width(1).Groupable(false).Filterable(false).Sortable(false);
                            //c.Bound(o => o.JobRole.Name).Title(Resources.Areas.JobDesc.Entities.JobDescription.JobDescriptionModel.JobRole);
                            c.Bound(o => o.JobTitle.Name).Title(Resources.Areas.JobDesc.Entities.JobDescription.JobDescriptionModel.JobTitle);
                            c.Bound(o => o.Summary);
                            
                            c.Command(s =>
                                            {
                                                s.Select().ButtonType(GridButtonType.Image);//.HtmlAttributes(new { @class = "MasterGridSelect" });
                                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                {
                                                    s.Delete().ButtonType(GridButtonType.Image);
                                                }
                                            }).Width("7%");
                        })
                        .DetailView(detailView => detailView.Template(e =>
                            {
                %>
                <% Html.Telerik().TabStrip()
                                    .Name("TabStrip_" + e.Id)
                                    .Effects(fx => fx.Opacity())
                                    .SelectedIndex(Session["SelectedTabIndex"] != null ? int.Parse(Session["SelectedTabIndex"].ToString()) : 0)
                                    .Items(items =>
                                    {
                                        // 1st Item In Tab
                                        items.Add().Text(Resources.Areas.JobDesc.Entities.JobDescription.JobDescriptionModel.AuthorityTitle).Content(() =>
                                    {
                %>
                <table>
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridAddAuthority()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title="<%: Resources.Shared.Buttons.Function.Add %>" alt="<%: Resources.Shared.Buttons.Function.Add %>"
                                height="24" width="24" align="middle" />
                            <script type="text/javascript">
                                function GridAddAuthority() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "Authority") %>', function () {
                                        $('#result').fadeIn('fast');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%:Html.Telerik().Grid(e.Authorities)
                                        .Name("Authorities" + e.Id)
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Authority")) 
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Groupable(false).Sortable(false).Filterable(false);
                                            columns.Bound(o => o.Type.Name);
                                            columns.Bound(o => o.Title);
                                            columns.Command(s =>
                                            {
                                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                {
                                                    s.Delete().ButtonType(GridButtonType.Image);
                                                }
                                            }).Width(1);
                                        })
                                        .ClientEvents(events => events.OnRowSelect("authoritiesRowSelected")).Selectable()                                  
                                        .Pageable(pager => pager.PageSize(3))
                                         .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                %>
                <%
                                    });

                                        // 2nd Item In Tab
                                        items.Add().Text(Resources.Areas.JobDesc.Entities.JobDescription.JobDescriptionModel.RoleTitle).Content(() =>
                                        {%>
                <table>
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridAddRole()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title="<%: Resources.Shared.Buttons.Function.Add %>" alt="<%: Resources.Shared.Buttons.Function.Add %>"
                                height="24" width="24" align="middle" />
                            <script type="text/javascript">
                                function GridAddRole() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "Role") %>', function () {
                                        $('#result').fadeIn('fast');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%:Html.Telerik().Grid(e.Roles)
                                        .Name("Roles" + e.Id)
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Role"))    
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Groupable(false).Sortable(false).Filterable(false);
                                            columns.Bound(o => o.Name);
                                            columns.Bound(o => o.Priority.Name);
                                            //columns.Bound(o => o.JobRole.Name).Title("Job Role/Specialization");
                                            columns.Command(s =>
                                                                {
                                                                    //s.Select().ButtonType(GridButtonType.Image).HtmlAttributes(Html.Action("MasterIndex", "Role"));
                                                                    if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                                    {
                                                                        s.Delete().ButtonType(GridButtonType.Image);
                                                                    }
                                                                }).Width(1);
                                        })
                                        .ClientEvents(events => events.OnRowSelect("rolesRowSelected"))  
                                        .Selectable()                                  
                                        .Pageable(pager => pager.PageSize(3))
                                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                                                             
                %>
                <%});

                                        // 3rd Item In Tab
                                        items.Add().Text(Resources.Areas.JobDesc.Entities.JobDescription.JobDescriptionModel.SpecificationTitle).Action("GoToSpecifications", "JobDescEntity", new { area = "JobDesc" }).Content(() =>
                                        { %>
                <table>
                    <tr>
                        <td>
                            <a href="<%= Url.Action("GoToSpecifications", "JobDescEntity", new { area = "JobDesc" }) %>">
                                <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/13.png") %>" title="<%: Resources.Shared.Buttons.Function.GoToDetails %>"
                                    alt="<%: Resources.Shared.Buttons.Function.GoToDetails %>" height="24" width="24"
                                    align="middle" />
                            </a>
                        </td>
                    </tr>
                </table>
                <%});

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
            </td>
            <script type="text/javascript">

                function tabStripSelect(e) {

                    var item = $(e.item);

                    $('#result').fadeOut('fast');

                    $.ajax({
                        url: '<%: Url.Action("SaveTabIndex", "JobDescEntity")%>/', type: "POST",
                        data: { selectedIndex: item.index() }
                    });
                }

                function SetMasterRecordValue(e) {


                    var x = e.masterRow.cells[1].innerHTML;
                    //                    alert(x);

                    $('#result').fadeOut('fast');
                    //                    var grid = $('#Grid').data('tGrid');
                    //                    var indexOfIdColumn = GetGridColumnIndexByName(grid, "Id", e);
                    //                    var idValue = e.masterRow.cells[indexOfIdColumn].innerHTML;
                    //                    alert(idValue);

                    $('#result').load('<%: Url.Action("PartialMasterInfo", "JobDescEntity") %>', { selectedRowId: x }, function () {
                        $('#result').fadeIn('fast');

                        loadRibbon();
                    });
                }

                function loadPartialView(e) {

                    $('> .t-hierarchy-cell > .t-icon', e.row).click();

                    $('#result').fadeOut('fast');
                }

                function authoritiesRowSelected(e) {

                    $('#result').fadeOut('fast');
                    //                    var grid = $('#Grid').data('tGrid');
                    //                    var indexOfIdColumn = GetGridColumnIndexByName(grid, "Id", e);
                    //                    var idValue = e.row.cells[indexOfIdColumn].innerHTML;
                    //                    alert(idValue);

                    var x = e.row.cells[0].innerHTML;

                    var url = '<%: Url.Action("Index", "Authority", new { selectedSubRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('fast');
                        loadRibbon();
                    });

                }

                function rolesRowSelected(e) {

                    $('#result').fadeOut('fast');
                    //                    var grid = $('#Grid').data('tGrid');
                    //                    var indexOfIdColumn = GetGridColumnIndexByName(grid, "Id", e);
                    //                    var idValue = e.row.cells[indexOfIdColumn].innerHTML;
                    //                    alert(idValue);

                    var x = e.row.cells[0].innerHTML;

                    var url = '<%: Url.Action("Index", "Role", new { selectedSubRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('fast');
                        loadRibbon();
                    });

                }

                function loadRibbon() {
                    $('#JobFunctionsArea').load('<%: Url.Action("GetFunctionsPartial", "JobDesc") %>');
                }
            </script>
        </tr>
    </table>
</fieldset>
