using System.Web.Mvc;

namespace Project.Web.Mvc4.Areas.PayrollSystem
{
    public class PayrollSystemAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "PayrollSystem";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PayrollSystem_default",
                "PayrollSystem/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
