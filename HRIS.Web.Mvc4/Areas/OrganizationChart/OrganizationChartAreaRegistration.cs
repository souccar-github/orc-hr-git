using System.Web.Mvc;

namespace Project.Web.Mvc4.Areas.OrganizationChart
{
    public class OrganizationChartAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "OrganizationChart";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "OrganizationChart_default",
                "OrganizationChart/{controller}/{action}/{id}",
                new { controller = "OrganizationChart", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
