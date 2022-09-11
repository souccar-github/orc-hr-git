using System.Web.Mvc;

namespace Project.Web.Mvc4.Areas.Workflow
{
    public class WorkflowAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Workflow";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Workflow_default",
                "Workflow/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
