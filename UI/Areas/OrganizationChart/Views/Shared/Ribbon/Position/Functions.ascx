<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<table width="80%">
    <tr>
        <td>
            <table>
                <tr>
                    <td>
                        <div class="box" style="z-index: 1000">
                            <ul id="ldd_menu" class="ldd_menu">
                                <li><span>
                                    <%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.OrganizationSectionTitle %></span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading">
                                                <%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.OrganizationTitle %></li>
                                            <li><a href="<%: Url.Action("Index", "Organization", new { area = "OrganizationChart" }) %>">
                                                <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/101.png") %>" title="<%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.BasicInformationTitle %>"
                                                    alt="<%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.BasicInformationTitle %>"
                                                    height="18" width="18" align="middle" /><%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.BasicInformationTitle %></a></li>
                                            <li><a href="<%: Url.Action("LoadTree", "Node", new { area = "OrganizationChart" }) %>">
                                                <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/101.png") %>" title="<%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.OrganizationHierarchyTitle %>"
                                                    alt="<%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.OrganizationHierarchyTitle %>"
                                                    height="18" width="18" align="middle" /><%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.OrganizationHierarchyTitle %></a></li>
                                            <li><a href="<%: Url.Action("Index", "NodeType", new { area = "OrganizationChart" }) %>">
                                                <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/101.png") %>" title="<%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.NodeTypeOrderTitle %>"
                                                    alt="<%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.NodeTypeOrderTitle %>"
                                                    height="18" width="18" align="middle" /><%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.NodeTypeOrderTitle %></a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span>
                                    <%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.NodeSectionTitle %></span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading">
                                                <%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.NodeTitle %></li>
                                            <li><a href="<%: Url.Action("LoadTree", "Node", new { area = "OrganizationChart" }) %>">
                                                <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/101.png") %>" title="<%:Resources.Shared.Buttons.Ribbon.AddBrowse%>"
                                                    alt="<%:Resources.Shared.Buttons.Ribbon.AddBrowse%>" height="18" width="18" />
                                                <%:Resources.Shared.Buttons.Ribbon.AddBrowse%>
                                            </a></li>
                                            <br />
                                            <li class="ldd_heading">
                                                <%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.NodeTypeOrderTitle %></li>
                                            <li><a href="<%: Url.Action("Index", "NodeType", new { area = "OrganizationChart" }) %>">
                                                <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/101.png") %>" title="<%:Resources.Shared.Buttons.Ribbon.AddBrowse%>"
                                                    alt="<%:Resources.Shared.Buttons.Ribbon.AddBrowse%>" height="18" width="18" /><%:Resources.Shared.Buttons.Ribbon.AddBrowse%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">
                                                <%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.NodeServicesTitle %></li>
                                            <li><a href="<%: Url.Action("LoadReassignNodesPositions", "Node", new { area = "OrganizationChart" }) %>">
                                                <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/101.png") %>" title="<%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.ReassignTitle %>"
                                                    alt="<%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.ReassignTitle %>"
                                                    height="18" width="18" onclick="ReassignMenuLink()" /><%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.ReassignTitle %>
                                            </a></li>
                                            <li><a href="<%: Url.Action("LoadNodesReorder", "Node", new { area = "OrganizationChart" }) %>">
                                                <img src="<%= Url.Content("~/Content/Ribbon/Icons/48/101.png") %>" title="<%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.ReorderTitle %>"
                                                    alt="<%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.ReorderTitle %>"
                                                    height="18" width="18" onclick="ReorderMenuLink()" /><%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.ReorderTitle %>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span>
                                    <%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.GradesSectionTitle %></span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading">
                                                <%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.GradeTitle %></li>
                                            <li>
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "Grade", new { selectedTabOrder = 0, ribbon = true }, null)%>
                                            </li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">
                                                <%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.InsurancesTitle %></li>
                                            <li>
                                                <%: Html.ActionLink(
                                                String.Format(Resources.Shared.Messages.Ribbon.ChooseA,Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.GradeTitle), "Index", "Grade", new { selectedTabOrder = 0 }, null)%>
                                            </li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">
                                                <%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.CashDeductionsTitle %></li>
                                            <li>
                                                <%: Html.ActionLink(String.Format(Resources.Shared.Messages.Ribbon.ChooseA, Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.GradeTitle), "Index", "Grade", new { selectedTabOrder = 0 }, null)%>
                                            </li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">
                                                <%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.AssetsTitle %></li>
                                            <li>
                                                <%: Html.ActionLink(String.Format(Resources.Shared.Messages.Ribbon.ChooseA, Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.GradeTitle), "Index", "Grade", new { selectedTabOrder = 0 }, null)%>
                                            </li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">
                                                <%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.CashBenefitsTitle %></li>
                                            <li>
                                                <%: Html.ActionLink(String.Format(Resources.Shared.Messages.Ribbon.ChooseA, Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.GradeTitle), "Index", "Grade", new { selectedTabOrder = 0 }, null)%>
                                            </li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">
                                                <%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.NonCashBenefitsTitle %></li>
                                            <li>
                                                <%: Html.ActionLink(String.Format(Resources.Shared.Messages.Ribbon.ChooseA, Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.GradeTitle), "Index", "Grade", new { selectedTabOrder = 0 }, null)%>
                                            </li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span>
                                    <%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.PositionSectionTitle %></span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading">
                                                <%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.PositionTitle %></li>
                                            <li>
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "Position", new { selectedTabOrder = 0, ribbon = true }, null)%>
                                            </li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">
                                                <%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.GradeTitle %></li>
                                            <li>
                                                <%
                                                    if (Session["CurrentSecondLevelRowId"] != null)
                                                    {
                                                        if ((int)Session["CurrentSecondLevelRowId"] != 0)
                                                        {	
                                                %>
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "Position", new { selectedTabOrder = 0 }, null)%>
                                                <%
                                                        }
                                                    }
                                                    else
                                                    {
                                                %>
                                                <%:String.Format(Resources.Shared.Messages.Ribbon.ChooseA,Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.PositionTitle)%>
                                                <%
                                                    }
                                                %>
                                            </li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">
                                                <%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.AssetsTitle %></li>
                                            <li>
                                                <%
                                                    if (Session["CurrentSecondLevelRowId"] != null)
                                                    {
                                                        if ((int)Session["CurrentSecondLevelRowId"] != 0)
                                                        {	
                                                %>
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "Position", new { selectedTabOrder = 1 }, null)%>
                                                <%
                                                        }
                                                    }
                                                    else
                                                    {
                                                %>
                                                <%:String.Format(Resources.Shared.Messages.Ribbon.ChooseA,Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.PositionTitle)%>
                                                <%
                                                    }
                                                %>
                                            </li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">
                                                <%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.CashBenefitsTitle %></li>
                                            <li>
                                                <%
                                                    if (Session["CurrentSecondLevelRowId"] != null)
                                                    {
                                                        if ((int)Session["CurrentSecondLevelRowId"] != 0)
                                                        {	
                                                %>
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "Position", new { selectedTabOrder = 2 }, null)%>
                                                <%
                                                        }
                                                    }
                                                    else
                                                    {
                                                %>
                                                <%:String.Format(Resources.Shared.Messages.Ribbon.ChooseA,Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.PositionTitle)%>
                                                <%
                                                    }
                                                %>
                                            </li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">
                                                <%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.CashDeductionsTitle %></li>
                                            <li>
                                                <%
                                                    if (Session["CurrentSecondLevelRowId"] != null)
                                                    {
                                                        if ((int)Session["CurrentSecondLevelRowId"] != 0)
                                                        {	
                                                %>
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "Position", new { selectedTabOrder = 3 }, null)%>
                                                <%
                                                        }
                                                    }
                                                    else
                                                    {
                                                %>
                                                <%:String.Format(Resources.Shared.Messages.Ribbon.ChooseA,Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.PositionTitle)%>
                                                <%
                                                    }
                                                %>
                                            </li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">
                                                <%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.InsurancesTitle %></li>
                                            <li>
                                                <%
                                                    if (Session["CurrentSecondLevelRowId"] != null)
                                                    {
                                                        if ((int)Session["CurrentSecondLevelRowId"] != 0)
                                                        {	
                                                %>
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "Position", new { selectedTabOrder = 4 }, null)%>
                                                <%
                                                        }
                                                    }
                                                    else
                                                    {
                                                %>
                                                <%:String.Format(Resources.Shared.Messages.Ribbon.ChooseA,Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.PositionTitle)%>
                                                <%
                                                    }
                                                %>
                                            </li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">
                                                <%:Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.NonCashBenefitsTitle %></li>
                                            <li>
                                                <%
                                                    if (Session["CurrentSecondLevelRowId"] != null)
                                                    {
                                                        if ((int)Session["CurrentSecondLevelRowId"] != 0)
                                                        {	
                                                %>
                                                <%: Html.ActionLink(Resources.Shared.Buttons.Ribbon.AddBrowse, "Index", "Position", new { selectedTabOrder = 5 }, null)%>
                                                <%
                                                        }
                                                    }
                                                    else
                                                    {
                                                %>
                                                <%:String.Format(Resources.Shared.Messages.Ribbon.ChooseA,Resources.Areas.OrgChart.Views.Shared.Ribbon.Functions.Functions.PositionTitle)%>
                                                <%
                                                    }
                                                %>
                                            </li>
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
