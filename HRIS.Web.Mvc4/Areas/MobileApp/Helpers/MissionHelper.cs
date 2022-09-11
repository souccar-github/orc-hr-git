using DevExpress.XtraRichEdit.Model;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Global.Enums;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Workflow;
using HRIS.Web.Mvc4.Areas.EmployeeRelationServices.Models;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Helper;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.ProjectModels;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Notification;
using Souccar.Domain.Security;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Web.Mvc4.Areas.MobileApp.Helpers
{
    public class MissionHelper
    {
        public static string saveMissionRequest(Employee emp, MissionRequestViewModel employeeMissionItem,User user,int locale)
        {
            var posistion = ServiceFactory.ORMService.All<AssigningEmployeeToPosition>().FirstOrDefault(x => x.Employee == emp);
            employeeMissionItem.EmployeeId = emp.Id;
            employeeMissionItem.FullName = emp.FullName;
            employeeMissionItem.PositionId = posistion.Id;
            employeeMissionItem.PositionName = posistion.Position.NameForDropdown;
            employeeMissionItem.Description = employeeMissionItem.Description ?? "";

            return SaveMissionRequestItem(emp.Id, posistion.Id, employeeMissionItem,user);
        }
        public static string SaveMissionRequestItem(int employeeId, int positionId, MissionRequestViewModel missionRequestItem,User user)
        {
            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            var generalSetting = ServiceFactory.ORMService.All<GeneralEmployeeRelationSetting>().FirstOrDefault();
            if (generalSetting == null || generalSetting.MissionRequestWorkflowName == null)
                return EmployeeRelationServicesLocalizationHelper.MsgWorkFlowSettingsNotExist;
            if (employee.EmployeeCard == null)
                return EmployeeRelationServicesLocalizationHelper.MsgEmployeeCardNotExist;
            if (missionRequestItem.IsHourlyMission)
            {
                var request = new HourlyMission()
                {
                    Employee = employee,
                    Note = missionRequestItem.Description,
                    Status = Status.Draft,
                    Date = new DateTime(missionRequestItem.StartDate.Year, missionRequestItem.StartDate.Month, missionRequestItem.StartDate.Day),
                    StartDateTime = missionRequestItem.StartDate,
                    EndDateTime = missionRequestItem.StartDate,
                    StartTime = missionRequestItem.FromTime ?? new DateTime(),
                    EndTime = missionRequestItem.ToTime ?? new DateTime(),
                };
                var defaultDate = new DateTime(2000, 1, 1);
                request.StartTime = defaultDate.Add(request.StartTime.TimeOfDay);
                request.EndTime = defaultDate.Add(request.EndTime.TimeOfDay);
                request.StartDateTime = new DateTime(request.Date.Year, request.Date.Month, request.Date.Day, request.StartTime.Hour, request.StartTime.Minute, request.StartTime.Second);
                request.EndDateTime = new DateTime(request.Date.Year, request.Date.Month, request.Date.Day, request.EndTime.Hour, request.EndTime.Minute, request.EndTime.Second);

                //اختبار تكرار الطلب
                var travelMissions = ServiceFactory.ORMService.All<TravelMission>().Where(x => x.Employee == employee);
                var hourlyMissions = ServiceFactory.ORMService.All<HourlyMission>().Where(x => x.Employee == employee);
                //اختبار تكرار الطلب
                if (travelMissions.Any(x =>
                    ((x.Status == Status.Approved) || (x.Status == Status.Draft)) && request.Date.Year == x.FromDate.Year
                    && request.Date.Month == x.FromDate.Month && request.Date.Day == x.FromDate.Day))
                {
                    return EmployeeRelationServicesLocalizationHelper.MissionAlreadyExistInTheSamePeriod;
                }
                if (hourlyMissions.Any(x =>
                    ((x.Status == Status.Approved) || (x.Status == Status.Draft)) && request.Date.Year == x.Date.Year
                    && request.Date.Month == x.Date.Month && request.Date.Day == x.Date.Day &&
                   (((missionRequestItem.FromTime >= x.StartTime && missionRequestItem.FromTime <= x.EndTime) ||
                    (missionRequestItem.ToTime >= x.StartTime && missionRequestItem.ToTime <= x.EndTime)))))
                {
                    return EmployeeRelationServicesLocalizationHelper.MissionAlreadyExistInTheSamePeriod;
                }
                //اختبار الموظف على راس عمله ومطالب بالدوام
                var employeeAttendanceCard = typeof(EmployeeCard).GetAll<EmployeeCard>().FirstOrDefault(x => x.Employee.Id == employee.Id);
                if (!(employeeAttendanceCard.CardStatus == EmployeeCardStatus.OnHeadOfHisWork && employeeAttendanceCard.AttendanceDemand))
                {
                    return EmployeeRelationServicesLocalizationHelper.MsgEmployeeIsNotOnHeadOfHisWork;
                }
                var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveAMissionRequestFor) + " " + employee.FullName;

                var body = string.Format("{0} {1} {2}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveAMissionRequestFor), employee.FullName, EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PleaseCheckItOut));

                var destinationTabName = NavigationTabName.Operational;
                var destinationModuleName = ModulesNames.EmployeeRelationServices;
                var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
                   ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
                var destinationControllerName = "EmployeeRelationServices/Service";
                var destinationActionName = "MissionRequest";
                var destinationEntityId = "HourlyMission";
                var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MissionRequest);
                var destinationEntityOperationType = Souccar.Domain.Notification.OperationType.Nothing;
                IDictionary<string, int> destinationData = new Dictionary<string, int>();
                var notify = new Notify();
                var serviceWorkflow = employee.EmployeeCard.ServiceWorkflows.FirstOrDefault(x => x.ServiceType == HRIS.Domain.EmployeeRelationServices.Enums.ServiceType.Mission);
                var workflowItem = Project.Web.Mvc4.Helpers.WorkflowHelper.InitWithSetting(serviceWorkflow != null ? serviceWorkflow.WorkflowSetting : generalSetting.MissionRequestWorkflowName, employee.User(),
                    title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                    destinationActionName, destinationEntityId, destinationEntityTitle
                    , destinationEntityOperationType, destinationData,
                    employee.User().Position(), WorkflowType.EmployeeMissionRequest, EmployeeRelationServicesLocalizationHelper.HourlyMission + " - " + missionRequestItem.Description, out notify);
                request.WorkflowItem = workflowItem;
                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, request }, user);
                notify.DestinationData.Add("WorkflowId", workflowItem.Id);
                notify.DestinationData.Add("ServiceId", request.Id);

                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, user);
                new PushNotification(title, body, notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
                return string.Empty;
            }
            else
            {
                var request = new TravelMission()
                {
                    Employee = employee,
                    Note = missionRequestItem.Description,
                    Status = Status.Draft,
                    Type = missionRequestItem.Type,
                    FromDate = new DateTime(missionRequestItem.StartDate.Year, missionRequestItem.StartDate.Month, missionRequestItem.StartDate.Day),
                    ToDate = new DateTime(missionRequestItem.EndDate.Year, missionRequestItem.EndDate.Month, missionRequestItem.EndDate.Day),
                };
                var travelMissions = ServiceFactory.ORMService.All<TravelMission>().Where(x => x.Employee == employee);
                var hourlyMissions = ServiceFactory.ORMService.All<HourlyMission>().Where(x => x.Employee == employee);
                //اختبار تكرار الطلب
                if (travelMissions.Any(x =>
                    ((x.Status == Status.Approved) || (x.Status == Status.Draft)) &&
                   (((request.FromDate >= x.FromDate && request.FromDate <= x.ToDate) ||
                    (request.ToDate >= x.FromDate && request.ToDate <= x.ToDate)))))
                {
                    return EmployeeRelationServicesLocalizationHelper.MissionAlreadyExistInTheSamePeriod;
                }
                if (hourlyMissions.Any(x =>
                    ((x.Status == Status.Approved) || (x.Status == Status.Draft)) &&
                   (((missionRequestItem.EndDate >= x.Date && missionRequestItem.StartDate <= x.Date)))))
                {
                    return EmployeeRelationServicesLocalizationHelper.MissionAlreadyExistInTheSamePeriod;
                }
                //اختبار الموظف على راس عمله ومطالب بالدوام
                var employeeAttendanceCard = typeof(EmployeeCard).GetAll<EmployeeCard>().FirstOrDefault(x => x.Employee.Id == employee.Id);
                if (!(employeeAttendanceCard.CardStatus == EmployeeCardStatus.OnHeadOfHisWork && employeeAttendanceCard.AttendanceDemand))
                {
                    return EmployeeRelationServicesLocalizationHelper.MsgEmployeeIsNotOnHeadOfHisWork;
                }
                var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveAMissionRequestFor) + " " + employee.FullName;

                var body = string.Format("{0} {1} {2}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveAMissionRequestFor), employee.FullName, EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PleaseCheckItOut));

                var destinationTabName = NavigationTabName.Operational;
                var destinationModuleName = ModulesNames.EmployeeRelationServices;
                var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
                   ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
                var destinationControllerName = "EmployeeRelationServices/Service";
                var destinationActionName = "MissionRequest";
                var destinationEntityId = "TravelMission";
                var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MissionRequest);
                var destinationEntityOperationType = Souccar.Domain.Notification.OperationType.Nothing;
                IDictionary<string, int> destinationData = new Dictionary<string, int>();
                var notify = new Notify();
                var serviceWorkflow = employee.EmployeeCard.ServiceWorkflows.FirstOrDefault(x => x.ServiceType == HRIS.Domain.EmployeeRelationServices.Enums.ServiceType.Mission);
                var workflowItem = Project.Web.Mvc4.Helpers.WorkflowHelper.InitWithSetting(serviceWorkflow != null ? serviceWorkflow.WorkflowSetting : generalSetting.MissionRequestWorkflowName, employee.User(),
                    title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                    destinationActionName, destinationEntityId, destinationEntityTitle
                    , destinationEntityOperationType, destinationData,
                    employee.User().Position(), WorkflowType.EmployeeMissionRequest, EmployeeRelationServicesLocalizationHelper.TravelMission + " - " + missionRequestItem.Description, out notify);
                request.WorkflowItem = workflowItem;
                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, request }, user);
                notify.DestinationData.Add("WorkflowId", workflowItem.Id);
                notify.DestinationData.Add("ServiceId", request.Id);
                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, user);
                new PushNotification(title, body, notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");

                return string.Empty;
            }



        }

        public static MissionRequestViewModel getMissionByWorkflow(int id, bool hourly)
        {
            var result = new MissionRequestViewModel();
            if (hourly)
            {
                var mission =
                     ServiceFactory.ORMService.All<HourlyMission>()
                     .FirstOrDefault(x => x.WorkflowItem.Id == id
                      && x.WorkflowItem.Status != WorkflowStatus.Completed && x.WorkflowItem.Status != WorkflowStatus.Canceled);
                if (mission == null)
                {
                    return null;
                }
                result = new MissionRequestViewModel()
                {
                    EmployeeId = mission.Employee.Id,
                    FullName = mission.Employee.FullName,
                    MissionId = mission.Id,
                    StartDate = DateTime.Parse(mission.StartDateTime.ToShortDateString()),
                    EndDate = DateTime.Parse(mission.EndDateTime.ToShortDateString()),
                    IsHourlyMission = true,
                    FromTime = mission.StartTime,
                    ToTime = mission.EndTime,
                    Description = mission.Note ?? string.Empty,
                    WorkflowItemId = mission.WorkflowItem.Id,
                };
            }
            else
            {
                var mission =
                     ServiceFactory.ORMService.All<TravelMission>()
                     .FirstOrDefault(x => x.WorkflowItem.Id == id
                       && x.WorkflowItem.Status != WorkflowStatus.Completed && x.WorkflowItem.Status != WorkflowStatus.Canceled);
                if (mission == null)
                {
                    return null;
                }
                result = new MissionRequestViewModel()
                {
                    EmployeeId = mission.Employee.Id,
                    FullName = mission.Employee.FullName,
                    MissionId = mission.Id,
                    StartDate = DateTime.Parse(mission.FromDate.ToShortDateString()),
                    EndDate = DateTime.Parse(mission.ToDate.ToShortDateString()),
                    IsHourlyMission = false,
                    Description = mission.Note ?? string.Empty,
                    WorkflowItemId = mission.WorkflowItem.Id,
                };
            }

            return result;
        }

        public static void SaveMissionRequestWorkflow(int workflowId, HourlyMission mission, WorkflowStepStatus status, string note,User user)
        {
            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveAMissionRequestFor) + " " + workflow.TargetUser.FullName;

            var body = string.Format("{0} {1} {2}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveAMissionRequestFor), workflow.TargetUser.FullName, EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PleaseCheckItOut));

            WorkflowStatus workflowStatus;
            entities.Add(workflow);
            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "MissionRequest";
            var destinationEntityId = "MissionRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.HourlyMission);
            var destinationEntityOperationType = Souccar.Domain.Notification.OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", mission.Id);
            var missionDate = mission.Date.Date.ToString("d") + "  " + mission.StartDateTime.ToString("t") + '-' + mission.EndDateTime.ToString("t");
            var strWhenCompleted = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YourMissionHasBeenApprovedWhichDate), missionDate);
            var strWhenCanceled = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YourMissionHasBeenRejectedWhichDate), missionDate);

            var notify = Mvc4.Helpers.WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
               destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus,strWhenCompleted,strWhenCompleted,strWhenCanceled,strWhenCanceled);
            if (notify != null)
            {
                entities.Add(notify);
            }

            if (workflowStatus == WorkflowStatus.Completed)
            {
                mission.Status = Status.Approved;
            }
            else if (workflowStatus == WorkflowStatus.Canceled)
            {
                mission.Status = Status.Rejected;
            }

            entities.Add(mission);

            ServiceFactory.ORMService.SaveTransaction(entities,user);
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

        public static void SaveMissionRequestWorkflow(int workflowId, TravelMission mission, WorkflowStepStatus status, string note,User user)
        {
            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveAMissionRequestFor) + " " + workflow.TargetUser.FullName;

            var body = string.Format("{0} {1} {2}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveAMissionRequestFor), workflow.TargetUser.FullName, EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PleaseCheckItOut));

            WorkflowStatus workflowStatus;
            entities.Add(workflow);
            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "MissionRequest";
            var destinationEntityId = "MissionRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TravelMission);
            var destinationEntityOperationType = Souccar.Domain.Notification.OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", mission.Id);
            var missionDate = mission.FromDate.Date.ToString("d") + "-" + mission.ToDate.Date.ToString("d");
            var strWhenCompleted = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YourMissionHasBeenApprovedWhichDate), missionDate);
            var strWhenCanceled = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YourMissionHasBeenRejectedWhichDate), missionDate);

            var notify = Mvc4.Helpers.WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
               destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus,strWhenCompleted,strWhenCompleted,strWhenCanceled,strWhenCanceled);
            if (notify != null)
            {
                entities.Add(notify);
            }

            if (workflowStatus == WorkflowStatus.Completed)
            {
                mission.Status = Status.Approved;
            }
            else if (workflowStatus == WorkflowStatus.Canceled)
            {
                mission.Status = Status.Rejected;
            }

            entities.Add(mission);

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

        public static void SavePSMissionWorkflow(int workflowId, int missionId, WorkflowStepStatus status, string note, User user, bool hourly, int locale)
        {
            if (hourly)
            {
                var mission = ServiceFactory.ORMService.GetById<HourlyMission>(missionId);
                SaveMissionRequestWorkflow(workflowId, mission, status, note,user);
            }
            else
            {
                var mission = ServiceFactory.ORMService.GetById<TravelMission>(missionId);
                SaveMissionRequestWorkflow(workflowId, mission, status, note,user);
            }
        }
    }

}