<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%: Resources.Areas.ProjectManagment.Entities.Project.ProjectModel.ProjectMasterGridTitle %></legend>
    <table width="100%">
        <tr>
            <td>
                <%
                 
                    Html.Telerik().Grid<Model.ProjectManagment.Entities.Project>("projects")
                        .Name("Grid")
                        .DataKeys(k => k.Add(o => o.Id))
                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Project"))
                        .DataBinding(dataBinding => dataBinding.Server().Insert("Insert", "Project"))
                        .Scrollable(builder => builder.Height(350))
                        .ToolBar(toolBar => toolBar.Template(() =>
                        { 
                %>
                <table class="GridToolBar">
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <a href="<%:Url.Action("Insert", "Project")%>">
                                <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>" title="<%: Resources.Shared.Buttons.Function.Add %>"
                                    alt="<%: Resources.Shared.Buttons.Function.Add %>" height="36" width="36" align="middle" />
                            </a>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <% 
                        }))
                        .Filterable()
                        .Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                        .Columns(c =>
                                     {
                                         c.Bound(b => b.Id).Width(35).Title(Resources.Shared.Model.Fields.Id).Groupable(false).Filterable(false).Sortable(false);
                                         c.Bound(o => o.Code);
                                         c.Bound(o => o.Name);
                                         c.Bound(o => o.Type.Name).Title(Resources.Areas.ProjectManagment.Entities.Project.ProjectModel.Type);
                                         c.Bound(o => o.Node.Name).Title(Resources.Areas.ProjectManagment.Entities.Project.ProjectModel.Node);
                                         c.Bound(o => o.Owner.FirstName).Title(Resources.Areas.ProjectManagment.Entities.Project.ProjectModel.Owner);
                                         c.Bound(o => o.Position.JobTitle.Name).Title(Resources.Areas.ProjectManagment.Entities.Project.ProjectModel.Position);
                                         
                                         c.Bound(o => o.PlannedStartingDate);
                                         c.Bound(o => o.PlannedClosingDate);
                                         c.Command(s =>
                                                       {
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

                                   //Project Team
                                   items.Add().Text(Resources.Areas.ProjectManagment.Entities.Project.ProjectModel.ProjectTeamPluralTitle).Content(() =>
                                                                                {
                %>
                <% Html.Telerik().Grid(e.ProjectTeams)
                           .Name("ProjectTeam" + e.Id)
                           .DataKeys(s => s.Add(x => x.Id))
                           .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "ProjectTeam"))
                           .ToolBar(toolBar => toolBar.Template(() =>
                            { 
                %>
                <table class="GridToolBar">
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridAddProjectTeam()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title="<%: Resources.Shared.Buttons.Function.Add %>" alt="<%: Resources.Shared.Buttons.Function.Add %>" height="24" width="24" align="middle" />
                            <script type="text/javascript">
                                function GridAddProjectTeam() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%:Url.Action("Index", "ProjectTeam")%>', function () {
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
                                            columns.Bound(b => b.Id).Width(35).Title(Resources.Shared.Model.Fields.Id).Groupable(false).Filterable(false).Sortable(false);
                                            columns.Bound(o => o.Name);

                                            columns.Bound(o => o.Id).Title("")
                                                   .Format(Html.ActionLink(Resources.Shared.Buttons.Function.Details, "MasterIndex", "ProjectTeam", new { id = "{0}" }, null).ToHtmlString())
                                                   .Encoded(false)
                                                   .Width("5%").Sortable(false).Filterable(false);

                                            columns.Command(s =>
                                                                {
                                                                    if (ViewData["CanDelete"] != null &&
                                                                        (bool)ViewData["CanDelete"])
                                                                    {
                                                                        s.Delete().ButtonType(GridButtonType.Image);
                                                                    }
                                                                }).Width(1);
                                        })
                            .ClientEvents(events => events.OnRowSelect("ProjectTeamRowSelected"))
                           .Selectable()
                           .Pageable(pager => pager.PageSize(5))
                           .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending())).Render();
                %>
                <%  
                                                                                });

                                   //End Project Team    

                                   //Project Phase
                                   items.Add().Text(Resources.Areas.ProjectManagment.Entities.Project.ProjectModel.ProjectPhasePluralTitle).Content(() =>
                                                                                 {
                %>
                <% Html.Telerik().Grid(e.ProjectPhases)
                           .Name("ProjectPhasesGrid" + e.Id)
                           .DataKeys(s => s.Add(x => x.Id))
                           .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "ProjectPhase"))
                           .ToolBar(toolBar => toolBar.Template(() =>
                            { 
                %>
                <table class="GridToolBar">
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridAddProjectPhase()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title="<%: Resources.Shared.Buttons.Function.Add %>" alt="<%: Resources.Shared.Buttons.Function.Add %>" height="24" width="24" align="middle" />
                            <script type="text/javascript">
                                function GridAddProjectPhase() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%:Url.Action("Index", "ProjectPhase")%>', function () {
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
                                            columns.Bound(b => b.Id).Width(35).Title(Resources.Shared.Model.Fields.Id).Groupable(false).Filterable(false).Sortable(false);
                                            columns.Bound(o => o.Name);
                                            columns.Bound(o => o.StartDate);
                                            columns.Bound(o => o.EndDate);
                                            columns.Bound(o => o.Status.Name);
                                            columns.Bound(o => o.CompletionPercentage);

                                            columns.Bound(o => o.Id).Title("")
                                                   .Format(Html.ActionLink(Resources.Shared.Buttons.Function.Details, "MasterIndex", "ProjectPhase", new { id = "{0}" }, null).ToHtmlString())
                                                   .Encoded(false)
                                                   .Width("5%").Sortable(false).Filterable(false);

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
                           .ClientEvents(events => events.OnRowSelect("ProjectPhasesServiceRowSelected"))
                           .Pageable(pager => pager.PageSize(5))
                           .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending())).Render();
                %>
                <%
                                                                                 });

                                   //End Project Phase 

                                   //Project Resource
                                   items.Add().Text(Resources.Areas.ProjectManagment.Entities.Project.ProjectModel.ProjectResourcePluralTitle).Content(() =>
                                                                                                                                {
                                
                %>
                <%Html.Telerik().Grid(e.ProjectResources)
                        .Name("ProjectResourceGrid" + e.Id)
                        .DataKeys(s => s.Add(x => x.Id))
                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "ProjectResource"))
                        .ToolBar(toolBar => toolBar.Template(() =>
                            { 
                %>
                <table class="GridToolBar">
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridAddProjectResource()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title="<%: Resources.Shared.Buttons.Function.Add %>" alt="<%: Resources.Shared.Buttons.Function.Add %>" height="24" width="24" align="middle" />
                            <script type="text/javascript">
                                function GridAddProjectResource() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "ProjectResource") %>', function () {
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
                            columns.Bound(b => b.Id).Width(35).Title(Resources.Shared.Model.Fields.Id).Groupable(false).Filterable(false).Sortable(false);
                            columns.Bound(o => o.Name);
                            columns.Bound(o => o.Type.Name);
                            columns.Bound(o => o.Status.Name);
                            columns.Command(s =>
                            {
                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                {
                                    s.Delete().ButtonType(GridButtonType.Image);
                                }
                            }).Width(1);
                        })
                        .Selectable()
                        .ClientEvents(events => events.OnRowSelect("ProjectResourceServiceRowSelected"))
                        .Pageable(pager => pager.PageSize(5))
                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending())).Render();
                %>
                <%
                                                                                                                                });
                                   // End Project Resource

                                   //Project Success Factor
                                   items.Add().Text(Resources.Areas.ProjectManagment.Entities.Project.ProjectModel.ProjectSuccessFactorPluralTitle).Content(() =>
                                                                                                                                {
                                
                %>
                <%Html.Telerik().Grid(e.SuccessFactors)
                        .Name("ProjectSuccessFactorGrid" + e.Id)
                        .DataKeys(s => s.Add(x => x.Id))
                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "ProjectSuccessFactor"))
                        .ToolBar(toolBar => toolBar.Template(() =>
                            { 
                %>
                <table class="GridToolBar">
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridAddProjectSuccessFactor()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title="<%: Resources.Shared.Buttons.Function.Add %>" alt="<%: Resources.Shared.Buttons.Function.Add %>" height="24" width="24" align="middle" />
                            <script type="text/javascript">
                                function GridAddProjectSuccessFactor() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "ProjectSuccessFactor") %>', function () {
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
                            columns.Bound(b => b.Id).Width(35).Title(Resources.Shared.Model.Fields.Id).Groupable(false).Filterable(false).Sortable(false);
                            columns.Bound(o => o.Description);
                            columns.Command(s =>
                            {
                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                {
                                    s.Delete().ButtonType(GridButtonType.Image);
                                }
                            }).Width(1);
                        })
                        .Selectable()
                        .ClientEvents(events => events.OnRowSelect("ProjectSuccessFactorServiceRowSelected"))
                        .Pageable(pager => pager.PageSize(5))
                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending())).Render();
                %>
                <%
                                                                                                                                });
                                   // End Project Success Factor

                                   //Project Constraint
                                   items.Add().Text(Resources.Areas.ProjectManagment.Entities.Project.ProjectModel.ProjectConstraintPluralTitle).Content(() =>
                                                                                                                                {
                                
                %>
                <%Html.Telerik().Grid(e.Constraints)
                            .Name("ProjectConstraintGrid" + e.Id)
                            .DataKeys(s => s.Add(x => x.Id))
                            .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "ProjectConstraint"))
                            .ToolBar(toolBar => toolBar.Template(() =>
                            { 
                %>
                <table class="GridToolBar">
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridAddProjectConstraint()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title="<%: Resources.Shared.Buttons.Function.Add %>" alt="<%: Resources.Shared.Buttons.Function.Add %>" height="24" width="24" align="middle" />
                            <script type="text/javascript">
                                function GridAddProjectConstraint() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "ProjectConstraint") %>', function () {
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
                                columns.Bound(b => b.Id).Width(35).Title(Resources.Shared.Model.Fields.Id).Groupable(false).Filterable(false).Sortable(false);
                                columns.Bound(o => o.Description);
                                columns.Command(s =>
                                {
                                    if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                    {
                                        s.Delete().ButtonType(GridButtonType.Image);
                                    }
                                }).Width(1);
                            })
                            .Selectable()
                            .ClientEvents(events => events.OnRowSelect("ProjectConstraintServiceRowSelected"))
                            .Pageable(pager => pager.PageSize(5))
                            .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending())).Render();
                %>
                <%
                                                                                                                                });
                                   // End Project Constraint


                                   //ProjectKpi
                                   items.Add().Text(Resources.Areas.ProjectManagment.Entities.Project.ProjectModel.ProjectKpiPluralTitle).Content(() =>
                                                                                                                                {
                                
                %>
                <% Html.Telerik().Grid(e.Kpis)
                        .Name("ProjectKpiGrid" + e.Id)
                        .DataKeys(s => s.Add(x => x.Id))
                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "ProjectKpi"))
                        .ToolBar(toolBar => toolBar.Template(() =>
                            { 
                %>
                <table class="GridToolBar">
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridAddProjectKpi()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title="<%: Resources.Shared.Buttons.Function.Add %>" alt="<%: Resources.Shared.Buttons.Function.Add %>" height="24" width="24" align="middle" />
                            <script type="text/javascript">
                                function GridAddProjectKpi() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "ProjectKpi") %>', function () {
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
                            columns.Bound(b => b.Id).Width(35).Title(Resources.Shared.Model.Fields.Id).Groupable(false).Filterable(false).Sortable(false);
                            columns.Bound(o => o.Type.Name);
                            columns.Bound(o => o.Value);
                            columns.Bound(o => o.Weight);
                            columns.Bound(o => o.Description);
                            columns.Command(s =>
                            {
                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                {
                                    s.Delete().ButtonType(GridButtonType.Image);
                                }
                            }).Width(1);
                        })
                        .Selectable()
                        .ClientEvents(events => events.OnRowSelect("ProjectKpiServiceRowSelected"))
                        .Pageable(pager => pager.PageSize(5))
                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending())).Render();
                %>
                <%
                                                                                                                                });
                                   // End Project Kpi


                                   //ProjectEvaluation
                                   items.Add().Text(Resources.Areas.ProjectManagment.Entities.Project.ProjectModel.ProjectEvaluationPluralTitle).Content(() =>
                                                                                                                                {
                                
                %>
                <%Html.Telerik().Grid(e.ProjectEvaluations)
                        .Name("ProjectEvaluationGrid" + e.Id)
                        .DataKeys(s => s.Add(x => x.Id))
                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "ProjectEvaluation"))
                        .ToolBar(toolBar => toolBar.Template(() =>
                            { 
                %>
                <table class="GridToolBar">
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridAddProjectEvaluation()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title="<%: Resources.Shared.Buttons.Function.Add %>" alt="<%: Resources.Shared.Buttons.Function.Add %>" height="24" width="24" align="middle" />
                            <script type="text/javascript">
                                function GridAddProjectEvaluation() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "ProjectEvaluation") %>', function () {
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
                            columns.Bound(b => b.Id).Width(35).Title(Resources.Shared.Model.Fields.Id).Groupable(false).Filterable(false).Sortable(false);
                            columns.Bound(o => o.Evaluator.FirstName);
                            columns.Bound(o => o.EvaluatorProjectRole).Title(Resources.Areas.ProjectManagment.ValueObjects.ProjectEvaluation.ProjectEvaluationModel.EvaluatorProjectRole);
                            columns.Bound(o => o.Date);
                            columns.Bound(o => o.Quarter);
                            columns.Bound(o => o.TotalProjectRate);
                            columns.Bound(o => o.Status.Name);

                            columns.Bound(o => o.Id).Title("")
                            .Format(Html.ActionLink(Resources.Shared.Buttons.Function.Details, "MasterIndex", "ProjectEvaluation", new { id = "{0}" }, null).ToHtmlString())
                            .Encoded(false)
                            .Width("5%").Sortable(false).Filterable(false);

                            columns.Command(s =>
                            {
                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                {
                                    s.Delete().ButtonType(GridButtonType.Image);
                                }
                            }).Width(1);
                        })
                        .Selectable()
                        .ClientEvents(events => events.OnRowSelect("ProjectEvaluationServiceRowSelected"))
                        .Pageable(pager => pager.PageSize(5))
                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending())).Render();
                %>
                <%
                                                                                                                                });
                                   // End Project Evaluation


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
                                             builder.OnRowSelect("collapseMVCGroupedGrids");
                                             builder.OnRowSelect("loadPartialView");
                                             builder.OnDetailViewExpand("SetMasterRecordValue");
                                             builder.OnDetailViewCollapse("SetMasterRecordValue");
                                         })
                       .Pageable(p => p.PageSize(5).PageTo((int)ViewData["PageTo"]))
                       .Selectable()
                       .Render();

                %>
            </td>
        </tr>
    </table>
