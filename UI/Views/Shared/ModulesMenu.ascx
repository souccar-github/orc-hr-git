<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div id="SlidingPanel">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".trigger").click(function () {
                $(".panel").toggle("fast");
                $(this).toggleClass("active");
                return false;
            });
        });
    </script>
    <div class="panel">
        <ul>
            <li>
                <%:Html.ActionLink(Resources.Views.Shared.Buttons.SouccarHRHome, "Index", "Home", new {area = ""}, null)%></li>
            <li>
                <%:Html.ActionLink(Resources.Views.Shared.Buttons.LiveApprisal, "Index", "LiveAppraisal", new { area = "PMSComprehensiveLive" }, null)%></li>
            <li>
                <%:Html.ActionLink(Resources.Views.Shared.Buttons.SouccarHRPortal, "Index", "Portal", new {area = "Portal"}, null)%></li>
            <li>
                <%:Html.ActionLink(Resources.Views.Shared.Buttons.EmployeeCard, "Index", "Personnel", new {area = "Personnel"}, null)%></li>
            <li>
                <%:Html.ActionLink(Resources.Views.Shared.Buttons.OrganizationChart, "Index", "OrganizationChart",
                                              new {area = "OrganizationChart"}, null)%></li>

            <li>
                <%:Html.ActionLink("Show Organization Chart Tree", "LoadJqueryTree", "Node",
                                              new {area = "OrganizationChart"}, null)%></li>
            <li>
                <%:Html.ActionLink(Resources.Views.Shared.Buttons.JobDescription, "Index", "JobDesc", new {area = "JobDesc"}, null)%></li>
            <li>
                <%:Html.ActionLink(Resources.Views.Shared.Buttons.Objectives, "Index", "ObjectiveModule", new {area = "Objective"}, null)%></li>
            <li>
                <%:Html.ActionLink(Resources.Views.Shared.Buttons.ProjectManagement, "Index", "ProjectManagement", new { area = "ProjectManagement" }, null)%></li>
            <li>
                <%:Html.ActionLink(Resources.Views.Shared.Buttons.AppraisalSection, "Index", "AppraisalSection", new { area = "PMSComprehensive" }, null)%></li>
            <li>
                <%:Html.ActionLink(Resources.Views.Shared.Buttons.AppraisalTemplate, "Index", "AppraisalTemplate", new { area = "PMSComprehensive" }, null)%></li>
            <li>
                <%:Html.ActionLink(Resources.Views.Shared.Buttons.AppraisalPhase, "Index", "AppraisalPhase", new { area = "PMSComprehensive" }, null)%></li>
        </ul>
        <div style="clear: both;">
        </div>
        <div class="columns">
            <div class="colleft">
            </div>
            <div class="colright">
            </div>
        </div>
        <div style="clear: both;">
        </div>
    </div>
    <a class="trigger" href="#">
        <%:Resources.Views.Shared.Buttons.StartHere%></a>
</div>
