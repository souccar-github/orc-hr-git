<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<table width="100%">
    <tr>
        <td>
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <div class="box" style="z-index: 1000">
                            <ul id="ldd_menu" class="ldd_menu">
                                <li><span>Position</span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading">Status</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "PositionStatus", new { area = "OrganizationChart" }, null)%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">Type</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "PositionType", new { area = "OrganizationChart" }, null)%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">Level</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "PositionLevel", new { area = "OrganizationChart" }, null)%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">Job Title</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "JobTitle", new { area = "OrganizationChart" }, null)%>
                                            </a></li>
                                        </ul>
                                        <%--<ul>
                                            <li class="ldd_heading">Job Role/Specialization</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "JobRole", new { area = "OrganizationChart" }, null)%>
                                            </a></li>
                                        </ul>--%>
                                        <ul>
                                            <li class="ldd_heading">Direct Reporting To</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "JobTitle", new { area = "OrganizationChart" }, null)%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">InDirect Reporting To</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "JobTitle", new { area = "OrganizationChart" }, null)%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">Cost Center</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "CostCenter", new { area = "OrganizationChart" }, null)%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">Disability Status</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "DisabilityStatus", new { area = "OrganizationChart" }, null)%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">Time Interval</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "TimeInterval", new { area = "OrganizationChart" }, null)%>
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
                                            <li class="ldd_heading">Currency Type</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "CurrencyType", null, new { area = "OrganizationChart" })%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">Status</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "AssetStatus", null, new { area = "OrganizationChart" })%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span>Cash Benefits</span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading">Type</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "CashBenefitType", new { area = "OrganizationChart" }, null)%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">Occurrence</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "TimeInterval", new { area = "OrganizationChart" }, null)%>
                                            </a></li>
                                        </ul>                                        
                                    </div>
                                </li>
                                <li><span>Cash Deduction</span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading">Type</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "CashDeductionType", new { area = "OrganizationChart" }, null)%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">Occurrence</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "TimeInterval", new { area = "OrganizationChart" }, null)%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span>Insurance</span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading">Type</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "InsuranceType", new { area = "OrganizationChart" }, null)%>
                                            </a></li>
                                        </ul>                                   <ul>
                                            <li class="ldd_heading">Insurance Company</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "InsuranceCompany", new { area = "OrganizationChart" }, null)%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span>Non-Cash Benefit</span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading">Type</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "NonCashBenefitType", new { area = "OrganizationChart" }, null)%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">Occurrence</li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "TimeInterval", new { area = "OrganizationChart" }, null)%>
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