</fieldset>
<script type="text/javascript">

    function tabStripSelect(e) {

        var item = $(e.item);

        $('#result').fadeOut('fast');

        $.ajax({
            url: '<%:Url.Action("SaveTabIndex", "Project")%>/',
            type: "POST",
            data: { selectedIndex: item.index() }
        });
    }

    function SetMasterRecordValue(e) {

        //collapseMVCGroupedGrids();
        
        var x = e.masterRow.cells[1].innerHTML;

        $('#result').load('<%:Url.Action("PartialMasterInfo", "Project")%>', { selectedRowId: x }, function () {
            $('#result').fadeIn('fast');

            loadRibbon();
        });
    }

    function loadPartialView(e) {
        
        $('> .t-hierarchy-cell > .t-icon', e.row).click();


//                var grid = $("#Grid").data("tGrid");

//                $("#Grid .t-grouping-row").each(function () {
//                    grid.collapseToEnd(this);
//                    grid.collapseToStart(this);
//                });


        $('#result').fadeOut('fast');
    }

    function collapseMVCGroupedGrids(e) {
        alert("sd")

        var grid = $("#Grid").data("tGrid");
        $("#Grid .t-grouping-row").each(function () {
            grid.collapseToEnd(this);
        });

        
//        var grid = $('#' + e.target.id);
//        $.each(
//            grid.find('td.t-group-cell').parent('tr'),
//            function (index, value) {
//                $(value).attr('style', 'display: none;');
//            });
//                    $.each(
//            grid.find('.t-icon'),
//            function (index, value) {
//                if ($(value).hasClass('t-collapse')) {
//                    $(value).removeClass('t-collapse')
//            .addClass('t-expand');
//                }
//            });
    }


    function loadRibbon() {

        $('#ProjectFunctionsArea').load('<%:Url.Action("GetFunctionsPartial", "ProjectManagement")%>');
    }

    function ProjectTeamRowSelected(e) {

        $('#result').fadeOut('fast');

        var x = e.row.cells[0].innerHTML;

        var url = '<%:Url.Action("Index", "ProjectTeam", new {selectedSubRowId = "Value1"})%>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('fast');
            loadRibbon();
        });
    }

    function ProjectKpiServiceRowSelected(e) {

        $('#result').fadeOut('fast');

        var x = e.row.cells[0].innerHTML;

        var url = '<%:Url.Action("Index", "ProjectKpi", new {selectedSubRowId = "Value1"})%>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('fast');
        });
    }


    function ProjectResourceServiceRowSelected(e) {
        $('#result').fadeOut('fast');

        var x = e.row.cells[0].innerHTML;

        var url = '<%:Url.Action("Index", "ProjectResource", new {selectedSubRowId = "Value1"})%>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('fast');
        });
    }

    function ProjectConstraintServiceRowSelected(e) {

        $('#result').fadeOut('fast');

        var x = e.row.cells[0].innerHTML;

        var url = '<%:Url.Action("Index", "ProjectConstraint", new {selectedSubRowId = "Value1"})%>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('fast');
        });
    }

    function ProjectSuccessFactorServiceRowSelected(e) {

        $('#result').fadeOut('fast');

        var x = e.row.cells[0].innerHTML;

        var url = '<%:Url.Action("Index", "ProjectSuccessFactor", new {selectedSubRowId = "Value1"})%>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('fast');
        });
    }

    function ProjectPhasesServiceRowSelected(e) {

        $('#result').fadeOut('fast');

        //var x = e.row.cells[0].innerHTML;
        var grid = $('#Grid').data('tGrid');
        var indexOfIdColumn = GetGridColumnIndexByName(grid, "Id");
        var x = e.row.cells[indexOfIdColumn].innerHTML;
        alert(x);

        var url = '<%:Url.Action("Index", "ProjectPhase", new {selectedSubRowId = "Value1"})%>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('fast');
            loadRibbon();
        });
    }

    
    function ProjectEvaluationServiceRowSelected(e) {

        $('#result').fadeOut('fast');

        //var x = e.row.cells["Id"].innerHTML;

        var grid = $('#Grid').data('tGrid');
        var indexOfIdColumn = GetGridColumnIndexByName(grid, "Id");
        //alert(indexOfIdColumn);
        var x = e.row.cells[indexOfIdColumn].innerHTML;
        alert(x);
        
        
        var url = '<%:Url.Action("Index", "ProjectEvaluation", new {selectedSubRowId = "Value1"})%>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('fast');
            loadRibbon();
        });
    }

</script>
