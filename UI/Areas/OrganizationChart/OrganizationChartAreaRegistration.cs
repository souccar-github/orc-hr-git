#region

using System.Web.Mvc;

#endregion

namespace UI.Areas.OrganizationChart
{
    public class OrganizationChartAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "OrganizationChart"; }
        }

        public static string GetAreaName
        {
            get { return "OrganizationChart"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "OrganizationChart_default",
                "{lang}/OrganizationChart/{controller}/{action}/{id}",
                new {action = "Index", id = UrlParameter.Optional}
                );
        }
    }
}