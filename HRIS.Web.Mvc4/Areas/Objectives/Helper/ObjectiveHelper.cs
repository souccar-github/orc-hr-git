using HRIS.Domain.Personnel.RootEntities;
using Project.Web.Mvc4.Areas.Objectives.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using HRIS.Domain.Objectives.RootEntities;
using Souccar.Infrastructure.Core;
using Souccar.Domain.Workflow.Enums;

using Project.Web.Mvc4.Helpers;
using Souccar.Domain.Workflow.RootEntities;
using HRIS.Domain.Objectives.Enums;
using Souccar.Domain.DomainModel;
using Project.Web.Mvc4.Helpers.Resource;
using HRIS.Domain.Objectives.Entities;
using HRIS.Domain.Workflow;
using Project.Web.Mvc4.ProjectModels;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.Notification;

namespace Project.Web.Mvc4.Areas.Objectives.Helper
{
    public static class ObjectiveHelper
    {
        public static List<ObjectiveDataViewModel> GetEmployeeObjectiveApprovalViewModel()
        {
             var phase = ObjectiveHelper.GetCurrentCreationPhase();
            var position = EmployeeExtensions.CurrentEmployee.PrimaryPosition();
            var objectives = ServiceFactory.ORMService.All<HRIS.Domain.Objectives.RootEntities.Objective>().
                Where(x => x.CreationPhase == phase && x.Status == HRIS.Domain.Objectives.Enums.ObjectiveStatus.Waiting).ToList();
            WorkflowPendingType pendingType;
            var result = new List<ObjectiveDataViewModel>();
            foreach (var obj in objectives)
            {
                if (WorkflowHelper.GetNextAppraiser(obj.CreationWorkflow, out pendingType) == position)
                {
                    result.Add(GetObjectiveDataViewModel(obj, obj.CreationWorkflow, pendingType));
                }
            }
            return result;
        }
        public static List<ObjectiveDataViewModel> GetEmployeeObjectiveTrakingViewModel()
        {
            var position = EmployeeExtensions.CurrentEmployee.PrimaryPosition();
            var objectives = ServiceFactory.ORMService.All<HRIS.Domain.Objectives.RootEntities.Objective>().
                Where(x => x.Status == ObjectiveStatus.InProcess || x.Status == ObjectiveStatus.Approved).ToList();
            var result = new List<ObjectiveDataViewModel>();
            foreach (var obj in objectives)
            {
                if (obj.Owner.Manager == position)
                {
                    result.Add(GetObjectiveDataViewModel(obj,null, null));
                }
            }
            return result;
        }
        public static List<ObjectiveDataViewModel> GetEmployeeObjectiveAppraisalViewModel()
        {
            var notify = new Notify();
            var result = new List<ObjectiveDataViewModel>();
            var phase = GetCurrentAppraisalPhase();
            if (phase == null)
                return result;
            var objectives = ServiceFactory.ORMService.All<HRIS.Domain.Objectives.RootEntities.Objective>().Where(x => x.Status == ObjectiveStatus.Finished).ToList();
            var entities = new List<IAggregateRoot>();
            foreach (var obj in objectives)
            {
                var workflow = phase.Workflows.FirstOrDefault(x => x.Objective == obj);

                if (workflow == null)
                {
                    var body = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.BodyAppraisalNotify) + " " + obj.Owner.Employee.FullName;
                    var title = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.SubjectPersonalAppraisalNotify);
                    var destinationTabName = NavigationTabName.Strategic;
                    var destinationModuleName = ModulesNames.Objectives;
                    var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
                      ModulesNames.ResourceGroupName + "_" + ModulesNames.Objectives);
                    var destinationControllerName = "Objective/Home";
                    var destinationActionName = "AppraisalService";
                    var destinationEntityId = "AppraisalService";
                    var destinationEntityTitle = ObjectiveLocalizationHelper.GetResource(ObjectiveLocalizationHelper.AppraisalService);
                    var destinationData = new Dictionary<string, int>();
                    //destinationData.Add("WorkflowId", workflowItem.Id);
                    destinationData.Add("ServiceId", phase.Id);
                    var  workflowitem = WorkflowHelper.InitWithSetting(phase.WorkflowSetting, obj.Owner.Employee.User(),
                       title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                       destinationActionName, destinationEntityId, destinationEntityTitle, OperationType.Nothing, destinationData,
                       obj.Owner.Employee.PrimaryPosition(), Souccar.Domain.Workflow.Enums.WorkflowType.Objective,
                        obj.NameForDropdown, out notify);
                    entities.Add(workflowitem);
                    phase.AddWorkflow(new ObjectiveAppraisalWorkflow()
                    {
                        Objective = obj,
                        WorkflowItem = workflowitem
                    });
                    ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser);
                    notify.DestinationData.Add("WorkflowId", workflowitem.Id);
                    entities = new List<IAggregateRoot>();
                }
            }
            entities.Add(phase);
            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, UserExtensions.CurrentUser);
            var position = EmployeeExtensions.CurrentEmployee.PrimaryPosition();
            WorkflowPendingType pendingType;

            foreach (var workflow in phase.Workflows)
            {
                if (WorkflowHelper.GetNextAppraiser(workflow.WorkflowItem, out pendingType) == position)
                {
                    result.Add(GetObjectiveDataViewModel(workflow.Objective, workflow.WorkflowItem, pendingType));
                }
            }

            return result;
        }
        
        public static ObjectiveDataViewModel GetObjectiveDataViewModel(HRIS.Domain.Objectives.RootEntities.Objective obj, WorkflowItem workflow =null,WorkflowPendingType? workflowPendingType=null)
        {
            var result = new ObjectiveDataViewModel();
            result.ObjectiveId = obj.Id;
            result.ObjectiveCode = obj.Code;
            result.ObjectiveName = obj.Name;
            if (workflow != null)
            {
                result.WorkflowPendingType =(WorkflowPendingType) workflowPendingType;
                result.PendingTypeName = LocalizationHelper.GetResourceForEnum(workflowPendingType);
                result.WorkflowId = workflow.Id;
            }
            result.EmployeeFullName = obj.Owner.NameForDropdown;
            result.CreationDate = obj.CreationDate.Date.ToString("d");
            result.PlannedStartingDate = obj.PlannedStartingDate.Date.ToString("d");
            result.PlannedClosingDate = obj.PlannedClosingDate.Date.ToString("d");
            result.Weight = obj.Weight;
            result.Description = obj.Description;
            result.Type = ServiceFactory.LocalizationService.GetResource(typeof(HRIS.Domain.Objectives.Enums.ObjectiveType).FullName + "." + obj.Type.ToString());
            result.Priority = ServiceFactory.LocalizationService.GetResource(typeof(HRIS.Domain.Objectives.Enums.Priority).FullName + "." + obj.Priority.ToString());
            result.ActionPlans = obj.ActionPlans.Select(x => new ActionPlanDataViewModel()
            {
                ActionPlanId = x.Id,
                ObjectiveId = x.Objective.Id,
                ActualEndDate = x.ActualEndDate,
                ActualStartDate = x.ActualStartDate,
                Description = x.Description,
                ExpectedResult = x.ExpectedResult,
                Owner = x.Owner.NameForDropdown,
                PercentageOfCompletion = x.PercentageOfCompletion,
                PlannedEndDate = x.PlannedEndDate,
                PlannedStartDate = x.PlannedStartDate,
                Status = x.Status,
                Mark = x.Mark,
                StatusText = LocalizationHelper.GetResourceForEnum(x.Status)
            }).ToList();
            return result;
        }

        public static ObjectiveCreationPhase GetCurrentCreationPhase()
        {
            return ServiceFactory.ORMService.All<ObjectiveCreationPhase>().Where(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now).OrderByDescending(x=>x.EndDate).FirstOrDefault();
        }
        public static ObjectiveAppraisalPhase GetCurrentAppraisalPhase()
        {
            return ServiceFactory.ORMService.All<ObjectiveAppraisalPhase>().Where(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now).OrderByDescending(x => x.EndDate).FirstOrDefault();
        }

        public static void SaveAppraisal(ObjectiveDataViewModel objectiveDataViewModel, HRIS.Domain.Objectives.RootEntities.Objective objective)
        {
            foreach (var actionPlan in objective.ActionPlans)
            {
                var actionPlanModel = objectiveDataViewModel.ActionPlans.SingleOrDefault(x => x.ActionPlanId == actionPlan.Id);
                if (actionPlanModel != null)
                    actionPlan.Mark = actionPlanModel.Mark;
            }
        }
    }
}