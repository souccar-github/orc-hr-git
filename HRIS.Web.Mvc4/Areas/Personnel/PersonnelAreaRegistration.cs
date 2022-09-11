using System.Web.Mvc;

namespace Project.Web.Mvc4.Areas.Personnel
{
    public class PersonnelAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Personnel";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Personnel_default",
                "Personnel/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
