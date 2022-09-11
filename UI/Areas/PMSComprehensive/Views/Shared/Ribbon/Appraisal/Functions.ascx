<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<table width="80%">
    <tr>
        <td>
            <table>
                <tr>
                    <td>
                        <div class="box" style="z-index: 1000">
                            <ul id="ldd_menu" class="ldd_menu">
                                <li><span>Appraisal Template</span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading">Appraisal Template</li>
                                            <li><a href="#">
                                                <%:Html.ActionLink("Add/Browse", "Index", "AppraisalTemplate",
                                              new {selectedTabOrder = 0, ribbon = true}, null)%></a></li>
                                        </ul>
                                    </div>
                                </li>
                                <li><span>Template Customized Section</span>
                                    <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading">Customized Section</li>
                                            <li><a href="#">
                                                <%:Html.ActionLink("Add/Browse", "Index", "AppraisalSection", new { selectedTabOrder = 0, ribbonSubEntity = true }, null)%>
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
