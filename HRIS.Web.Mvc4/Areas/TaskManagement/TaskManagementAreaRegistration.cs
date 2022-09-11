using System.Web.Mvc;

namespace Project.Web.Mvc4.Areas.TaskManagement
{
    public class TaskManagementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "TaskManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "TaskManagement_default",
                "TaskManagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
