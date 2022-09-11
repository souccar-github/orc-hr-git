<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<table width="80%">
    <tr>
        <td>
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <div class="box" style="z-index: 1000">
                            <ul id="ldd_menu" class="ldd_menu">
                                <li><span>  <%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.EmployeeInfo%></span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.BloodGroup%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "BloodType", new { area = "Personnel" }, null)%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.Gender%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Gender", new { area = "Personnel" }, null)%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.MaritalStatus%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "MaritalStatus", new { area = "Personnel" }, null)%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.MilitaryStatus%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "MilitaryStatus", new { area = "Personnel" }, null)%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.Nationality%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Nationality", new { area = "Personnel" }, null)%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.PlaceOfBirth%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Country", new { area = "Personnel" }, null)%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.Race%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Race", new { area = "Personnel" }, null)%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.Religion%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Religion", new { area = "Personnel" }, null)%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.Passports%></span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.IssuancePlace%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Country", null, new { area = "Personnel" })%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.Residencies%></span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.Nationality%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Nationality", null, new { area = "Personnel" })%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.ResidencyType%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "ResidencyType", null, new { area = "Personnel" })%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.DrivingLicense%></span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.IssuancePlace%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Country", null, new { area = "Personnel" })%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.DrivingLicenseType%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "DrivingLicenseType", null, new { area = "Personnel" })%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.Spouse%></span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.PlaceOfBirth%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Country", null, new { area = "Personnel" })%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.Nationality%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Nationality", null, new { area = "Personnel" })%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.Children%></span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.PlaceOfBirth%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Country", null, new { area = "Personnel" })%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.Nationality%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Nationality", null, new { area = "Personnel" })%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.Dependents%></span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.PlaceOfBirth%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Country", null, new { area = "Personnel" })%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.Nationality%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Nationality", null, new { area = "Personnel" })%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.Education%></span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.AcademicDegree%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "MajorType", null, new { area = "Personnel" })%>
                                            </a></li>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.Major%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Major", null, new { area = "Personnel" })%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.ScoreType%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "ScoreType", null, new { area = "Personnel" })%>
                                            </a></li>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.Rank%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Level", null, new { area = "Personnel" })%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.IssuancePlace%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Country", null, new { area = "Personnel" })%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.Certifications%></span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.CertificationType%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "CertificationType", null, new { area = "Personnel" })%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.IssuancePlace%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Country", null, new { area = "Personnel" })%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.Languages%></span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.Level%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Level", null, new { area = "Personnel" })%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.Skills%></span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.SkillName%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "SkillType", null, new { area = "Personnel" })%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.Level%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Level", null, new { area = "Personnel" })%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.Convictions%></span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.Personnel.Views.Shared.Ribbon._1stLevel.Indexes.ConvictionRule%></li>
                                            <li><a href="#">
                                                <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "ConvictionRule", null, new { area = "Personnel" })%>
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
