#region

using System.Web.Mvc;

#endregion

namespace UI.Areas.Personnel
{
    public class PersonnelAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Personnel"; }
        }

        public static string GetAreaName
        {
            get { return "Personnel"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Personnel_default",
                "{lang}/Personnel/{controller}/{action}/{id}",
                new {controller = "Home", action = "Index", id = UrlParameter.Optional}
                );
        }
    }
}