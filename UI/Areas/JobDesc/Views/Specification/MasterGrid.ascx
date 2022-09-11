<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend"><%:Resources.Areas.JobDesc.Entities.JobDescription.JobDescriptionModel.SpecificationTitle %></legend>
    <table width="100%">
        <tr valign="top">
            <td>
                <%
                    Html.Telerik().Grid<HRIS.Domain.JobDesc.ValueObjects.Specification>("ValueObjectsList")
                        .Name("SpecificationGrid")
                        .DataKeys(k => k.Add(o => o.Id))
                        .Columns(c => c.Bound(o => o.JobDescription.Summary).Title(Resources.Areas.JobDesc.ValueObjects.Specification.SpecificationModel.Summury).Groupable(false))
                        .DetailView(detailView => detailView.Template(e => // Set the server template
                        {
                %>
                <% Html.Telerik().TabStrip()
                                                      .Name("TabStrip_" + e.Id)
                                                      .Effects(fx => fx.Opacity())

                                                      .SelectedIndex(Session["SelectedTabIndexSecondLevel"] != null ? int.Parse(Session["SelectedTabIndexSecondLevel"].ToString()) : 0)
                                                      .Items(items =>
                                                      {

                                                          items.Add().Text(Resources.Areas.JobDesc.ValueObjects.Specification.SpecificationModel.CompetencyGridMasterTitle).Content(() =>
                                                          {
                %>
                <table>
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="AddCompetency()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png")%>"
                                title="<%: Resources.Shared.Buttons.Function.Add %>" alt="<%: Resources.Shared.Buttons.Function.Add %>" height="24" width="24" />
                            <script type="text/javascript">
                                function AddCompetency() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "Competency")%>', function () {
                                        $('#result').fadeIn('slow');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%: Html.Telerik().Grid(e.Competencies)
                                                         .Name("Competency" + e.Id)
                                                        .DataKeys(s => s.Add(x => x.Id))
                                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Competency"))
                                    .HtmlAttributes(new { style = "z-index:99999" })                                                                                       
                                                        .Columns(columns =>
                                                        {
                                                            columns.Bound(o => o.Id).Width(1).Groupable(false).Sortable(false).Filterable(false);
                                                            columns.Bound(o => o.Name);
                                                            columns.Bound(o => o.Required);
                                                            columns.Bound(o => o.Weight);                                                            
                                                            columns.Bound(o => o.Level.Name);
                                                            columns.Bound(o => o.Type.Name);
                                                            columns.Command(s =>
                                                            {
                                                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                                {
                                                                    s.Delete().ButtonType(GridButtonType.Image);
                                                                }
                                                            }).Width(1);                                                            
                                                        })
                                                         .ClientEvents(events =>
                                                                                             events.OnRowSelect("rowSelectedCompetency")
                                                          )
                                                        .Selectable()
                                                        .Pageable(pager => pager.PageSize(5))
                                                         .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                %>
                <%
                                                          });

                                                          items.Add().Text(Resources.Areas.JobDesc.ValueObjects.Specification.SpecificationModel.ComputerSkillGridMasterTitle).Content(() =>
                                                          {
                %>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="AddComputerSkill()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png")%>"
                                title="<%: Resources.Shared.Buttons.Function.Add %>" alt="<%: Resources.Shared.Buttons.Function.Add %>" height="24" width="24" />
                            <script type="text/javascript">
                                function AddComputerSkill() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "ComputerSkill")%>', function () {
                                        $('#result').fadeIn('slow');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%: Html.Telerik().Grid(e.ComputerSkills)
                                                        .Name("ComputerSkill" + e.Id)
                                                        .DataKeys(s => s.Add(x => x.Id))
                                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "ComputerSkill"))
                                                        .Columns(columns =>
                                                        {
                                                            columns.Bound(o => o.Id).Width(1).Groupable(false).Sortable(false).Filterable(false);
                                                            columns.Bound(o => o.Type.Name);
                                                            columns.Bound(o => o.Level.Name);
                                                            columns.Bound(o => o.Weight);                                                            
                                                            columns.Bound(o => o.Required);
                                                            columns.Command(s =>
                                                            {
                                                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                                {
                                                                    s.Delete().ButtonType(GridButtonType.Image);
                                                                }
                                                            }).Width(1);  
                                                        })
                                                         .ClientEvents(events => events.OnRowSelect("rowSelectedComputerSkill"))
                                                        .Selectable()
                                                        .Pageable(pager => pager.PageSize(5))
                                                       .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                %>
                <%
                                                          });

                                                          items.Add().Text(Resources.Areas.JobDesc.ValueObjects.Specification.SpecificationModel.EducationGridMasterTitle).Content(() =>
                                                          {
                %>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="AddEducation()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png")%>"
                                title="<%: Resources.Shared.Buttons.Function.Add %>" alt="<%: Resources.Shared.Buttons.Function.Add %>" height="24" width="24" />
                            <script type="text/javascript">
                                function AddEducation() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "JEducation")%>', function () {
                                        $('#result').fadeIn('slow');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%: Html.Telerik().Grid(e.Educations)
                                                        .Name("Educations" + e.Id)
                                                        .DataKeys(s => s.Add(x => x.Id))
                                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "JEducation")) 
                                                        .Columns(columns =>
                                                        {
                                                            columns.Bound(o => o.Id).Width(1).Groupable(false).Sortable(false).Filterable(false);
                                                            columns.Bound(o => o.Type.Name).Title(Resources.Areas.JobDesc.ValueObjects.JEducation.JEducationModel.Type);
                                                            columns.Bound(o => o.Major.Name).Title(Resources.Areas.JobDesc.ValueObjects.JEducation.JEducationModel.Major);
                                                            columns.Bound(o => o.ScoreType.Name).Title(Resources.Areas.JobDesc.ValueObjects.JEducation.JEducationModel.ScoreType);
                                                            columns.Bound(o => o.Score);
                                                            columns.Bound(o => o.Rank.Name).Title(Resources.Areas.JobDesc.ValueObjects.JEducation.JEducationModel.Rank);
                                                            columns.Bound(o => o.Weight);                                                            
                                                            columns.Bound(o => o.Required);
                                                            columns.Command(s =>
                                                            {
                                                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                                {
                                                                    s.Delete().ButtonType(GridButtonType.Image);
                                                                }
                                                            }).Width(1);  
                                                        })
                                                         .ClientEvents(events =>
                                                         events.OnRowSelect("rowSelectedEducation")
                                                          )
                                                        .Selectable()
                                                        .Pageable(pager => pager.PageSize(5))
                                                       .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                                                                        
                %>
                <%
                                                          });

                                                          items.Add().Text(Resources.Areas.JobDesc.ValueObjects.Specification.SpecificationModel.ExperienceGridMasterTitle).Content(() =>
                                                          {
                %>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="AddExperiences()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png")%>"
                                title="<%: Resources.Shared.Buttons.Function.Add %>" alt="<%: Resources.Shared.Buttons.Function.Add %>" height="24" width="24" />
                            <script type="text/javascript">
                                function AddExperiences() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "JExperience")%>', function () {
                                        $('#result').fadeIn('slow');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%: Html.Telerik().Grid(e.Experiences)
                                                         .Name("Experiences" + e.Id)
                                                        .DataKeys(s => s.Add(x => x.Id))
                                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "JExperience"))    
                                                        .Columns(columns =>
                                                        {
                                                            columns.Bound(o => o.Id).Width(1).Groupable(false).Sortable(false).Filterable(false);
                                                            columns.Bound(o => o.Industry);
                                                            columns.Bound(o => o.CareerLevel.Name).Title(Resources.Areas.JobDesc.ValueObjects.JExperience.JExperienceModel.CareerLevel);
                                                            columns.Bound(o => o.Weight);
                                                            
                                                            columns.Bound(o => o.Required);
                                                            columns.Command(s =>
                                                            {
                                                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                                {
                                                                    s.Delete().ButtonType(GridButtonType.Image);
                                                                }
                                                            }).Width(1);  
                                                        })
                                                         .ClientEvents(events =>
                                                          events.OnRowSelect("rowSelectedExperiences")
                                                          )
                                                        .Selectable()
                                                        .Pageable(pager => pager.PageSize(5))
                                                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                                                                                                            
                %>
                <%
                                                          });//end tab

                                                          items.Add().Text(Resources.Areas.JobDesc.ValueObjects.Specification.SpecificationModel.KnowledgeGridMasterTitle).Content(() =>
                                                          {
                %>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="AddKnowledges()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png")%>"
                                title="<%: Resources.Shared.Buttons.Function.Add %>" alt="<%: Resources.Shared.Buttons.Function.Add %>" height="24" width="24" />
                            <script type="text/javascript">
                                function AddKnowledges() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "Knowledge")%>', function () {
                                        $('#result').fadeIn('slow');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%: Html.Telerik().Grid(e.Knowledges)
                                                        .Name("Knowledges" + e.Id)
                                                        .DataKeys(s => s.Add(x => x.Id))
                                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Knowledge"))    
                                                        .Columns(columns =>
                                                        {
                                                            columns.Bound(o => o.Id).Width(1).Groupable(false).Sortable(false).Filterable(false);
                                                            columns.Bound(o => o.Field);
                                                            columns.Bound(o => o.Level.Name);
                                                            columns.Bound(o => o.Weight);                                                            
                                                            columns.Bound(o => o.Required);
                                                            columns.Command(s =>
                                                            {
                                                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                                {
                                                                    s.Delete().ButtonType(GridButtonType.Image);
                                                                }
                                                            }).Width(1);  
                                                        })
                                                         .ClientEvents(events =>
                                                                     events.OnRowSelect("rowSelectedKnowledges")
                                                          )
                                                        .Selectable()
                                                        .Pageable(pager => pager.PageSize(5))
                                                       .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                                                                        
                %>
                <%
                                                          });

                                                          items.Add().Text(Resources.Areas.JobDesc.ValueObjects.Specification.SpecificationModel.LanguageGridMasterTitle).Content(() =>
                                                                                                      {
                %>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="AddLanguage()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png")%>"
                                title="<%: Resources.Shared.Buttons.Function.Add %>" alt="<%: Resources.Shared.Buttons.Function.Add %>" height="24" width="24" />
                            <script type="text/javascript">
                                function AddLanguage() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "JLanguage")%>', function () {
                                        $('#result').fadeIn('slow');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%: Html.Telerik().Grid(e.Languages)
                                                                .Name("Languages" + e.Id)
                                                                .DataKeys(s => s.Add(x => x.Id))
                                                                .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "JLanguage"))   
                                                                .Columns(columns =>
                                                                {
                                                                    columns.Bound(o => o.Id).Width(1).Groupable(false).Sortable(false).Filterable(false);
                                                                    columns.Bound(o => o.Name.Name);
                                                                    columns.Bound(o => o.Required);
                                                                    columns.Bound(o => o.Weight);

                                                                    columns.Bound(o => o.Reading.Name).Title(Resources.Areas.JobDesc.ValueObjects.JLanguage.JLanguageModel.Reading);
                                                                    columns.Bound(o => o.Writing.Name).Title(Resources.Areas.JobDesc.ValueObjects.JLanguage.JLanguageModel.Writing);
                                                                    columns.Bound(o => o.Speaking.Name).Title(Resources.Areas.JobDesc.ValueObjects.JLanguage.JLanguageModel.Speaking);
                                                                    columns.Bound(o => o.Listening.Name).Title(Resources.Areas.JobDesc.ValueObjects.JLanguage.JLanguageModel.Listening);
                                                                    columns.Command(s =>
                                                                    {
                                                                        if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                                        {
                                                                            s.Delete().ButtonType(GridButtonType.Image);
                                                                        }
                                                                    }).Width(1);  
                                                                })
                                                                 .ClientEvents(events =>
                                                                             events.OnRowSelect("rowSelectedLangauge")
                                                                  )
                                                                .Selectable()
                                                                .Pageable(pager => pager.PageSize(5))
                                                                                           .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                                                                                                                    
                %>
                <%
                                                                                                      });

                                                          items.Add().Text(Resources.Areas.JobDesc.ValueObjects.Specification.SpecificationModel.SkillGridMasterTitle).Content(() =>
                                                          {
                %>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="AddSkill()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png")%>"
                                title="<%: Resources.Shared.Buttons.Function.Add %>" alt="<%: Resources.Shared.Buttons.Function.Add %>" height="24" width="24" />
                            <script type="text/javascript">
                                function AddSkill() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "JSkill")%>', function () {
                                        $('#result').fadeIn('slow');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%: Html.Telerik().Grid(e.Skills)
                                                        .Name("JSkill" + e.Id)
                                                        .DataKeys(s => s.Add(x => x.Id))
                                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "JSkill"))
                                                        .Columns(columns =>
                                                        {
                                                            columns.Bound(o => o.Id).Width(1).Groupable(false).Sortable(false).Filterable(false);
                                                            columns.Bound(o => o.Type.Name);
                                                            columns.Bound(o => o.Level.Name);
                                                            columns.Bound(o => o.Weight);
                                                            columns.Bound(o => o.Required);                                                       
                                                            columns.Command(s =>
                                                            {
                                                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                                {
                                                                    s.Delete().ButtonType(GridButtonType.Image);
                                                                }
                                                            }).Width(1);  
                                                        })
                                                         .ClientEvents(events => events.OnRowSelect("rowSelectedSkill"))
                                                        .Selectable()
                                                        .Pageable(pager => pager.PageSize(5))
                                                       .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                                                                        
                %>
                <%
                                                          });
                                                          items.Add().Text(Resources.Areas.JobDesc.ValueObjects.Specification.SpecificationModel.WorkingConditionGridMasterTitle).Content(() =>
                                                          {
                %>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <img type="image" value="Add" onclick="AddWorkingCondition()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png")%>"
                                title="<%: Resources.Shared.Buttons.Function.Add %>" alt="<%: Resources.Shared.Buttons.Function.Add %>" height="24" width="24" />
                            <script type="text/javascript">
                                function AddWorkingCondition() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "WorkingCondition")%>', function () {
                                        $('#result').fadeIn('slow');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <% Html.Telerik().Grid(e.WorkingConditions)
                                            .Name("WorkingCondition" + e.Id)
                                            .DataKeys(s => s.Add(x => x.Id))
                                            .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "WorkingCondition"))
                                            .Columns(columns =>
                                            {
                                                columns.Bound(o => o.Id).Width(1).Groupable(false).Sortable(false).Filterable(false);
                                                columns.Bound(o => o.InternalRelationships);
                                                columns.Bound(o => o.ExternalRelationships);
                                                columns.Command(s =>
                                                {
                                                    if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                    {
                                                        s.Delete().ButtonType(GridButtonType.Image);
                                                    }
                                                }).Width(1);
                                            })
                                            .ClientEvents(events => events
                                            .OnRowSelect("rowSelectedWorkingCondition")
                                            .OnDetailViewExpand("OnDetailViewExpandWorkingCondition")
                                            .OnDetailViewExpand("OnDetailViewExpandWorkingCondition"))
                                            .Selectable()
                                             .RowAction(row =>
                                             {
                                                 if (ViewData["WorkingConditionSelectedRow"] != null)
                                                     if (row.DataItem.Id == (int)ViewData["WorkingConditionSelectedRow"])
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
                                            .Pageable(pager => pager.PageSize(5))
                                            .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                    .DetailView(ConditionDetailsView => ConditionDetailsView.Template(c =>
                                    {                            
                %>
                <fieldset class="ParentFieldset">
                    <legend class="ParentLegend"><%: Resources.Areas.JobDesc.ValueObjects.WorkingCondition.WorkingConditionModel.ConditionGridMasterTitle%></legend>
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                                   {%>
                                <img type="image" value="Add" onclick="AddCondition()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png")%>"
                                    title="<%: Resources.Shared.Buttons.Function.Add %>" alt="<%: Resources.Shared.Buttons.Function.Add %>" height="24" width="24" />
                                <script type="text/javascript">
                                    function AddCondition() {
                                        $('#result').fadeOut('fast');

                                        $('#result').load('<%: Url.Action("Index", "Condition")%>', function () {
                                            $('#result').fadeIn('slow');
                                        });
                                    }
                                </script>
                                <% } %>
                            </td>
                        </tr>
                    </table>
                    <%:Html.Telerik().Grid(c.Conditions)
                                                        .Name("Conditions" + c.Id)
                                                        .DataKeys(s => s.Add(x => x.Id))
                                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Condition"))
                                                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                                        .Columns(columns =>
                                                        {
                                                            columns.Bound(o => o.Id).Width(1).Groupable(false).Sortable(false).Filterable(false);
                                                            columns.Bound(o => o.Type.Name).Title(Resources.Areas.JobDesc.ValueObjects.Condition.ConditionModel.Type);
                                                            columns.Bound(o => o.Required);
                                                            columns.Command(s =>
                                                            {
                                                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                                {
                                                                    s.Delete().ButtonType(GridButtonType.Image);
                                                                }
                                                            }).Width(1);  
                                                        })
                                                        .Selectable()                                           
                                                        .ClientEvents(events =>events.OnRowSelect("ConditionsRowSelected"))
                    %>
                    <%
                                    })).Render();
                                                      

                    %>
                </fieldset>
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
                                        //Expand the first detail view
                                        if (row.Index == 0)
                                        {
                                            row.DetailRow.Expanded = true;
                                        }
                                        else
                                        {
                                            var requestKeys = Request.QueryString.Keys.Cast<string>();
                                            var expanded = requestKeys.Any(key => key.StartsWith("Specification" + row.DataItem.Id));
                                            row.DetailRow.Expanded = expanded;
                                        }
                                    })
                                    .ClientEvents(events => events.OnRowSelect("loadPartialView"))
                                    .Pageable(p => p.PageSize(5))
                                    .Render();
                %>
            </td>
            <script type="text/javascript">

                function tabStripSelect(e) {

                    var item = $(e.item);

                    $('#result').fadeOut();

                    $.ajax({
                        url: '<%=Url.Action("SaveTabIndexSecondLevel", "Specification")%>/', type: "POST",
                        data: { selectedIndex: item.index() }
                    });
                }

                function loadPartialView(e) {

                }

                function rowSelectedSkill(e) {

                    $('#result').fadeOut('fast');

                    //$(window).scrollTop($(window).height() - $(window).scrollTop());

                    var x = e.row.cells[0].innerHTML;

                    var url = '<%: Url.Action("Index", "JSkill", new { selectedRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('slow');
                    });
                }

                function rowSelectedCompetency(e) {

                    $('#result').fadeOut('fast');

                    //$(window).scrollTop($(window).height() - $(window).scrollTop());

                    var x = e.row.cells[0].innerHTML;
                    var url = '<%: Url.Action("Index", "Competency", new { selectedRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('slow');
                    });
                }

                function rowSelectedKnowledges(e) {

                    $('#result').fadeOut('fast');

                    //$(window).scrollTop($(window).height() - $(window).scrollTop());

                    var x = e.row.cells[0].innerHTML;

                    var url = '<%: Url.Action("Index", "Knowledge", new { selectedSubRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('slow');
                    });
                }



                function rowSelectedComputerSkill(e) {

                    $('#result').fadeOut('fast');

                    //$(window).scrollTop($(window).height() - $(window).scrollTop());

                    var x = e.row.cells[0].innerHTML;

                    var url = '<%: Url.Action("Index", "ComputerSkill", new { selectedSubRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('slow');
                    });
                }

                function rowSelectedEducation(e) {

                    $('#result').fadeOut('fast');

                    var x = e.row.cells[0].innerHTML;

                    var url = '<%: Url.Action("Index", "JEducation", new { selectedRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('slow');
                    });
                }

                function rowSelectedExperiences(e) {

                    $('#result').fadeOut('fast');

                    //$(window).scrollTop($(window).height() - $(window).scrollTop());

                    var x = e.row.cells[0].innerHTML;

                    var url = '<%: Url.Action("Index", "JExperience", new { selectedSubRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('slow');
                    });
                }


                function rowSelectedWorkingCondition(e) {

                    $('#result').fadeOut('fast');

                    //$(window).scrollTop($(window).height() - $(window).scrollTop());

                    var x = e.row.cells[1].innerHTML;

                    var url = '<%: Url.Action("Index", "WorkingCondition", new { selectedSubRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('slow');
                    });
                }


                function rowSelectedLangauge(e) {

                    $('#result').fadeOut('fast');

                    //$(window).scrollTop($(window).height() - $(window).scrollTop());

                    var x = e.row.cells[0].innerHTML;


                    var url = '<%: Url.Action("Index", "JLanguage", new { selectedRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('slow');
                    });
                }


                function ConditionsRowSelected(e) {

                    $('#result').fadeOut('fast');

                    //$(window).scrollTop($(window).height() - $(window).scrollTop());

                    var x = e.row.cells[0].innerHTML;

                    var url = '<%: Url.Action("Index", "Condition", new { selectedSubRowId = "Value1" }) %>'; //, selectedWorkingCondition = "Value2" }) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('slow');
                    });
                }

                function OnDetailViewExpandWorkingCondition(e) {

                    var x = e.masterRow.cells[1].innerHTML;

                    var url = '<%: Url.Action("Index", "WorkingCondition", new { selectedSubRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('slow');
                    });
                }
            </script>
        </tr>
    </table>
</fieldset>
