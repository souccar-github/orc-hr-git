using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using  Project.Web.Mvc4.Controllers;
using  Project.Web.Mvc4.Extensions;
using  Project.Web.Mvc4.Factories;
using  Project.Web.Mvc4.Models;
using  Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Global.Enums;
using  Project.Web.Mvc4.Helpers.Resource;
using  Project.Web.Mvc4.Helpers;




namespace Project.Web.Mvc4.Areas.AttendanceSystem.Controllers
{
    public class AttendanceFormController : Controller
    {
        //[HttpPost]
        //public ActionResult AcceptPenalty(string entityType, int entityId = 0)
        //{
        //    string message = "";
        //    var isSuccess = false;

        //    if (entityType == typeof(AttendanceMonthlyAdjustment).FullName) 
        //    {
        //        try
        //        {
        //            var attendanceMonthlyAdjustment = ServiceFactory.ORMService.GetById<AttendanceMonthlyAdjustment>(entityId);
        //            if(attendanceMonthlyAdjustment.AttendanceRecord.AttendanceMonthStatus == HRIS.Domain.AttendanceSystem.Enums.AttendanceMonthStatus.Calculated)
        //            {
        //                if(attendanceMonthlyAdjustment.Penalty == null)
        //                {
        //                    isSuccess = false;
        //                    message = AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.ThereIsNoPenalty);
        //                }
        //                else
        //                {
        //                    var penalty = ServiceFactory.ORMService.GetById<DisciplinarySetting>(attendanceMonthlyAdjustment.Penalty.Id);
        //                    var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(attendanceMonthlyAdjustment.EmployeeAttendanceCard.Employee.EmployeeCard.Id);
        //                    var datetime = DateTime.Now;

        //                    EmployeeDisciplinary employeeDisciplinary = new EmployeeDisciplinary
        //                    {
        //                        DisciplinarySetting = penalty,
        //                        DisciplinaryDate = datetime,
        //                        DisciplinaryReason = AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.GeneratedFromAttendanceSystem),
        //                        EmployeeCard = employeeCard,
        //                        CreationDate = datetime,
        //                        DisciplinaryStatus = Status.Approved
        //                    };

        //                    employeeCard.AddEmployeeDisciplinary(employeeDisciplinary);

        //                    employeeDisciplinary.Save();
        //                    isSuccess = true;
        //                    message = LocalizationHelper.SuccessMessage;
        //                }
        //            }
        //            else
        //            {
        //                isSuccess = false;
        //                message = AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.YouCantAcceptPenaltiesBecauseTheMonthStatusIsNotCalculated);
        //            }
                    
        //        }
        //        catch (Exception)
        //        {
        //            isSuccess = false;
        //            message = LocalizationHelper.FailMessage;
        //        }
        //    }


        //    if (entityType == typeof(AttendanceDailyAdjustment).FullName)
        //    {
        //        try
        //        {
        //            var attendanceDailyAdjustment = ServiceFactory.ORMService.GetById<AttendanceDailyAdjustment>(entityId);
        //            if (attendanceDailyAdjustment.AttendanceRecord.AttendanceMonthStatus == HRIS.Domain.AttendanceSystem.Enums.AttendanceMonthStatus.Calculated)
        //            {
        //                if (attendanceDailyAdjustment.Penalty == null)
        //                {
        //                    isSuccess = false;
        //                    message = AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.ThereIsNoPenalty);
        //                }
        //                else
        //                {
        //                    var penalty = ServiceFactory.ORMService.GetById<DisciplinarySetting>(attendanceDailyAdjustment.Penalty.Id);
        //                    var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(attendanceDailyAdjustment.EmployeeAttendanceCard.Employee.EmployeeCard.Id);
        //                    var datetime = DateTime.Now;

        //                    EmployeeDisciplinary employeeDisciplinary = new EmployeeDisciplinary
        //                    {
        //                        DisciplinarySetting = penalty,
        //                        DisciplinaryDate = datetime,
        //                        DisciplinaryReason = AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.GeneratedFromAttendanceSystem),
        //                        EmployeeCard = employeeCard,
        //                        CreationDate = datetime,
        //                        DisciplinaryStatus = Status.Approved
        //                    };

        //                    employeeCard.AddEmployeeDisciplinary(employeeDisciplinary);

