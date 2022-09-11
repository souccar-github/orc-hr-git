<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div class="box" style="z-index: 1000">
    <ul id="ldd_menu" class="ldd_menu">
        <li><span><%: Resources.Areas.ProjectManagment.Views.Shared.Ribbon.Functions.Functions.Project%></span>
            <div class="ldd_submenu" style="z-index: 1002">
                <ul>
                    <li class="ldd_heading"><%: Resources.Areas.ProjectManagment.Views.Shared.Ribbon.Functions.Functions.Project%></li>
                    <li><a href="#">
                        <%: Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "Index", "Project", new { selectedTabOrder = 0, ribbon = true }, null)%></a></li>
                </ul>
            </div>
        </li>
        <li><span><%: Resources.Areas.ProjectManagment.Views.Shared.Ribbon.Functions.Functions.ProjectTeam%></span>
            <div class="ldd_submenu" style="z-index: 1000">
                <ul>
                    <li class="ldd_heading"><%: Resources.Areas.ProjectManagment.Views.Shared.Ribbon.Functions.Functions.ProjectTeam%></li>
                    <li><a href="#">
                        <%:Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToProject", "Project", new { selectedTabOrder = 11, ribbonSubEntity = true }, null)%>
                    </a></li>
                </ul>
                <ul>
                    <li class="ldd_heading"><%: Resources.Areas.ProjectManagment.Views.Shared.Ribbon.Functions.Functions.ProjectTeamRole%></li>
                    <li><a href="#">
                        <%:Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToProject", "Project", new { selectedTabOrder = 11, ribbonSubEntity = true }, null)%>
                    </a></li>
                </ul>
                <ul>
                    <li class="ldd_heading"><%: Resources.Areas.ProjectManagment.Views.Shared.Ribbon.Functions.Functions.TeamMember%></li>
                    <li><a href="#">
                        <%:Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, "GoToProject", "Project", new { selectedTabOrder = 11, ribbonSubEntity = true }, null)%>
                    </a></li>
                </ul>
            </div>
        </li>
        <li><span><%: Resources.Areas.ProjectManagment.Views.Shared.Ribbon.Functions.Functions.ProjectPhase%></span>
            <div class="ldd_submenu" style="z-index: 1000">
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.ProjectManagment.Views.Shared.Ribbon.Functions.Functions.ProjectPhase%></li>
                                            <li><a href="#">
                                                <%
                                                    if (Session["CurrentlyInFirstLevel"] != null)
                                                    {
                                                        if ((bool)Session["CurrentlyInFirstLevel"])
                                                        {%>
                                                <%:Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, 
                                                "GoToProject", "Project", new { selectedTabOrder = 1, ribbonSubEntity = true }, null)%>
                                                <%
                                                        }
                                                        else
                                                        {
                                                %>
                                                <%:String.Format(Resources.Shared.Messages.Ribbon.ChooseA,
                                                Resources.Areas.ProjectManagment.Views.Shared.Ribbon.Functions.Functions.Project) %>
                                                <%
                                                        }
                                                    }
                                                    else
                                                    {
                                                %>
                                                <%:String.Format(Resources.Shared.Messages.Ribbon.ChooseA,
                                                Resources.Areas.ProjectManagment.Views.Shared.Ribbon.Functions.Functions.Project) %>
                                                <%
                                                    }
                                                %>
                                            </a></li>
                                        </ul>
                                        <ul>
                                            <li class="ldd_heading"><%: Resources.Areas.ProjectManagment.Views.Shared.Ribbon.Functions.Functions.PhaseTask%></li>
                                            <li><a href="#">
                                                <%
                                                    if (Session["CurrentSecondLevelRowId"] != null)
                                                    {
                                                        if ((int)Session["CurrentSecondLevelRowId"] != 0 && (int)Session["SelectedTabIndex"] == 1)
                                                        {%>
                                                <%:Html.ActionLink(Resources.Shared.RibbonLinks.Add.AddBrowse, 
                                                    "GoToProjectPhases", "Project", new { selectedTabOrder = 0, ribbon = false }, null)%>
                                                <%
                                                        }
                                                        else
                                                        {
                                                %>
                                                <%:Html.ActionLink(String.Format(Resources.Shared.Messages.Ribbon.ChooseA,
                                                Resources.Areas.ProjectManagment.Views.Shared.Ribbon.Functions.Functions.ProjectPhase), 
                                                    "GoToProject", "Project", new { selectedTabOrder = 1, ribbonSubEntity = true }, null)%>
                                                <%
                                                        }
                                                    }
                                                    else
                                                    {
                                                %>
                                                <%:Html.ActionLink(String.Format(Resources.Shared.Messages.Ribbon.ChooseA,
                                                Resources.Areas.ProjectManagment.Views.Shared.Ribbon.Functions.Functions.ProjectPhase), 
                                                    "GoToProject", "Project", new { selectedTabOrder = 1, ribbonSubEntity = true }, null)%>
                                                <%
                                                    }
                                                %>
                                            </a></li>
                                        </ul>
                                    </div>
        </li>
    </ul>
</div>
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
