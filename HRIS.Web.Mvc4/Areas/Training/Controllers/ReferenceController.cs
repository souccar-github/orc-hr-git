using Project.Web.Mvc4.Models;
using Souccar.Infrastructure.Core;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.Training.Entities;
using HRIS.Domain.Training.Enums;

namespace Project.Web.Mvc4.Areas.Training.Controllers
{
    public class ReferenceController : Controller
    {

        public ActionResult GetAllTrainees(string typeName, RequestInformation requestInformation)
        {
            var course = ServiceFactory.ORMService.GetById<Course>(requestInformation.NavigationInfo.Previous
                .SingleOrDefault(x => x.TypeName == typeof(Course).FullName).RowId);

            if(course == null)
                return Json(null, JsonRequestBehavior.AllowGet);

            var result = course.CourseEmployees.Where(x => x.Type == CourseEmployeeType.Trainee).Select(x => new { Id = x.Employee.Id, Name = x.Employee.FullName }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

    }
}
