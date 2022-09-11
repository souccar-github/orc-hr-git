#region

using System.Web.Mvc;

#endregion

namespace UI.Areas.Services
{
    public class ServicesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Services"; }
        }

        public static string GetAreaName
        {
            get { return "Services"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Services_default",
                "{lang}/Services/{controller}/{action}/{id}",
                new {action = "Index", id = UrlParameter.Optional}
                );
        }
    }
}