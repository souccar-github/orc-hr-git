#region

using System.Web.Mvc;

#endregion

namespace UI.Areas.Objective
{
    public class ObjectiveAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Objective"; }
        }

        public static string GetAreaName
        {
            get { return "Objective"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Objective_default",
                "{lang}/Objective/{controller}/{action}/{id}",
                new {action = "Index", id = UrlParameter.Optional}
                );
        }
    }
}