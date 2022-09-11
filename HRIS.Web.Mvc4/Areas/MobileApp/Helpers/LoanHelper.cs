using DevExpress.XtraRichEdit.Model;
using HRIS.Domain.AttendanceSystem.RootEntities;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Global.Enums;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Workflow;
using HRIS.Web.Mvc4.Areas.EmployeeRelationServices.Models;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Helper;
using Project.Web.Mvc4.Areas.EmployeeRelationServices.Models;
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
    public class LoanHelper
    {
        public static string saveLoanRequest(Employee emp, EmployeeLoanRequestViewModel employeeLoanItem,User user,int locale)
        {
            var posistion = ServiceFactory.ORMService.All<AssigningEmployeeToPosition>().FirstOrDefault(x => x.Employee == emp);
            employeeLoanItem.EmployeeId = emp.Id;
            employeeLoanItem.FullName = emp.FullName;
            employeeLoanItem.PositionId = posistion.Id;
            employeeLoanItem.PositionName = posistion.Position.NameForDropdown;
            employeeLoanItem.Description = employeeLoanItem.Description ?? "";

            return SaveLoanRequestItem(emp.Id, posistion.Id, employeeLoanItem,user);
        }
        public static string SaveLoanRequestItem(int employeeId, int positionId, EmployeeLoanRequestViewModel loanRequestItem,User user)
        {
            var start = DateTime.Now;
            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            var position = ServiceFactory.ORMService.GetById<Position>(positionId);
            if (employee.EmployeeCard == null)
                return EmployeeRelationServicesLocalizationHelper.MsgEmployeeCardNotExist;
            var FirstRepresentative = ServiceFactory.ORMService.GetById<Employee>(loanRequestItem.FirstRepresentative);
            var SecondRepresentative = ServiceFactory.ORMService.GetById<Employee>(loanRequestItem.SecondRepresentative);
            var empCard = ServiceFactory.ORMService.All<EmployeeCard>().FirstOrDefault(x => x.Employee == employee);

            var request = new EmployeeLoan()
            {
                Creator = user,
                EmployeeCard = empCard,
                Note = loanRequestItem.Note,
                RequestStatus = Status.Draft,
                RequestDate = loanRequestItem.RequestDate,
                Date = loanRequestItem.RequestDate,
                FirstRepresentativeEmployee = FirstRepresentative,
                SecondRepresentativeEmployee = SecondRepresentative,
                PaymentsCount = loanRequestItem.PaymentsCount,
                MonthlyInstalmentValue = loanRequestItem.TotalAmount / loanRequestItem.PaymentsCount,
                TotalAmountOfLoan = loanRequestItem.TotalAmount,
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
                return EmployeeRelationServicesLocalizationHelper.MsgEmployeeIsNotOnHeadOfHisWork;
            }
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
            var destinationEntityOperationType = Souccar.Domain.Notification.OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            var notify = new Notify();
            var workflowItem = Project.Web.Mvc4.Helpers.WorkflowHelper.InitWithSetting(workflowSetting, employee.User(),
                title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                destinationActionName, destinationEntityId, destinationEntityTitle
                , destinationEntityOperationType, destinationData,
                position, WorkflowType.EmployeeLoanRequest, EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.LoanRequestRepresentative) + " - " + request.RequestDate + " - " + request.Note, out notify, true);
            request.WorkflowItem = workflowItem;
            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EmployeeLoanRequest);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, request }, user, request, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { request.EmployeeCard.Employee });

            notify.DestinationData.Add("WorkflowId", workflowItem.Id);
            notify.DestinationData.Add("ServiceId", request.Id);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, user);
            new PushNotification(title, body, notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");

            return string.Empty;
        }

        public static EmployeeLoanRequestViewModel getLoanByWorkflow(int id)
        {
            var result = new EmployeeLoanRequestViewModel();
            var loan =
                 ServiceFactory.ORMService.All<EmployeeLoan>()
                 .FirstOrDefault(x => x.WorkflowItem.Id == id);
            WorkflowPendingType pendingType;
            Project.Web.Mvc4.Helpers.WorkflowHelper.GetNextAppraiser(loan.WorkflowItem, out pendingType);
            result = new EmployeeLoanRequestViewModel()
            {
                EmployeeId = loan.EmployeeCard.Employee.Id,
                FullName = loan.EmployeeCard.Employee.FullName,
                RequestId = loan.Id,
                Description = loan.Note ?? string.Empty,
                WorkflowItemId = loan.WorkflowItem.Id,
                TotalAmount = loan.TotalAmountOfLoan,
                Note = loan.Note,
                PendingType = pendingType,
                PositionId = loan.EmployeeCard.Employee.GetSecondaryPositionElsePrimary().Id,
                PositionName = loan.EmployeeCard.Employee.GetSecondaryPositionElsePrimary().NameForDropdown,
                FirstRepresentative = loan.FirstRepresentativeEmployee.Id,
                SecondRepresentative = loan.SecondRepresentativeEmployee.Id,
                FirstRepresentativeName = loan.FirstRepresentative,
                SecondRepresentativeName = loan.SecondRepresentative,
                MonthlyInstallment = loan.MonthlyInstalmentValue,
                PaymentsCount = loan.PaymentsCount,
                RequestDate = loan.RequestDate??DateTime.Now
            };
            return result;
        }

        public static void SaveLoanRequestWorkflow(int workflowId, EmployeeLoan loan, WorkflowStepStatus status, string note,User user)
        {
            var start = DateTime.Now;
            var entities = new List<IAggregateRoot>();
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
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
            var destinationEntityOperationType = Souccar.Domain.Notification.OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", loan.Id);
            var recordDate = loan.Date.ToString("dd / MM / yyyy");
            var strWhenCompleted = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YourLoanRequestHasBeenApprovedWhichDate), recordDate);
            var strWhenCanceled = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YourLoanRequestHasBeenRejectedWhichDate), recordDate);
            Notify notify = null;
            if (loan.WorkflowItem.Description.StartsWith(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.LoanRequestRepresentative)))
            {
                notify = Project.Web.Mvc4.Helpers.WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
              destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus, strWhenCompleted, strWhenCompleted, strWhenCanceled, strWhenCanceled, false);
            }
            else
            {
                notify = Project.Web.Mvc4.Helpers.WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
              destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus, strWhenCompleted, strWhenCompleted, strWhenCanceled, strWhenCanceled);
            }

            if (notify != null)
            {
                entities.Add(notify);
            }

            if (workflowStatus == WorkflowStatus.Completed)
            {
                if (loan.WorkflowItem.Description.StartsWith(EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.LoanRequestRepresentative)))
                {
                    var requestItem = new EmployeeLoanRequestViewModel();
                    var emp = ServiceFactory.ORMService.All<Employee>().FirstOrDefault(x => x.Id == loan.EmployeeCard.Employee.Id);
                    var posistion = ServiceFactory.ORMService.All<AssigningEmployeeToPosition>().FirstOrDefault(x => x.Employee == emp);
                    requestItem.EmployeeId = emp.Id;
                    requestItem.FullName = emp.FullName;
                    requestItem.PositionId = posistion.Id;
                    requestItem.PositionName = posistion.Position.NameForDropdown;
                    requestItem.Note = loan.Note ?? "";
                    requestItem.FirstRepresentative = loan.FirstRepresentativeEmployee.Id;
                    requestItem.SecondRepresentative = loan.SecondRepresentativeEmployee.Id;
                    requestItem.RequestDate = loan.Date;
                    requestItem.RequestId = loan.Id;
                    requestItem.WorkflowItemId = loan.WorkflowItem.Id;
                    requestItem.TotalAmount = loan.TotalAmountOfLoan;
                    requestItem.PaymentsCount = loan.PaymentsCount;
                    requestItem.MonthlyInstallment = loan.MonthlyInstalmentValue;

                    SaveSecLoanRequestItem(loan.EmployeeCard.Employee.Id, loan.EmployeeCard.Employee.Positions.FirstOrDefault(x => x.IsPrimary).Position.Id, requestItem,user);
                }
                else
                {
                    loan.RequestStatus = Status.Approved;
                }
            }
            else if (workflowStatus == WorkflowStatus.Canceled)
            {
                loan.RequestStatus = Status.Rejected;
            }

            entities.Add(loan);

            var info = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AcceptEntraceExitRecord);
            var info_type = status == WorkflowStepStatus.Accept ? LocalizationHelper.GetResource(LocalizationHelper.Accept)
                : (status == WorkflowStepStatus.Reject ? LocalizationHelper.GetResource(LocalizationHelper.Reject)
                : LocalizationHelper.GetResource(LocalizationHelper.Pending));

            ServiceFactory.ORMService.SaveTransaction(entities, user, loan, Souccar.Domain.Audit.OperationType.Update, info + " '" + info_type + "' ", start, new List<Entity>() { loan.EmployeeCard.Employee });
            new PushNotification(title, body, notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");
        }

        public static string SaveSecLoanRequestItem(int employeeId, int positionId, EmployeeLoanRequestViewModel RequestItem, User user)
        {
            var start = DateTime.Now;
            var employee = ServiceFactory.ORMService.GetById<Employee>(employeeId);
            var position = ServiceFactory.ORMService.GetById<Position>(positionId);
            var generalSetting = ServiceFactory.ORMService.All<GeneralEmployeeRelationSetting>().FirstOrDefault();
            if (generalSetting == null || generalSetting.EmployeeLoanRequest == null)
                return EmployeeRelationServicesLocalizationHelper.MsgWorkFlowSettingsNotExist;
            if (employee.EmployeeCard == null)
                return EmployeeRelationServicesLocalizationHelper.MsgEmployeeCardNotExist;
            var FirstRepresentative = ServiceFactory.ORMService.GetById<Employee>(RequestItem.FirstRepresentative);
            var SecondRepresentative = ServiceFactory.ORMService.GetById<Employee>(RequestItem.SecondRepresentative);

            var employeeAttendanceCard = typeof(EmployeeCard).GetAll<EmployeeCard>().FirstOrDefault(x => x.Employee.Id == employee.Id);
            //اختبار الموظف على راس عمله ومطالب بالدوام
            if (!(employeeAttendanceCard.CardStatus == EmployeeCardStatus.OnHeadOfHisWork && employeeAttendanceCard.AttendanceDemand))
            {
                return EmployeeRelationServicesLocalizationHelper.MsgEmployeeIsNotOnHeadOfHisWork;
            }
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
            var destinationEntityOperationType = Souccar.Domain.Notification.OperationType.Nothing;
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
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem, request },user, request, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { request.EmployeeCard.Employee });

            notify.DestinationData.Add("WorkflowId", workflowItem.Id);
            notify.DestinationData.Add("ServiceId", request.Id);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, user);
            new PushNotification(title, body, notify.Receivers.FirstOrDefault().Receiver.FCMToken ?? "");

            return string.Empty;
        }

        public static void SavePSLoanWorkflow(int workflowId, int loanId, WorkflowStepStatus status, string note, User user, int locale)
        {
            var loan = ServiceFactory.ORMService.GetById<EmployeeLoan>(loanId);
            SaveLoanRequestWorkflow(workflowId, loan, status, note, user);

        }
      
    }

}