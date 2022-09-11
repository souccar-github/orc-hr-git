using HRIS.Domain.Personnel.RootEntities;
using Souccar.Domain.Security;
using Souccar.Infrastructure.Core;
using Souccar.Core.Extensions;
using Souccar.Infrastructure.Extenstions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Areas.Security.Helpers;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.PayrollSystem.Configurations;

namespace Project.Web.Mvc4.Areas.Personnel.Controllers
{
    public class ServiceController : Controller
    {
        //
        // GET: /Personnel/Service/

        public ActionResult ActiveUserForEmployee(int id)
        {
            var emplyee = ServiceFactory.ORMService.GetById<Employee>(id);
            var result = UserHelper.ActiveUserForEmployee(emplyee, emplyee.Username, UserHelper.DefaultPassword);
            switch (result)
            {
                case CreateUserResult.Success:
                    return Json(new { Status = true, MessageTitle = GlobalResource.Success, Message = GlobalResource.SuccessMessage }, JsonRequestBehavior.AllowGet);
                    
                case CreateUserResult.UserExist:
                    return Json(new { Status = false, MessageTitle = GlobalResource.Fail, Message = SecurityLocalizationHelper.GetResource(SecurityLocalizationHelper.AlreadyUserExist) }, JsonRequestBehavior.AllowGet);
                    
                case CreateUserResult.LimitNumber:
                    return Json(new { Status = false, MessageTitle = GlobalResource.Fail, Message = SecurityLocalizationHelper.GetResource(SecurityLocalizationHelper.LimitNumberOfUser) }, JsonRequestBehavior.AllowGet);
                    
                default:
                    break;
            }
            return Json(new { Status = true, MessageTitle = GlobalResource.Fail, Message = GlobalResource.FailMessage }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeactiveUserForEmployee(int id)
        {
            var emplyee = ServiceFactory.ORMService.GetById<Employee>(id);
            UserHelper.DeactiveUserForEmployee(emplyee, emplyee.Username, UserHelper.DefaultPassword);
            return Json(new { Status = true, MessageTitle = GlobalResource.Success, Message = GlobalResource.SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDeservableAdvanceAmount(int employeeCardId ,int employeeId)
        {
            var generalOption = ServiceFactory.ORMService.All<GeneralOption>().FirstOrDefault();
            float employeeFee;
            if (employeeCardId != 0)
            {
                var emplyeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(employeeCardId);

                 employeeFee = emplyeeCard.FinancialCard.PackageSalary / generalOption.TotalMonthDays;
            }
            else
            {
                var emplyeeCard = ServiceFactory.ORMService.All<EmployeeCard>().Where(x=>x.Employee.Id== employeeId).FirstOrDefault();

                 employeeFee = emplyeeCard.FinancialCard.PackageSalary / generalOption.TotalMonthDays;
            }
            
            int EmployeeAttendanceDays=  DateTime.Now.Day;
            var deservableAdvanceAmount = Math.Floor(employeeFee * (EmployeeAttendanceDays - generalOption.AdvanceDeductionDaysFromEmployeeAttendance));
           
            return Json(new {  deservableAdvanceAmount, MessageTitle = GlobalResource.Success, Message = GlobalResource.SuccessMessage }, JsonRequestBehavior.AllowGet);

        }
    }
}
