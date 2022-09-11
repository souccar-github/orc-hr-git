using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.Recruitment.Entities.Evaluations;
using HRIS.Domain.Recruitment.RootEntities;
using HRIS.Domain.Workflow;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models;
using Project.Web.Mvc4.Models.GridModel;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Validation;
using Souccar.Domain.Workflow.Entities;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;
using Project.Web.Mvc4.ProjectModels;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.Notification;
using Souccar.Domain.Security;

namespace Project.Web.Mvc4.Areas.Recruitment.Models
{
    public class InterviewViewModel : ViewModel
    {
        public override void CustomizeGridModel(GridViewModel model, Type type, RequestInformation requestInformation)
        {
            model.ViewModelTypeFullName = typeof(InterviewViewModel).FullName;
            model.Views[0].EditHandler = "interviewEditHandler";
        }

        public override ActionResult BeforeCreate(RequestInformation requestInformation, string customInformation = null)
        {
            var jobApplication = ServiceFactory.ORMService.GetById<JobApplication>(requestInformation.NavigationInfo.Previous[0].RowId);
            if (jobApplication != null && jobApplication.Position == null)
            {
                return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new { Data = false, message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.PositionMustBeChosenForTheRequest) });
            }

            return null;
        }

        public override void AfterInsert(RequestInformation requestInformation, Entity entity, string customInformation = null)
        {
            var interview = entity as Interview;
            var recruitmentRequest = interview.JobApplication != null ? interview.JobApplication.RecruitmentRequest : null;
            var jobDescriptionName = "";
            if (recruitmentRequest != null)
            {
                jobDescriptionName = recruitmentRequest.RequestedPosition != null
                    ? (recruitmentRequest.RequestedPosition.JobDescription != null ? recruitmentRequest.RequestedPosition.JobDescription.Name : string.Empty)
                    : string.Empty;
            }
            var workflowItem = new WorkflowItem()
            {
                Date = DateTime.Now,
                Creator = UserExtensions.CurrentUser,
                Status = WorkflowStatus.Pending,
                Type =  WorkflowType.InterviewEvaluation, 
                StepCount = 0,
                Description = string.Format("{0}-{1}", interview.JobApplication.FullName, jobDescriptionName)
            };

            workflowItem.Save();

            var interviewAppraisalSetting = interview?.InterviewAppraisalSetting;
            if (interviewAppraisalSetting != null)
            {
                var workflowSetting = interviewAppraisalSetting.WorkflowSetting;
                if (workflowSetting != null)
                {
                    var settingApprovals = workflowSetting.SettingApprovals;
                    foreach (var workflowSettingApproval in settingApprovals)
                    {
                        //Add approval
                        var approval = new WorkflowApproval()
                        {
                            Date = DateTime.Now,
                            Order = workflowSettingApproval.Order,
                            Status = WorkflowStepStatus.Pending,
                            User = workflowSettingApproval.Position.User(),
                            Workflow = workflowItem
                        };

                        workflowItem.Approvals.Add(approval);

                        //Add evaluator
                        var evaluator = new Evaluator()
                        {
                            Interview = interview,
                            Date = DateTime.Now,
                            User = workflowSettingApproval.Position.User()
                        };

                        ServiceFactory.ORMService.Save(evaluator, UserExtensions.CurrentUser);

                        foreach (var sectionWeight in interview.InterviewAppraisalTemplate.TemplateSectionWeights)
                        {
                            var section = sectionWeight.AppraisalSection;
                            var interviewCustomSection = new InterviewCustomSection()
                            {
                                Evaluator = evaluator,
                                Rate = 0,
                                Weight = sectionWeight.Weight,
                                AppraisalSection = sectionWeight.AppraisalSection
                            };

                            interviewCustomSection.Save();

                            foreach (var appraisalSectionItem in section.Items)
                            {
                                var interviewCustomSectionItem = new InterviewCustomSectionItem()
                                {
                                    Description = "",
                                    AppraisalSectionItem = appraisalSectionItem,
                                    Rate = 0,
                                    InterviewCustomSection = interviewCustomSection,
                                    Weight = appraisalSectionItem.Weight
                                };

                                interviewCustomSection.AddInterviewCustomSectionItem(interviewCustomSectionItem);
                            }

                            evaluator.AddInterviewCustomSection(interviewCustomSection);
                        }


                        interview.Evaluators.Add(evaluator);

                    }

                    ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { workflowItem }, UserExtensions.CurrentUser);

                    interview.WorkflowItem = workflowItem;
                    interview.Save();

                    var firstWorkflowApproval = workflowItem.Approvals.FirstOrDefault();
                    var body = $"{RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.YouHaveAnInterviewEvaluationFor)} {interview.JobApplication?.FullName}";
                    var title = $"{RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.YouHaveAnInterviewEvaluationFor)} {interview.JobApplication?.FullName}";
                    var destinationTabName = NavigationTabName.Strategic;
                    var destinationModuleName = ModulesNames.Recruitment;
                    var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
                        ModulesNames.ResourceGroupName + "_" + ModulesNames.Recruitment);
                    var destinationControllerName = "Recruitment/Home";
                    var destinationActionName = "ApplicantsEvaluation";
                    var destinationEntityId = "ApplicantsEvaluation";
                    var destinationEntityTitle = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.ApplicantsEvaluation);
                    var destinationEntityOperationType = OperationType.Nothing;
                    IDictionary<string, int> destinationData = new Dictionary<string, int>();
                    destinationData.Add("WorkflowId", workflowItem.Id);
                    destinationData.Add("InterviewId", interview.Id);

                    SendNotification(firstWorkflowApproval,title, body, destinationTabName, destinationModuleName,
                        destinationLocalizationModuleName,
                        destinationControllerName, destinationActionName, destinationEntityId, destinationEntityTitle,
                        destinationEntityOperationType, destinationData, workflowItem, UserExtensions.CurrentUser);

                }
            }

        }

        public override void AfterValidation(RequestInformation requestInformation, Entity entity, IDictionary<string, object> originalState,
            IList<ValidationResult> validationResults, CrudOperationType operationType, string customInformation = null,
            Entity parententity = null)
        {
            var interview = entity as Interview;
            var jobApplicationHasAnotherInterview = ServiceFactory.ORMService.All<Interview>()
                .Any(x => x.JobApplication.Id == interview.JobApplication.Id && x.Id != interview.Id);
            if (jobApplicationHasAnotherInterview)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = $"{RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.YouHaveAnInterviewEvaluationFor)} {interview?.JobApplication.FullName}",
                    Property = typeof(Interview).GetProperty("JobApplication")
                });
            }

            if (interview.InterviewEndTime <= interview.InterviewStartingTime)
            {
                validationResults.Add(new ValidationResult()
                {
                    Message = RecruitmentLocalizationHelper.GetResource(RecruitmentLocalizationHelper.InterviewEndTimeMustBeGreaterInterviewStartingTime),
                    Property = typeof(Interview).GetProperty("InterviewEndTime")
                });
            }

        }

        private void SendNotification(WorkflowApproval workflowApproval, string title, string body, string destinationTabName,
         string destinationModuleName, string destinationLocalizationModuleName, string destinationControllerName,
         string destinationActionName, string destinationEntityId, string destinationEntityTitle,
         OperationType destinationEntityOperationType, IDictionary<string, int> destinationData, WorkflowItem workflowItem, User user)
        {

            var _notification = ServiceFactory.ORMService.All<Notify>().OrderByDescending(x => x.Date)
                .FirstOrDefault(x => x.DestinationData["WorkflowId"] == workflowItem.Id);
            if (_notification != null)
            {
                foreach (var receiver in _notification.Receivers)
                    receiver.IsRead = true;
                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { _notification },
                    UserExtensions.CurrentUser);
            }

            var nextUser = workflowApproval.User;
            if (nextUser != null)
            {
                var notify = new Notify()
                {
                    Sender = user,
                    Body = body,
                    Subject = title,
                    Type = NotificationType.Request,
                    DestinationTabName = destinationTabName,
                    DestinationModuleName = destinationModuleName,
                    DestinationLocalizationModuleName = destinationLocalizationModuleName,
                    DestinationControllerName = destinationControllerName,
                    DestinationActionName = destinationActionName,
                    DestinationEntityId = destinationEntityId,
                    DestinationEntityTitle = destinationEntityTitle,
                    DestinationEntityOperationType = destinationEntityOperationType,
                    DestinationData = destinationData
                };
                notify.AddNotifyReceiver(new NotifyReceiver()
                {
                    Date = DateTime.Now,
                    Receiver = nextUser
                });

                ServiceFactory.ORMService.Save(notify, user);
            }


        }
    }
}