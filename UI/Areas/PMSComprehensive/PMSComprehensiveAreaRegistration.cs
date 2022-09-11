using System.Web.Mvc;

namespace UI.Areas.PMSComprehensive
{
    public class PMSComprehensiveAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "PMSComprehensive"; }
        }

        public static string GetAreaName
        {
            get { return "PMSComprehensive"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PMSComprehensive_default",
                "{lang}/PMSComprehensive/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
                );
        }

    }
}
