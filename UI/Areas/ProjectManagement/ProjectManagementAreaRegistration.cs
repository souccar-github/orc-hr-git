#region

using System.Web.Mvc;

#endregion

namespace UI.Areas.ProjectManagement
{
    public class ProjectManagementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "ProjectManagement"; }
        }

        public static string GetAreaName
        {
            get { return "ProjectManagement"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ProjectManagement_default",
                "{lang}/ProjectManagement/{controller}/{action}/{id}",
                new {action = "Index", id = UrlParameter.Optional}
                );
        }
    }
}
