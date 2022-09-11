#region

using System.Web.Mvc;

#endregion

namespace UI.Areas.JobDesc
{
    public class JobDescAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "JobDesc"; }
        }

        public static string GetAreaName
        {
            get { return "JobDesc"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "JobDesc_default",
                "{lang}/JobDesc/{controller}/{action}/{id}",
                new {action = "Index", id = UrlParameter.Optional}
                );
        }
    }
}