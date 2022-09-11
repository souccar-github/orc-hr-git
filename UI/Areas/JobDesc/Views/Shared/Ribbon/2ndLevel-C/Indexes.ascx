<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<table width="100%">
    <tr>
        <td>
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <div class="box" style="z-index: 1000">
                            <ul id="ldd_menu" class="ldd_menu">
                                <li><span><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Indexes.Competency%></span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Indexes.Level%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Level", new { area = "Personnel" }, null)%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Indexes.Type%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "CompetencyType", new { area = "JobDesc" }, null)%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Indexes.ComputerSkills%></span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Indexes.Level%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Level", new { area = "Personnel" },null)%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Indexes.Type%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "ComputerSkillType", new { area = "JobDesc" }, null)%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Indexes.Education%></span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Indexes.AcademicDegree%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "MajorType", new { area = "Personnel" }, null)%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Indexes.Major%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Major", new { area = "Personnel" }, null)%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Indexes.ScoreType%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "ScoreType", new { area = "Personnel" }, null)%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Indexes.Rank%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Level", new { area = "Personnel" }, null)%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Indexes.Experiences%></span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Indexes.CareerLevel%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "CareerLevel", new { area = "JobDesc" }, null)%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Indexes.Knowledge%></span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Indexes.Level%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Level", new { area = "Personnel" }, null)%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Indexes.Language%></span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Indexes.Level%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Level", new { area = "Personnel" }, null)%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Indexes.Name%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "LanguageName", new { area = "JobDesc" }, null)%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Indexes.Skill%></span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Indexes.Level%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Level", new { area = "Personnel" }, null)%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Indexes.Type%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "SkillType", new { area = "Personnel" }, null)%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Indexes.WorkingCondition%></span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.JobDesc.Views.Shared.Ribbon._2ndLevel_C.Indexes.Type%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "ConditionType", new { area = "JobDesc" }, null)%>
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



