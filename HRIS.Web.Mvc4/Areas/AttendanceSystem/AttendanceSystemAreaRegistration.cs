using System.Web.Mvc;

namespace Project.Web.Mvc4.Areas.AttendanceSystem
{
    public class AttendanceSystemAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AttendanceSystem";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "AttendanceSystem_default",
                "AttendanceSystem/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