        //                    employeeDisciplinary.Save();
        //                    isSuccess = true;
        //                    message = LocalizationHelper.SuccessMessage;
        //                }
        //            }
        //            else
        //            {
        //                isSuccess = false;
        //                message = AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.YouCantAcceptPenaltiesBecauseTheMonthStatusIsNotCalculated);
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            isSuccess = false;
        //            message = LocalizationHelper.FailMessage;
        //        }
        //    }


        //    return Json(new
        //    {
        //        Success = isSuccess,
        //        Msg = message,
        //    });
        //}



        //[HttpPost]
        //public ActionResult AcceptLatenessPenalty(string entityType, int entityId = 0)
        //{
        //    string message = "";
        //    var isSuccess = false;

        //    if (entityType == typeof(AttendanceWithoutAdjustment).FullName)
        //    {
        //        try
        //        {
        //            var attendanceWithoutAdjustment = ServiceFactory.ORMService.GetById<AttendanceWithoutAdjustment>(entityId);
        //            if (attendanceWithoutAdjustment.AttendanceRecord.AttendanceMonthStatus == HRIS.Domain.AttendanceSystem.Enums.AttendanceMonthStatus.Calculated)
        //            {
        //                if (attendanceWithoutAdjustment.LatenessPenalty == null)
        //                {
        //                    isSuccess = false;
        //                    message = AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.ThereIsNoPenalty);
        //                }
        //                else
        //                {
        //                    var penalty = ServiceFactory.ORMService.GetById<DisciplinarySetting>(attendanceWithoutAdjustment.LatenessPenalty.Id);
        //                    var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(attendanceWithoutAdjustment.EmployeeAttendanceCard.Employee.EmployeeCard.Id);
        //                    var datetime = DateTime.Now;

        //                    EmployeeDisciplinary employeeDisciplinary = new EmployeeDisciplinary
        //                    {
        //                        DisciplinarySetting = penalty,
        //                        DisciplinaryDate = datetime,
        //                        DisciplinaryReason = AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.GeneratedFromAttendanceSystem),
        //                        EmployeeCard = employeeCard,
        //                        CreationDate = datetime,
        //                        DisciplinaryStatus = Status.Approved
        //                    };

        //                    employeeCard.AddEmployeeDisciplinary(employeeDisciplinary);

        //                    employeeDisciplinary.Save();
        //                    isSuccess = true;
        //                    message = LocalizationHelper.SuccessMessage;
        //                }
        //            }
        //            else
        //            {
        //                isSuccess = false;
        //                message = AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.YouCantAcceptPenaltiesBecauseTheMonthStatusIsNotCalculated);
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            isSuccess = false;
        //            message = LocalizationHelper.FailMessage;
        //        }
        //    }


        //    return Json(new
        //    {
        //        Success = isSuccess,
        //        Msg = message,
        //    });
        //}




        //[HttpPost]
        //public ActionResult AcceptNonAttendancePenalty(string entityType, int entityId = 0)
        //{
        //    string message = "";
        //    var isSuccess = false;

        //    if (entityType == typeof(AttendanceWithoutAdjustment).FullName)
        //    {
        //        try
        //        {
        //            var attendanceWithoutAdjustment = ServiceFactory.ORMService.GetById<AttendanceWithoutAdjustment>(entityId);
        //            var penalty = ServiceFactory.ORMService.GetById<DisciplinarySetting>(attendanceWithoutAdjustment.NonAttendancePenalty.Id);
        //            var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(attendanceWithoutAdjustment.EmployeeAttendanceCard.Employee.EmployeeCard.Id);
        //            var datetime = DateTime.Now;

        //            EmployeeDisciplinary employeeDisciplinary = new EmployeeDisciplinary
        //            {
        //                DisciplinarySetting = penalty,
        //                DisciplinaryDate = datetime,
        //                DisciplinaryReason = AttendanceLocalizationHelper.GetResource(AttendanceLocalizationHelper.GeneratedFromAttendanceSystem),
        //                EmployeeCard = employeeCard,
        //                CreationDate = datetime,
        //                DisciplinaryStatus = Status.Approved
        //            };

        //            employeeCard.AddEmployeeDisciplinary(employeeDisciplinary);

        //            employeeDisciplinary.Save();
        //            isSuccess = true;
        //            message = LocalizationHelper.SuccessMessage;
        //        }
        //        catch (Exception)
        //        {
        //            isSuccess = false;
        //            message = LocalizationHelper.FailMessage;
        //        }
        //    }


        //    return Json(new
        //    {
        //        Success = isSuccess,
        //        Msg = message,
        //    });
        //}
    }
}
