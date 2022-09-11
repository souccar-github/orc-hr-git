using System.Web.Mvc;

namespace UI.Areas.PMSComprehensiveLive
{
    public class PMSComprehensiveLiveAreaRegistration : AreaRegistration
    {
 

        public override string AreaName
        {
            get { return "PMSComprehensiveLive"; }
        }
        public static string GetAreaName
        {
            get { return "PMSComprehensiveLive"; }
        }
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PMSComprehensiveLive_default",
                "PMSComprehensiveLive/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
