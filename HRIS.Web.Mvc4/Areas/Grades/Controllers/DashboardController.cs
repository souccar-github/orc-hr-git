using HRIS.Domain.Grades.RootEntities;
using Souccar.Infrastructure.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Web.Mvc4.Areas.Grades.Controllers
{
    public class DashboardController : Controller
    {
        //
        // GET: /Grades/Dashboard/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GradeDashboard()
        {
            return PartialView();
        }

        public ActionResult GetGradeChartData()
        {
            var data = new ArrayList();
            var grades = ServiceFactory.ORMService.All<Grade>();

            foreach(var grade in grades)
            {
                var avarageSalary = (int) ((grade.MidSalary + grade.MaxSalary) / 2);
                var name = string.Format("{0} {1}", grade.OrganizationalLevel != null ? grade.OrganizationalLevel.Name : string.Empty, grade.Name);
                data.Add(new { Name = name, Value = avarageSalary });
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        
    }
}
