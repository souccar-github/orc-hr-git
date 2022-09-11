<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="HRIS.Domain.OrgChart.ValueObjects" %>
<fieldset class="ParentFieldset">
    <legend class="ParentLegend">
        <%:Resources.Areas.OrgChart.ValueObjects.Position.PositionModel.PositionsListTitle %></legend>
    <table width="100%">
        <tr>
            <td>
                <% if (ViewData["CanCreate"] != null && (bool)ViewData["CanCreate"])
                   {%>
                <a href="#" ondblclick="AddPosition()">
                    <img src="<%: Url.Content("~/Content/Ribbon/Icons/48/112.png") %>" title="<%:Resources.Shared.Buttons.Function.Add %>"
                        alt="<%:Resources.Shared.Buttons.Function.Add %>" height="36" width="36" align="middle"
                        onclick="AddPosition()" />
                    <script type="text/javascript">
                        function AddPosition() {
                            $('#result').fadeOut('fast');

                            $('#result').load('<%: Url.Action("Insert", "Position") %>', function () {
                                $('#result').fadeIn('slow');
                            });
                        }
                    </script>
                </a>
                <% } %>
                <%
                    Html.Telerik().Grid<Position>("positions")
                        .Name("PositionGrid")
                        .DataKeys(k => k.Add(o => o.Id))
                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "Position"))
                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                        .ClientEvents(client => client.OnError("Grid_onError"))
                        .Columns(c =>
                        {
                            c.Bound(o => o.Id).Width(1).Groupable(false).Filterable(false).Sortable(true);
                            c.Bound(o => o.Code).Width(100);
                            c.Bound(o => o.JobTitle.Name).Width(100).Title(Resources.Areas.OrgChart.ValueObjects.Position.PositionModel.JobTitle);
                            c.Bound(o => o.Level.Name).Width(100).Title(Resources.Areas.OrgChart.ValueObjects.Position.PositionModel.Level);
                            c.Bound(o => o.Type.Name).Width(100).Title(Resources.Areas.OrgChart.ValueObjects.Position.PositionModel.Type);
                            c.Bound(o => o.Id).Title("")
                                    .Format(Html.ActionLink(Resources.Shared.Buttons.Function.ViewReport, "Index", "JobDescReport", new { positionId = "{0}", Area = "Reporting" }, new { target = "_blank" }).ToHtmlString())
                                    .Encoded(false)
                                    .Width("5%").Sortable(false).Filterable(false);
                            c.Command(s =>
                            {
                                s.Select().ButtonType(GridButtonType.Image);
                                s.Delete().ButtonType(GridButtonType.Image);
                            }).Width("7%");
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

                                            if (e.Grades.Count != 0)
                                            {
                                                // 1st Item In Tab
                                                items.Add().Text(Resources.Areas.OrgChart.ValueObjects.Position.PositionModel.GradeTitle).Content(() =>
                                                {%>
                <%:Html.Telerik().Grid(e.Grades)
                                        .Name("PositionGrade" + e.Id)
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "PositionGrade")) 
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Groupable(false).Filterable(false).Sortable(true);
                                            columns.Bound(o => o.Name);
                                            columns.Bound(o => o.Step);
                                            columns.Command(s =>
                                            {
                                                s.Delete().ButtonType(GridButtonType.Image);
                                            
                                            }).Width(1); 
                                        })
                                        .ClientEvents(events => events.OnRowSelect("positionGradeRowSelected")).Selectable()                                  
                                        .Pageable(pager => pager.PageSize(5))
                                            .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                                                        
                %>
                <%
                                                });

                                                // 2nd Item In Tab
                                                items.Add().Text(Resources.Areas.OrgChart.ValueObjects.Position.PositionModel.AssetTitle).Content(() =>
                                                {%>
                <table>
                    <tr>
                        <td>
                            <input type="image" value="<%:Resources.Shared.Buttons.Function.Add %>" onclick="GridAddAssignedAsset()"
                                src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>" title="<%:Resources.Shared.Buttons.Function.Add %>"
                                alt="<%:Resources.Shared.Buttons.Function.Add %>" height="24" width="24"></input>
                            <script type="text/javascript">
                                function GridAddAssignedAsset() {
                                    $('#result').fadeOut('fast');
                                    $('#result').load('<%: Url.Action("Index", "AssignedAsset") %>', function () {
                                        $('#result').fadeIn('slow');
                                    });
                                }
                            </script>
                        </td>
                    </tr>
                </table>
                <%:Html.Telerik().Grid(e.ActiveGrade.Assets)
                                        .Name("Assets" + e.Id)
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "AssignedAsset"))
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Groupable(false).Filterable(false).Sortable(true);
                                            columns.Bound(o => o.SerialNo);
                                            columns.Bound(o => o.Name);
                                            columns.Bound(o => o.Type.Name);
                                            columns.Bound(o => o.Status.Name).Title(Resources.Areas.OrgChart.ValueObjects.AssignedGrade.AssignedAsset.AssignedAssetModel.Status);
                                            columns.Command(s =>
                                            {
                                                s.Delete().ButtonType(GridButtonType.Image);
                                                
                                            }).Width(1); 
                                        })
                                        .ClientEvents(events => events.OnRowSelect("assignedAssetRowSelected"))  
                                        .Selectable()                                  
                                        .Pageable(pager => pager.PageSize(5))
                                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                                                             
                %>
                <%});

                                                // 3rd Item In Tab
                                                items.Add().Text(Resources.Areas.OrgChart.ValueObjects.Position.PositionModel.CashBenefitTitle).Content(() =>
                                                {%>
                <table>
                    <tr>
                        <td>
                            <input type="image" value="<%:Resources.Shared.Buttons.Function.Add %>" onclick="GridAddAssignedCashBenefit()"
                                src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>" title="<%:Resources.Shared.Buttons.Function.Add %>"
                                alt="<%:Resources.Shared.Buttons.Function.Add %>" height="24" width="24" />
                            <script type="text/javascript">
                                function GridAddAssignedCashBenefit() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "AssignedCashBenefit") %>', function () {
                                        $('#result').fadeIn('slow');
                                    });
                                }
                            </script>
                        </td>
                    </tr>
                </table>
                <%:Html.Telerik().Grid(e.ActiveGrade.CashBenefits)
                                        .Name("CashBenefits" + e.Id)
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "AssignedCashBenefit"))  
                                                                                                                        
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Groupable(false).Filterable(false).Sortable(true);
                                            columns.Bound(o => o.Type.Name).Title(Resources.Areas.OrgChart.ValueObjects.AssignedGrade.AssignedCashBenefit.AssignedCashBenefitModel.Type);
                                            columns.Bound(o => o.Occurrence.Name).Title(Resources.Areas.OrgChart.ValueObjects.AssignedGrade.AssignedCashBenefit.AssignedCashBenefitModel.Occurrence);
                                            columns.Bound(o => o.Status.Name).Title(Resources.Areas.OrgChart.ValueObjects.AssignedGrade.AssignedCashBenefit.AssignedCashBenefitModel.Status);
                                            columns.Bound(o => o.ActiveDate);
                                            columns.Bound(o => o.InactiveDate);
                                            columns.Command(s =>
                                            {
                                                s.Delete().ButtonType(GridButtonType.Image);
                                                
                                            }).Width(1);
                                        })
                                        .ClientEvents(events => events.OnRowSelect("assignedCashBenefitRowSelected"))  
                                        .Selectable()                                  
                                        .Pageable(pager => pager.PageSize(5))
                                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                                                             
                %>
                <%});

                                                // Four Item In Tab
                                                items.Add().Text(Resources.Areas.OrgChart.ValueObjects.Position.PositionModel.CashDeductionTitle).Content(() =>
                                                {%>
                <table>
                    <tr>
                        <td>
                            <input type="image" value="<%:Resources.Shared.Buttons.Function.Add %>" onclick="GridAddAssignedCashDeduction()"
                                src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>" title="<%:Resources.Shared.Buttons.Function.Add %>"
                                alt="<%:Resources.Shared.Buttons.Function.Add %>" height="24" width="24" />
                            <script type="text/javascript">
                                function GridAddAssignedCashDeduction() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "AssignedCashDeduction") %>', function () {
                                        $('#result').fadeIn('slow');
                                    });
                                }
                            </script>
                        </td>
                    </tr>
                </table>
                <%:Html.Telerik().Grid(e.ActiveGrade.CashDeductions)
                                        .Name("CashDeductions" + e.Id)
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "AssignedCashDeduction"))  
                                                                                                                        
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Groupable(false).Filterable(false).Sortable(true);
                                            columns.Bound(o => o.Type.Name).Title(Resources.Areas.OrgChart.ValueObjects.AssignedGrade.AssignedCashDeduction.AssignedCashDeductionModel.Type);
                                            columns.Bound(o => o.Status.Name).Title(Resources.Areas.OrgChart.ValueObjects.AssignedGrade.AssignedCashDeduction.AssignedCashDeductionModel.Status);
                                            columns.Bound(o => o.ActiveDate);
                                            columns.Bound(o => o.InactiveDate);
                                            columns.Bound(o => o.Occurrence.Name).Title(Resources.Areas.OrgChart.ValueObjects.AssignedGrade.AssignedCashDeduction.AssignedCashDeductionModel.Occurrence);
                                            columns.Command(s =>
                                            {
                                                s.Delete().ButtonType(GridButtonType.Image);
                                            }).Width(1);
                                        })
                                        .ClientEvents(events => events.OnRowSelect("assignedCashDeductionRowSelected"))  
                                        .Selectable()                                  
                                        .Pageable(pager => pager.PageSize(5))
                                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                                                             
                %>
                <%});

                                                // Five Item In Tab
                                                items.Add().Text(Resources.Areas.OrgChart.ValueObjects.Position.PositionModel.InsuranceTitle).Content(() =>
                                                {%>
                <table>
                    <tr>
                        <td>
                            <input type="image" value="<%:Resources.Shared.Buttons.Function.Add %>" onclick="GridAddAssignedInsurance()"
                                src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>" title="<%:Resources.Shared.Buttons.Function.Add %>"
                                alt="<%:Resources.Shared.Buttons.Function.Add %>" height="24" width="24" />
                            <script type="text/javascript">
                                function GridAddAssignedInsurance() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "AssignedInsurance") %>', function () {
                                        $('#result').fadeIn('slow');
                                    });
                                }
                            </script>
                        </td>
                    </tr>
                </table>
                <%:Html.Telerik().Grid(e.ActiveGrade.Insurances)
                                    .Name("Insurances" + e.Id)
                                    .DataKeys(s => s.Add(x => x.Id))
                                    .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "AssignedInsurance"))  
                                                                                                                        
                                    .Columns(columns =>
                                    {
                                        columns.Bound(o => o.Id).Width(1).Groupable(false).Filterable(false).Sortable(true);
                                        columns.Bound(o => o.InsuranceNo);
                                        columns.Bound(o => o.Type.Name).Title(Resources.Areas.OrgChart.ValueObjects.AssignedGrade.AssignedInsurance.AssignedInsuranceModel.Type);
                                        columns.Bound(o => o.InsuranceCompany.Name).Title(Resources.Areas.OrgChart.ValueObjects.AssignedGrade.AssignedInsurance.AssignedInsuranceModel.InsuranceCompany);
                                        columns.Bound(o => o.ActiveDate);
                                        columns.Bound(o => o.ExpiryDate);
                                        columns.Command(s =>
                                        {
                                            s.Delete().ButtonType(GridButtonType.Image);
                                        }).Width(1);
                                    })
                                    .ClientEvents(events => events.OnRowSelect("assignedInsuranceRowSelected"))  
                                    .Selectable()                                  
                                    .Pageable(pager => pager.PageSize(5))
                                    .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                                                             
                %>
                <%});

                                                // Six Item In Tab
                                                items.Add().Text(Resources.Areas.OrgChart.ValueObjects.Position.PositionModel.NonCashBenefitTitle).Content(() =>
                                                {%>
                <table>
                    <tr>
                        <td>
                            <input type="image" value="<%:Resources.Shared.Buttons.Function.Add %>" onclick="GridAddAssignedNonCashBenefit()"
                                src="<%= Url.Content("~/Content/Ribbon/Icons/48/112.png") %>" title="<%:Resources.Shared.Buttons.Function.Add %>"
                                alt="<%:Resources.Shared.Buttons.Function.Add %>" height="24" width="24" />
                            <script type="text/javascript">
                                function GridAddAssignedNonCashBenefit() {
                                    $('#result').fadeOut('fast');

                                    $('#result').load('<%: Url.Action("Index", "AssignedNonCashBenefit") %>', function () {
                                        $('#result').fadeIn('slow');
                                    });
                                }
                            </script>
                        </td>
                    </tr>
                </table>
                <%:Html.Telerik().Grid(e.ActiveGrade.NonCashBenefits)
                                        .Name("NonCashBenefits" + e.Id)
                                        .DataKeys(s => s.Add(x => x.Id))
                                        .DataBinding(dataBinding => dataBinding.Server().Delete("Delete", "AssignedNonCashBenefit"))  
                                                                                                                        
                                        .Columns(columns =>
                                        {
                                            columns.Bound(o => o.Id).Width(1).Groupable(false).Filterable(false).Sortable(true);
                                            columns.Bound(o => o.Type.Name).Title(Resources.Areas.OrgChart.ValueObjects.AssignedGrade.AssignedNonCashBenefit.AssignedNonCashBenefitModel.Type);
                                            columns.Bound(o => o.Status.Name).Title(Resources.Areas.OrgChart.ValueObjects.AssignedGrade.AssignedNonCashBenefit.AssignedNonCashBenefitModel.Status);
                                            columns.Bound(o => o.Occurrence.Name).Title(Resources.Areas.OrgChart.ValueObjects.AssignedGrade.AssignedNonCashBenefit.AssignedNonCashBenefitModel.Occurrence);
                                            columns.Bound(o => o.ActiveDate);
                                            columns.Bound(o => o.InactiveDate);
                                            columns.Command(s => s.Delete().ButtonType(GridButtonType.Image)).Width(1);
                                        })
                                        .ClientEvents(events => events.OnRowSelect("assignedNonCashBenefitRowSelected"))  
                                        .Selectable()                                  
                                        .Pageable(pager => pager.PageSize(5))
                                        .Filterable().Sortable(s => s.OrderBy(order => order.Add(o => o.Id).Descending()))
                                                                             
                %>
                <%});
                                                
                                            }
                                            else
                                            {
                                                items.Add().Text(Resources.Areas.OrgChart.ValueObjects.Position.PositionRules.NoGradeAssignedMessage);
                                            }

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
                                        .Selectable()
                                        .Render();
                %>
            </td>
            <script type="text/javascript">

                function tabStripSelect(e) {

                    var item = $(e.item);

                    $('#result').fadeOut('fast');

                    $.ajax({
                        url: '<%=Url.Action("SaveTabIndex", "Position")%>/', type: "POST",
                        data: { selectedIndex: item.index() }
                    });
                }

                function SetMasterRecordValue(e) {

                    var x = e.masterRow.cells[1].innerHTML;

                    $('#result').fadeOut('fast');

                    $('#result').load('<%: Url.Action("PartialInfo", "Position") %>', { selectedPositionId: x }, function () {
                        $('#result').fadeIn('fast');

                        loadRibbon();
                    });
                }

                function loadPartialView(e) {

                    $('> .t-hierarchy-cell > .t-icon', e.row).click();

                    $('#result').fadeOut('fast');
                }

                function positionGradeRowSelected(e) {

                    $('#result').fadeOut('fast');

                    var x = e.row.cells[0].innerHTML;

                    var url = '<%: Url.Action("Index", "PositionGrade") %>';

                    $('#result').load(url, function () {
                        $('#result').fadeIn('fast');
                        loadRibbon();
                    });
                }

                function assignedAssetRowSelected(e) {

                    $('#result').fadeOut('fast');

                    var x = e.row.cells[0].innerHTML;

                    var url = '<%: Url.Action("Index", "AssignedAsset", new { selectedSubRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('fast');
                        loadRibbon();
                    });
                }

                function assignedCashBenefitRowSelected(e) {

                    $('#result').fadeOut('fast');

                    var x = e.row.cells[0].innerHTML;

                    var url = '<%: Url.Action("Index", "AssignedCashBenefit", new { selectedSubRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('fast');
                        loadRibbon();
                    });
                }

                function assignedCashDeductionRowSelected(e) {

                    $('#result').fadeOut('fast');

                    var x = e.row.cells[0].innerHTML;

                    var url = '<%: Url.Action("Index", "AssignedCashDeduction", new { selectedSubRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('slow');
                        loadRibbon();
                    });
                }

                function assignedInsuranceRowSelected(e) {

                    $('#result').fadeOut('fast');

                    var x = e.row.cells[0].innerHTML;

                    var url = '<%: Url.Action("Index", "AssignedInsurance", new { selectedSubRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('fast');
                        loadRibbon();
                    });
                }

                function assignedNonCashBenefitRowSelected(e) {

                    $('#result').fadeOut('fast');

                    var x = e.row.cells[0].innerHTML;

                    var url = '<%: Url.Action("Index", "AssignedNonCashBenefit", new { selectedSubRowId = "Value1"}) %>';
                    url = url.replace("Value1", x);

                    $('#result').load(url, function () {
                        $('#result').fadeIn('fast');
                        loadRibbon();
                    });
                }

                function loadRibbon() {
                    $('#OrgFunctionsArea').load('<%: Url.Action("GetFunctionsPartial", "OrganizationChart") %>');
                }
            </script>
        </tr>
    </table>
</fieldset>
