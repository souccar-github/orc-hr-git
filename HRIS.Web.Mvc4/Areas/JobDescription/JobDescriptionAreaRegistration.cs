using System.Web.Mvc;

namespace Project.Web.Mvc4.Areas.JobDescription
{
    public class JobDescriptionAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "JobDescription";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "JobDescription_default",
                "JobDescription/{controller}/{action}/{id}",
                new { controller = "JobDescription", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
