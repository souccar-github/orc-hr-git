using DevExpress.XtraRichEdit.Model;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.EmployeeRelationServices.Indexes;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Global.Enums;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.RootEntities;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Helper;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Models;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Services;
using Project.Web.Mvc4.Areas.MobileApp.Dtos;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.ProjectModels;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Notification;
using Souccar.Domain.Security;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Web.Mvc4.Areas.MobileApp.Helpers
{
    public class LeaveHelper
    {
        public static LeaveInfoDto GetInformationForLeaveRequest(int employeeCardId, int leaveSettingId, DateTime startDate, int locale)
        {
            try
            {
                var employeeCard = ServiceFactory.ORMService.GetById<EmployeeCard>(employeeCardId);
                var leaveSetting = ServiceFactory.ORMService.GetById<LeaveSetting>(leaveSettingId);
                var result = new LeaveInfoDto();

                if (leaveSetting == null)
                    return result;

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
                result = new LeaveInfoDto()
                {
                    Id = leaveSetting.Id,
                    Title = leaveSetting.AliasName,
                    Balance = Math.Round(balance, 2),
                    Granted = granted,
                    HasMaximumNumber = hasMonthlyBalance,
                    HasMonthlyBalance = hasMonthlyBalance,
                    IsDivisibleToHours = leaveSetting.IsDivisibleToHours,
                    IsIndivisible = leaveSetting.IsIndivisible,
                    MaximumNumber = leaveSetting.MaximumNumber,
                    MonthlyBalance = leaveSetting.MonthlyBalance,
                    MonthlyGranted = monthlyGranted,
                    MonthlyRemain = monthlyRemain,
                    Remain = remain
                };
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static LeaveRequest saveLeaveRequest(Employee employee, LeaveRequestViewModel employeeLeaveItem, int locale)
        {
            var start = DateTime.Now;
            employeeLeaveItem.StartDate = new DateTime(employeeLeaveItem.StartDate.Year, employeeLeaveItem.StartDate.Month, employeeLeaveItem.StartDate.Day, 0, 0, 0);
            employeeLeaveItem.EndDate = new DateTime(employeeLeaveItem.EndDate.Year, employeeLeaveItem.EndDate.Month, employeeLeaveItem.EndDate.Day, 0, 0, 0);
            employeeLeaveItem.RequestDate = new DateTime(employeeLeaveItem.RequestDate.Year, employeeLeaveItem.RequestDate.Month, employeeLeaveItem.RequestDate.Day, 0, 0, 0);
            var setting = ServiceFactory.ORMService.GetById<LeaveSetting>(employeeLeaveItem.LeaveSettingId);
            var leaveReason = ServiceFactory.ORMService.GetById<LeaveReason>(employeeLeaveItem.LeaveReasonId);

            if (employee.EmployeeCard == null)
                throw new Exception(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEmployeeCardNotExist, locale));

            if (setting == null)
                throw new Exception(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgLeaveSettingNotExist, locale));
            if (setting.MaturityPeriod > (DateTime.Now.Subtract((DateTime)employee.EmployeeCard.StartWorkingDate)).Days / (365.25 / 12))
            {
                throw new Exception(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgTheMaturityPeriodHasNotBeenMet, locale));
            }
            if (!LeaveService.IsValidIntervalDays(employeeLeaveItem.RequestDate, employeeLeaveItem.StartDate, setting.IntervalDays))
                throw new Exception(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgDifferenceBetweenRequestDateAndStartDateSmallerThanIntervalDays, locale));

            //---------------------------------------------

            if (employee.EmployeeCard.CardStatus != EmployeeCardStatus.OnHeadOfHisWork)
                throw new Exception(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgYouCanNotTakeLeaveBecauseEmployeeIsNotOnHeadOfHisWork, locale));
            if (employeeLeaveItem.SpentDays == 0)
                throw new Exception(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgYouCanNotTakeLeaveBecauseTheDurationIsZero, locale));

            if (setting.HasMaximumNumber)
            {
                var countInYears = LeaveService.GetCountInYears(employee.EmployeeCard.Employee, setting);
                if (countInYears == setting.MaximumNumber)
                    throw new Exception(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgSorryYouPassedMaximumNumberForThisLeave, locale));
            }

            //Check Balance & Monthly Balance
            if (employeeLeaveItem.StartDate.Year != employeeLeaveItem.EndDate.Year && !setting.IsIndivisible)
            {
                throw new Exception(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEmployeeCardNotExist, locale));
            }
            var balance = LeaveService.GetBalance(setting, employee.EmployeeCard.Employee, false, employeeLeaveItem.StartDate.Date);
            if (setting.HasMaximumNumber)
                balance = balance * setting.MaximumNumber;
            var recycledBalance = LeaveService.GetRecycledBalance(employee.EmployeeCard.Employee, setting, employeeLeaveItem.StartDate.Year - 1);
            balance += recycledBalance;
            var granted = LeaveService.GetGranted(employee.EmployeeCard.Employee, setting, employeeLeaveItem.StartDate.Year);
            if (setting.HasMaximumNumber || setting.IsIndivisible)
                granted = LeaveService.GetGranted(employee, setting);
            var remain = Math.Round(balance - granted, 2);
            var hasMonthlyBalance = LeaveService.HasMonthlyBalance(setting, employee.EmployeeCard.Employee);
            double monthlyBalance = 0;
            double monthlyGranted = 0;
            if (hasMonthlyBalance)
            {
                monthlyBalance = LeaveService.GetMonthlyBalance(setting, employee.EmployeeCard.Employee, employeeLeaveItem.StartDate.Date);
                monthlyGranted = LeaveService.GetMonthlyGranted(employee.EmployeeCard.Employee, setting, employeeLeaveItem.StartDate.Date);
            }
            var monthlyRemain = Math.Round(monthlyBalance - monthlyGranted, 2);

            if (setting.IsIndivisible)
            {

                var endDate = LeaveService.GetEndDate(employeeLeaveItem.StartDate, balance, setting.IsContinuous, employee);
                if (setting.HasMaximumNumber)
                    balance = balance / setting.MaximumNumber;
                if (balance > remain)
                {
                    if (setting.HasMaximumNumber)
                    {
                        throw new Exception(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgSorryYouPassedMaximumNumberForThisLeave, locale));
                    }
                    else
                        throw new Exception(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgYouDoNotHaveEnoughBalanceTheRemainDaysIsGreaterThanTheRequiredDays, locale));
                }
                if (!LeaveService.IsValidLeaveDate(employee.EmployeeCard, setting, DateTime.Parse(employeeLeaveItem.StartDate.ToShortDateString()),
                    DateTime.Parse(endDate.ToShortDateString())))
                    throw new Exception(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgThereIsLeaveWithSameDate, locale));
            }
            else
            {
                if (employeeLeaveItem.IsHourlyLeave)
                {


                    var minutes = (employeeLeaveItem.ToTime.GetValueOrDefault().TimeOfDay - employeeLeaveItem.FromTime.GetValueOrDefault().TimeOfDay).TotalMinutes;
                    var spentDays =
                        Math.Round(1 / ((setting.HoursEquivalentToOneLeaveDay * EmployeeRelationServicesConstants.NumberOfMinutesInHour) / minutes), 2);

                    if (spentDays > remain)
                        throw new Exception(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgYouDoNotHaveEnoughBalanceTheRemainDaysIsGreaterThanTheRequiredDays, locale));

                    if (hasMonthlyBalance)
                    {
                        if (spentDays > monthlyRemain)
                            throw new Exception(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgYouDoNotHaveEnoughMonthlyBalanceTheRemainMonthlyDaysIsAndTheRequiredDaysIs, locale));
                    }
                    bool isHourlyLeaveValidLeave = LeaveService.IsHourlyLeaveValidLeave(
                       employee.EmployeeCard,
                       setting,
                       DateTime.Parse(employeeLeaveItem.StartDate.ToShortDateString()),
                       employeeLeaveItem.FromTime.Value.TimeOfDay,
                       employeeLeaveItem.ToTime.Value.TimeOfDay, new LeaveRequest
                       {
                           FromTime = employeeLeaveItem.FromTime,
                           ToTime = employeeLeaveItem.ToTime,
                           StartDate = employeeLeaveItem.StartDate,
                           EndDate = employeeLeaveItem.EndDate
                       });
                    if (!isHourlyLeaveValidLeave)
                        throw new Exception(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgThereIsLeaveWithSameDate, locale));


                }
                else
                {
                    var spentDays = LeaveService.GetSpentDays(employeeLeaveItem.StartDate, employeeLeaveItem.EndDate, setting.IsContinuous, employee);

                    if (spentDays > remain)
                        throw new Exception(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgYouDoNotHaveEnoughBalanceTheRemainDaysIsGreaterThanTheRequiredDays, locale));

                    if (hasMonthlyBalance)
                    {
                        if (spentDays > monthlyRemain)
                            throw new Exception(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgYouDoNotHaveEnoughMonthlyBalanceTheRemainMonthlyDaysIsAndTheRequiredDaysIs, locale));

                    }
                }

                bool isValidLeaveDate = LeaveService.IsValidLeaveDate(employee.EmployeeCard, setting, DateTime.Parse(employeeLeaveItem.StartDate.ToShortDateString()),
                    DateTime.Parse(employeeLeaveItem.EndDate.ToShortDateString()));

                if (!isValidLeaveDate && !employeeLeaveItem.IsHourlyLeave)
                    throw new Exception(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgThereIsLeaveWithSameDate, locale));



            }

            if (employeeLeaveItem.IsHourlyLeave)
            {
                var diffrence = 0;
                if (employeeLeaveItem.IsSummerDate)
                    diffrence = 1;

                employeeLeaveItem.FromTime = new DateTime(2000, 1, 1, employeeLeaveItem.FromTime.Value.Hour + diffrence, employeeLeaveItem.FromTime.Value.Minute, employeeLeaveItem.FromTime.Value.Second);
                employeeLeaveItem.ToTime = new DateTime(2000, 1, 1, employeeLeaveItem.ToTime.Value.Hour + diffrence, employeeLeaveItem.ToTime.Value.Minute, employeeLeaveItem.ToTime.Value.Second);

                if (string.IsNullOrEmpty(employeeLeaveItem.FromTime.GetValueOrDefault().ToShortTimeString()))
                    throw new Exception(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgFromTimeIsRequired, locale));

                if (string.IsNullOrEmpty(employeeLeaveItem.ToTime.GetValueOrDefault().ToShortTimeString()))
                    throw new Exception(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgToTimeIsRequired, locale));


                employeeLeaveItem.FromDateTime = new DateTime(employeeLeaveItem.StartDate.Year, employeeLeaveItem.StartDate.Month,
                    employeeLeaveItem.StartDate.Day, employeeLeaveItem.FromTime.Value.Hour, employeeLeaveItem.FromTime.Value.Minute,
                    employeeLeaveItem.FromTime.Value.Second);

                employeeLeaveItem.ToDateTime = new DateTime(employeeLeaveItem.EndDate.Year, employeeLeaveItem.EndDate.Month,
                employeeLeaveItem.EndDate.Day, employeeLeaveItem.ToTime.Value.Hour, employeeLeaveItem.ToTime.Value.Minute,
                employeeLeaveItem.ToTime.Value.Second);
                if (employeeLeaveItem.ToDateTime < employeeLeaveItem.FromDateTime)
                {
                    var dateswapFornull = new DateTime(employeeLeaveItem.EndDate.Year, employeeLeaveItem.EndDate.Month,
                    employeeLeaveItem.EndDate.Day, employeeLeaveItem.ToTime.Value.Hour, employeeLeaveItem.ToTime.Value.Minute,
                    employeeLeaveItem.ToTime.Value.Second);
                    employeeLeaveItem.ToDateTime = dateswapFornull.AddDays(1);
                }
                var minutes = 0.00;
                if (employeeLeaveItem.FromTime > employeeLeaveItem.ToTime)
                {
                    var maxDay = new DateTime(2000, 1, 1, 23, 59, 59);
                    var minDay = new DateTime(2000, 1, 1, 0, 0, 0);
                    var minutesbefore = (maxDay.TimeOfDay - employeeLeaveItem.FromTime.GetValueOrDefault().TimeOfDay).TotalMinutes;
                    var minutesafter = (employeeLeaveItem.ToTime.GetValueOrDefault().TimeOfDay - minDay.TimeOfDay).TotalMinutes;
                    minutes = Math.Round(minutesafter + minutesbefore, 0);

                }
                else
                {
                    minutes = (employeeLeaveItem.ToTime.GetValueOrDefault().TimeOfDay - employeeLeaveItem.FromTime.GetValueOrDefault().TimeOfDay).TotalMinutes;

                }
                var maximumMinutesPerDay =
                    setting.MaximumHoursPerDay * EmployeeRelationServicesConstants.NumberOfMinutesInHour;
                if (minutes > maximumMinutesPerDay)
                    throw new Exception(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgRequiredHoursIsGreaterThanAllowedHoursPerDay, locale));

            }
            else
            {
                if (string.IsNullOrEmpty(employeeLeaveItem.StartDate.ToShortDateString()))
                    throw new Exception(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgStartDateIsRequired, locale));

                if (!setting.IsIndivisible)
                {
                    if (string.IsNullOrEmpty(employeeLeaveItem.EndDate.ToShortDateString()))
                        throw new Exception(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEndDateIsRequired, locale));

                    if (employeeLeaveItem.EndDate < employeeLeaveItem.StartDate)
                        throw new Exception(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEndDateShouldBeGreaterThanOrEqualStartDate, locale));

                }
            }
            //---------------------------------------------
            var notify = new Notify();
            var leave = new LeaveRequest
            {
                Description = employeeLeaveItem.Description,
                CreationDate = DateTime.Now,
                RequestDate = employeeLeaveItem.RequestDate,
                Creator = UserExtensions.CurrentUser,
                StartDate = new DateTime(employeeLeaveItem.StartDate.Year, employeeLeaveItem.StartDate.Month, employeeLeaveItem.StartDate.Day, 0, 0, 0),
                EndDate = new DateTime(employeeLeaveItem.EndDate.Year, employeeLeaveItem.EndDate.Month, employeeLeaveItem.EndDate.Day, 0, 0, 0),
                IsHourlyLeave = employeeLeaveItem.IsHourlyLeave,
                FromTime = (employeeLeaveItem.IsHourlyLeave != true) ? null : employeeLeaveItem.FromTime,
                ToTime = (employeeLeaveItem.IsHourlyLeave != true) ? null : employeeLeaveItem.ToTime,
                SpentDays = employeeLeaveItem.SpentDays,
                //LastWorkingDate = employeeLeaveItem.LastWorkingDate,
                //RequestDate = DateTime.Now,

                LeaveSetting = setting,
                LeaveReason = leaveReason,
                EmployeeCard = employee.EmployeeCard,
                LeaveStatus = HRIS.Domain.Global.Enums.Status.Draft,
                FromDateTime = (employeeLeaveItem.IsHourlyLeave != true) ? null : employeeLeaveItem.FromDateTime,
                ToDateTime = (employeeLeaveItem.IsHourlyLeave != true) ? null : employeeLeaveItem.ToDateTime
            };
            var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveALeaveRequestFor) + " "
                            + employee.FullName;
            //  var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.LeaveApprovalSupject) + " " + employee.FullName;
            var body = string.Format("{0} {1} {2}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveALeaveRequestFor), employee.FullName, EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PleaseCheckItOut));

            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
              ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "EmployeeLeaveRequest";
            var destinationEntityId = "EmployeeLeaveRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.ApproveLeave);
            var destinationData = new Dictionary<string, int>();
            var workflowItem = Project.Web.Mvc4.Helpers.WorkflowHelper.InitWithSetting(setting.WorkflowSetting, employee.User(),
                title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                destinationActionName, destinationEntityId, destinationEntityTitle, Souccar.Domain.Notification.OperationType.Nothing, destinationData,
                employee.User().Position(), Souccar.Domain.Workflow.Enums.WorkflowType.LeaveRequest, leaveReason.Name, out notify);
            leave.WorkflowItem = workflowItem;
            ServiceHelper.GetLeaveInfo(leave, setting, employee);
            // ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, leave, employee.EmployeeCard }, UserExtensions.CurrentUser);
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EmployeeLeaveRequest);

            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, leave, employee.EmployeeCard }, UserExtensions.CurrentUser, leave, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { leave.EmployeeCard.Employee });

            if (notify != null)
            {
                notify.DestinationData.Add("WorkflowId", workflowItem.Id);
                notify.DestinationData.Add("ServiceId", leave.Id);
            }

            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, UserExtensions.CurrentUser);
            new MobileApp.Helpers.PushNotification(title, body, notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
            return leave;
        }
        public static void SavePSLeaveWorkflow(int workflowId, int leaveId, WorkflowStepStatus status, string note, User user, int locale)
        {
            var leave = ServiceFactory.ORMService.GetById<LeaveRequest>(leaveId);
            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveALeaveRequestFor, locale) + " " + workflow.TargetUser.FullName;

            var body = string.Format("{0} {1} {2}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveALeaveRequestFor, locale), workflow.TargetUser.FullName, EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PleaseCheckItOut, locale));

            WorkflowStatus workflowStatus;
            entities.Add(workflow);
            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "EmployeeLeaveRequest";
            var destinationEntityId = "EmployeeLeaveRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.ApproveLeave, locale);
            var destinationEntityOperationType = Souccar.Domain.Notification.OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            if (leave != null)
                destinationData.Add("ServiceId", leave.Id);
            var leaveDate =
                leave.IsHourlyLeave ?
                leave.StartDate.Date.ToString("d") + "  " + leave.FromTime.Value.ToString("t") + '-' + leave.ToTime.Value.ToString("t") :
                leave.StartDate.Date.ToString("d") + "-" + leave.EndDate.Date.ToString("d");
            var strWhenCompleted = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YourLeaveHasBeenApprovedWhichDate), leaveDate);
            var strWhenCanceled = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YourLeaveHasBeenRejectedWhichDate), leaveDate);

            var notify = Project.Web.Mvc4.Helpers.WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, body, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
               destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus, strWhenCompleted, strWhenCompleted, strWhenCanceled, strWhenCanceled);
            if (notify != null)
            {
                entities.Add(notify);
            }

            if (workflowStatus == WorkflowStatus.Completed)
            {
                leave.LeaveStatus = Status.Approved;
                //entities.AddRange(ChangeEmployeeResignationInfo(resignation));
            }
            else if (workflowStatus == WorkflowStatus.Canceled)
            {
                leave.LeaveStatus = Status.Rejected;
            }

            ServiceFactory.ORMService.SaveTransaction(entities, user);
            if (status != WorkflowStepStatus.Pending)
            {
                if (workflowStatus == WorkflowStatus.Completed)
                {
                    new PushNotification(strWhenCompleted, "", notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
                }
                else if (workflowStatus == WorkflowStatus.Canceled)
                {
                    new PushNotification(strWhenCanceled, "", notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
                }
                else
                {
                    new PushNotification(title, body, notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
                }
            }
        }
    }

}