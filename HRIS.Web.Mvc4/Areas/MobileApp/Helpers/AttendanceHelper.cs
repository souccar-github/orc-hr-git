using HRIS.Domain.AttendanceSystem.Enums;
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

namespace Project.Web.Mvc4.Areas.MobileApp.Helpers
{
    public class AttendanceHelper
    {
        public static string SaveEntranceExitRecordRequestItem(EntranceExitRequestViewModel recordRequestItem, User user,int locale)
        {
            var employee = ServiceFactory.ORMService.GetById<Employee>(recordRequestItem.EmployeeId);
            var generalSetting = ServiceFactory.ORMService.All<GeneralEmployeeRelationSetting>().FirstOrDefault();
            if (generalSetting == null || generalSetting.EntranceExitRequestWorkflowName == null)
                throw new Exception(EmployeeRelationServicesLocalizationHelper.MsgWorkFlowSettingsNotExist);
            if (employee.EmployeeCard == null)
                throw new Exception(EmployeeRelationServicesLocalizationHelper.MsgEmployeeCardNotExist);

            var request = new EntranceExitRecordRequest()
            {
                Creator = user,
                Employee = employee,
                LogType = recordRequestItem.LogType,
                Note = recordRequestItem.Note,
                RecordDate = recordRequestItem.RecordDate,
                RecordStatus = Status.Draft,
                RecordTime = recordRequestItem.RecordTime
            };
            DateTime logDateTime = new DateTime(recordRequestItem.RecordDate.Year, recordRequestItem.RecordDate.Month, recordRequestItem.RecordDate.Day, recordRequestItem.RecordTime.Hour, recordRequestItem.RecordTime.Minute, recordRequestItem.RecordTime.Second);
            request.LogDateTime = logDateTime;
            //اختبار تكرار الطلب
            if (ServiceFactory.ORMService.All<EntranceExitRecordRequest>()
                .Where(x => x.Employee.Id == request.Employee.Id && x.LogType == request.LogType
                && (x.LogDateTime.Year == request.LogDateTime.Year
                && x.LogDateTime.Month == request.LogDateTime.Month
                && x.LogDateTime.Day == request.LogDateTime.Day
                && x.LogDateTime.Hour == request.LogDateTime.Hour
                && x.LogDateTime.Minute == request.LogDateTime.Minute) && x.RecordStatus != Status.Rejected).Any())
            {
                throw new Exception(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EntranceExitRecordAlreadyExist,locale));
            }
            //اختبار تكرار السجل
            var employeeAttendanceCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Employee.Id == employee.Id);
            var allFingerprintTransferredDataOfEmployee = ServiceFactory.ORMService.All<FingerprintTransferredData>().Where(x => x.Employee.Id == employee.Id).ToList();
            var allEntranceExitRecordDataOfEmployee = ServiceFactory.ORMService.All<EntranceExitRecord>().Where(x => x.Employee.Id == employee.Id).ToList();
            if (AttendanceSystem.Services.AttendanceService.CheckEntranceExitRecordDuplicate(allEntranceExitRecordDataOfEmployee,
                allFingerprintTransferredDataOfEmployee, request.LogDateTime, InsertSource.ByEmployee, request.LogType, 0))
            {
                throw new Exception(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EntranceExitRecordAlreadyExist, locale));
            }
            //اختبار السجل بعد التاريخ الحالي
            if (recordRequestItem.RecordDate > DateTime.Now)
            {
                return EmployeeRelationServicesLocalizationHelper.YouCannotAddRecordWithFutureDate;
            }
            //اختبار الموظف على راس عمله ومطالب بالدوام
            if (!(employeeAttendanceCard.CardStatus == EmployeeCardStatus.OnHeadOfHisWork && employeeAttendanceCard.AttendanceDemand))
            {
                throw new Exception(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEmployeeIsNotOnHeadOfHisWork, locale));
            }
            var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveAEntranceOrExitRecordRequestFor, locale) + " " + employee.FullName;

            var body = string.Format("{0} {1} {2}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveAEntranceOrExitRecordRequestFor, locale), employee.FullName, EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PleaseCheckItOut, locale));
            
            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "EntranceExitRequest";
            var destinationEntityId = "EntranceExitRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EntranceExitRecordRequest, locale);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            var notify = new Notify();
            var serviceWorkflow = employee.EmployeeCard.ServiceWorkflows.FirstOrDefault(x => x.ServiceType == HRIS.Domain.EmployeeRelationServices.Enums.ServiceType.EntranceExitRecord);
            var workflowItem = Project.Web.Mvc4.Helpers.WorkflowHelper.InitWithSetting(serviceWorkflow != null ? serviceWorkflow.WorkflowSetting : generalSetting.EntranceExitRequestWorkflowName, employee.User(),
                title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                destinationActionName, destinationEntityId, destinationEntityTitle
                , destinationEntityOperationType, destinationData,
                employee.User().Position(), WorkflowType.EmployeeEntranceExitRecordRequest, recordRequestItem.LogType == LogType.Entrance ?
                    EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.Entrance,locale)
                    : EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.Exit,locale) + " - " + recordRequestItem.Note, out notify);
            request.WorkflowItem = workflowItem;
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, request }, user);
            notify.DestinationData.Add("WorkflowId", workflowItem.Id);
            notify.DestinationData.Add("ServiceId", request.Id);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, user);
            new PushNotification(title,body, notify.Receivers.FirstOrDefault().Receiver.FCMToken??"");
            return string.Empty;
        }

        public static void SaveEntranceExitRecordRequestWorkflow(int workflowId, EntranceExitRecordRequest recordRequest, WorkflowStepStatus status, string note, User user,int locale)
        {
            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveAEntranceOrExitRecordRequestFor, locale) + " " + workflow.TargetUser.FullName;

            var body = string.Format("{0} {1} {2}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveAEntranceOrExitRecordRequestFor, locale), workflow.TargetUser.FullName, EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PleaseCheckItOut, locale));

            WorkflowStatus workflowStatus;
            entities.Add(workflow);
            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "EntranceExitRecordRequest";
            var destinationEntityId = "EntranceExitRecordRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EntranceExitRecordRequest, locale);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", recordRequest.Id);
            var recordDate = recordRequest.LogDateTime.Date.ToString("d") + "-" + recordRequest.LogDateTime.ToString("t");
            var strWhenCompleted = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YourEntraceExitRecordHasBeenApprovedWhichDate), recordDate);
            var strWhenCanceled = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YourEntraceExitRecordHasBeenRejectedWhichDate), recordDate);
            

            var notify = Project.Web.Mvc4.Helpers.WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
               destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus, strWhenCompleted, strWhenCompleted, strWhenCanceled, strWhenCanceled);
            if (notify != null)
            {
                entities.Add(notify);
            }

            if (workflowStatus == WorkflowStatus.Completed)
            {
                recordRequest.RecordStatus = Status.Approved;
                var entraceExitRecord = new EntranceExitRecord()
                {
                    Employee = recordRequest.Employee,
                    InsertSource = InsertSource.ByEmployee,
                    LogDate = recordRequest.RecordDate,
                    LogTime = recordRequest.RecordTime,
                    LogDateTime = recordRequest.LogDateTime,
                    LogType = recordRequest.LogType,
                    Note = recordRequest.Note
                };
                entities.Add(entraceExitRecord);
            }
            else if (workflowStatus == WorkflowStatus.Canceled)
            {
                recordRequest.RecordStatus = Status.Rejected;
            }

            entities.Add(recordRequest);

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