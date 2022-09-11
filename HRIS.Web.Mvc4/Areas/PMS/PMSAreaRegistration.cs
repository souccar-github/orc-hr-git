using System.Web.Mvc;

namespace Project.Web.Mvc4.Areas.PMS
{
    public class PMSAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "PMS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PMS_default",
                "PMS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
