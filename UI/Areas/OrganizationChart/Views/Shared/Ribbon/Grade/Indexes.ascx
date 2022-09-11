<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<table width="80%">
    <tr>
        <td>
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <div class="box" style="z-index: 1000">
                            <ul id="ldd_menu" class="ldd_menu">
                                <li><span>Grade</span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading">Group Salary Scales</li>
                                            <li><a href="#">
                                                    <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "GroupSalaryScale", null, new { area = "OrganizationChart" })%>
                                                </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">Job Group</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "JobGroup", null, new { area = "OrganizationChart" })%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li> 
                                <li><span>Insurances</span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading">Type</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "InsuranceType", null, new { area = "OrganizationChart" })%>
                                            </a></li>
                                        </ul>                                      <ul>
                                            <li class="ldd_heading">Insurance Company</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "InsuranceCompany", null, new { area = "OrganizationChart" })%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span>Cash Deduction</span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading">Type</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "CashDeductionType", null, new { area = "OrganizationChart" })%>
                                            </a></li> 
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">Occurrence</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "TimeInterval", null, new { area = "OrganizationChart" })%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span>Assets</span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading">Type</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "AssetType", null, new { area = "OrganizationChart" })%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">Status</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "AssetStatus", null, new { area = "OrganizationChart" })%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">Currency Type</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "CurrencyType", null, new { area = "OrganizationChart" })%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span>Cash Benefit</span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading">Type</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "CashBenefitType", null, new { area = "OrganizationChart" })%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">Occurrence</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "TimeInterval", null, new { area = "OrganizationChart" })%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span>Non-Cash Benefit</span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading">Type</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "NonCashBenefitType", null, new { area = "OrganizationChart" })%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">Occurrence</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "TimeInterval", null, new { area = "OrganizationChart" })%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<script type="text/javascript">
    $(function () {
        /**
        * the menu
        */
        var $menu = $('#ldd_menu');

        /**
        * for each list element,
        * we show the submenu when hovering and
        * expand the span element (title) to 510px
        */
        $menu.children('li').each(function () {
            var $this = $(this);
            var $span = $this.children('span');
            $span.data('width', $span.width());

            $this.bind('mouseenter', function () {
                $menu.find('.ldd_submenu').stop(true, true).hide();
                $span.stop().animate({ 'width': '100px' }, 300, function () {
                    $this.find('.ldd_submenu').slideDown(300);
                });
            }).bind('mouseleave', function () {
                $this.find('.ldd_submenu').stop(true, true).hide();
                $span.stop().animate({ 'width': $span.data('width') + 'px' }, 300);
            });
        });
    });
</script>
