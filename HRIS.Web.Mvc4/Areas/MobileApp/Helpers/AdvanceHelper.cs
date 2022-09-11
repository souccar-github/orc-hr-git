using DevExpress.XtraRichEdit.Model;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Global.Enums;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Workflow;
using HRIS.Web.Mvc4.Areas.EmployeeRelationServices.Models;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Helper;
using Project.Web.Mvc4.Areas.PayrollSystem.Models;
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
using Souccar.Infrastructure.Extenstions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Web.Mvc4.Areas.MobileApp.Helpers
{
    public class AdvanceHelper
    {
        public static string saveAdvanceRequest(Employee emp, EmployeeAdvanceViewModel employeeAdvanceItem,User user,int locale)
        {
            var posistion = ServiceFactory.ORMService.All<AssigningEmployeeToPosition>().FirstOrDefault(x => x.Employee == emp);
            employeeAdvanceItem.EmployeeId = emp.Id;
            employeeAdvanceItem.FullName = emp.FullName;
            employeeAdvanceItem.PositionId = posistion.Id;
            employeeAdvanceItem.PositionName = posistion.Position.NameForDropdown;
            employeeAdvanceItem.Description = employeeAdvanceItem.Description ?? "";

            return SaveAdvanceRequestItem(emp.Id, posistion.Id, employeeAdvanceItem,user);
        }
        public static string SaveAdvanceRequestItem(int employeeId, int positionId, EmployeeAdvanceViewModel advanceRequestItem,User user)
        {
            var start = DateTime.Now;
            var notify = new Notify();
            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            var generalSetting = ServiceFactory.ORMService.All<GeneralEmployeeRelationSetting>().SingleOrDefault();

            if (employee.EmployeeCard == null)
                return EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEmployeeCardNotExist);

            if (generalSetting == null)
                return EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgGeneralSettingNotExist);


            var advance = new EmployeeAdvance
            {

                OperationDate = DateTime.Now,
                OperationCreator = UserExtensions.CurrentUser,
                DeservableAdvanceAmount = advanceRequestItem.DeservableAdvanceAmount,
                AdvanceAmount = advanceRequestItem.AdvanceAmount,
                Note = advanceRequestItem.Note,

                EmployeeCard = employee.EmployeeCard,
                AdvanceStatus = HRIS.Domain.Global.Enums.Status.Draft
            };
            var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AdvanceApprovalBody) + " "
                            + employee.FullName;
            //   var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PromotionApprovalSupject) + " " + employee.FullName ;
            var title = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AdvanceApprovalSubjectFor), employee.FullName);

            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
              ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "EmployeeAdvanceRequest";
            var destinationEntityId = "EmployeeAdvanceRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EmployeeAdvanceRequest);
            var destinationData = new Dictionary<string, int>();

            var serviceWorkflow = employee.EmployeeCard.ServiceWorkflows.FirstOrDefault(x => x.ServiceType == HRIS.Domain.EmployeeRelationServices.Enums.ServiceType.Advance);
            var workflowItem = Project.Web.Mvc4.Helpers.WorkflowHelper.InitWithSetting(serviceWorkflow != null ? serviceWorkflow.WorkflowSetting : generalSetting.AdvanceWorkflowName, employee.User(),
                title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                destinationActionName, destinationEntityId, destinationEntityTitle, Souccar.Domain.Notification.OperationType.Nothing, destinationData,
                employee.User().Position(), Souccar.Domain.Workflow.Enums.WorkflowType.EmployeeAdvance, advanceRequestItem.Note, out notify);
            advance.WorkflowItem = workflowItem;
            advance.DeservableAdvanceAmount = advanceRequestItem.DeservableAdvanceAmount;

            // ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, promotion }, UserExtensions.CurrentUser);
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.Advance);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, advance }, UserExtensions.CurrentUser, advance, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { advance.EmployeeCard.Employee });

            var workflowitem = ServiceFactory.ORMService.All<WorkflowItem>().OrderByDescending(x => x.Id).FirstOrDefault();
            notify.DestinationData.Add("WorkflowId", workflowitem.Id);
            notify.DestinationData.Add("ServiceId", advance.Id);

            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, UserExtensions.CurrentUser);
            new PushNotification(title, body, notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
            return string.Empty;
        }

        public static EmployeeAdvanceViewModel getAdvanceByWorkflow(int id)
        {
            var result = new EmployeeAdvanceViewModel();
            var advance =
                 ServiceFactory.ORMService.All<EmployeeAdvance>()
                 .FirstOrDefault(x => x.WorkflowItem.Id == id);
            WorkflowPendingType pendingType;
            Project.Web.Mvc4.Helpers.WorkflowHelper.GetNextAppraiser(advance.WorkflowItem, out pendingType);
            result = new EmployeeAdvanceViewModel()
            {
                EmployeeId = advance.EmployeeCard.Employee.Id,
                FullName = advance.EmployeeCard.Employee.FullName,
                AdvanceId = advance.Id,
                Description = advance.Note ?? string.Empty,
                WorkflowItemId = advance.WorkflowItem.Id,
                AdvanceAmount = advance.AdvanceAmount,
                DeservableAdvanceAmount = advance.DeservableAdvanceAmount,
                Note = advance.Note,
                OperationDate = advance.OperationDate,
                PendingType = pendingType,
                PositionId = advance.EmployeeCard.Employee.GetSecondaryPositionElsePrimary().Id,
                PositionName = advance.EmployeeCard.Employee.GetSecondaryPositionElsePrimary().NameForDropdown
            };
            return result;
        }

        public static void SaveAdvanceRequestWorkflow(int workflowId, EmployeeAdvance advance, WorkflowStepStatus status, string note,User user)
        {
            var start = DateTime.Now;
            if (advance.AdvanceAmount > 0)
                advance.AdvanceAmount = advance.AdvanceAmount;
            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AdvanceApprovalBody);

            var title = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AdvanceApprovalSubjectFor), workflow.TargetUser.FullName);

            WorkflowStatus workflowStatus;
            entities.Add(workflow);
            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "EmployeeAdvanceRequest";
            var destinationEntityId = "EmployeeAdvanceRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EmployeeAdvanceRequest);
            var destinationEntityOperationType = Souccar.Domain.Notification.OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", advance.Id);
            var strWhenCompleted = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AdvanceHasBeenApprovedForEmployee), advance.EmployeeCard.Employee.FullName);
            var strWhenCanceled = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AdvanceHasBeenRejectedForEmployee), advance.EmployeeCard.Employee.FullName);

            var notify = Project.Web.Mvc4.Helpers.WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
               destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus, strWhenCompleted, strWhenCompleted, strWhenCanceled, strWhenCanceled);
            if (notify != null)
            {
                entities.Add(notify);
            }

            if (workflowStatus == WorkflowStatus.Completed)
            {
                entities.AddRange(ChangeEmployeeAdvanceInfo(advance));
            }
            else if (workflowStatus == WorkflowStatus.Canceled)
            {
                advance.AdvanceStatus = Status.Rejected;
            }

            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AcceptAdvance);
            var info_type = status == WorkflowStepStatus.Accept ? LocalizationHelper.GetResource(LocalizationHelper.Accept)
                : (status == WorkflowStepStatus.Reject ? LocalizationHelper.GetResource(LocalizationHelper.Reject)
                : LocalizationHelper.GetResource(LocalizationHelper.Pending));

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, advance, Souccar.Domain.Audit.OperationType.Update, info + " '" + info_type + "' ", start, new List<Entity>() { advance.EmployeeCard.Employee });
            new PushNotification(title, body, notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
        }

        public static void SavePSAdvanceWorkflow(int workflowId, int advanceId, WorkflowStepStatus status, string note, User user, int locale)
        {
            var advance = ServiceFactory.ORMService.GetById<EmployeeAdvance>(advanceId);
            SaveAdvanceRequestWorkflow(workflowId, advance, status, note, user);

        }
        public static List<IAggregateRoot> ChangeEmployeeAdvanceInfo(EmployeeAdvance advance)
        {
            var entities = new List<IAggregateRoot>();
            //  var setting = Advance.RewardSetting;
            advance.AdvanceStatus = Status.Approved;

            //  var newSalary = GetNewSalary(setting.IsPercentage, employeeCard.Salary, setting.Percentage, setting.FixedValue);//الراتب الذي يجب اضافته إلى البطاقة الشهرية todo
            entities.Add(advance.EmployeeCard);

            return entities;
        }
    }

}