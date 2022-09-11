using HRIS.Domain.AttendanceSystem.Enums;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.PayrollSystem.Entities;
using Project.Web.Mvc4.Areas.MobileApp.Dtos;
using Project.Web.Mvc4.Helpers.Resource;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Mvc4.Areas.MobileApp.Helpers
{
    public class WorkflowHelper
    {
        public static List<WorkflowInfoDto> getPendingItems(int userId, int workflowType,int locale)
        {
            var workflowItems = ServiceFactory.ORMService.All<WorkflowItem>().Where(x => x.Type == (WorkflowType)workflowType && x.TargetUser.Id == userId && x.Status == WorkflowStatus.Pending);
            var result = new List<WorkflowInfoDto>();
            foreach(var item in workflowItems)
            {
                string type = "";
                string leaveSetting = "";
                LogType logType = LogType.Entrance;
                DateTime? requestDate =null;
                DateTime? fromTime =null;
                DateTime? toTime =null;
                bool isHourly =false;
                switch (item.Type)
                {
                    case WorkflowType.LeaveRequest:
                        var leaveRequest = ServiceFactory.ORMService.All<LeaveRequest>().Where(x=>x.WorkflowItem == item).FirstOrDefault();
                        if (leaveRequest == null)
                        {
                            continue;
                        }
                        requestDate = leaveRequest.RequestDate;
                        leaveSetting = leaveRequest.LeaveSetting.AliasName;
                        isHourly = leaveRequest.IsHourlyLeave;
                        if (leaveRequest.IsHourlyLeave)
                        {
                            fromTime = leaveRequest.FromTime;
                            toTime = leaveRequest.ToTime;
                        }
                        break;
                    case WorkflowType.EmployeeEntranceExitRecordRequest:
                        var entranceExitRequest = ServiceFactory.ORMService.All<EntranceExitRecordRequest>().Where(x => x.WorkflowItem == item).FirstOrDefault();
                        if (entranceExitRequest == null)
                        {
                            continue;
                        }
                        requestDate = entranceExitRequest.RecordDate; 
                        logType = entranceExitRequest.LogType;
                        break;
                    case WorkflowType.EmployeeAdvance:
                        var employeeAdvance = ServiceFactory.ORMService.All<EmployeeAdvance>().Where(x => x.WorkflowItem == item).FirstOrDefault();
                        if (employeeAdvance == null)
                        {
                            continue;
                        }
                        requestDate = employeeAdvance.OperationDate;
                        break;
                    case WorkflowType.EmployeeLoanRequest:
                        var employeeLoan = ServiceFactory.ORMService.All<EmployeeLoan>().Where(x => x.WorkflowItem == item).FirstOrDefault();
                        if (employeeLoan == null)
                        {
                            continue;
                        }
                        requestDate = employeeLoan.RequestDate;
                        break;
                    case WorkflowType.EmployeeMissionRequest:
                        var missionRequest = ServiceFactory.ORMService.All<TravelMission>().Where(x => x.WorkflowItem == item).FirstOrDefault();
                        if (missionRequest == null)
                        {
                            continue;
                        }
                        if (missionRequest == null)
                        {
                           var hourlyMissionRequest = ServiceFactory.ORMService.All<HourlyMission>().Where(x => x.WorkflowItem == item).FirstOrDefault();
                            if (hourlyMissionRequest == null)
                            {
                                continue;
                            }
                            requestDate = hourlyMissionRequest.CreationDate;
                            isHourly = true;
                            fromTime = hourlyMissionRequest.StartTime;
                            toTime = hourlyMissionRequest.EndTime;
                        }
                        else
                        {
                            requestDate = missionRequest.CreationDate;
                        }
                        break;
                }
                var pendingUser = item.CurrentUser==null? "":item.CurrentUser.FullName;
                var waitingApprove = false;
                if (item.Status == WorkflowStatus.Pending || item.Status == WorkflowStatus.InProgress)
                {
                    waitingApprove = true;
                }
                var info = new WorkflowInfoDto()
                {
                    LeaveSetting = leaveSetting,
                    Type = workflowType.ToString(),
                    WaitingApprove = waitingApprove,
                    PendingStep = pendingUser,
                    Date = requestDate,
                    FromTime = fromTime,
                    ToTime = toTime,
                    LogType = logType,
                    IsHourly = isHourly
                };
                result.Add(info);
            }
            return result;
        }
    }
}