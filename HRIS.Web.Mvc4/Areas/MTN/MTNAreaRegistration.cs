using System.Web.Mvc;

namespace Project.Web.Mvc4.Areas.MTN
{
    public class MTNAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "MTN";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "MTN_default",
                "MTN/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
