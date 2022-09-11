using System.Web.Mvc;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices
{
    public class EmployeeRelationServicesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "EmployeeRelationServices";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "EmployeeRelationServices_default",
                "EmployeeRelationServices/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
