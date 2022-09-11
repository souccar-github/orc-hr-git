using HRIS.Domain.Global.Constant;
using Project.Web.Mvc4.Models;
using System.Web.Mvc;
using HRIS.Domain.Training.Entities;
using Project.Web.Mvc4.Helpers;
using Souccar.Infrastructure.Core;

namespace Project.Web.Mvc4.Areas.Training.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index(RequestInformation.Navigation.Step moduleInfo)
        {
            if (TempData["Module"] == null)
                return RedirectToAction("Welcome", "Module", new { area = "", id = ModulesNames.Training });

            return View();
        }

        public ActionResult CheckCourseStatus(int courseId)
        {
            var course = ServiceFactory.ORMService.GetById<Course>(courseId);
            if (course == null)
            {
                return Json(new {Success = false, Message = GlobalResource.ExceptionMessage},
                    JsonRequestBehavior.AllowGet);
            }

            return Json(new {Success = true, CourseStatus = (int) course.Status},
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetTrainingNeedsForCourse(int courseId)
        {
            var course = ServiceFactory.ORMService.GetById<Course>(courseId);

            return Json(new { TrainingNeedsCount = course.CourseTrainingNeeds.Count }, JsonRequestBehavior.AllowGet);
        }
    }
    
}
