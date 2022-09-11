<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<table width="80%">
    <tr>
        <td>
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <div class="box" style="z-index: 1000">
                            <ul id="ldd_menu" class="ldd_menu">
                                <li><span><%: Resources.Areas.Objective.Views.Shared.Ribbon._1stLevel.Indexes.Objective %></span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Objective.Views.Shared.Ribbon._1stLevel.Indexes.Type %></li>
                                            <li><a href="#">
                                                <%:Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "ObjectiveType", new { area = "Objective" }, null)%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Objective.Views.Shared.Ribbon._1stLevel.Indexes.Priority %></li>
                                            <li><a href="#">
                                                <%:Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Priority", new { area = "JobDesc" }, null)%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span><%: Resources.Areas.Objective.Views.Shared.Ribbon._1stLevel.Indexes.ObjectiveKPI %></span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Objective.Views.Shared.Ribbon._1stLevel.Indexes.Type %></li>
                                            <li><a href="#">
                                                <%:Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "ObjectiveKpiType", new { area = "Objective" }, null)%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span><%: Resources.Areas.Objective.Views.Shared.Ribbon._1stLevel.Indexes.ActionPlanStep %></span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Objective.Views.Shared.Ribbon._1stLevel.Indexes.Status %></li>
                                            <li><a href="#">
                                                <%:Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "StepStatus", new { area = "Objective" }, null)%>
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
