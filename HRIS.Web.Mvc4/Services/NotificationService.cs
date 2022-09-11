using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Objectives.Entities;
using HRIS.Domain.Objectives.Enums;
using HRIS.Domain.Objectives.RootEntities;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Domain.PMS.Entities;
using HRIS.Domain.Workflow;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models.Navigation;
using Project.Web.Mvc4.ProjectModels;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Notification;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using Souccar.NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Project.Web.Mvc4.Services
{
    /// <summary>
    /// Author:Yaseen
    /// this class to manage internal notifications 
    /// </summary>
    /// 
    public class NotificationService
    {
        public const int MaxNotify = 25;
        private NotificationService() { }
        private static NotificationService instance = new NotificationService();
        public static NotificationService Instance { get { return instance; } }

        public IList<Notify> GetSendNotifications(int senderId)
        {
            var repository = new NHibernateRepository<Notify>();
            return ServiceFactory.ORMService.All<Notify>().Where(x => x.Sender.Id == senderId).OrderByDescending(x => x.Date).ThenByDescending(x => x.Time).ToList();
        }

        /// <summary>
        /// Get all notifications with deleted.
        /// </summary>
        /// <param name="receiverId">The 'id' of user how is received the notification</param>
        /// <returns></returns>
        public IList<Notify> GetAllNotifications(int receiverId)
        {
            var repository = new NHibernateRepository<Notify>();
            return ServiceFactory.ORMService.All<Notify>().Where(x => x.Receivers.Any(y => y.Receiver.Id == receiverId)).OrderByDescending(x => x.Date).ThenByDescending(x => x.Time).ToList();
        }

        /// <summary>
        /// Get all notifications with out deleted.
        /// </summary>
        /// <param name="receiverId"></param>
        /// <returns></returns>
        public IList<Notify> GetNotifications(int receiverId)
        {
            var repository = new NHibernateRepository<Notify>();
            //to latest 25 notifications         
            var all = ServiceFactory.ORMService.All<Notify>().Where(x => x.Receivers.Any(y => y.Receiver.Id == receiverId && !y.IsDeleted)).OrderByDescending(x => x.Date).ThenByDescending(x => x.Time).ToList();
            return all.ToList().Take(MaxNotify).OrderBy(x => x.Date).ThenBy(x => x.Time).ToList();
        }

        /// <summary>
        /// Get unread notifications
        /// </summary>
        /// <param name="receiverId"></param>
        /// <returns></returns>
        public IList<Notify> GetUnreadNotifications(int receiverId)
        {
            var repository = new NHibernateRepository<Notify>();
            var notifications = ServiceFactory.ORMService.All<Notify>().Where(x => x.Receivers.Any(y => y.Receiver.Id == receiverId && !y.IsDeleted && !y.IsRead)).OrderByDescending(x => x.Date).ThenByDescending(x => x.Time).ToList();
            return notifications;
        }

        //public void UpdateNotifications(int receiverId)
        //{
        //    var workflowItems = ServiceFactory.ORMService.All<WorkflowItem>().Where(x => x.FirstUser.Id == receiverId && x.Status == WorkflowStatus.Pending);

        //    foreach (var workflowItem in workflowItems)
        //    {
        //        bool okToAdd = false;
        //        var title = string.Empty;
        //        var body = string.Empty;
        //        var destinationTabName = string.Empty;
        //        var destinationModuleName = string.Empty;
        //        var destinationLocalizationModuleName = string.Empty;
        //        var destinationControllerName = string.Empty;
        //        var destinationActionName = string.Empty;
        //        var destinationEntityId = string.Empty;
        //        var destinationEntityTitle = string.Empty;
        //        var destinationData = new Dictionary<string, int>();

        //        if (workflowItem.Type ==Souccar.Domain.Workflow.Enums.WorkflowType.LeaveRequest)
        //        {
        //            var leaveRequest = ServiceFactory.ORMService.All<LeaveRequest>().FirstOrDefault(x => x.WorkflowItem.Id == workflowItem.Id);
        //            var data = ServiceFactory.ORMService.All<Notify>().FirstOrDefault(x => x.DestinationData["WorkflowId"] == workflowItem.Id);
        //            if (data == null && leaveRequest!=null)
        //            {
        //                okToAdd = true;
        //                title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveALeaveRequestFor) + " " + workflowItem.TargetUser.FullName;
        //                body = string.Format("{0} {1} {2}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.YouHaveALeaveRequestFor), workflowItem.TargetUser.FullName, EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PleaseCheckItOut));
        //                destinationTabName = NavigationTabName.Operational;
        //                destinationModuleName = ModulesNames.EmployeeRelationServices;
        //                destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
        //                  ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
        //                destinationControllerName = "EmployeeRelationServices/Service";
        //                destinationActionName = "EmployeeLeaveRequest";
        //                destinationEntityId = "EmployeeLeaveRequest";
        //                destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.ApproveLeave);
        //                destinationData.Add("WorkflowId", workflowItem.Id);
        //                destinationData.Add("ServiceId", leaveRequest.Id);
        //            }
        //        }

        //        else if (workflowItem.Type ==Souccar.Domain.Workflow.Enums.WorkflowType.EmployeeReward)
        //        {
        //            var employeeReward = ServiceFactory.ORMService.All<EmployeeReward>().FirstOrDefault(x => x.WorkflowItem.Id == workflowItem.Id);
        //            var data = ServiceFactory.ORMService.All<Notify>().FirstOrDefault(x => x.DestinationData["WorkflowId"] == workflowItem.Id);
        //            if (data == null && employeeReward!=null)
        //            {
        //                okToAdd = true;
        //                body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.RewardApprovalBody) + " " 
        //                    + workflowItem.TargetUser.FullName;
        //                title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.RewardApprovalSubjectFor);
        //                destinationTabName = NavigationTabName.Operational;
        //                destinationModuleName = ModulesNames.EmployeeRelationServices;
        //                destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
        //                  ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
        //                destinationControllerName = "EmployeeRelationServices/Service";
        //                destinationActionName = "RewardRequest";
        //                destinationEntityId = "RewardRequest";
        //                destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.RewardRequest);
        //                destinationData.Add("WorkflowId", workflowItem.Id);
        //                destinationData.Add("ServiceId", employeeReward.Id);

        //            }
        //        }
        //        else if (workflowItem.Type == Souccar.Domain.Workflow.Enums.WorkflowType.EmployeeDisciplinary)
        //        {
        //            var employeeDisciplinary = ServiceFactory.ORMService.All<EmployeeDisciplinary>().FirstOrDefault(x => x.WorkflowItem.Id == workflowItem.Id);
        //            var data = ServiceFactory.ORMService.All<Notify>().FirstOrDefault(x => x.DestinationData["WorkflowId"] == workflowItem.Id);
        //            if (data == null && employeeDisciplinary!=null)
        //            {
        //                okToAdd = true;
        //                body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.DisciplinaryNotifyBody) + " "
        //                    + workflowItem.TargetUser.FullName;
        //                title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.DisciplinaryApprovalSubjectFor);
        //                destinationTabName = NavigationTabName.Operational;
        //                destinationModuleName = ModulesNames.EmployeeRelationServices;
        //                destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
        //                  ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
        //                destinationControllerName = "EmployeeRelationServices/Service";
        //                destinationActionName = "DisciplinaryRequest";
        //                destinationEntityId = "DisciplinaryRequest";
        //                destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.DisciplinaryRequest);
        //                destinationData.Add("ServiceId", employeeDisciplinary.Id);
        //                destinationData.Add("WorkflowId", workflowItem.Id);
        //            }
        //        }
        //        else if (workflowItem.Type ==Souccar.Domain.Workflow.Enums.WorkflowType.EmployeeFinancialPromotion)
        //        {
        //            var employeeFinancialPromotion = ServiceFactory.ORMService.All<FinancialPromotion>().FirstOrDefault(x => x.WorkflowItem.Id == workflowItem.Id);
        //            var data = ServiceFactory.ORMService.All<Notify>().FirstOrDefault(x => x.DestinationData["WorkflowId"] == workflowItem.Id);
        //            if (data == null && employeeFinancialPromotion!=null)
        //            {
        //                okToAdd = true;
        //                body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.FinancialPromotionApprovalBody) + " " 
        //                    + workflowItem.TargetUser.Username;
        //                title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.FinancialPromotionApprovalSubjectFor);
        //                destinationTabName = NavigationTabName.Operational;
        //                destinationModuleName = ModulesNames.EmployeeRelationServices;
        //                destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
        //                  ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
        //                destinationControllerName = "EmployeeRelationServices/Service";
        //                destinationActionName = "FinancialPromotionRequest";
        //                destinationEntityId = "FinancialPromotionRequest";
        //                destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.FinancialPromotionRequest);
        //                destinationData.Add("WorkflowId", workflowItem.Id);
        //                destinationData.Add("ServiceId", employeeFinancialPromotion.Id);
        //            }
        //        }
        //        else if (workflowItem.Type ==Souccar.Domain.Workflow.Enums.WorkflowType.EmployeePromotion)
        //        {
        //            var employeePromotion = ServiceFactory.ORMService.All<EmployeePromotion>().FirstOrDefault(x => x.WorkflowItem.Id == workflowItem.Id);
        //            var data = ServiceFactory.ORMService.All<Notify>().FirstOrDefault(x => x.DestinationData["WorkflowId"] == workflowItem.Id);
        //            if (data == null && employeePromotion!=null)
        //            {
        //                okToAdd = true;
        //                body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PromotionApprovalBody) + " " 
        //                    + workflowItem.TargetUser.FullName;
        //                title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PromotionApprovalSubjectFor);
        //                destinationTabName = NavigationTabName.Operational;
        //                destinationModuleName = ModulesNames.EmployeeRelationServices;
        //                destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
        //                  ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
        //                destinationControllerName = "EmployeeRelationServices/Service";
        //                destinationActionName = "PromotionRequest";
        //                destinationEntityId = "PromotionRequest";
        //                destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.PromotionRequest);
        //                destinationData.Add("WorkflowId", workflowItem.Id);
        //                destinationData.Add("ServiceId", employeePromotion.Id);
        //            }
        //        }
        //        else if (workflowItem.Type ==Souccar.Domain.Workflow.Enums.WorkflowType.EmployeeResignation)
        //        {
        //            var employeeResignation = ServiceFactory.ORMService.All<EmployeeResignation>().FirstOrDefault(x => x.WorkflowItem.Id == workflowItem.Id);
        //            var data = ServiceFactory.ORMService.All<Notify>().FirstOrDefault(x => x.DestinationData["WorkflowId"] == workflowItem.Id);
        //            if (data == null && employeeResignation!=null)
        //            {
        //                okToAdd = true;
        //                body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.ResignationApprovalBody) + " " 
        //                    + workflowItem.TargetUser.FullName;
        //                title = string.Format("{0} {1}", EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.ResignationApprovalSubjectFor), workflowItem.TargetUser.FullName);
        //                destinationTabName = NavigationTabName.Operational;
        //                destinationModuleName = ModulesNames.EmployeeRelationServices;
        //                destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
        //                  ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
        //                destinationControllerName = "EmployeeRelationServices/Service";
        //                destinationActionName = "ResignationRequest";
        //                destinationEntityId = "ResignationRequest";
        //                destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.ResignationRequest);
        //                destinationData.Add("WorkflowId", workflowItem.Id);
        //                destinationData.Add("ServiceId", employeeResignation.Id);
        //            }
        //        }
        //        else if (workflowItem.Type == Souccar.Domain.Workflow.Enums.WorkflowType.EmployeeTermination)
        //        {
        //            var employeeTermination = ServiceFactory.ORMService.All<EmployeeTermination>().FirstOrDefault(x => x.WorkflowItem.Id == workflowItem.Id);
        //            var data = ServiceFactory.ORMService.All<Notify>().FirstOrDefault(x => x.DestinationData["WorkflowId"] == workflowItem.Id);
        //            if (data == null && employeeTermination!=null)
        //            {
        //                okToAdd = true;
        //                body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TerminationApprovalBody) + " " 
        //                    + workflowItem.TargetUser.FullName;
        //                title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TerminationApprovalSubjectFor);
        //                destinationTabName = NavigationTabName.Operational;
        //                destinationModuleName = ModulesNames.EmployeeRelationServices;
        //                destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
        //                  ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
        //                destinationControllerName = "EmployeeRelationServices/Service";
        //                destinationActionName = "TerminationRequest";
        //                destinationEntityId = "TerminationRequest";
        //                destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.TerminationRequest);
        //                destinationData.Add("WorkflowId", workflowItem.Id);
        //                destinationData.Add("ServiceId", employeeTermination.Id);
        //            }
        //        }
        //        else if (workflowItem.Type == Souccar.Domain.Workflow.Enums.WorkflowType.EmployeeAdvance)
        //        {
        //            var employeeAdvance = ServiceFactory.ORMService.All<EmployeeAdvance>().FirstOrDefault(x => x.WorkflowItem.Id == workflowItem.Id);
        //            var data = ServiceFactory.ORMService.All<Notify>().FirstOrDefault(x => x.DestinationData["WorkflowId"] == workflowItem.Id);
        //            if (data == null && employeeAdvance != null)
        //            {
        //                okToAdd = true;
        //                body = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AdvanceApprovalBody) + " "
        //                    + workflowItem.TargetUser.FullName;
        //                title = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.AdvanceApprovalSubjectFor);
        //                destinationTabName = NavigationTabName.Operational;
        //                destinationModuleName = ModulesNames.EmployeeRelationServices;
        //                destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
        //                  ModulesNames.ResourceGroupName + "_" + ModulesNames.EmployeeRelationServices);
        //                destinationControllerName = "EmployeeRelationServices/Service";
        //                destinationActionName = "EmployeeAdvanceRequest";
        //                destinationEntityId = "EmployeeAdvanceRequest";
        //                destinationEntityTitle = EmployeeRelationServicesLocalizationHelper.GetResource(EmployeeRelationServicesLocalizationHelper.EmployeeAdvanceRequest);
        //                destinationData.Add("WorkflowId", workflowItem.Id);
        //                destinationData.Add("ServiceId", employeeAdvance.Id);
        //            }
        //        }
        //        /*  else if (workflowItem.Type ==Souccar.Domain.Workflow.Enums.WorkflowType.Incentive)
        //          {
        //              var incentivePhaseWorkflow = ServiceFactory.ORMService.All<IncentivePhaseWorkflow>().FirstOrDefault(x => x.Workflow.Id == workflowItem.Id);
        //              var data = ServiceFactory.ORMService.All<Notify>().FirstOrDefault(x => x.DestinationData["WorkflowId"] == workflowItem.Id);
        //              if (data == null && incentivePhaseWorkflow!=null)
        //              {
        //                  okToAdd = true;
        //                  body = IncentiveLocalizationHelper.GetResource(IncentiveLocalizationHelper.BodyIncentiveAppraisalNotify) + " " + workflowItem.TargetUser.FullName;
        //                  title = IncentiveLocalizationHelper.GetResource(IncentiveLocalizationHelper.SubjectIncentiveAppraisalNotify);
        //                  destinationTabName = NavigationTabName.Strategic;
        //                  destinationModuleName = ModulesNames.Incentive;
        //                  destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
        //                    ModulesNames.ResourceGroupName + "_" + ModulesNames.Incentive);
        //                  destinationControllerName = "Incentive/Home";
        //                  destinationActionName = "IncentiveAppraisalService";
        //                  destinationEntityId = "IncentiveAppraisalService";
        //                  destinationEntityTitle = IncentiveLocalizationHelper.GetResource(IncentiveLocalizationHelper.IncentiveAppraisal);
        //                  destinationData.Add("WorkflowId", workflowItem.Id);
        //                  destinationData.Add("ServiceId", incentivePhaseWorkflow.Phase.Id);
        //              }
        //          }*/
        //        else if (workflowItem.Type ==Souccar.Domain.Workflow.Enums.WorkflowType.Appraisal)
        //        {
        //            var appraisalPhaseWorkflow = ServiceFactory.ORMService.All<AppraisalPhaseWorkflow>().FirstOrDefault(x => x.WorkflowItem.Id == workflowItem.Id);
                    

        //            var data = ServiceFactory.ORMService.All<Notify>().FirstOrDefault(x => x.DestinationData["WorkflowId"] == workflowItem.Id);
        //            if (data != null)
        //            {
        //                var Alldata = ServiceFactory.ORMService.All<Notify>().Where(x => x.Body == data.Body && x.DestinationTabName == data.DestinationTabName && x.Subject == data.Subject).ToList();
        //                foreach (var notify in Alldata)
        //                {
        //                    if (data.Id != notify.Id && notify.Id!=0)
        //                    {
        //                        notify.IsVertualDeleted = true;
        //                        try
        //                        {
        //                            notify.Receivers.SingleOrDefault(x => x.Receiver.Id == UserExtensions.CurrentUser.Id).IsDeleted = true;
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            Console.WriteLine(ex.ToString());
        //                        }
                                   
        //                        ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>(){notify}, UserExtensions.CurrentUser);
        //                    }

        //                }

        //            }
                    
        //            if (data == null && appraisalPhaseWorkflow!=null)
        //            {
        //                okToAdd = true;
        //                body = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.YouHaveANotifyToEvaluate) + " " + workflowItem.TargetUser.FullName;
        //                title = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.YouHaveANotifyToEvaluate) + " " + workflowItem.TargetUser.FullName;
        //                destinationTabName = NavigationTabName.Strategic;
        //                destinationModuleName = ModulesNames.PMS;
        //                destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
        //                  ModulesNames.ResourceGroupName + "_" + ModulesNames.PMS);
        //                destinationControllerName = "PMS/Home";
        //                destinationActionName = "GetEmployeesAppraisal";
        //                destinationEntityId = "GetEmployeesAppraisal";
        //                destinationEntityTitle = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.EmployeesAppraisal);
        //                destinationData.Add("WorkflowId", workflowItem.Id);
        //                destinationData.Add("ServiceId", appraisalPhaseWorkflow.AppraisalPhase.Id);
        //            }
        //        }
        //        else 
        //            if (workflowItem.Type == Souccar.Domain.Workflow.Enums.WorkflowType.Objective)
        //        {

                   
        //            var objectiveAppraisalPhase = ServiceFactory.ORMService.All<HRIS.Domain.Objectives.Entities.ObjectiveAppraisalWorkflow>().
        //                FirstOrDefault(x => x.WorkflowItem.Id == workflowItem.Id);
        //            if (objectiveAppraisalPhase != null)
        //            {
        //                var data = ServiceFactory.ORMService.All<Notify>().FirstOrDefault(x => x.DestinationData["WorkflowId"] == workflowItem.Id);
        //                if (data == null)
        //                {
        //                    okToAdd = true;
        //                    body = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.BodyAppraisalNotify) + " " + workflowItem.TargetUser.FullName;
        //                    title = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.SubjectPersonalAppraisalNotify);
        //                    destinationTabName = NavigationTabName.Strategic;
        //                    destinationModuleName = ModulesNames.Objectives;
        //                    destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
        //                      ModulesNames.ResourceGroupName + "_" + ModulesNames.Objectives);
        //                    destinationControllerName = "Objective/Home";
        //                    destinationActionName = "AppraisalService";
        //                    destinationEntityId = "AppraisalService";
        //                    destinationEntityTitle = ObjectiveLocalizationHelper.GetResource(ObjectiveLocalizationHelper.AppraisalService);
        //                    destinationData.Add("WorkflowId", workflowItem.Id);
        //                    destinationData.Add("ServiceId", objectiveAppraisalPhase.Objective.Id);
        //                }
        //            }
        //        }
        //        if (okToAdd)
        //        {
        //            var notify = new Notify()
        //            {
        //                Sender = UserExtensions.CurrentUser,
        //                DestinationEntityOperationType = OperationType.Nothing,
        //                Subject = body,
        //                Body = body,
        //                DestinationActionName = destinationActionName,
        //                DestinationControllerName = destinationControllerName,
        //                DestinationEntityId = destinationEntityId,
        //                DestinationData = destinationData,
        //                DestinationEntityTitle = destinationEntityTitle,
        //                DestinationTabName = destinationTabName,
        //                DestinationModuleName = destinationModuleName,
        //                DestinationLocalizationModuleName = destinationLocalizationModuleName,
        //                Type = NotificationType.Request
        //            };

        //            notify.AddNotifyReceiver(new NotifyReceiver()
        //            {
        //                Date = DateTime.Now,
        //                Receiver = workflowItem.FirstUser
        //            });
        //            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, UserExtensions.CurrentUser);
        //        }

        //    }
        //}
    }
}