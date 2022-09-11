<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HRIS.Domain.ProjectManagment.ValueObjects.ProjectTeam>" %>
<%@ Import Namespace="HRIS.Domain.ProjectManagment.ValueObjects" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%: Resources.Areas.ProjectManagment.ValueObjects.ProjectTeam.ProjectTeamModel.ProjectTeamMasterGridTitle %></legend>
    <table width="100%">
        <tr>
            <td>
                <%
                    Html.Telerik().Grid<ProjectTeam>("ProjectTeams")
                        .Name("Grid")
                        .DataKeys(k => k.Add(o => o.Id))
                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "ProjectTeam"))
                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                        .Columns(c =>
                                     {
                                         c.Bound(o => o.Id).Width(1).Groupable(false).Filterable(false).Sortable(false);
                                         c.Bound(o => o.Name);
                                         c.Command(s =>
                                                       {
                                                           if (ViewData["CanDelete"] != null &&
                                                               (bool)ViewData["CanDelete"])
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
    .SelectedIndex(Session["SelectedTabIndexSecondLevel"] != null
                       ? int.Parse(Session["SelectedTabIndexSecondLevel"].ToString())
                       : 0)
    .Items(items =>
               {
                   items.Add().Text(Resources.Areas.ProjectManagment.ValueObjects.ProjectTeam.ProjectTeamModel.ProjectTeamRolePluralTitle).Content(() =>
                                                                  {
                %>
                <table>
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="AddTeamRole()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title="<%: Resources.Shared.Buttons.Function.Add %>" alt="<%: Resources.Shared.Buttons.Function.Add %>" height="24" width="24" align="middle" />
                            <script type="text/javascript">
                                function AddTeamRole() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%:Url.Action("Index", "ProjectTeamRole")%>', function () {
                                        $('#result').fadeIn('fast');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <% Html.Telerik().Grid(e.ProjectTeamRoles)
                                .Name("ProjectTeamRoles" + e.Id)
                                .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "ProjectTeamRole"))
                                .DataKeys(s => s.Add(x => x.Id))
                                .Columns(columns =>
                                             {
                                                 columns.Bound(o => o.Id).Width(1).Groupable(false).Filterable(
                                                     false).Sortable(false);
                                                 columns.Bound(o => o.Role.Name).Title(Resources.Areas.ProjectManagment.ValueObjects.ProjectTeamRole.ProjectTeamRoleModel.Role.ToLower());
                                                 columns.Bound(o => o.ParentRole.Name).Title(Resources.Areas.ProjectManagment.ValueObjects.ProjectTeamRole.ProjectTeamRoleModel.ParentRole.ToLower());
                                                 columns.Bound(o => o.Weight);
                                                 columns.Bound(o => o.Count);
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
                                .ClientEvents(events => events
                                            .OnRowSelect("TeamRolesRowSelected")
                                            .OnDetailViewExpand("OnDetailViewExpandTeamRoles"))
                                            .Selectable()
                                            .RowAction(row =>
                                            {
                                                if (ViewData["ProjectTeamRolesSelectedRow"] != null)
                                                    if (row.DataItem.Id == (int)ViewData["ProjectTeamRolesSelectedRow"])
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
                                .DetailView(teamMembersDetailsView => teamMembersDetailsView.Template(c =>
                                    {                            
                %>
                <table>
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridAddTeamMember()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title="<%: Resources.Shared.Buttons.Function.Add %>" alt="<%: Resources.Shared.Buttons.Function.Add %>" height="24" width="24" align="middle" />
                            <script type="text/javascript">
                                function GridAddTeamMember() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%:Url.Action("Index", "TeamMember")%>', function () {
                                        $('#result').fadeIn('fast');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                    <legend class="ParentLegend"><%:Resources.Areas.ProjectManagment.ValueObjects.ProjectTeam.ProjectTeamModel.TeamMemberPluralTitle%></legend>
                <%:
                                                Html.Telerik().Grid(c.TeamMembers)
                                               .Name("TeamMembers" + c.Id)
                                               .DataKeys(s => s.Add(x => x.Id))
                                               .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "TeamMember"))
                                               .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                                .Columns(columns =>
                                                {
                                                    columns.Bound(o => o.Id).Width(1).Groupable(false).Filterable(false).Sortable(false);
                                                    columns.Bound(o => o.Node.Name);
                                                    columns.Bound(o => o.Position.JobTitle.Name);
                                                    columns.Bound(o => o.Employee.FirstName);
                                                    columns.Bound(o => o.IsEvaluator);
                                                    columns.Bound(o => o.IsCross);
                                                    columns.Command(s =>
                                                    {
                                                        if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                        {
                                                            s.Delete().ButtonType(GridButtonType.Image);
                                                        }
                                                    }).Width(1); 
                                                })
                                                .ClientEvents(events => events.OnRowSelect("TeamMembersRowSelected"))
                                                .Selectable()
                                                .Pageable(pager => pager.PageSize(3))
                %>
                <%
                                    })).Render();
                                        
                                        
                                                                          
                                                                             
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
                                              builder.OnRowSelect("SetMasterRecordValue");
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
                        url: '<%=Url.Action("SaveTabIndexSecondLevel", "ProjectManagement")%>/',
                        type: "POST",
                        data: { selectedIndex: item.index() }
                    });
                }

                function SetMasterRecordValue(e) {

                    $('> .t-hierarchy-cell > .t-icon', e.row).click();
                    
                    var x = e.masterRow.cells[1].innerHTML;

                    $('#result').fadeOut('fast');

                    $('#result').load('<%:Url.Action("Index", "ProjectTeam")%>', { selectedSubRowId: x.toString() }, function () {
                        $('#result').fadeIn('slow');
                    });
                }

                function loadPartialView(e) {

                    $('> .t-hierarchy-cell > .t-icon', e.row).click();

                    $('#result').fadeOut('fast');
                }

                function TeamRolesRowSelected(e) {

                    $('> .t-hierarchy-cell > .t-icon', e.row).click();

                    $('#result').fadeOut('fast');

                    var x = e.row.cells[1].innerHTML;

                    var url = '<%: Url.Action("Index", "ProjectTeamRole", new { selectedSubRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('slow');
                    });

                }

                function TeamMembersRowSelected(e) {

                    $('#result').fadeOut('fast');

                    //$(window).scrollTop($(window).height() - $(window).scrollTop());

                    var x = e.row.cells[0].innerHTML;                   
                    
                    var url = '<%: Url.Action("Index", "TeamMember", new { selectedSubRowId = "Value1" }) %>'; //, selectedWorkingCondition = "Value2" }) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('slow');
                    });
                }


                function OnDetailViewExpandTeamRoles(e) {

                    var x = e.masterRow.cells[1].innerHTML;

                    var url = '<%: Url.Action("Index", "ProjectTeamRole", new { selectedSubRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('slow');
                    });
                }
            </script>
        </tr>
    </table>
</fieldset>
