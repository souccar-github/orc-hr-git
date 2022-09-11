using System.Web.Mvc;

namespace Project.Web.Mvc4.Areas.Grades
{
    public class GradeAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Grades";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Grade_default",
                "Grades/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
