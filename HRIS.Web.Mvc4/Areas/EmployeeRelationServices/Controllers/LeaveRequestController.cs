using System.Collections;
using System.Linq;
using System.Linq.Dynamic;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.JobDescription.Enum;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.RootEntities;
using  Project.Web.Mvc4.Areas.EmployeeRelationServices.Models;
using  Project.Web.Mvc4.Helpers.Resource;
using  Project.Web.Mvc4.Models;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Souccar.Core.Extensions;
using  Project.Web.Mvc4.Areas.EmployeeRelationServices.Services;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Controllers
{
    public class LeaveRequestController : Controller
    {
        private string _message = string.Empty;
        private bool _isSuccess;
        private List<ValidationResult> _validationResults;
        private Dictionary<string, string> _errorsMessages;

        [HttpPost]
        public ActionResult GetInformationForLeaveRequest(int employeeCardId, int leaveSettingId, DateTime startDate)
        {
            var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(employeeCardId);
            var leaveSetting = ServiceFactory.ORMService.GetById<LeaveSetting>(leaveSettingId);

            var result = new Dictionary<string, object>();

            if (leaveSetting == null)
                return Json(result, JsonRequestBehavior.AllowGet);

            var balance = LeaveService.GetBalance(leaveSetting, employeeCard.Employee, false, startDate);

            if (leaveSetting.HasMaximumNumber)
                balance = balance * leaveSetting.MaximumNumber;

            var recycledBalance = LeaveService.GetRecycledBalance(employeeCard.Employee, leaveSetting, startDate.Year - 1);
            balance += recycledBalance;
            var granted = LeaveService.GetGranted(employeeCard.Employee, leaveSetting, startDate.Year);
            if (leaveSetting.HasMaximumNumber || leaveSetting.IsIndivisible)
                granted = LeaveService.GetGranted(employeeCard.Employee, leaveSetting);
            var remain = Math.Round(balance - granted, 2);

            var hasMonthlyBalance = LeaveService.HasMonthlyBalance(leaveSetting, employeeCard.Employee);
            double monthlyBalance = 0;
            double monthlyGranted = 0;
            if (hasMonthlyBalance)
            {
                monthlyBalance = LeaveService.GetMonthlyBalance(leaveSetting, employeeCard.Employee, startDate);
                monthlyGranted = LeaveService.GetMonthlyGranted(employeeCard.Employee, leaveSetting, startDate);
            }
            var monthlyRemain = Math.Round(monthlyBalance - monthlyGranted, 2);

            result["Balance"] = Math.Round(balance, 2);
            result["Granted"] = granted;
            result["Remain"] = remain;
            result["MonthlyBalance"] = monthlyBalance;
            result["MonthlyGranted"] = monthlyGranted;
            result["MonthlyRemain"] = monthlyRemain;
            result["HasMonthlyBalance"] = hasMonthlyBalance;
            result["IsDivisibleToHours"] = leaveSetting.IsDivisibleToHours;
            result["IsIndivisible"] = leaveSetting.IsIndivisible;
            result["MaximumNumber"] = leaveSetting.MaximumNumber;
            result["HasMaximumNumber"] = leaveSetting.HasMaximumNumber;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetInformationForLeaveRequestWithWorkFlow(int employeeId, int leaveSettingId,DateTime startDate)
        {
            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            var leaveSetting = ServiceFactory.ORMService.GetById<LeaveSetting>(leaveSettingId);
            var result = new Dictionary<string, object>();

            if (leaveSetting==null)
                return Json(result, JsonRequestBehavior.AllowGet);

            var balance = LeaveService.GetBalance(leaveSetting, employee, false, startDate);
            if (leaveSetting.HasMaximumNumber)
                balance = balance * leaveSetting.MaximumNumber;
            var recycledBalance = LeaveService.GetRecycledBalance(employee, leaveSetting, startDate.Year - 1);
            balance = Math.Round(balance + recycledBalance, 2);
            var granted = LeaveService.GetGranted(employee, leaveSetting, startDate.Year);
            if (leaveSetting.HasMaximumNumber || leaveSetting.IsIndivisible)
                granted = LeaveService.GetGranted(employee, leaveSetting);
            var remain = Math.Round(balance - granted, 2);

            var hasMonthlyBalance = LeaveService.HasMonthlyBalance(leaveSetting, employee);
            double monthlyBalance = 0;
            double monthlyGranted = 0;
            if (hasMonthlyBalance)
            {
                monthlyBalance = Math.Round(LeaveService.GetMonthlyBalance(leaveSetting, employee, startDate), 2);
                monthlyGranted = Math.Round(LeaveService.GetMonthlyGranted(employee, leaveSetting, startDate), 2);
            }
            var monthlyRemain = Math.Round(monthlyBalance - monthlyGranted, 2);

            result["Balance"] = balance;
            result["Granted"] = granted;
            result["Remain"] = remain;
            result["MonthlyBalance"] = monthlyBalance;
            result["MonthlyGranted"] = monthlyGranted;
            result["MonthlyRemain"] = monthlyRemain;
            result["HasMonthlyBalance"] = hasMonthlyBalance;
            result["IsDivisibleToHours"] = leaveSetting.IsDivisibleToHours;
            result["IsIndivisible"] = leaveSetting.IsIndivisible;
            result["MaximumNumber"] = leaveSetting.MaximumNumber;
            result["HasMaximumNumber"] = leaveSetting.HasMaximumNumber;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetSpentDays(
            int employeeId, LeaveRequestViewModel leaveItem)
        {
            var startDate = new DateTime(leaveItem.StartDate.Year, leaveItem.StartDate.Month, leaveItem.StartDate.Day);
            var endDate = new DateTime(leaveItem.EndDate.Year, leaveItem.EndDate.Month, leaveItem.EndDate.Day);
            double spentDays = 0.0;
            var result = new Dictionary<string, object>();
            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            var leaveSetting = ServiceFactory.ORMService.GetById<LeaveSetting>(leaveItem.LeaveSettingId);

            if (leaveSetting != null)
            if (leaveSetting.IsIndivisible)
            {
                var balance = LeaveService.GetBalance(leaveSetting, employee,false,DateTime.Today);
                var recycledBalance = LeaveService.GetRecycledBalance(employee, leaveSetting, DateTime.Today.Year - 1);
                balance += recycledBalance;
                spentDays = balance;

            }
            else
            {
                if (leaveItem.IsHourlyLeave)
                {
                        if (leaveItem.FromTime == null || leaveItem.ToTime == null)
                        {
                            spentDays = 0;
                        }
                        else
                        {
                            var minutes = 0.00;
                            //if (leaveItem.FromTime > leaveItem.ToTime)
                            //{
                            //    var maxDay = new DateTime(2000, 1, 1, 23, 59, 59);
                            //    var minDay = new DateTime(2000, 1, 1, 0, 0, 0);
                            //    var minutesbefore = (maxDay.TimeOfDay - leaveItem.FromTime.GetValueOrDefault().TimeOfDay).TotalMinutes;
                            //    var minutesafter = (leaveItem.ToTime.GetValueOrDefault().TimeOfDay - minDay.TimeOfDay).TotalMinutes;
                            //    minutes = minutesafter + minutesbefore;

                            //}
                            leaveItem.FromDateTime = new DateTime(leaveItem.StartDate.Year, leaveItem.StartDate.Month, leaveItem.StartDate.Day, leaveItem.FromTime.Value.Hour, leaveItem.FromTime.Value.Minute, leaveItem.FromTime.Value.Second);
                            leaveItem.ToDateTime = new DateTime(leaveItem.EndDate.Year, leaveItem.EndDate.Month, leaveItem.EndDate.Day, leaveItem.ToTime.Value.Hour, leaveItem.ToTime.Value.Minute, leaveItem.ToTime.Value.Second);
                            if (leaveItem.FromDateTime > leaveItem.ToDateTime)
                            {
                                minutes = (leaveItem.ToDateTime.Value.TimeOfDay - leaveItem.FromDateTime.Value.TimeOfDay).TotalMinutes;
                            }
                            else
                            {
                                minutes = (leaveItem.ToDateTime.Value - leaveItem.FromDateTime.Value).TotalMinutes;
                                //  minutes = (leaveItem.ToTime.GetValueOrDefault().TimeOfDay - leaveItem.FromTime.GetValueOrDefault().TimeOfDay).TotalMinutes;

                            }
                            //var minutes =
                            //    (leaveItem.ToTime.GetValueOrDefault() - leaveItem.FromTime.GetValueOrDefault()).TotalMinutes;
                            spentDays =
                                Math.Round(
                                    1 /
                                    ((leaveSetting.HoursEquivalentToOneLeaveDay *
                                      EmployeeRelationServicesConstants.NumberOfMinutesInHour) / minutes), 2);
                            //spentDays = Math.Round(minutes / (60 * 24), 2);

                        }
                    }
                else
                {
                    if (startDate == DateTime.MinValue || endDate == DateTime.MinValue)
                    {
                        spentDays = 0;
                    }
                    else
                    {
                        spentDays = LeaveService.GetSpentDays(startDate, endDate,
                            leaveSetting.IsContinuous, employee);
                    }
                }
            }
            result["SpentDays"] = spentDays;
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        private void InitialzeDefaultValues()
        {
            _isSuccess = false;
            _message = Helpers.GlobalResource.FailMessage;
        }
        

    }


}
