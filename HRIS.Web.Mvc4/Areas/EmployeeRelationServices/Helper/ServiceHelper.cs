#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 31/03/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.Helpers;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.PayrollSystem.Enums;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.RootEntities;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Services;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Notification;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Models.Navigation;
using HRIS.Domain.Global.Constant;
using Status = HRIS.Domain.Global.Enums.Status;
using Project.Web.Mvc4.ProjectModels;
using HRIS.Domain.JobDescription.Entities;
using Souccar.Infrastructure.Extenstions;
using HRIS.Web.Mvc4.Areas.EmployeeRelationServices.Models;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.AttendanceSystem.Enums;
using HRIS.Domain.Workflow;
using HRIS.Domain.PayrollSystem.Entities;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Models;
using HRIS.Domain.Personnel.Entities;
using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.PayrollSystem.Configurations;

#endregion

namespace Project.Web.Mvc4.Areas.EmployeeRelationServices.Helper
{
    public static class ServiceHelper
    {
        #region تدفق عمل منح المكافئة
        public static void SaveRewardWorkflow(int workflowId, EmployeeReward reward, WorkflowStepStatus status, string note)
        {
            var start = DateTime.Now;
            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var user = UserExtensions.CurrentUser;
            var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.RewardApprovalBody);

            var title = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.RewardApprovalSubjectFor), workflow.TargetUser.FullName);

            WorkflowStatus workflowStatus;
            entities.Add(workflow);
            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "RewardRequest";
            var destinationEntityId = "RewardRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.RewardRequest);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", reward.Id);
            var strWhenCompleted = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.RewardHasBeenApprovedForEmployee), reward.EmployeeCard.Employee.FullName);
            var strWhenCanceled = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.RewardHasBeenRejectedForEmployee), reward.EmployeeCard.Employee.FullName);

            var notify = WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
            destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData,
             out workflowStatus, strWhenCompleted, strWhenCompleted, strWhenCanceled, strWhenCanceled);
            if (notify != null)
            {
                entities.Add(notify);
            }

            if (workflowStatus == WorkflowStatus.Completed)
            {
                entities.AddRange(ChangeEmployeeRewardInfo(reward, reward.EmployeeCard));
            }
            else if (workflowStatus == WorkflowStatus.Canceled)
            {
                reward.RewardStatus = Status.Rejected;
            }

            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AcceptReward);
            var info_type = status == WorkflowStepStatus.Accept ? LocalizationHelper.GetResource(LocalizationHelper.Accept)
                : (status == WorkflowStepStatus.Reject ? LocalizationHelper.GetResource(LocalizationHelper.Reject)
                : LocalizationHelper.GetResource(LocalizationHelper.Pending));

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, reward, Souccar.Domain.Audit.OperationType.Update, info + " '" + info_type + "' ", start, new List<Entity>() { reward.EmployeeCard.Employee });

        }
        #endregion

        #region تدفق عمل فرض عقوبة
        public static void SaveDisciplinaryWorkflow(int workflowId, EmployeeDisciplinary disciplinary, WorkflowStepStatus status, string note)
        {
            //var currentUser = UserExtensions.CurrentUser;
            //disciplinary.Creator = currentUser;
            //disciplinary.CreationDate = DateTime.Now;
            var start = DateTime.Now;
            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var user = UserExtensions.CurrentUser;
            var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.DisciplinaryApprovalBody);

            var title = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.DisciplinaryApprovalSubjectFor), workflow.TargetUser.FullName);

            WorkflowStatus workflowStatus;
            entities.Add(workflow);
            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "DisciplinaryRequest";
            var destinationEntityId = "DisciplinaryRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.DisciplinaryRequest);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", disciplinary.Id);
            var strWhenCompleted = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.DisciplinaryHasBeenApprovedForEmployee), disciplinary.EmployeeCard.Employee.FullName);
            var strWhenCanceled = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.DisciplinaryHasBeenRejectedForEmployee), disciplinary.EmployeeCard.Employee.FullName);

            var notify = WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
               destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus, strWhenCompleted, strWhenCompleted, strWhenCanceled, strWhenCanceled);
            if (notify != null)
            {
                entities.Add(notify);
            }

            if (workflowStatus == WorkflowStatus.Completed)
            {
                var terminationDecisionNotify = AddTerminationDecisionNotify(disciplinary, disciplinary.EmployeeCard);
                if (terminationDecisionNotify != null)
                {
                    entities.Add(terminationDecisionNotify);
                }
            }
            else if (workflowStatus == WorkflowStatus.Canceled)
            {
                disciplinary.DisciplinaryStatus = Status.Rejected;
            }
            entities.Add(disciplinary);



            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AcceptDisciplinary);
            var info_type = status == WorkflowStepStatus.Accept ? LocalizationHelper.GetResource(LocalizationHelper.Accept)
                : (status == WorkflowStepStatus.Reject ? LocalizationHelper.GetResource(LocalizationHelper.Reject)
                : LocalizationHelper.GetResource(LocalizationHelper.Pending));

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, disciplinary, Souccar.Domain.Audit.OperationType.Update, info + " '" + info_type + "' ", start, new List<Entity>() { disciplinary.EmployeeCard.Employee });

        }
        #endregion

        #region تدفق عمل انهاء الخدمة
        public static void SaveTerminationWorkflow(int workflowId, EmployeeTermination termination, WorkflowStepStatus status, string note)
        {
            var start = DateTime.Now;
            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var user = UserExtensions.CurrentUser;
            var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TerminationApprovalBody);

            var title = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TerminationApprovalSubjectFor), workflow.TargetUser.FullName);

            WorkflowStatus workflowStatus;
            entities.Add(workflow);
            var destinationTabName = NavigationTabName.Strategic;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "TerminationRequest";
            var destinationEntityId = "TerminationRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TerminationRequest);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", termination.Id);
            var strWhenCompleted = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TerminationHasBeenApprovedForEmployee), termination.EmployeeCard.Employee.FullName);
            var strWhenCanceled = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TerminationHasBeenRejectedForEmployee), termination.EmployeeCard.Employee.FullName);

            var notify = WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
               destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus, strWhenCompleted, strWhenCompleted, strWhenCanceled, strWhenCanceled);
            if (notify != null)
            {
                entities.Add(notify);
            }

            if (workflowStatus == WorkflowStatus.Completed)
            {
                entities.AddRange(ChangeEmployeeTerminationInfo(termination, termination.EmployeeCard));
            }
            else if (workflowStatus == WorkflowStatus.Canceled)
            {
                termination.TerminationStatus = Status.Rejected;
            }

            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AcceptTermination);
            var info_type = status == WorkflowStepStatus.Accept ? LocalizationHelper.GetResource(LocalizationHelper.Accept)
                : (status == WorkflowStepStatus.Reject ? LocalizationHelper.GetResource(LocalizationHelper.Reject)
                : LocalizationHelper.GetResource(LocalizationHelper.Pending));

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, termination, Souccar.Domain.Audit.OperationType.Update, info + " '" + info_type + "' ", start, new List<Entity>() { termination.EmployeeCard.Employee });

        }
        #endregion

        #region تدفق عمل منح مكافئة
        public static void SavePromotionWorkflow(int workflowId, EmployeePromotion promotion, WorkflowStepStatus status, string note)
        {
            var start = DateTime.Now;
            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var user = UserExtensions.CurrentUser;
            var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PromotionApprovalBody);

            var title = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PromotionApprovalSubjectFor), workflow.TargetUser.FullName);

            WorkflowStatus workflowStatus;
            entities.Add(workflow);
            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "PromotionRequest";
            var destinationEntityId = "PromotionRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PromotionRequest);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", promotion.Id);
            var strWhenCompleted = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PromotionHasBeenApprovedForEmployee), promotion.EmployeeCard.Employee.FullName);
            var strWhenCanceled = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PromotionHasBeenRejectedForEmployee), promotion.EmployeeCard.Employee.FullName);

            var notify = WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
               destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus, strWhenCompleted, strWhenCompleted, strWhenCanceled, strWhenCanceled);
            if (notify != null)
            {
                entities.Add(notify);
            }

            if (workflowStatus == WorkflowStatus.Completed)
            {
                entities.AddRange(ChangeEmployeePromotionInfo(promotion));
            }
            else if (workflowStatus == WorkflowStatus.Canceled)
            {
                promotion.PromotionStatus = Status.Rejected;
            }

            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AcceptPromotion);
            var info_type = status == WorkflowStepStatus.Accept ? LocalizationHelper.GetResource(LocalizationHelper.Accept)
                : (status == WorkflowStepStatus.Reject ? LocalizationHelper.GetResource(LocalizationHelper.Reject)
                : LocalizationHelper.GetResource(LocalizationHelper.Pending));

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, promotion, Souccar.Domain.Audit.OperationType.Update, info + " '" + info_type + "' ", start, new List<Entity>() { promotion.EmployeeCard.Employee });

        }
        #endregion

        #region تدفق عمل منح مكافئة مالية
        public static void SaveFinancialPromotionWorkflow(int workflowId, FinancialPromotion financialPromotion, WorkflowStepStatus status, string note)
        {
            var start = DateTime.Now;
            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var user = UserExtensions.CurrentUser;
            var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.FinancialPromotionApprovalBody) + " " + workflow.TargetUser.FullName;

            var title = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.FinancialPromotionApprovalSubjectFor), workflow.TargetUser.FullName);

            WorkflowStatus workflowStatus;
            entities.Add(workflow);
            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "FinancialPromotionRequest";
            var destinationEntityId = "FinancialPromotionRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.FinancialPromotionRequest);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", financialPromotion.Id);
            var strWhenCompleted = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.FinancialPromotionHasBeenApprovedForEmployee), financialPromotion.EmployeeCard.Employee.FullName);
            var strWhenCanceled = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.FinancialPromotionHasBeenRejectedForEmployee), financialPromotion.EmployeeCard.Employee.FullName);

            var notify = WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
               destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus, strWhenCompleted, strWhenCompleted, strWhenCanceled, strWhenCanceled);
            if (notify != null)
            {
                entities.Add(notify);
            }

            if (workflowStatus == WorkflowStatus.Completed)
            {

                financialPromotion.ApprovedDate = DateTime.Now;
                entities.AddRange(ChangeEmployeeFinancialPromotionInfo(financialPromotion));
            }
            else if (workflowStatus == WorkflowStatus.Canceled)
            {
                financialPromotion.FinancialPromotionStatus = Status.Rejected;
            }

            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AcceptFinancialPromotion);
            var info_type = status == WorkflowStepStatus.Accept ? LocalizationHelper.GetResource(LocalizationHelper.Accept)
                : (status == WorkflowStepStatus.Reject ? LocalizationHelper.GetResource(LocalizationHelper.Reject)
                : LocalizationHelper.GetResource(LocalizationHelper.Pending));

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, financialPromotion, Souccar.Domain.Audit.OperationType.Update, info + " '" + info_type + "' ", start, new List<Entity>() { financialPromotion.EmployeeCard.Employee });

        }
        #endregion

        #region تدفق عمل الاستقالة
        public static void SaveResignationWorkflow(int workflowId, EmployeeResignation resignation, WorkflowStepStatus status, string note)
        {
            var start = DateTime.Now;
            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var user = UserExtensions.CurrentUser;
            var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.ResignationApprovalBody);
            var title = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.ResignationApprovalSubjectFor), workflow.TargetUser.FullName);
            WorkflowStatus workflowStatus;
            entities.Add(workflow);
            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "ResignationRequest";
            var destinationEntityId = "ResignationRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.ResignationRequest);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", resignation.Id);
            var strWhenCompleted = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YourResignationHasBeenApproved);
            var strWhenCanceled = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YourResignationHasBeenRejected);

            var notify = WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
               destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus, strWhenCompleted, strWhenCompleted, strWhenCanceled, strWhenCanceled);
            if (notify != null)
            {
                entities.Add(notify);
            }

            if (workflowStatus == WorkflowStatus.Completed)
            {
                entities.AddRange(ChangeEmployeeResignationInfo(resignation, resignation.EmployeeCard));
            }
            else if (workflowStatus == WorkflowStatus.Canceled)
            {
                resignation.ResignationStatus = Status.Rejected;
            }

            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AcceptResignation);
            var info_type = status == WorkflowStepStatus.Accept ? LocalizationHelper.GetResource(LocalizationHelper.Accept)
                : (status == WorkflowStepStatus.Reject ? LocalizationHelper.GetResource(LocalizationHelper.Reject)
                : LocalizationHelper.GetResource(LocalizationHelper.Pending));

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, resignation, Souccar.Domain.Audit.OperationType.Update, info + " '" + info_type + "' ", start, new List<Entity>() { resignation.EmployeeCard.Employee });

        }
        #endregion

        #region تدفق عمل الاجازات
        public static void SaveLeaveWorkflow(int workflowId, LeaveRequest leave, WorkflowStepStatus status, string note)
        {
            var entities = new List<IAggregateRoot>();
            var start = DateTime.Now;
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var user = UserExtensions.CurrentUser;
            var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveALeaveRequestFor) + " " + workflow.TargetUser.FullName;

            var body = string.Format("{0} {1} {2}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveALeaveRequestFor), workflow.TargetUser.FullName, EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PleaseCheckItOut));

            leave = ServiceFactory.ORMService.All<LeaveRequest>().Where(x => x.WorkflowItem.Id == workflow.Id).FirstOrDefault();
            WorkflowStatus workflowStatus;
            entities.Add(workflow);
            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "EmployeeLeaveRequest";
            var destinationEntityId = "EmployeeLeaveRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.ApproveLeave);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            if (leave != null)
                destinationData.Add("ServiceId", leave.Id);

            var leaveDate = leave.StartDate != null && leave.EndDate != null ?
                leave.IsHourlyLeave ?
                leave.StartDate.ToString("d") + "  " + leave.FromTime.Value.ToString("t") + '-' + leave.ToTime.Value.ToString("t") :
                leave.StartDate.ToString("d") + "-" + leave.EndDate.ToString("d") : "";
            var strWhenCompleted = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YourLeaveHasBeenApprovedWhichDate), leaveDate);
            var strWhenCanceled = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YourLeaveHasBeenRejectedWhichDate), leaveDate);

            var notify = WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
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

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser);
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AcceptRequestedLeaves);
            var info_type = status == WorkflowStepStatus.Accept ? LocalizationHelper.GetResource(LocalizationHelper.Accept)
                : (status == WorkflowStepStatus.Reject ? LocalizationHelper.GetResource(LocalizationHelper.Reject)
                : LocalizationHelper.GetResource(LocalizationHelper.Pending));

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, leave, Souccar.Domain.Audit.OperationType.Update, info + " '" + info_type + "' ", start, new List<Entity>() { leave.EmployeeCard.Employee });
            if (status != WorkflowStepStatus.Pending)
            {
                if (workflowStatus == WorkflowStatus.Completed)
                {
                    new MobileApp.Helpers.PushNotification(strWhenCompleted, "", notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
                }
                else if (workflowStatus == WorkflowStatus.Canceled)
                {
                    new MobileApp.Helpers.PushNotification(strWhenCanceled, "", notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
                }
                else
                {
                    new MobileApp.Helpers.PushNotification(title, body, notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
                }
            }

        }
        #endregion

        #region تدفق عمل طلب مهمة
        public static void SaveMissionRequestWorkflow(int workflowId, HourlyMission mission, WorkflowStepStatus status, string note)
        {
            var start = DateTime.Now;
            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var user = UserExtensions.CurrentUser;
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
            var destinationEntityId = "HourlyMission";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.HourlyMission);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", mission.Id);
            var missionDate = mission.Date.Date.ToString("d") + "  " + mission.StartDateTime.ToString("t") + '-' + mission.EndDateTime.ToString("t");
            var strWhenCompleted = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YourMissionHasBeenApprovedWhichDate), missionDate);
            var strWhenCanceled = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YourMissionHasBeenRejectedWhichDate), missionDate);

            var notify = WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
               destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus, strWhenCompleted, strWhenCompleted, strWhenCanceled, strWhenCanceled);
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

            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AcceptHourlyMission);
            var info_type = status == WorkflowStepStatus.Accept ? LocalizationHelper.GetResource(LocalizationHelper.Accept)
                : (status == WorkflowStepStatus.Reject ? LocalizationHelper.GetResource(LocalizationHelper.Reject)
                : LocalizationHelper.GetResource(LocalizationHelper.Pending));

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, mission, Souccar.Domain.Audit.OperationType.Update, info + " '" + info_type + "' ", start, new List<Entity>() { mission.Employee });
            if (status != WorkflowStepStatus.Pending)
            {
                if (workflowStatus == WorkflowStatus.Completed)
                {
                    new MobileApp.Helpers.PushNotification(strWhenCompleted, "", notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
                }
                else if (workflowStatus == WorkflowStatus.Canceled)
                {
                    new MobileApp.Helpers.PushNotification(strWhenCanceled, "", notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
                }
                else
                {
                    new MobileApp.Helpers.PushNotification(title, body, notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
                }
            }

        }
        public static void SaveMissionRequestWorkflow(int workflowId, TravelMission mission, WorkflowStepStatus status, string note)
        {
            var start = DateTime.Now;
            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var user = UserExtensions.CurrentUser;
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
            var destinationEntityId = "TravelMission";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TravelMission);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", mission.Id);
            var missionDate = mission.FromDate.Date.ToString("d") + "-" + mission.ToDate.Date.ToString("d");
            var strWhenCompleted = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YourMissionHasBeenApprovedWhichDate), missionDate);
            var strWhenCanceled = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YourMissionHasBeenRejectedWhichDate), missionDate);
            var notify = WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
               destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus, strWhenCompleted, strWhenCompleted, strWhenCanceled, strWhenCanceled);
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

            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AcceptTravelMission);
            var info_type = status == WorkflowStepStatus.Accept ? LocalizationHelper.GetResource(LocalizationHelper.Accept)
                : (status == WorkflowStepStatus.Reject ? LocalizationHelper.GetResource(LocalizationHelper.Reject)
                : LocalizationHelper.GetResource(LocalizationHelper.Pending));

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, mission, Souccar.Domain.Audit.OperationType.Update, info + " '" + info_type + "' ", start, new List<Entity>() { mission.Employee });
            if (status != WorkflowStepStatus.Pending)
            {
                if (workflowStatus == WorkflowStatus.Completed)
                {
                    new MobileApp.Helpers.PushNotification(strWhenCompleted, "", notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
                }
                else if (workflowStatus == WorkflowStatus.Canceled)
                {
                    new MobileApp.Helpers.PushNotification(strWhenCanceled, "", notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
                }
                else
                {
                    new MobileApp.Helpers.PushNotification(title, body, notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
                }
            }

        }
        public static string SaveMissionRequestItem(int employeeId, int positionId, MissionRequestViewModel missionRequestItem)
        {
            var start = DateTime.Now;
            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            var generalSetting = ServiceFactory.ORMService.All<GeneralEmployeeRelationSetting>().FirstOrDefault();
            if (generalSetting == null || generalSetting.MissionRequestWorkflowName == null)
                return EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgWorkFlowSettingsNotExist);
            if (employee.EmployeeCard == null)
                return EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEmployeeCardNotExist);
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
                    return EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MissionAlreadyExistInTheSamePeriod);
                }

                var fromTimeRequest = request.StartDateTime.Date.TimeOfDay;
                var toTimeRequest = request.EndDateTime.Date.TimeOfDay;


                if (hourlyMissions.Any(x =>
                   ((x.Status == Status.Approved) || (x.Status == Status.Draft)) && request.Date.Date == x.Date &&
                   (missionRequestItem.FromTime >= x.StartDateTime && missionRequestItem.FromTime <= x.EndDateTime) ||
                  (missionRequestItem.ToTime >= x.StartDateTime && missionRequestItem.ToTime <= x.EndDateTime)
                    ))
                {
                    return EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MissionAlreadyExistInTheSamePeriod);
                }

                //اختبار الموظف على راس عمله ومطالب بالدوام
                var employeeAttendanceCard = typeof(EmployeeCard).GetAll<EmployeeCard>().FirstOrDefault(x => x.Employee.Id == employee.Id);
                if (!(employeeAttendanceCard.CardStatus == EmployeeCardStatus.OnHeadOfHisWork && employeeAttendanceCard.AttendanceDemand))
                {
                    return EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEmployeeIsNotOnHeadOfHisWork);
                }
                var user = UserExtensions.CurrentUser;
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
                var destinationEntityOperationType = OperationType.Nothing;
                IDictionary<string, int> destinationData = new Dictionary<string, int>();
                var notify = new Notify();
                var serviceWorkflow = employee.EmployeeCard.ServiceWorkflows.FirstOrDefault(x => x.ServiceType == HRIS.Domain.EmployeeRelationServices.Enums.ServiceType.Mission);
                var workflowItem = WorkflowHelper.InitWithSetting(serviceWorkflow != null ? serviceWorkflow.WorkflowSetting : generalSetting.MissionRequestWorkflowName, employee.User(),
                    title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                    destinationActionName, destinationEntityId, destinationEntityTitle
                    , destinationEntityOperationType, destinationData,
                    employee.User().Position(), WorkflowType.EmployeeMissionRequest, EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.HourlyMission) + " - " + missionRequestItem.Description, out notify);
                request.WorkflowItem = workflowItem;
                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, request }, UserExtensions.CurrentUser);
                notify.DestinationData.Add("WorkflowId", workflowItem.Id);
                notify.DestinationData.Add("ServiceId", request.Id);
                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, UserExtensions.CurrentUser);
                new MobileApp.Helpers.PushNotification(title, body, notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
                return string.Empty;
            }
            else
            {
                var request = new TravelMission()
                {
                    Employee = employee,
                    Note = missionRequestItem.Description,
                    Status = Status.Draft,
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
                    return EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MissionAlreadyExistInTheSamePeriod);
                }
                if (hourlyMissions.Any(x =>
                    ((x.Status == Status.Approved) || (x.Status == Status.Draft)) &&
                   (missionRequestItem.EndDate >= x.EndDateTime.Date)
                   &&
                   (missionRequestItem.StartDate <= x.StartDateTime.Date)))
                {
                    return EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MissionAlreadyExistInTheSamePeriod);
                }
                //اختبار الموظف على راس عمله ومطالب بالدوام
                var employeeAttendanceCard = typeof(EmployeeCard).GetAll<EmployeeCard>().FirstOrDefault(x => x.Employee.Id == employee.Id);
                if (!(employeeAttendanceCard.CardStatus == EmployeeCardStatus.OnHeadOfHisWork && employeeAttendanceCard.AttendanceDemand))
                {
                    return EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEmployeeIsNotOnHeadOfHisWork);
                }
                var user = UserExtensions.CurrentUser;
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
                var destinationEntityOperationType = OperationType.Nothing;
                IDictionary<string, int> destinationData = new Dictionary<string, int>();
                var notify = new Notify();
                var serviceWorkflow = employee.EmployeeCard.ServiceWorkflows.FirstOrDefault(x => x.ServiceType == HRIS.Domain.EmployeeRelationServices.Enums.ServiceType.Mission);
                var workflowItem = WorkflowHelper.InitWithSetting(serviceWorkflow != null ? serviceWorkflow.WorkflowSetting : generalSetting.MissionRequestWorkflowName, employee.User(),
                    title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                    destinationActionName, destinationEntityId, destinationEntityTitle
                    , destinationEntityOperationType, destinationData,
                    employee.User().Position(), WorkflowType.EmployeeMissionRequest, EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TravelMission) + " - " + missionRequestItem.Description, out notify);
                request.WorkflowItem = workflowItem;
                // ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, request }, UserExtensions.CurrentUser);
                var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MissionRequest);
                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, request }, UserExtensions.CurrentUser, request, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { request.Employee });

                notify.DestinationData.Add("WorkflowId", workflowItem.Id);
                notify.DestinationData.Add("ServiceId", request.Id);
                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, UserExtensions.CurrentUser);
                new MobileApp.Helpers.PushNotification(title, body, notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
                return string.Empty;
            }



        }
        #endregion

        #region تدفق عمل سجل الدخول والخروج
        public static void SaveEntranceExitRecordRequestWorkflow(int workflowId, EntranceExitRecordRequest recordRequest, WorkflowStepStatus status, string note)
        {
            var start = DateTime.Now;
            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var user = UserExtensions.CurrentUser;
            var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveAEntranceOrExitRecordRequestFor) + " " + workflow.TargetUser.FullName;

            var body = string.Format("{0} {1} {2}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveAEntranceOrExitRecordRequestFor), workflow.TargetUser.FullName, EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PleaseCheckItOut));

            WorkflowStatus workflowStatus;
            entities.Add(workflow);
            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "EntranceExitRequest";
            var destinationEntityId = "EntranceExitRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EntranceExitRecordRequest);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", recordRequest.Id);
            var recordDate = recordRequest.LogDateTime.Date.ToString("d") + "-" + recordRequest.LogDateTime.ToString("t");
            var strWhenCompleted = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YourEntraceExitRecordHasBeenApprovedWhichDate), recordDate);
            var strWhenCanceled = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YourEntraceExitRecordHasBeenRejectedWhichDate), recordDate);

            var notify = WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
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

            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AcceptEntraceExitRecord);
            var info_type = status == WorkflowStepStatus.Accept ? LocalizationHelper.GetResource(LocalizationHelper.Accept)
                : (status == WorkflowStepStatus.Reject ? LocalizationHelper.GetResource(LocalizationHelper.Reject)
                : LocalizationHelper.GetResource(LocalizationHelper.Pending));

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, recordRequest, Souccar.Domain.Audit.OperationType.Update, info + " '" + info_type + "' ", start, new List<Entity>() { recordRequest.Employee });
            if (status != WorkflowStepStatus.Pending)
            {
                if (workflowStatus == WorkflowStatus.Completed)
                {
                    new MobileApp.Helpers.PushNotification(strWhenCompleted, "", notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
                }
                else if (workflowStatus == WorkflowStatus.Canceled)
                {
                    new MobileApp.Helpers.PushNotification(strWhenCanceled, "", notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
                }
                else
                {
                    new MobileApp.Helpers.PushNotification(title, body, notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
                }
            }
        }
        public static string SaveEntranceExitRecordRequestItem(int employeeId, int positionId, EntranceExitRequestViewModel recordRequestItem)
        {
            var start = DateTime.Now;
            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            var generalSetting = ServiceFactory.ORMService.All<GeneralEmployeeRelationSetting>().FirstOrDefault();
            if (generalSetting == null || generalSetting.EntranceExitRequestWorkflowName == null)
                return EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgWorkFlowSettingsNotExist);
            if (employee.EmployeeCard == null)
                return EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEmployeeCardNotExist);

            var request = new EntranceExitRecordRequest()
            {
                Creator = UserExtensions.CurrentUser,
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
                return EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EntranceExitRecordAlreadyExist);
            }
            //اختبار تكرار السجل
            var employeeAttendanceCard = typeof(EmployeeCard).GetAll<EmployeeCard>().FirstOrDefault(x => x.Employee.Id == employee.Id);
            var allEntranceExitRecordDataOfEmployee = ServiceFactory.ORMService.All<EntranceExitRecord>().Where(x => x.Employee.Id == employee.Id).ToList();
            if (AttendanceSystem.Services.AttendanceService.CheckEntranceExitRecordDuplicate(allEntranceExitRecordDataOfEmployee, request.LogDateTime, InsertSource.ByEmployee, request.LogType, 0))
            {
                return EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EntranceExitRecordAlreadyExist);
            }
            //اختبار السجل بعد التاريخ الحالي
            if (recordRequestItem.RecordDate > DateTime.Now)
            {
                return EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouCannotAddRecordWithFutureDate);
            }
            //اختبار الموظف على راس عمله ومطالب بالدوام
            if (!(employeeAttendanceCard.CardStatus == EmployeeCardStatus.OnHeadOfHisWork && employeeAttendanceCard.AttendanceDemand))
            {
                return EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEmployeeIsNotOnHeadOfHisWork);
            }
            var user = UserExtensions.CurrentUser;
            var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveAEntranceOrExitRecordRequestFor) + " " + employee.FullName;

            var body = string.Format("{0} {1} {2}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveAEntranceOrExitRecordRequestFor), employee.FullName, EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PleaseCheckItOut));

            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "EntranceExitRequest";
            var destinationEntityId = "EntranceExitRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EntranceExitRecordRequest);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            var notify = new Notify();
            var serviceWorkflow = employee.EmployeeCard.ServiceWorkflows.FirstOrDefault(x => x.ServiceType == HRIS.Domain.EmployeeRelationServices.Enums.ServiceType.EntranceExitRecord);
            var workflowItem = WorkflowHelper.InitWithSetting(serviceWorkflow != null ? serviceWorkflow.WorkflowSetting : generalSetting.EntranceExitRequestWorkflowName, employee.User(),
                title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                destinationActionName, destinationEntityId, destinationEntityTitle
                , destinationEntityOperationType, destinationData,
                employee.User().Position(), WorkflowType.EmployeeEntranceExitRecordRequest, (recordRequestItem.LogType == LogType.Entrance ?
                    EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.Entrance)
                    : EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.Exit)) + " - " + recordRequestItem.Note, out notify);
            request.WorkflowItem = workflowItem;
            // ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, request }, UserExtensions.CurrentUser);
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EntranceExitRecord);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, request }, UserExtensions.CurrentUser, request, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { request.Employee });

            notify.DestinationData.Add("WorkflowId", workflowItem.Id);
            notify.DestinationData.Add("ServiceId", request.Id);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, UserExtensions.CurrentUser);
            new MobileApp.Helpers.PushNotification(title, body, notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
            return string.Empty;
        }
        public static List<EntranceExitRequestViewModel> GetEmployeeEntranceExitRequestApproval()
        {
            var result = new List<EntranceExitRequestViewModel>();
            Position currentPosition = null;
            if (EmployeeExtensions.CurrentEmployee == null)
            {
                return result;
            }
            currentPosition = EmployeeExtensions.CurrentEmployee.PrimaryPosition();
            if (currentPosition == null)
            {
                return result;
            }

            var employeeEntranceExitRequests =
                ServiceFactory.ORMService.All<EntranceExitRecordRequest>()
                .Where(x => x.WorkflowItem.Status == WorkflowStatus.InProgress ||
                            x.WorkflowItem.Status == WorkflowStatus.Pending).ToList();

            foreach (var record in employeeEntranceExitRequests)
            {
                WorkflowPendingType pendingType;
                var startPosition = WorkflowHelper.GetNextAppraiser(record.WorkflowItem, out pendingType);
                if (startPosition == currentPosition)
                    result.Add(new EntranceExitRequestViewModel()
                    {
                        EmployeeId = record.Employee.Id,
                        FullName = record.Employee.FullName,
                        PositionId = record.Employee.PrimaryPosition().Id,
                        PositionName = record.Employee.PrimaryPosition().NameForDropdown,
                        RecordId = record.Id,
                        LogType = record.LogType,
                        Note = record.Note,
                        RecordDate = record.RecordDate,
                        RecordTime = record.RecordTime,
                        WorkflowItemId = record.WorkflowItem.Id,
                        PendingType = pendingType
                    });
            }

            return result;
        }
        #endregion

        #region تدفق عمل طلب نقل
        public static void SaveTransferRequestWorkflow(int workflowId, EmployeeTransferRequest request, WorkflowStepStatus status, string note)
        {
            var start = DateTime.Now;
            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var user = UserExtensions.CurrentUser;
            var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveATransferRequestFor) + " " + workflow.TargetUser.FullName;

            var body = string.Format("{0} {1} {2}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveATransferRequestFor), workflow.TargetUser.FullName, EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PleaseCheckItOut));

            WorkflowStatus workflowStatus;
            entities.Add(workflow);
            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "EmployeeTransferRequest";
            var destinationEntityId = "EmployeeTransferRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TransferRequest);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", request.Id);
            var recordDate = request.Date.ToString("dd / MM / yyyy");
            var strWhenCompleted = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YourTransferRequestHasBeenApprovedWhichDate), recordDate);
            var strWhenCanceled = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YourTransferRequestHasBeenRejectedWhichDate), recordDate);
            Notify notify = null;
            if (request.WorkflowItem.Description.StartsWith(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TransferProcess)
                + " - " + EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.DestWorkflow)))
            {
                notify = WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
              destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus, strWhenCompleted, strWhenCompleted, strWhenCanceled, strWhenCanceled, false);
            }
            else
            {
                notify = WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
              destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus, strWhenCompleted, strWhenCompleted, strWhenCanceled, strWhenCanceled);
            }

            if (notify != null)
            {
                entities.Add(notify);
            }

            if (workflowStatus == WorkflowStatus.Completed)
            {
                if (request.WorkflowItem.Description.StartsWith(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TransferProcess)
                + " - " + EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.DestWorkflow)))
                {
                    var requestItem = new EmployeeTransferRequestViewModel();
                    var emp = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == request.Employee.Id);
                    var posistion = ServiceFactory.ORMService.All<AssigningEmployeeToPosition>().FirstOrDefault(x => x.Employee == emp);
                    requestItem.EmployeeId = emp.Id;
                    requestItem.FullName = emp.FullName;
                    requestItem.PositionId = posistion.Id;
                    requestItem.PositionName = posistion.Position.NameForDropdown;
                    requestItem.Note = request.Note ?? "";
                    requestItem.DestNode = request.DestNode.Id;
                    requestItem.DestPosition = request.DestPosition.Id;
                    requestItem.SourceNode = request.SourceNode.Id;
                    requestItem.SourcePosition = request.SourcePosition.Id;
                    requestItem.RequestDate = request.Date;
                    requestItem.RequestId = request.Id;
                    requestItem.WorkflowItemId = request.WorkflowItem.Id;
                    SaveSecTransferRequestItem(request.Employee.Id, request.Employee.Positions.FirstOrDefault(x => x.IsPrimary).Id, requestItem);
                }
                else
                {
                    var req = ServiceFactory.ORMService.GetById<EmployeeLoan>(request.Id);
                    request.RequestStatus = Status.Approved;
                }
            }
            else if (workflowStatus == WorkflowStatus.Canceled)
            {
                request.RequestStatus = Status.Rejected;
            }

            entities.Add(request);

            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AcceptEntraceExitRecord);
            var info_type = status == WorkflowStepStatus.Accept ? LocalizationHelper.GetResource(LocalizationHelper.Accept)
                : (status == WorkflowStepStatus.Reject ? LocalizationHelper.GetResource(LocalizationHelper.Reject)
                : LocalizationHelper.GetResource(LocalizationHelper.Pending));

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, request, Souccar.Domain.Audit.OperationType.Update, info + " '" + info_type + "' ", start, new List<Entity>() { request.Employee });
            if (status != WorkflowStepStatus.Pending)
            {
                if (workflowStatus == WorkflowStatus.Completed)
                {
                    new MobileApp.Helpers.PushNotification(strWhenCompleted, "", notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
                }
                else if (workflowStatus == WorkflowStatus.Canceled)
                {
                    new MobileApp.Helpers.PushNotification(strWhenCanceled, "", notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
                }
                else
                {
                    new MobileApp.Helpers.PushNotification(title, body, notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
                }
            }
        }
        public static string SaveFirstTransferRequestItem(int employeeId, int positionId, EmployeeTransferRequestViewModel RequestItem)
        {
            var start = DateTime.Now;
            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            var generalSetting = ServiceFactory.ORMService.All<GeneralEmployeeRelationSetting>().FirstOrDefault();
            if (generalSetting == null || generalSetting.EmployeeDistenationTransferRequest == null)
                return EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgWorkFlowSettingsNotExist);
            if (employee.EmployeeCard == null)
                return EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEmployeeCardNotExist);
            var destNode = ServiceFactory.ORMService.GetById<HRIS.Domain.OrganizationChart.RootEntities.Node>(RequestItem.DestNode);
            var destPosition = ServiceFactory.ORMService.GetById<Position>(RequestItem.DestPosition);
            var sourcePosition = ServiceFactory.ORMService.GetById<Position>(positionId);
            var sourceNode = sourcePosition.JobDescription.Node;
            var request = new EmployeeTransferRequest()
            {
                Creator = UserExtensions.CurrentUser,
                Employee = employee,
                Note = RequestItem.Note,
                RequestStatus = Status.Draft,
                Date = RequestItem.RequestDate,
                DestNode = destNode,
                DestPosition = destPosition,
                SourceNode = sourceNode,
                SourcePosition = sourcePosition
            };
            var employeeAttendanceCard = typeof(EmployeeCard).GetAll<EmployeeCard>().FirstOrDefault(x => x.Employee.Id == employee.Id);
            //اختبار الموظف على راس عمله ومطالب بالدوام
            if (!(employeeAttendanceCard.CardStatus == EmployeeCardStatus.OnHeadOfHisWork && employeeAttendanceCard.AttendanceDemand))
            {
                return EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEmployeeIsNotOnHeadOfHisWork);
            }
            var user = UserExtensions.CurrentUser;
            var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveATransferRequestFor) + " " + employee.FullName;

            var body = string.Format("{0} {1} {2}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveATransferRequestFor), employee.FullName, EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PleaseCheckItOut));

            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "EmployeeTransferRequest";
            var destinationEntityId = "EmployeeTransferRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TransferRequest);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            var notify = new Notify();
            var workflowItem = WorkflowHelper.InitWithSetting(generalSetting.EmployeeDistenationTransferRequest, employee.User(),
                title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                destinationActionName, destinationEntityId, destinationEntityTitle
                , destinationEntityOperationType, destinationData,
                destPosition, WorkflowType.EmployeeTransferRequest, EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TransferProcess)
                + " - " + EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.DestWorkflow)
                + " - " + request.Date.ToString("dd / MM / yyyy")
                + " - " + request.Employee.FullName
                , out notify, true);
            request.WorkflowItem = workflowItem;
            // ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, request }, UserExtensions.CurrentUser);
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EmployeeTransferRequest);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, request }, UserExtensions.CurrentUser, request, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { request.Employee });

            notify.DestinationData.Add("WorkflowId", workflowItem.Id);
            notify.DestinationData.Add("ServiceId", request.Id);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, UserExtensions.CurrentUser);
            if (workflowItem.Status == WorkflowStatus.Completed)
            {
                var requestItem = new EmployeeTransferRequestViewModel();
                var emp = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == request.Employee.Id);
                var posistion = ServiceFactory.ORMService.All<AssigningEmployeeToPosition>().FirstOrDefault(x => x.Employee == emp);
                requestItem.EmployeeId = emp.Id;
                requestItem.FullName = emp.FullName;
                requestItem.PositionId = posistion.Id;
                requestItem.PositionName = posistion.Position.NameForDropdown;
                requestItem.Note = request.Note ?? "";
                requestItem.DestNode = request.DestNode.Id;
                requestItem.DestPosition = request.DestPosition.Id;
                requestItem.SourceNode = request.SourceNode.Id;
                requestItem.SourcePosition = request.SourcePosition.Id;
                requestItem.RequestDate = request.Date;
                requestItem.RequestId = request.Id;
                requestItem.WorkflowItemId = request.WorkflowItem.Id;
                SaveSecTransferRequestItem(request.Employee.Id, request.Employee.Positions.FirstOrDefault(x => x.IsPrimary).Id, requestItem);
            }
            new MobileApp.Helpers.PushNotification(title, body, notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
            return string.Empty;
        }
        public static string SaveSecTransferRequestItem(int employeeId, int positionId, EmployeeTransferRequestViewModel RequestItem)
        {
            var start = DateTime.Now;
            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            var generalSetting = ServiceFactory.ORMService.All<GeneralEmployeeRelationSetting>().FirstOrDefault();
            if (generalSetting == null || generalSetting.EmployeeSourceTransferRequest == null)
                return EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgWorkFlowSettingsNotExist);
            if (employee.EmployeeCard == null)
                return EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEmployeeCardNotExist);
            var destNode = ServiceFactory.ORMService.GetById<HRIS.Domain.OrganizationChart.RootEntities.Node>(RequestItem.DestNode);
            var destPosition = ServiceFactory.ORMService.GetById<Position>(RequestItem.DestPosition);
            var sourcePosition = ServiceFactory.ORMService.GetById<Position>(RequestItem.SourcePosition);
            var sourceNode = sourcePosition.JobDescription.Node;
            var employeeAttendanceCard = typeof(EmployeeCard).GetAll<EmployeeCard>().FirstOrDefault(x => x.Employee.Id == employee.Id);
            //اختبار الموظف على راس عمله ومطالب بالدوام
            if (!(employeeAttendanceCard.CardStatus == EmployeeCardStatus.OnHeadOfHisWork && employeeAttendanceCard.AttendanceDemand))
            {
                return EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEmployeeIsNotOnHeadOfHisWork);
            }
            var user = UserExtensions.CurrentUser;
            var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveATransferRequestFor) + " " + employee.FullName;

            var body = string.Format("{0} {1} {2}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveATransferRequestFor), employee.FullName, EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PleaseCheckItOut));

            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "EmployeeTransferRequest";
            var destinationEntityId = "EmployeeTransferRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TransferRequest);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            var notify = new Notify();
            var workflowItem = WorkflowHelper.InitWithSetting(generalSetting.EmployeeSourceTransferRequest, employee.User(),
                title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                destinationActionName, destinationEntityId, destinationEntityTitle
                , destinationEntityOperationType, destinationData,
                sourcePosition, WorkflowType.EmployeeTransferRequest, EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TransferProcess)
                + " - " + EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.SourceWorkflow)
                + " - " + RequestItem.RequestDate.ToString("dd / MM / yyyy")
                + " - " + employee.FullName
                , out notify, true);
            var request = ServiceFactory.ORMService.GetById<EmployeeTransferRequest>(RequestItem.RequestId);
            request.WorkflowItem = workflowItem;
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EmployeeTransferRequest);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, request }, UserExtensions.CurrentUser, request, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { employee });

            notify.DestinationData.Add("WorkflowId", workflowItem.Id);
            notify.DestinationData.Add("ServiceId", request.Id);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, UserExtensions.CurrentUser);
            new MobileApp.Helpers.PushNotification(title, body, notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
            return string.Empty;
        }
        #endregion

        #region تدفق عمل طلب قرض
        public static void SaveLoanRequestWorkflow(int workflowId, EmployeeLoan request, WorkflowStepStatus status, string note)
        {
            var start = DateTime.Now;
            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var user = UserExtensions.CurrentUser;
            var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveALoanRequestFor) + " " + workflow.TargetUser.FullName;

            var body = string.Format("{0} {1} {2}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveALoanRequestFor), workflow.TargetUser.FullName, EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PleaseCheckItOut));

            WorkflowStatus workflowStatus;
            entities.Add(workflow);
            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "EmployeeLoanRequest";
            var destinationEntityId = "EmployeeLoanRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.LoanRequest);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", request.Id);
            var recordDate = request.Date.ToString("dd / MM / yyyy");
            var strWhenCompleted = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YourLoanRequestHasBeenApprovedWhichDate), recordDate);
            var strWhenCanceled = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YourLoanRequestHasBeenRejectedWhichDate), recordDate);
            Notify notify = null;
            if (request.WorkflowItem.Description.StartsWith(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.LoanRequestRepresentative)))
            {
                notify = WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
              destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus, strWhenCompleted, strWhenCompleted, strWhenCanceled, strWhenCanceled, false);
            }
            else
            {
                notify = WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
              destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus, strWhenCompleted, strWhenCompleted, strWhenCanceled, strWhenCanceled);
            }

            if (notify != null)
            {
                entities.Add(notify);
            }

            if (workflowStatus == WorkflowStatus.Completed)
            {
                if (request.WorkflowItem.Description.StartsWith(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.LoanRequestRepresentative)))
                {
                    var requestItem = new EmployeeLoanRequestViewModel();
                    var emp = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == request.EmployeeCard.Employee.Id);
                    var posistion = ServiceFactory.ORMService.All<AssigningEmployeeToPosition>().FirstOrDefault(x => x.Employee == emp);
                    requestItem.EmployeeId = emp.Id;
                    requestItem.FullName = emp.FullName;
                    requestItem.PositionId = posistion.Id;
                    requestItem.PositionName = posistion.Position.NameForDropdown;
                    requestItem.Note = request.Note ?? "";
                    requestItem.FirstRepresentative = request.FirstRepresentativeEmployee.Id;
                    requestItem.SecondRepresentative = request.SecondRepresentativeEmployee.Id;
                    requestItem.RequestDate = request.Date;
                    requestItem.RequestId = request.Id;
                    requestItem.WorkflowItemId = request.WorkflowItem.Id;
                    requestItem.TotalAmount = request.TotalAmountOfLoan;
                    requestItem.PaymentsCount = request.PaymentsCount;
                    requestItem.MonthlyInstallment = request.MonthlyInstalmentValue;

                    SaveSecLoanRequestItem(request.EmployeeCard.Employee.Id, request.EmployeeCard.Employee.Positions.FirstOrDefault(x => x.IsPrimary).Position.Id, requestItem);
                }
                else
                {
                    request.RequestStatus = Status.Approved;
                }
            }
            else if (workflowStatus == WorkflowStatus.Canceled)
            {
                request.RequestStatus = Status.Rejected;
            }

            entities.Add(request);

            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AcceptEntraceExitRecord);
            var info_type = status == WorkflowStepStatus.Accept ? LocalizationHelper.GetResource(LocalizationHelper.Accept)
                : (status == WorkflowStepStatus.Reject ? LocalizationHelper.GetResource(LocalizationHelper.Reject)
                : LocalizationHelper.GetResource(LocalizationHelper.Pending));

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, request, Souccar.Domain.Audit.OperationType.Update, info + " '" + info_type + "' ", start, new List<Entity>() { request.EmployeeCard.Employee });
            if (status != WorkflowStepStatus.Pending)
            {
                if (workflowStatus == WorkflowStatus.Completed)
                {
                    new MobileApp.Helpers.PushNotification(strWhenCompleted, "", notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
                }
                else if (workflowStatus == WorkflowStatus.Canceled)
                {
                    new MobileApp.Helpers.PushNotification(strWhenCanceled, "", notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
                }
                else
                {
                    new MobileApp.Helpers.PushNotification(title, body, notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
                }
            }
        }
        public static string SaveFirstLoanRequestItem(int employeeId, int positionId, EmployeeLoanRequestViewModel RequestItem)
        {
            var start = DateTime.Now;
            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            var position = ServiceFactory.ORMService.GetById<Position>(positionId);
            if (employee.EmployeeCard == null)
                return EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEmployeeCardNotExist);
            var FirstRepresentative = ServiceFactory.ORMService.GetById<Employee>(RequestItem.FirstRepresentative);
            var SecondRepresentative = ServiceFactory.ORMService.GetById<Employee>(RequestItem.SecondRepresentative);
            var empCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Employee == employee);

            var request = new EmployeeLoan()
            {
                Creator = UserExtensions.CurrentUser,
                EmployeeCard = empCard,
                Note = RequestItem.Note,
                RequestStatus = Status.Draft,
                RequestDate = RequestItem.RequestDate,
                Date = RequestItem.RequestDate,
                FirstRepresentativeEmployee = FirstRepresentative,
                SecondRepresentativeEmployee = SecondRepresentative,
                PaymentsCount = RequestItem.PaymentsCount,
                MonthlyInstalmentValue = RequestItem.TotalAmount / RequestItem.PaymentsCount,
                TotalAmountOfLoan = RequestItem.TotalAmount,
                FirstRepresentative = FirstRepresentative.FullName,
                SecondRepresentative = SecondRepresentative.FullName,
                PrePayed = 0,
            };

            var lastLoan = ServiceFactory.ORMService.All<EmployeeLoan>().OrderByDescending(x => x.Id).FirstOrDefault();
            if (lastLoan != null)
            {
                request.LoanNumber = (int.Parse(lastLoan.LoanNumber) + 1).ToString();
            }
            else
            {
                request.LoanNumber = "1";
            }
            var firstApproval = new WorkflowSettingApproval()
            {
                Position = FirstRepresentative.Positions.Where(x => x.IsPrimary).FirstOrDefault().Position,
                Order = 1
            };
            var secApproval = new WorkflowSettingApproval()
            {
                Position = SecondRepresentative.Positions.Where(x => x.IsPrimary).FirstOrDefault().Position,
                Order = 2
            };
            var workflowSetting = new WorkflowSetting()
            {
                Title = "LoanWorkflowSetting",
                CreationDate = DateTime.Now,
                InitStepCount = 0,
            };
            workflowSetting.AddApprovals(firstApproval);
            workflowSetting.AddApprovals(secApproval);
            var employeeAttendanceCard = typeof(EmployeeCard).GetAll<EmployeeCard>().FirstOrDefault(x => x.Employee.Id == employee.Id);
            //اختبار الموظف على راس عمله ومطالب بالدوام
            if (!(employeeAttendanceCard.CardStatus == EmployeeCardStatus.OnHeadOfHisWork && employeeAttendanceCard.AttendanceDemand))
            {
                return EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEmployeeIsNotOnHeadOfHisWork);
            }
            var user = UserExtensions.CurrentUser;
            var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveALoanRequestFor) + " " + employee.FullName;

            var body = string.Format("{0} {1} {2}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveALoanRequestFor), employee.FullName, EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PleaseCheckItOut));

            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "EmployeeLoanRequest";
            var destinationEntityId = "EmployeeLoanRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.LoanRequest);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            var notify = new Notify();
            var workflowItem = WorkflowHelper.InitWithSetting(workflowSetting, employee.User(),
                title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                destinationActionName, destinationEntityId, destinationEntityTitle
                , destinationEntityOperationType, destinationData,
                position, WorkflowType.EmployeeLoanRequest, EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.LoanRequestRepresentative) + " - " + request.RequestDate + " - " + request.Note, out notify, true);
            request.WorkflowItem = workflowItem;
            // ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, request }, UserExtensions.CurrentUser);
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EmployeeLoanRequest);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, request }, UserExtensions.CurrentUser, request, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { request.EmployeeCard.Employee });

            notify.DestinationData.Add("WorkflowId", workflowItem.Id);
            notify.DestinationData.Add("ServiceId", request.Id);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, UserExtensions.CurrentUser);
            new MobileApp.Helpers.PushNotification(title, body, notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
            return string.Empty;
        }
        public static string SaveSecLoanRequestItem(int employeeId, int positionId, EmployeeLoanRequestViewModel RequestItem)
        {
            var start = DateTime.Now;
            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            var position = ServiceFactory.ORMService.GetById<Position>(positionId);
            var generalSetting = ServiceFactory.ORMService.All<GeneralEmployeeRelationSetting>().FirstOrDefault();
            if (generalSetting == null || generalSetting.EmployeeLoanRequest == null)
                return EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgWorkFlowSettingsNotExist);
            if (employee.EmployeeCard == null)
                return EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEmployeeCardNotExist);
            var FirstRepresentative = ServiceFactory.ORMService.GetById<Employee>(RequestItem.FirstRepresentative);
            var SecondRepresentative = ServiceFactory.ORMService.GetById<Employee>(RequestItem.SecondRepresentative);

            var employeeAttendanceCard = typeof(EmployeeCard).GetAll<EmployeeCard>().FirstOrDefault(x => x.Employee.Id == employee.Id);
            //اختبار الموظف على راس عمله ومطالب بالدوام
            if (!(employeeAttendanceCard.CardStatus == EmployeeCardStatus.OnHeadOfHisWork && employeeAttendanceCard.AttendanceDemand))
            {
                return EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.MsgEmployeeIsNotOnHeadOfHisWork);
            }
            var user = UserExtensions.CurrentUser;
            var title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveALoanRequestFor) + " " + employee.FullName;

            var body = string.Format("{0} {1} {2}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveALoanRequestFor), employee.FullName, EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PleaseCheckItOut));

            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "EmployeeLoanRequest";
            var destinationEntityId = "EmployeeLoanRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.LoanRequest);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            var notify = new Notify();
            var serviceWorkflow = employee.EmployeeCard.ServiceWorkflows.FirstOrDefault(x => x.ServiceType == HRIS.Domain.EmployeeRelationServices.Enums.ServiceType.Loan);
            var workflowItem = Project.Web.Mvc4.Helpers.WorkflowHelper.InitWithSetting(serviceWorkflow != null ? serviceWorkflow.WorkflowSetting : generalSetting.EmployeeLoanRequest, employee.User(),
                title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                destinationActionName, destinationEntityId, destinationEntityTitle
                , destinationEntityOperationType, destinationData,
                position, WorkflowType.EmployeeLoanRequest, EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.LoanRequestMain) + " - " + RequestItem.RequestDate + " - " + RequestItem.Note, out notify, true);
            var request = ServiceFactory.ORMService.GetById<EmployeeLoan>(RequestItem.RequestId);
            request.WorkflowItem = workflowItem;
            request.WorkflowItem = workflowItem;
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EmployeeLoanRequest);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, request }, UserExtensions.CurrentUser, request, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { request.EmployeeCard.Employee });

            notify.DestinationData.Add("WorkflowId", workflowItem.Id);
            notify.DestinationData.Add("ServiceId", request.Id);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, UserExtensions.CurrentUser);
            new MobileApp.Helpers.PushNotification(title, body, notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
            return string.Empty;
        }
        #endregion

        #region تدفق عمل السلفة
        public static void SaveAdvanceWorkflow(int workflowId, EmployeeAdvance advance, WorkflowStepStatus status, string note, int advanceAmount)
        {
            var start = DateTime.Now;
            if (advanceAmount > 0)
                advance.AdvanceAmount = advanceAmount;
            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var user = UserExtensions.CurrentUser;
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
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", advance.Id);
            var strWhenCompleted = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AdvanceHasBeenApprovedForEmployee), advance.EmployeeCard.Employee.FullName);
            var strWhenCanceled = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AdvanceHasBeenRejectedForEmployee), advance.EmployeeCard.Employee.FullName);

            var notify = WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
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
            if (status != WorkflowStepStatus.Pending)
            {
                if (workflowStatus == WorkflowStatus.Completed)
                {
                    new MobileApp.Helpers.PushNotification(strWhenCompleted, "", notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
                }
                else if (workflowStatus == WorkflowStatus.Canceled)
                {
                    new MobileApp.Helpers.PushNotification(strWhenCanceled, "", notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
                }
                else
                {
                    new MobileApp.Helpers.PushNotification(title, body, notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
                }
            }
        }
        #endregion
        #region تدفق عمل العهدة
        public static void SaveCustodiesWorkflow(int workflowId, EmployeeCustodie custodies, WorkflowStepStatus status, string note)
        {
            var start = DateTime.Now;

            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var user = UserExtensions.CurrentUser;
            var body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.CustodiesApprovalBody);

            var title = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.CustodiesApprovalSubjectFor), workflow.TargetUser.FullName);

            WorkflowStatus workflowStatus;
            entities.Add(workflow);
            var destinationTabName = NavigationTabName.Operational;
            var destinationModuleName = ModulesNames.EmployeeRelationServices;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
            var destinationControllerName = "EmployeeRelationServices/Service";
            var destinationActionName = "EmployeeCustodiesRequest";
            var destinationEntityId = "EmployeeCustodiesRequest";
            var destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EmployeeCustodiesRequest);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", custodies.Id);
            var strWhenCompleted = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.CustodiesHasBeenApprovedForEmployee), custodies.EmployeeCard.Employee.FullName);
            var strWhenCanceled = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.CustodiesHasBeenRejectedForEmployee), custodies.EmployeeCard.Employee.FullName);

            var notify = WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
               destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus, strWhenCompleted, strWhenCompleted, strWhenCanceled, strWhenCanceled);
            if (notify != null)
            {
                entities.Add(notify);
            }

            if (workflowStatus == WorkflowStatus.Completed)
            {
                entities.AddRange(ChangeEmployeeCustodiesInfo(custodies));
            }
            else if (workflowStatus == WorkflowStatus.Canceled)
            {
                custodies.CustodieStatus = Status.Rejected;
            }

            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AcceptCustodies);
            var info_type = status == WorkflowStepStatus.Accept ? LocalizationHelper.GetResource(LocalizationHelper.Accept)
                : (status == WorkflowStepStatus.Reject ? LocalizationHelper.GetResource(LocalizationHelper.Reject)
                : LocalizationHelper.GetResource(LocalizationHelper.Pending));

            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, custodies, Souccar.Domain.Audit.OperationType.Update, info + " '" + info_type + "' ", start, new List<Entity>() { custodies.EmployeeCard.Employee });

        }
        #endregion

        public static Notify AddTerminationDecisionNotify(EmployeeDisciplinary employeeDisciplinary, EmployeeCard employeeCard)
        {
            var generalOption = ServiceFactory.ORMService.All<GeneralOption>().FirstOrDefault();
            employeeDisciplinary.DisciplinaryStatus = Status.Approved;
            if ((employeeDisciplinary.DisciplinarySetting.DisciplinaryNumber != 0) && (employeeCard.EmployeeDisciplinarys.Count(x => x.WorkflowItem == null || x.WorkflowItem.Status == WorkflowStatus.Completed) > employeeDisciplinary.DisciplinarySetting.DisciplinaryNumber))
            {
                var notify = new Notify()
                {
                    Sender = UserExtensions.CurrentUser,
                    Body = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.DisciplinaryNotifyBody, employeeCard.Employee.FullName),
                    Subject = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.DisciplinaryApprovalSubjectFor, employeeCard.Employee.FullName),
                    Type = NotificationType.Information
                };
                notify.AddNotifyReceiver(new NotifyReceiver()
                {
                    Date = DateTime.Now,
                    Receiver = employeeCard.Employee.Manager().User()
                });
                return notify;
            }
            else
            {
                var newSalary = GetNewSalary(generalOption, employeeDisciplinary.DisciplinarySetting.DeductionType, employeeCard.FinancialCard.Salary, employeeDisciplinary.DisciplinarySetting.Value);//الراتب الذي يجب خصمه من البطاقة الشهرية todo
            }
            return null;
        }
        public static List<IAggregateRoot> ChangeEmployeeTerminationInfo(EmployeeTermination termination, EmployeeCard employeeCard)
        {
            var entities = new List<IAggregateRoot>();
            employeeCard.CardStatus = EmployeeCardStatus.Terminated;
            employeeCard.FinancialCard.SalaryDeservableType = SalaryDeservableType.Nothing;
            employeeCard.EndWorkingDate = termination.LastWorkingDate;
            termination.TerminationStatus = Status.Approved;
            foreach (var item in employeeCard.EmployeeCustodies)
            {
                if (item.CustodyEndDate == null)
                    item.CustodyEndDate = DateTime.Now;
            }

            var aeps = employeeCard.Employee.Positions;
            foreach (var aep in aeps.ToList())
            {
                var historyOfPrimaryPosition = employeeCard.Employee.LogOfPositions.FirstOrDefault(x => x.Position.Id == aep.Position.Id);
                aep.Position.AddPositionStatus(HRIS.Domain.JobDescription.Enum.PositionStatusType.Vacant);
                aep.Position.JobDescription.JobTitle.Vacancies++;

                var assignment =
                    ServiceFactory.ORMService.All<Assignment>()
                        .SingleOrDefault(x => x.AssigningEmployeeToPosition == aep.Position.AssigningEmployeeToPosition);
                aep.Position.AssigningEmployeeToPosition = null;

                if (assignment != null)
                    assignment.AssigningEmployeeToPosition = null;

                aep.Position.Save();
                aep.Position = null;
                aeps.Remove(aep);
                aep.Save();
                if (historyOfPrimaryPosition != null)
                {
                    historyOfPrimaryPosition.DisengagementType = HRIS.Domain.EmployeeRelationServices.Enums.DisengagementType.Termination;
                    historyOfPrimaryPosition.LeavingDate = termination.LastWorkingDate;
                    historyOfPrimaryPosition.Save();
                }
            }
            //while (termination.EmployeeCard.Employee.Positions.Any())
            //{
            //    termination.EmployeeCard.Employee.Positions.First().Position.AssigningEmployeeToPosition = null;
            //    entities.Add(termination.EmployeeCard.Employee.Positions.First().Position);
            //    termination.EmployeeCard.Employee.Positions.Remove(termination.EmployeeCard.Employee.Positions.First());
            //}

            entities.Add(employeeCard.Employee);

            return entities;
        }
        public static List<IAggregateRoot> ChangeEmployeePromotionInfo(EmployeePromotion promotion)
        {
            var entities = new List<IAggregateRoot>();
            var employeeCard = promotion.EmployeeCard;
            PositionsLogOfEmployee historyOfPrimaryPosition = null;
            var primaryPosition = promotion.EmployeeCard.Employee.Positions.SingleOrDefault(x => x.IsPrimary);

            promotion.Position.AddPositionStatus(HRIS.Domain.JobDescription.Enum.PositionStatusType.Assigned);
            promotion.Position.JobDescription.JobTitle.Vacancies--;

            if (primaryPosition != null)
            {
                primaryPosition.Position.AddPositionStatus(HRIS.Domain.JobDescription.Enum.PositionStatusType.Vacant);
                primaryPosition.Position.JobDescription.JobTitle.Vacancies++;
                historyOfPrimaryPosition = employeeCard.Employee.LogOfPositions.FirstOrDefault(x => x.Position.Id == primaryPosition.Position.Id);
            }

            var ep = primaryPosition.Position.AssigningEmployeeToPosition;
            primaryPosition.Position.AssigningEmployeeToPosition = null;
            ep.Position = promotion.Position;
            promotion.Position.AssigningEmployeeToPosition = ep;
            primaryPosition.CreationDate = DateTime.Now;
            promotion.PromotionStatus = Status.Approved;
            var positionsLogOfEmployee = new PositionsLogOfEmployee()
            {
                Position = promotion.Position,
                JobDescription = promotion.Position?.JobDescription,
                Employee = employeeCard.Employee,
                IsPrimary = ep.IsPrimary,
                AssigningDate = promotion.PositionJoiningDate,
                AssigmentType = HRIS.Domain.EmployeeRelationServices.Enums.AssigmentType.Promotion
            };
            employeeCard.Employee.AddPositionLogToEmployee(positionsLogOfEmployee);
            entities.Add(ep);
            entities.Add(employeeCard);
            entities.Add(employeeCard.Employee);
            if (historyOfPrimaryPosition != null)
            {
                historyOfPrimaryPosition.DisengagementType = HRIS.Domain.EmployeeRelationServices.Enums.DisengagementType.DisengageFromPromotion;
                historyOfPrimaryPosition.LeavingDate = promotion.PositionSeparationDate;
                historyOfPrimaryPosition.Save();
            }
            return entities;
        }
        public static List<IAggregateRoot> ChangeEmployeeResignationInfo(EmployeeResignation resignation, EmployeeCard employeeCard)
        {
            var entities = new List<IAggregateRoot>();

            resignation.ResignationStatus = Status.Approved;

            foreach (var item in employeeCard.EmployeeCustodies)
            {
                if (item.CustodyEndDate == null)
                    item.CustodyEndDate = DateTime.Now;
            }

            employeeCard.CardStatus = EmployeeCardStatus.Resigned;
            employeeCard.FinancialCard.SalaryDeservableType = SalaryDeservableType.Nothing;
            employeeCard.FinancialCard.SalaryDeservableType = SalaryDeservableType.Nothing;
            employeeCard.EndWorkingDate = resignation.LastWorkingDate;

            var aeps = resignation.EmployeeCard.Employee.Positions;
            foreach (var aep in aeps.ToList())
            {
                var historyOfPrimaryPosition = employeeCard.Employee.LogOfPositions.FirstOrDefault(x => x.Position.Id == aep.Position.Id);
                aep.Position.AddPositionStatus(HRIS.Domain.JobDescription.Enum.PositionStatusType.Vacant);
                aep.Position.JobDescription.JobTitle.Vacancies++;

                var assignment =
                    ServiceFactory.ORMService.All<Assignment>()
                        .SingleOrDefault(x => x.AssigningEmployeeToPosition == aep.Position.AssigningEmployeeToPosition);
                aep.Position.AssigningEmployeeToPosition = null;

                if (assignment != null)
                    assignment.AssigningEmployeeToPosition = null;
                aep.Position.Save();
                aep.Position = null;
                aeps.Remove(aep);
                aep.Save();
                if (historyOfPrimaryPosition != null)
                {
                    historyOfPrimaryPosition.DisengagementType = HRIS.Domain.EmployeeRelationServices.Enums.DisengagementType.Resignation;
                    historyOfPrimaryPosition.LeavingDate = resignation.LastWorkingDate;
                    historyOfPrimaryPosition.Save();
                }
            }

            entities.Add(employeeCard.Employee);

            return entities;
        }
        public static List<IAggregateRoot> ChangeEmployeeFinancialPromotionInfo(FinancialPromotion financialPromotion)
        {
            var entities = new List<IAggregateRoot>();

            financialPromotion.FinancialPromotionStatus = Status.Approved;

            switch (financialPromotion.Salary)
            {
                case SalaryType.Salary:
                    {
                        financialPromotion.EmployeeCard.FinancialCard.Salary = GetNewSalary(financialPromotion.IsPercentage, financialPromotion.EmployeeCard.FinancialCard.Salary,
                        financialPromotion.Percentage, financialPromotion.FixedValue);
                    }
                    break;
                case SalaryType.BenefitSalary:
                    {
                        financialPromotion.EmployeeCard.FinancialCard.BenefitSalary = GetNewSalary(financialPromotion.IsPercentage, financialPromotion.EmployeeCard.FinancialCard.BenefitSalary,
                        financialPromotion.Percentage, financialPromotion.FixedValue);
                    }
                    break;
                case SalaryType.InsuranceSalary:
                    {
                        financialPromotion.EmployeeCard.FinancialCard.InsuranceSalary = GetNewSalary(financialPromotion.IsPercentage, financialPromotion.EmployeeCard.FinancialCard.InsuranceSalary,
                        financialPromotion.Percentage, financialPromotion.FixedValue);
                    }
                    break;
                case SalaryType.TempSalary1:
                    {
                        financialPromotion.EmployeeCard.FinancialCard.TempSalary1 = GetNewSalary(financialPromotion.IsPercentage, financialPromotion.EmployeeCard.FinancialCard.TempSalary1,
                        financialPromotion.Percentage, financialPromotion.FixedValue);
                    }
                    break;
                default:
                    {
                        financialPromotion.EmployeeCard.FinancialCard.TempSalary2 = GetNewSalary(financialPromotion.IsPercentage, financialPromotion.EmployeeCard.FinancialCard.TempSalary2,
                        financialPromotion.Percentage, financialPromotion.FixedValue);

                    }
                    break;
            }

            entities.Add(financialPromotion.EmployeeCard);

            return entities;
        }
        public static List<IAggregateRoot> ChangeEmployeeRewardInfo(EmployeeReward employeeReward, EmployeeCard employeeCard)
        {
            var entities = new List<IAggregateRoot>();
            var setting = employeeReward.RewardSetting;
            employeeReward.RewardStatus = Status.Approved;
            var newSalary = GetNewSalary(setting.IsPercentage, employeeCard.FinancialCard.Salary, setting.Percentage, setting.FixedValue);//الراتب الذي يجب اضافته إلى البطاقة الشهرية todo
            entities.Add(employeeReward.EmployeeCard);

            return entities;
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
        public static List<IAggregateRoot> ChangeEmployeeCustodiesInfo(EmployeeCustodie custodies)
        {
            var entities = new List<IAggregateRoot>();
            //  var setting = Advance.RewardSetting;
            custodies.CustodieStatus = Status.Approved;
            custodies.CustodyStartDate = DateTime.Now;
            //  var newSalary = GetNewSalary(setting.IsPercentage, employeeCard.Salary, setting.Percentage, setting.FixedValue);//الراتب الذي يجب اضافته إلى البطاقة الشهرية todo
            entities.Add(custodies.EmployeeCard);

            return entities;
        }

        public static float GetNewSalary(GeneralOption generalOption, DeductionType deductionType, float salary, float value)
        {
            return deductionType == DeductionType.PercentageOfPackageSalary ? salary + ((salary * value) / 100) :
                deductionType == DeductionType.DaysOfPackageSalary ?
                 value * salary / (generalOption != null ? generalOption.TotalMonthDays : 0) : salary + value;
        }
        public static float GetNewSalary(bool isPercentage, float salary, float percentageValue, float fixedValue)
        {
            return isPercentage ? salary + ((salary * percentageValue) / 100) : salary + fixedValue;
        }
        public static float GetOldSalary(bool isPercentage, float salary, float percentageValue, float fixedValue)
        {
            if (isPercentage)
            {
                return (salary * 100) / (100 + percentageValue);
            }
            else
            {
                return salary - fixedValue;
            }
        }

        public static void GetLeaveInfo(LeaveRequest leaveRequest, LeaveSetting leaveSetting, Employee employee)
        {

            if (leaveSetting.IsIndivisible)
            {
                var balance = LeaveService.GetBalance(leaveSetting, employee, false, DateTime.Today);
                var recycledBalance = LeaveService.GetRecycledBalance(employee, leaveSetting, DateTime.Today.Year - 1);
                balance += recycledBalance;
                leaveRequest.SpentDays = balance;
                leaveRequest.EndDate = LeaveService.GetEndDate(leaveRequest.StartDate, balance, leaveSetting.IsContinuous, employee);
                leaveRequest.FromTime = null;
                leaveRequest.ToTime = null;
            }
            else
            {
                if (leaveRequest.IsHourlyLeave)
                {
                    var minutes = 0.00;
                    if (leaveRequest.FromTime > leaveRequest.ToTime)
                    {
                        var maxDay = new DateTime(2000, 1, 1, 23, 59, 59);
                        var minDay = new DateTime(2000, 1, 1, 0, 0, 0);
                        var minutesbefore = (maxDay.TimeOfDay - leaveRequest.FromTime.GetValueOrDefault().TimeOfDay).TotalMinutes;
                        var minutesafter = (leaveRequest.ToTime.GetValueOrDefault().TimeOfDay - minDay.TimeOfDay).TotalMinutes;
                        minutes = Math.Round(minutesafter + minutesbefore, 0);

                    }
                    else
                    {
                        minutes = (leaveRequest.ToTime.GetValueOrDefault().TimeOfDay - leaveRequest.FromTime.GetValueOrDefault().TimeOfDay).TotalMinutes;

                    }
                    var spentDays =
                        Math.Round(1 / ((leaveSetting.HoursEquivalentToOneLeaveDay * EmployeeRelationServicesConstants.NumberOfMinutesInHour) / minutes), 2);
                    leaveRequest.SpentDays = spentDays;
                }
                else
                {
                    leaveRequest.SpentDays = LeaveService.GetSpentDays(leaveRequest.StartDate, leaveRequest.EndDate,
                    leaveSetting.IsContinuous, employee);
                    leaveRequest.FromTime = null;
                    leaveRequest.ToTime = null;
                }
            }

            employee.EmployeeCard.AddLeaveRequest(leaveRequest);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { leaveRequest.WorkflowItem, employee.EmployeeCard }, UserExtensions.CurrentUser);
        }




    }
}