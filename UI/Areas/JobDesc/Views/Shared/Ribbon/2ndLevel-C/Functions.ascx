<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<table width="80%">
    <tr>
        <td>
            <table>
                <tr>
                    <td>
                        <div class="box" style="z-index: 1000">
                            <ul id="ldd_menu" class="ldd_menu">
                                <li><span><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Functions.GeneralInfo%></span>
                                    <div class="ldd_submenu" style="z-index: 1002">
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Functions.JobDescription%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "JobDescEntity", new { selectedTabOrder = 0 , ribbon = true }, null)%></a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Functions.Authority%></span>
                                    <div class="ldd_submenu" style="z-index: 1002">
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Functions.Authority%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoBackward", "JobDescEntity", new { stepOrder = 1, selectedTabOrder = 11 }, null)%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Functions.Role%></span>
                                    <div class="ldd_submenu" style="z-index: 1002">
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Functions.Role%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoBackward", "JobDescEntity", new { stepOrder = 1, selectedTabOrder = 1 }, null)%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Functions.Responsibility%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Areas.JobDesc.Views.Shared.RibbonLinks.Add.ChooseRoleToAddResponsibility, "GoBackward", "JobDescEntity", new { stepOrder = 1, selectedTabOrder = 1 }, null)%></a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Functions.KPI%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Areas.JobDesc.Views.Shared.RibbonLinks.Add.ChooseRoleToAddKPI, "GoBackward", "JobDescEntity", new { stepOrder = 1, selectedTabOrder = 1 }, null)%></a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Functions.Specification%></span>
                                    <div class="ldd_submenu" style="z-index: 1002">
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Functions.Competency%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToSpecifications", "JobDescEntity", new { selectedTabOrder = 0 }, null)%></a>
                                            </li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Functions.Skills%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Areas.JobDesc.Views.Shared.RibbonLinks.Add.AddBrowseGeneralSkills, "GoToSpecifications", "JobDescEntity", new { selectedTabOrder = 6 }, null)%></a></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Areas.JobDesc.Views.Shared.RibbonLinks.Add.AddBrowseComputerSkills, "GoToSpecifications", "JobDescEntity", new { selectedTabOrder = 1 }, null)%></a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Functions.Education%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToSpecifications", "JobDescEntity", new { selectedTabOrder = 2 }, null)%></a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Functions.JobExperience%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToSpecifications", "JobDescEntity", new { selectedTabOrder = 3 }, null)%></a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Functions.Knowledge%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToSpecifications", "JobDescEntity", new { selectedTabOrder = 4 }, null)%></a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Functions.Language%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToSpecifications", "JobDescEntity", new { selectedTabOrder = 5 }, null)%></a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Functions.WorkingCondition%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToSpecifications", "JobDescEntity", new { selectedTabOrder = 7 }, null)%></a></li>
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
