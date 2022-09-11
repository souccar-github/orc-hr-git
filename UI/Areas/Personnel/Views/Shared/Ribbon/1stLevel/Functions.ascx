<%@ Control Language="C#" ClassName="Function" %>
<table width="80%">
    <tr>
        <td>
            <table>
                <tr>
                    <td>
                        <div>
                            <ul id="ldd_menu" class="ldd_menu">
                                <li><span>
                                    <%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Functions.General%></span>
                                    <div class="ldd_submenu">
                                        <ul>
                                            <li class="ldd_heading">
                                                <%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Functions.BasicDetails%></li>
                                            <li>
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToEmployee", "Employee",
                                                new { selectedTabOrder = 0 }, null)%>
                                            </li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">
                                                <%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Functions.Contacts%></li>
                                            <li>
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToEmployee", "Employee", new { selectedTabOrder = 1 }, null)%>
                                            </li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span>
                                    <%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Functions.Identifications%></span>
                                    <div class="ldd_submenu">
                                        <ul>
                                            <li class="ldd_heading">
                                                <%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Functions.Passports%></li>
                                            <li>
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToEmployee", "Employee", new { selectedTabOrder = 2 }, null)%>
                                            </li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">
                                                <%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Functions.Residencies%></li>
                                            <li>
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToEmployee", "Employee", new { selectedTabOrder = 3 }, null)%>
                                            </li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">
                                                <%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Functions.DrivingLicense%></li>
                                            <li>
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToEmployee", "Employee", new { selectedTabOrder = 4 }, null)%>
                                            </li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">
                                                <%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Functions.MilitaryService%></li>
                                            <li>
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToEmployee", "Employee", new { selectedTabOrder = 5 }, null)%>
                                            </li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span>
                                    <%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Functions.FamilyDetails%></span>
                                    <div class="ldd_submenu">
                                        <ul>
                                            <li class="ldd_heading">
                                                <%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Functions.Spouse%></li>
                                            <li>
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToEmployee", "Employee", new { selectedTabOrder = 6 }, null)%>
                                            </li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">
                                                <%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Functions.Children%></li>
                                            <li>
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToEmployee", "Employee", new { selectedTabOrder = 7 }, null)%>
                                            </li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">
                                                <%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Functions.Dependents%></li>
                                            <li>
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToEmployee", "Employee", new { selectedTabOrder = 8 }, null)%>
                                            </li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span>
                                    <%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Functions.Qualifications%></span>
                                    <div class="ldd_submenu">
                                        <ul>
                                            <li class="ldd_heading">
                                                <%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Functions.Education%></li>
                                            <li>
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToEmployee", "Employee", new { selectedTabOrder = 9 }, null)%>
                                            </li>
                                            <br />
                                            <li class="ldd_heading">
                                                <%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Functions.Certifications%></li>
                                            <li>
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToEmployee", "Employee", new { selectedTabOrder = 10 }, null)%>
                                            </li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">
                                                <%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Functions.Experiences%></li>
                                            <li>
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToEmployee", "Employee", new { selectedTabOrder = 11 }, null)%>
                                            </li>
                                            <br />
                                            <li class="ldd_heading">
                                                <%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Functions.TrainingSkills%></li>
                                            <li>
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToEmployee", "Employee", new { selectedTabOrder = 12 }, null)%>
                                            </li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">
                                                <%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Functions.Languages%></li>
                                            <li>
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToEmployee", "Employee", new { selectedTabOrder = 13 }, null)%>
                                            </li>
                                            <br />
                                            <li class="ldd_heading">
                                                <%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Functions.Skills%></li>
                                            <li>
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToEmployee", "Employee", new { selectedTabOrder = 14 }, null)%>
                                            </li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span>
                                    <%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Functions.Convictions%></span>
                                    <div class="ldd_submenu">
                                        <ul>
                                            <li class="ldd_heading">
                                                <%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Functions.Convictions%></li>
                                            <li>
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToEmployee", "Employee", new { selectedTabOrder = 15 }, null)%>
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
