<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<table width="80%">
    <tr>
        <td>
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <div class="box" style="z-index: 1000">
                            <ul id="ldd_menu" class="ldd_menu">
                                <li><span>Appraisal Template</span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading">Type</li>
                                            <li><a href="#">
                                                <%:Html.ActionLink("Add/Browse", "Index", "TemplateType", new { area = "PMSComprehensive" }, null)%>
                                            </a></li>
                                        </ul> 
                                        <ul>
                                            <li class="ldd_heading">Position Level</li>
                                            <li><a href="#"> 
                                                <%:Html.ActionLink("Add/Browse", "Index", "PositionLevel", new { area = "OrganizationChart" }, null)%>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading">Appraisal Section Evaluator</li>
                                            <li><a href="#">
                                                <%:Html.ActionLink("Add/Browse", "Index", "AppraisalSectionEvaluator", new { area = "PMSComprehensive" }, null)%>
                                            </a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span>Customized Section</span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading">Appraisal Section Evaluator</li>
                                            <li><a href="#">
                                                <%:Html.ActionLink("Add/Browse", "Index", "AppraisalSectionEvaluator", new { area = "PMSComprehensive" }, null)%>
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
