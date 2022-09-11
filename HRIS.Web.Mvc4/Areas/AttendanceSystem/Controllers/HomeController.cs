using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.Global.Constant;
using Project.Web.Mvc4.Models;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Infrastructure.Core;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Training.Entities;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Areas.AttendanceSystem.Services;

namespace Project.Web.Mvc4.Areas.AttendanceSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(RequestInformation.Navigation.Step moduleInfo)
        {
            AttendanceService.ResetNonAttendanceFormLastReset();//تصفير تاريخ أخر تصفير لنماذج نقص الدوام والتأخر
            if (TempData["Module"] == null)
                return RedirectToAction("Welcome", "Module", new { area = "", id = ModulesNames.AttendanceSystem });
            return View();
        }
        [HttpPost]
        public ActionResult FilterEmployeeCard(EmployeeCard EmployeeCard, RequestInformation requestInformation)
        {
            Type typeOfClass = typeof(EmployeeCard);
            var employeeCards = ServiceFactory.ORMService.All<EmployeeCard>().ToList();
            var user = UserExtensions.CurrentUser;
            var employeesCanViewThem = UserExtensions.GetChildrenAsEmployess(user);
            var result = employeeCards.Select(x => new { Id = x.Id, Name = x.NameForDropdown }).ToList();
            if (!UserExtensions.IsCurrentUserFullAuthorized(typeOfClass.FullName))
            {
                employeeCards = employeeCards.Where(x => employeesCanViewThem.Any(y => y.Id == x.Employee.Id)).ToList();
                result = employeeCards.Select(x => new { Id = x.Id, Name = x.NameForDropdown }).ToList();
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult FilterEmployee(Employee Employee, RequestInformation requestInformation)
        {
            Type typeOfClass = typeof(EmployeeCard);
            var user = UserExtensions.CurrentUser;
            var employeesCanViewThem = UserExtensions.GetChildrenAsEmployess(user);
            var employeeCards = ServiceFactory.ORMService.All<EmployeeCard>().Where(x => x.CardStatus == EmployeeCardStatus.OnHeadOfHisWork && x.AttendanceDemand).ToList();
            var result = employeeCards.Select(x => new { Id = x.Employee.Id, Name = x.NameForDropdown }).ToList();
            if (!UserExtensions.IsCurrentUserFullAuthorized(typeOfClass.FullName))
            {
                employeeCards = employeeCards.Where(x => employeesCanViewThem.Any(y => y.Id == x.Employee.Id)).ToList();
                result = employeeCards.Select(x => new { Id = x.Employee.Id, Name = x.NameForDropdown }).ToList();
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult FilterManager(Employee Employee, RequestInformation requestInformation)
        {
            Type typeOfClass = typeof(EmployeeCard);
            var user = UserExtensions.CurrentUser;
            var employeesCanViewThem = UserExtensions.GetChildrenAsEmployess(user);
            var employeeCards = ServiceFactory.ORMService.All<EmployeeCard>().Where(x => x.CardStatus == EmployeeCardStatus.OnHeadOfHisWork && x.AttendanceDemand).ToList();
            var result = employeeCards.Select(x => new { Id = x.Employee.Id, Name = x.NameForDropdown }).ToList();
            if (!UserExtensions.IsCurrentUserFullAuthorized(typeOfClass.FullName))
            {
                employeeCards = new List<EmployeeCard>();
                result = employeeCards.Select(x => new { Id = x.Employee.Id, Name = x.NameForDropdown }).ToList();
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EmployeesAreOnHeadOfHisWork(Employee Employee, RequestInformation requestInformation)
        {
            var result = ServiceFactory.ORMService.All<EmployeeCard>().Where(x => x.CardStatus == EmployeeCardStatus.OnHeadOfHisWork).ToList().Select(x => new { Id = x.Employee.Id, Name = x.NameForDropdown }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
    }
}
