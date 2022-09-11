<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<fieldset class="ParentFieldset">
    <div id="dialog-form" title="Competency Section Items">
    </div>
    <div id="dialog-form-ObjectiveSectionItemKpi" title="Objective Section Item KPI">
    </div>
    <legend class="ParentLegend">Appraisal List - Select Row For Details</legend>
    <table width="100%">
        <tr>
            <td>
                <%
                 
                    Html.Telerik().Grid<Model.PMS.Entities.Appraisal>("appraisal")
                        .Name("Grid")
                        .DataKeys(k => k.Add(o => o.Id))
                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Appraisal"))
                        .DataBinding(dataBinding => dataBinding.Server().Insert("Insert", "Appraisal"))
                        .ToolBar(toolBar => toolBar.Template(() =>
                        {  
                %>
                <table class="GridToolBar">
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <a href="<%:Url.Action("Insert", "Appraisal")%>">
                                <input type="button" value="Add Appraisal" />
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
                                         c.Bound(o => o.Id).Title("No").Width(1).Groupable(false).Filterable(false).Sortable(false);
                                         c.Bound(o => o.Date);
                                         c.Bound(o => o.Employee.FirstName);
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

                                   //Competency Section
                                   items.Add().Text("Competency Section").Content(() =>
                                                                                {
                %>
                <% Html.Telerik().Grid(e.CompetencySections)
                           .Name("CompetencySections" + e.Id)
                           .DataKeys(s => s.Add(x => x.Id))
                           .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "CompetencySection"))

                           .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Title("No").Groupable(false).Filterable(
                                                false).Sortable(false);
                                            columns.Bound(o => o.Name);
                                            columns.Bound(o => o.Weight);
                                            columns.Bound(o => o.TotalRate);
                                            columns.Bound(o => o.FinalSubmit);
                                        })
                            .ClientEvents(events => events.OnRowSelect("CompetencySectionRowSelected")
                            .OnDetailViewExpand("OnDetailViewExpandCompetencySection"))
                           .Selectable()
                           .Pageable(pager => pager.PageSize(5))
                           .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                           .DetailView(competencyJobDescriptionsDetailsView => competencyJobDescriptionsDetailsView.Template(c =>
                                    {
                                        Html.Telerik().TabStrip()
                                                     .Name("TabStrip_" + c.Id)
                                                     .Effects(fx => fx.Opacity())
                                                     .SelectedIndex(0)
                                                     .Items(competencyJobDescriptions =>
                                                     {

                                                         // 1st Item In Tab
                                                         competencyJobDescriptions.Add().Text("Competency Job Description").Content(() =>
                                   {
                                       
                %>
                <%
                                       
                                       Html.Telerik().Grid(c.CompetencyJobDescriptions)
                                      .Name("CompetencyJobDescriptions" + c.Id)
                                      .DataKeys(s => s.Add(x => x.Id))
                                      .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "CompetencyJobDescription"))

                              .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                               .Columns(columns =>
                               {
                                   columns.Bound(o => o.Id).Title("No").Width(1).Groupable(false).Filterable(false).Sortable(false);
                                   columns.Bound(o => o.JobTitle);
                                   columns.Bound(o => o.Weight);
                                   columns.Bound(o => o.Rate);
                                   columns.Bound(o => o.Id).Title("")
                                       .Format(Html.ActionLink("Details", "MasterIndex", "CompetencyJobDescription", new { id = "{0}" }, null).ToHtmlString())
                                       .Encoded(false)
                                       .Width(1).Sortable(false).Filterable(false);
                               })
                              .ClientEvents(events => events.OnRowSelect("CompetencyJobDescriptionRowSelected"))
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

                                   //End Competency Section 

                                   //Job Description Section
                                   items.Add().Text("Job Description Section").Content(() =>
                                                                                {
                %>
                <% Html.Telerik().Grid(e.JobDescriptionSections)
                           .Name("JobDescriptionSections" + e.Id)
                           .DataKeys(s => s.Add(x => x.Id))
                           .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "JobDescriptionSection"))

                           .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Title("No").Groupable(false).Filterable(
                                                false).Sortable(false);
                                            columns.Bound(o => o.Name);
                                            columns.Bound(o => o.Weight);
                                            columns.Bound(o => o.TotalRate);
                                            columns.Bound(o => o.FinalSubmit);
                                        })
                            .ClientEvents(events => events.OnRowSelect("JobDescriptionSectionRowSelected")
                            .OnDetailViewExpand("OnDetailViewExpandJobDescriptionSection"))
                           .Selectable()
                           .Pageable(pager => pager.PageSize(5))
                           .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                           .DetailView(jobDescriptionSectionItemsDetailsView => jobDescriptionSectionItemsDetailsView.Template(c =>
                                    {
                                        Html.Telerik().TabStrip()
                                                     .Name("TabStrip_" + c.Id)
                                                     .Effects(fx => fx.Opacity())
                                                     .SelectedIndex(0)
                                                     .Items(jobDescriptionSectionItems =>
                                                     {
                                                         // 1st Item In Tab
                                                         jobDescriptionSectionItems.Add().Text("Job Description Section Item").Content(() =>
                                   {
                                       
                %>
                <%
                                       
                                       Html.Telerik().Grid(c.JobDescriptionSectionItems)
                                      .Name("JobDescriptionSectionItems" + c.Id)
                                      .DataKeys(s => s.Add(x => x.Id))
                                      .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "JobDescriptionSectionItem"))

                              .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                               .Columns(columns =>
                               {
                                   columns.Bound(o => o.Id).Title("No").Width(1).Groupable(false).Filterable(false).Sortable(false);
                                   columns.Bound(o => o.JobTitle);
                                   columns.Bound(o => o.Weigth);
                                   columns.Bound(o => o.Rate);
                                   columns.Bound(o => o.Id).Title("")
                                       .Format(Html.ActionLink("Details", "MasterIndex", "JobDescriptionSectionItem", new { id = "{0}" }, null).ToHtmlString())
                                       .Encoded(false)
                                       .Width(1).Sortable(false).Filterable(false);
                               })
                              .ClientEvents(events => events.OnRowSelect("JobDescriptionSectionItemRowSelected"))
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

                                   //End Job Description Section 

                                   //ObjectiveSection

                                   items.Add().Text("Objective Section").Content(() =>
                                                                                {
                %>
                <% Html.Telerik().Grid(e.ObjectiveSections)
                           .Name("ObjectiveSections" + e.Id)
                           .DataKeys(s => s.Add(x => x.Id))
                           .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Title("No").Groupable(false).Filterable(
                                                false).Sortable(false);
                                            columns.Bound(o => o.Name);
                                            columns.Bound(o => o.Weight);
                                            columns.Bound(o => o.TotalRate);
                                            columns.Bound(o => o.FinalSubmit);
                                        })
                            .ClientEvents(events => events.OnRowSelect("ObjectiveSectionRowSelected")
                            .OnDetailViewExpand("OnDetailViewExpandObjectiveSection"))
                           .Selectable()
                           .Pageable(pager => pager.PageSize(5))
                           .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                           .DetailView(objectiveSectionItemDetailsView => objectiveSectionItemDetailsView.Template(c =>
                                    {
                                        Html.Telerik().TabStrip()
                                                     .Name("TabStrip_" + c.Id)
                                                     .Effects(fx => fx.Opacity())
                                                     .SelectedIndex(0)
                                                     .Items(competencyJobDescriptions =>
                                                     {
                                                         // 1st Item In Tab
                                                         competencyJobDescriptions.Add().Text("Objective Section Item").Content(() =>
                                   {
                                       
                %>
                <%
                                       
                                       Html.Telerik().Grid(c.ObjectiveSectionItems)
                                      .Name("ObjectiveSectionItems" + c.Id)
                                      .DataKeys(s => s.Add(x => x.Id))
                              .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                               .Columns(columns =>
                               {
                                   columns.Bound(o => o.Id).Title("No").Width(1).Groupable(false).Filterable(false).Sortable(false);
                                   columns.Bound(o => o.Name);
                                   columns.Bound(o => o.IsShared);
                                   columns.Bound(o => o.Weight);
                                   columns.Bound(o => o.Rate);
                                   columns.Bound(o => o.SharedWithPercentage);
                               })
                              .ClientEvents(events => events.OnRowSelect("ObjectiveSectionItemRowSelected")
                              .OnDetailViewExpand("OnDetailViewExpandObjectiveSectionItem"))
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
                                   //End Objective Section                                                                   

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
        </tr>
    </table>
</fieldset>
<script type="text/javascript">

    function tabStripSelect(e) {

        var item = $(e.item);

        $('#result').fadeOut('fast');

        $.ajax({
            url: '<%:Url.Action("SaveTabIndex", "Appraisal")%>/',
            type: "POST",
            data: { selectedIndex: item.index() }
        });
    }

    function SetMasterRecordValue(e) {

        var x = e.masterRow.cells[1].innerHTML;


        $('#result').load('<%:Url.Action("PartialMasterInfo", "Appraisal")%>', { selectedRowId: x }, function () {
            $('#result').fadeIn('fast');
            loadRibbon();
        });
    }

    function loadPartialView(e) {

        $('> .t-hierarchy-cell > .t-icon', e.row).click();

        $('#result').fadeOut('fast');
    }

    function loadRibbon() {
        $('#ProjectFunctionsArea').load('<%:Url.Action("GetFunctionsPartial", "PMSComprehensive")%>');
    }

    // Competency Section

    function CompetencySectionRowSelected(e) {
        
        $('> .t-hierarchy-cell > .t-icon', e.row).click();
        $('#result').fadeOut('fast');

        var x = e.row.cells[1].innerHTML;

        var url = '<%:Url.Action("Index", "CompetencySection", new {selectedSubRowId = "Value1"})%>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('fast');
            loadRibbon();
        });
    }

    function OnDetailViewExpandCompetencySection(e) {

        var x = e.masterRow.cells[1].innerHTML;

        var url = '<%: Url.Action("Index", "CompetencySection", new { selectedSubRowId = "Value1"}) %>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('slow');
            loadRibbon();
        });
    }

    function CompetencyJobDescriptionRowSelected(e) {
        
        $('#result').fadeOut('fast');

        var x = e.row.cells[0].innerHTML;

        var url = '<%: Url.Action("Index", "CompetencyJobDescription", new { selectedSubRowId = "Value1" }) %>';
        url = url.replace("Value1", x);
        $('#result').load(url, function () {
            $('#result').fadeIn('slow');
        });
    }

    function ShowDialog() {
        
        $.ajax({
            type: "POST",
            traditional: true,
            url: '<%: Url.Action("ReadOnly", "CompetencySectionItem") %>',
            success: function (result) {
                if (result.Success == false) {
                    alert(result.Message);
                } else {
                    $('#dialog-form').html(result.PartialViewHtml);
                    $('#dialog-form').dialog('open');
                }
            }
        });
    }

    // End Competency Section

    // Job Description Section

    function JobDescriptionSectionRowSelected(e) {
        
        $('> .t-hierarchy-cell > .t-icon', e.row).click();
        $('#result').fadeOut('fast');

        var x = e.row.cells[1].innerHTML;

        var url = '<%:Url.Action("Index", "JobDescriptionSection", new {selectedSubRowId = "Value1"})%>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('fast');
            loadRibbon();
        });
    }

    function OnDetailViewExpandJobDescriptionSection(e) {

        var x = e.masterRow.cells[1].innerHTML;

        var url = '<%: Url.Action("Index", "JobDescriptionSection", new { selectedSubRowId = "Value1"}) %>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('slow');
            loadRibbon();
        });
    }

    function JobDescriptionSectionItemRowSelected(e) {
        
        $('#result').fadeOut('fast');

        var x = e.row.cells[0].innerHTML;

        var url = '<%: Url.Action("Index", "JobDescriptionSectionItem", new { selectedSubRowId = "Value1" }) %>';
        url = url.replace("Value1", x);
        $('#result').load(url, function () {
            $('#result').fadeIn('slow');
        });
    }

    // End Job Description Section

    function ShowDialogObjectiveSectionItemKpi() {
        
        $.ajax({
            type: "POST",
            traditional: true,
            url: '<%: Url.Action("ReadOnly", "ObjectiveSectionItemKpi") %>',
            success: function (result) {
                if (result.Success == false) {
                    alert(result.Message);
                } else {
                    $('#dialog-form-ObjectiveSectionItemKpi').html(result.PartialViewHtml);
                    $('#dialog-form-ObjectiveSectionItemKpi').dialog('open');
                }
            }
        });
    }

    function ProjectSectionItemRowSelected(e) {
        
        $('#result').fadeOut('fast');

        var x = e.row.cells[0].innerHTML;

        var url = '<%: Url.Action("Index", "ProjectSectionItems", new { selectedSubRowId = "Value1" }) %>';
        url = url.replace("Value1", x);
        $('#result').load(url, function () {
            $('#result').fadeIn('slow');
            ShowDialog(x);
        });
    }

    function ObjectiveSectionRowSelected(e) {
        
        $('> .t-hierarchy-cell > .t-icon', e.row).click();
        $('#result').fadeOut('fast');

        var x = e.row.cells[1].innerHTML;

        var url = '<%:Url.Action("Index", "ObjectiveSection", new {selectedSubRowId = "Value1"})%>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('fast');
            loadRibbon();
        });
    }


    function ObjectiveSectionItemRowSelected(e) {

        $('> .t-hierarchy-cell > .t-icon', e.row).click();

        $('#result').fadeOut('fast');

        var x = e.row.cells[0].innerHTML;

        var url = '<%: Url.Action("Index", "ObjectiveSectionItem", new { selectedSubRowId = "Value1"}) %>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('slow');
            ShowDialogObjectiveSectionItemKpi();
            loadRibbon();
        });
    }

    function OnDetailViewExpandObjectiveSection(e) {

        var x = e.masterRow.cells[1].innerHTML;

        var url = '<%: Url.Action("Index", "ObjectiveSection", new { selectedSubRowId = "Value1"}) %>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('slow');
            loadRibbon();
        });
    }

    function OnDetailViewExpandObjectiveSectionItem(e) {

        var x = e.masterRow.cells[1].innerHTML;

        var url = '<%: Url.Action("Index", "ObjectiveSectionItem", new { selectedSubRowId = "Value1"}) %>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('slow');
            loadRibbon();
        });
    }

    function ProjectSectionRowSelected(e) {

        $('#result').fadeOut('fast');

        var x = e.row.cells[1].innerHTML;

        var url = '<%:Url.Action("Index", "ProjectSection", new {selectedSubRowId = "Value1"})%>';
        url = url.replace("Value1", x);

        $('#result').load(url, function () {
            $('#result').fadeIn('fast');
        });
    }

    $(document).ready(function () {

        $("#dialog").dialog("destroy");
        $("#dialog-form").dialog({
            autoOpen: false,
            height: 'auto',
            width: 'auto',
            modal: true,
            resizable: false,
            buttons: {
                Cancel: function () {
                    $(this).dialog('close');
                }
            }
        });

        $("#dialog-form-ObjectiveSectionItemKpi").dialog({
            autoOpen: false,
            height: 'auto',
            width: 'auto',
            modal: true,
            resizable: false,
            buttons: {
                Cancel: function () {
                    $(this).dialog('close');
                }
            }
        });
    });
    
</script>
