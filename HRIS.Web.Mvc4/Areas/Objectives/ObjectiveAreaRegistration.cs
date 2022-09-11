using System.Web.Mvc;

namespace Project.Web.Mvc4.Areas.Objectives
{
    public class ObjectiveAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Objectives";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Objective_default",
                "Objectives/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
