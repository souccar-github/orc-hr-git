<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="HRIS.Domain.OrgChart.Entities" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend">
        <%:Resources.Areas.OrgChart.Entities.Grade.GradeModel.GradesMasterGridTitle %></legend>
    <table width="100%">
        <tr>
            <td>
                <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                   {%>
                <a href="<%: Url.Action("Insert", "Grade") %>">
                    <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>" title="<%: Resources.Shared.Buttons.Function.Add %>"
                        alt="<%: Resources.Shared.Buttons.Function.Add %>" height="36" width="36" />
                </a>
                <% } %>
                <%
                    Html.Telerik().Grid<Grade>("Grades")
                        .Name("Grade")
                        .DataKeys(k => k.Add(o => o.Id))
                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Grade"))
                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                        .Columns(c =>
                        {
                            c.Bound(o => o.Id).Width(1).Groupable(false).Filterable(false).Sortable(true);
                            c.Bound(b => b.Level.Name).Title(Resources.Areas.OrgChart.Entities.Grade.GradeModel.Level);
                            c.Bound(b => b.Name.Name).Title(Resources.Areas.OrgChart.Entities.Grade.GradeModel.Name);
                            c.Bound(b => b.Step.Name).Title(Resources.Areas.OrgChart.Entities.Grade.GradeModel.Step);
                            c.Command(s =>
                            {
                                s.Select().ButtonType(GridButtonType.Image);
                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                {
                                    s.Delete().ButtonType(GridButtonType.Image);
                                }
                            }
                            ).Width("7%");
                        })

                        .DetailView(detailView => detailView.Template(e => // Set the server template
                            {
                %>
                <% Html.Telerik().TabStrip()
                                                      .Name("TabStrip_" + e.Id)
                                                      .Effects(fx => fx.Opacity())
                                                      .SelectedIndex(Session["SelectedTabIndex"] != null ? int.Parse(Session["SelectedTabIndex"].ToString()) : 0)
                                                      .Items(items =>
                                                      {
                                                          // 1st Item In Tab
                                                          items.Add().Text(Resources.Areas.OrgChart.Entities.Grade.GradeModel.InsurancesTitle).Content(() =>
                                    {
                %>
                <table>
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridAddInsurance()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title="<%: Resources.Shared.Buttons.Function.Add %>" alt="<%: Resources.Shared.Buttons.Function.Add %>"
                                height="24" width="24" />
                            <script type="text/javascript">
                                function GridAddInsurance() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "Insurance") %>', function () {
                                        $('#result').fadeIn('fast');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%: Html.Telerik().Grid(e.Insurances)
                                        .Name("Insurances" + e.Id)
                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Insurance"))
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Groupable(false).Filterable(false).Sortable(true);
                                            columns.Bound(o => o.Type.Name).Title(Resources.Areas.OrgChart.ValueObjects.Insurance.InsuranceModel.Type);
                                            columns.Bound(o => o.InsuranceCoverageRatio);
                                            columns.Bound(o => o.InsuranceCompany.Name);
                                            columns.Bound(o => o.CompanyAddress);
                                            columns.Bound(o => o.RepresentativeContact);
                                            columns.Command(s =>
                                                                {
                                                                    if (ViewData["CanDelete"] != null &&
                                                                        (bool) ViewData["CanDelete"])
                                                                    {
                                                                        s.Delete().ButtonType(GridButtonType.Image);
                                                                    }
                                                                }).Width(1);
                                        })
                                        .ClientEvents(events => events.OnRowSelect("InsurancesRowSelected")).Selectable()
                                        .Pageable(pager => pager.PageSize(2))
                                         .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))                                                                          
                                                                             
                %>
                <%
                                    });

                                                          // 2nd Item In Tab
                                                          items.Add().Text(Resources.Areas.OrgChart.Entities.Grade.GradeModel.CashDeductionsTitle).Content(() =>
                                                                                {
                %>
                <table>
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridAddCashDetuction()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title="<%: Resources.Shared.Buttons.Function.Add %>" alt="<%: Resources.Shared.Buttons.Function.Add %>"
                                height="24" width="24" align="middle" />
                            <script type="text/javascript">
                                function GridAddCashDetuction() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "CashDeduction") %>', function () {
                                        $('#result').fadeIn('fast');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%:Html.Telerik().Grid(e.CashDeductions)
                                         .Name("CashDeductions" + e.Id)
                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "CashDeduction"))                                                                                                             
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Groupable(false).Filterable(false).Sortable(true);
                                            columns.Bound(o => o.Type.Name).Title(Resources.Areas.OrgChart.ValueObjects.CashDeduction.CashDeductionModel.Type);
                                            columns.Bound(o => o.Occurrence.Name).Title(Resources.Areas.OrgChart.ValueObjects.CashDeduction.CashDeductionModel.Occurrence);
                                            columns.Bound(o => o.Description);
                                            columns.Command(s =>
                                            {
                                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                {
                                                    s.Delete().ButtonType(GridButtonType.Image);
                                                }
                                            }).Width(1);                                                                                                                                                         
                                        })
                                        .ClientEvents(events => events.OnRowSelect("CashDeductionsRowSelected"))  
                                        .Selectable()                                  
                                        .Pageable(pager => pager.PageSize(5))
                                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                %>
                <%
                                                                                });

                                                          // 2nd Item In Tab
                                                          items.Add().Text(Resources.Areas.OrgChart.Entities.Grade.GradeModel.AssetTitle).Content(() =>
                                                                                {
                %>
                <table>
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridAddAsset()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title="<%: Resources.Shared.Buttons.Function.Add %>" alt="<%: Resources.Shared.Buttons.Function.Add %>"
                                height="24" width="24" align="middle" />
                            <script type="text/javascript">
                                function GridAddAsset() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "Assets") %>', function () {
                                        $('#result').fadeIn('fast');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%:Html.Telerik().Grid(e.Assets)
                                                                            .Name("Assets" + e.Id)
                                                                          .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Assets"))                                                                                                             
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Groupable(false).Filterable(false).Sortable(true);
                                            columns.Bound(o => o.SerialNo);
                                            columns.Bound(o => o.Name);
                                            columns.Bound(o => o.PurchaseDate);
                                            columns.Bound(o => o.Status.Name).Title(Resources.Areas.OrgChart.ValueObjects.Asset.AssetModel.Status);   
                                            columns.Bound(o => o.Type.Name).Title(Resources.Areas.OrgChart.ValueObjects.Asset.AssetModel.Type);
                                            columns.Bound(o => o.UnitCost);
                                            columns.Command(s =>
                                             {
                                                 if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                 {
                                                     s.Delete().ButtonType(GridButtonType.Image);
                                                 }
                                             }).Width(1);                                                                                                                                                       
                                        })
                                        .ClientEvents(events => events.OnRowSelect("AssetsRowSelected"))  
                                        .Selectable()                                  
                                        .Pageable(pager => pager.PageSize(5))
                                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                %>
                <%
                                                                                });

                                                          // 2nd Item In Tab
                                                          items.Add().Text(Resources.Areas.OrgChart.Entities.Grade.GradeModel.CashBenefitsTitle).Content(() =>
                                                                                {
                %>
                <table>
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridAddCashBenefitst()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title="<%: Resources.Shared.Buttons.Function.Add %>" alt="<%: Resources.Shared.Buttons.Function.Add %>"
                                height="24" width="24" align="middle" />
                            <script type="text/javascript">
                                function GridAddCashBenefitst() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "CashBenefit") %>', function () {
                                        $('#result').fadeIn('fast');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%:Html.Telerik().Grid(e.CashBenefits)
                                                                            .Name("CashBenefits" + e.Id)
                                                                          .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "CashBenefit"))                                                                                                             
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Groupable(false).Filterable(false).Sortable(true);
                                            columns.Bound(o => o.Type.Name).Title(Resources.Areas.OrgChart.ValueObjects.CashBenefit.CashBenefitModel.Type);
                                            columns.Bound(o => o.Occurrence.Name).Title(Resources.Areas.OrgChart.ValueObjects.CashBenefit.CashBenefitModel.Occurrence);    
                                            columns.Bound(o => o.EmployeePaymentAmount); 
                                            columns.Bound(o => o.CompanyPaymentAmount); 
                                            columns.Bound(o => o.CompanyPaymentRatio);  
                                            columns.Bound(o => o.CompanyDeductionRatio);
                                            columns.Command(s =>
                                            {
                                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                {
                                                    s.Delete().ButtonType(GridButtonType.Image);
                                                }
                                            }).Width(1);
                                        })
                                        .ClientEvents(events => events.OnRowSelect("CashBenefitsSelected"))  
                                        .Selectable()                                  
                                        .Pageable(pager => pager.PageSize(5))
                                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                %>
                <%
                                                                                });

                                                          // 2nd Item In Tab
                                                          items.Add().Text(Resources.Areas.OrgChart.Entities.Grade.GradeModel.NonCashBenefitsTitle).Content(() =>
                                                                                {
                %>
                <table>
                    <tr>
                        <td>
                            <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                               {%>
                            <input type="image" value="Add" onclick="GridNonCashBenefit()" src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>"
                                title="<%: Resources.Shared.Buttons.Function.Add %>" alt="<%: Resources.Shared.Buttons.Function.Add %>"
                                height="24" width="24" align="middle" />
                            <script type="text/javascript">
                                function GridNonCashBenefit() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "NonCashBenefit") %>', function () {
                                        $('#result').fadeIn('fast');
                                    });
                                }
                            </script>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <%:Html.Telerik().Grid(e.NonCashBenefits)
                                                                            .Name("NonCashBenefits" + e.Id)
                                                                          .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "NonCashBenefit"))                                                                                                             
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Groupable(false).Filterable(false).Sortable(true);
                                            columns.Bound(o => o.Type.Name).Title(Resources.Areas.OrgChart.ValueObjects.NonCashBenefit.NonCashBenefitModel.Type);
                                            columns.Bound(o => o.Occurrence.Name).Title(Resources.Areas.OrgChart.ValueObjects.NonCashBenefit.NonCashBenefitModel.Occurrence);  
                                            columns.Bound(o => o.Description);
                                            columns.Command(s =>
                                            {
                                                if (ViewData["CanDelete"] != null && (bool)ViewData["CanDelete"])
                                                {
                                                    s.Delete().ButtonType(GridButtonType.Image);
                                                }
                                            }).Width(1);                                                                                                                                                   
                                        })
                                        .ClientEvents(events => events.OnRowSelect("NonCashBenefitsSelected"))  
                                        .Selectable()                                  
                                        .Pageable(pager => pager.PageSize(5))
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
                                        .Selectable()
                                        .Render();
                %>
            </td>
            <script type="text/javascript">

                function tabStripSelect(e) {

                    var item = $(e.item);

                    $('#result').fadeOut();

                    $.ajax({
                        url: '<%=Url.Action("SaveTabIndex", "Grade")%>/', type: "POST",
                        data: { selectedIndex: item.index() }
                    });
                }

                function SetMasterRecordValue(e) {

                    var x = e.masterRow.cells[1].innerHTML;

                    $('#result').load('<%: Url.Action("PartialMasterInfo", "Grade") %>', { selectedRowId: x }, function () {
                        $('#result').fadeIn('slow');

                        loadGradeRibbon();
                    });
                }

                function loadPartialView(e) {

                    $('> .t-hierarchy-cell > .t-icon', e.row).click();

                    $('#result').fadeOut('fast');
                }

                function loadMasterPartialView(id) {

                    $('> .t-hierarchy-cell > .t-icon', e.row).click();

                    $('#result').fadeOut('fast');

                    $('#result').load('<%: Url.Action("PartialMasterInfo", "Grade") %>', { selectedRowId: id.toString() }, function () {
                        $('#result').fadeIn('slow');

                        loadGradeRibbon();
                    });
                }

                function InsurancesRowSelected(e) {

                    $('#result').fadeOut('fast');

                    var x = e.row.cells[0].innerHTML;
                    var url = '<%: Url.Action("Index", "Insurance", new { selectedSubRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('slow');
                    });
                }

                function CashDeductionsRowSelected(e) {

                    $('#result').fadeOut('fast');

                    var x = e.row.cells[0].innerHTML;
                    var url = '<%: Url.Action("Index", "CashDeduction", new { selectedSubRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('fast');
                    });
                }

                function AssetsRowSelected(e) {

                    $('#result').fadeOut('fast');

                    var x = e.row.cells[0].innerHTML;
                    var url = '<%: Url.Action("Index", "Assets", new { selectedSubRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('fast');
                    });
                }

                function CashBenefitsSelected(e) {

                    $('#result').fadeOut('fast');

                    var x = e.row.cells[0].innerHTML;
                    var url = '<%: Url.Action("Index", "CashBenefit", new { selectedSubRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('fast');
                    });
                }

                function NonCashBenefitsSelected(e) {

                    $('#result').fadeOut('fast');

                    var x = e.row.cells[0].innerHTML;
                    var url = '<%: Url.Action("Index", "NonCashBenefit", new { selectedSubRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('fast');
                    });
                }

                function loadGradeRibbon() {
                    $('#OrgFunctionsArea').load('<%: Url.Action("GetFunctionsPartial", "OrganizationChart") %>');
                }
            </script>
        </tr>
    </table>
</fieldset>
