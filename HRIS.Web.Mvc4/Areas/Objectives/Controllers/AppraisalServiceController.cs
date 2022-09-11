
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Objectives.Enums;
using Project.Web.Mvc4.Areas.Objectives.Helper;
using Project.Web.Mvc4.Areas.Objectives.Models;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models.Navigation;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Notification;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project.Web.Mvc4.ProjectModels;
using HRIS.Domain.Objectives.Entities;

namespace Project.Web.Mvc4.Areas.Objectives.Controllers
{
    public class AppraisalServiceController : Controller
    {

        // GET: /Objective/ApprovalService/
        public ActionResult GetObjectiveForAppraisal()
        {
            return Json(new { Objectives = ObjectiveHelper.GetEmployeeObjectiveAppraisalViewModel() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetObjectiveForTracking()
        {
            return Json(new { Objectives = ObjectiveHelper.GetEmployeeObjectiveTrakingViewModel() }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AcceptAppraisal(int workflowId, ObjectiveDataViewModel objective, string note)
        {
            var start = DateTime.Now;
            var info = ObjectiveLocalizationHelper.GetResource(ObjectiveLocalizationHelper.AppraisalService) + "'" + GlobalResource.Accept + "'";
            SaveAppraisalWorkflow(workflowId, WorkflowStepStatus.Accept, objective, note, info, start);
            return Json(true);
        }

        public ActionResult PendingAppraisal(int workflowId, ObjectiveDataViewModel objective, string note)
        {
            var start = DateTime.Now;
            var info = ObjectiveLocalizationHelper.GetResource(ObjectiveLocalizationHelper.AppraisalService) + "'" + GlobalResource.Accept + "'";
            SaveAppraisalWorkflow(workflowId, WorkflowStepStatus.Pending, objective, note, info, start);
            return Json(true);
        }
        public ActionResult RejectAppraisal(int workflowId, ObjectiveDataViewModel objective, string note)
        {
            var start = DateTime.Now;
            var info = ObjectiveLocalizationHelper.GetResource(ObjectiveLocalizationHelper.AppraisalService) + "'" + GlobalResource.Accept + "'";
            SaveAppraisalWorkflow(workflowId, WorkflowStepStatus.Reject, objective, note, info, start);
            return Json(true);
        }

        public void SaveAppraisalWorkflow(int workflowId, WorkflowStepStatus status, ObjectiveDataViewModel objectiveDataViewModel, string note, string info, DateTime startTimeOfProcess)
        {
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var user = UserExtensions.CurrentUser;
            var body = ObjectiveLocalizationHelper.GetResource(ObjectiveLocalizationHelper.NotificationApprovalBody);
            var title = ObjectiveLocalizationHelper.GetResource(ObjectiveLocalizationHelper.NotificationApprovalTitle);
            var objective = ServiceFactory.ORMService.GetById<HRIS.Domain.Objectives.RootEntities.Objective>(objectiveDataViewModel.ObjectiveId);
            if (objective.Status == ObjectiveStatus.Finished)
            {
                objective.IsFinishedAndEvaluated = true;
            }
            ObjectiveHelper.SaveAppraisal(objectiveDataViewModel, objective);
            var entities = new List<IAggregateRoot>() { objective, workflow };
            WorkflowStatus workflowStatus;
            var destinationTabName = NavigationTabName.Strategic;
            var destinationModuleName = ModulesNames.Objectives;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.Objectives);
            var destinationControllerName = "Objectives/Home";
            var destinationActionName = "AppraisalService";
            var destinationEntityId = "AppraisalService";
            var destinationEntityTitle = ObjectiveLocalizationHelper.GetResource(ObjectiveLocalizationHelper.AppraisalService);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            destinationData.Add("ServiceId", objectiveDataViewModel.ObjectiveId);

            entities.Add(WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName,destinationControllerName,
               destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus));
            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, objective.Creator, Souccar.Domain.Audit.OperationType.Update, info, startTimeOfProcess, new List<Entity>() { objective });
        }

        public ActionResult GetLestOfActionPlanStatus()
        {
            var result = new List<Dictionary<string, object>>();
            var values = Enum.GetValues(typeof(ActionPlanStatus));

            foreach (var value in values)
            {
                if ((ActionPlanStatus)value == ActionPlanStatus.InProgress ||
                    (ActionPlanStatus)value == ActionPlanStatus.Accepted)
                    continue;
                var data = new Dictionary<string, object>();
                data["Name"] = value.ToString();
                data["Id"] = (int)value;
                result.Add(data);
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveTraking(ActionPlanDataViewModel model)
        {
            var start = DateTime.Now;
            var actionPlan = ServiceFactory.ORMService.GetById<ActionPlan>(model.ActionPlanId);
            actionPlan.ActualEndDate = model.ActualEndDate;
            actionPlan.ActualStartDate = model.ActualStartDate;
            actionPlan.Status = model.Status;
            actionPlan.PercentageOfCompletion = model.PercentageOfCompletion;
            var errors = ServiceFactory.ValidationService.Validate(actionPlan, null);
            actionPlan.Objective.UpdateStatusByActionPlan();
            var info = ObjectiveLocalizationHelper.GetResource(ObjectiveLocalizationHelper.SaveTracking);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { actionPlan.Objective }, UserExtensions.CurrentUser, actionPlan.Owner?.Employee, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { actionPlan, actionPlan.Objective });

            return Json(new { Success = errors.Count == 0, Errors = errors.Select(x => new { Message = x.Message, Property = x.Property.Name }).ToList() },
                JsonRequestBehavior.AllowGet);
        }
    }
}
