using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Personnel.Enums;
using HRIS.Domain.PMS.Entities;
using HRIS.Domain.Personnel.RootEntities;
using Project.Web.Mvc4.Areas.Appraisal;
using Project.Web.Mvc4.Areas.PMS.Models;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Factories;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Helpers.Resource;

using Souccar.Domain.DomainModel;
using Souccar.Domain.Notification;
using Souccar.Domain.Workflow.Entities;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Extenstions;
using Project.Web.Mvc4.Areas.PMS.Helper;
using Project.Web.Mvc4.Models.Navigation;
using HRIS.Domain.Global.Constant;
using Project.Web.Mvc4.Areas.Appraisals;
using Project.Web.Mvc4.ProjectModels;
using Souccar.Domain.Security;

namespace Project.Web.Mvc4.Areas.PMS.Controllers
{
    public class AppraisalController : Controller
    {
        private bool IsExistPosition(Employee employee, Position position)
        {
            if (employee.Positions.Count == 1)
                return true;
            else
            {
                foreach (var employeePosition in employee.Positions.Where(x => !x.IsPrimary))
                {
                    if (employeePosition == position.AssigningEmployeeToPosition)
                        return true;
                }
            }

            return false;
        }

        void GetChildren(Position position, List<Position> allPositions)
        {
            var positions = ServiceFactory.ORMService.All<Position>().ToList()
                .Where(x => x.Manager == position && x.AssigningEmployeeToPosition != null);

            if (positions != null)
            {
                allPositions.AddRange(positions);

                foreach (var pos in positions)
                {
                    GetChildren(pos, allPositions);
                }
            }
        }
        public ActionResult GetEmployeeForAppraisal()
        {
            return Json(PmsHelper.GetEmployeeForAppraisal(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult CkeckWorkflow(int workflowId)
        {


            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var currentPosition = EmployeeExtensions.CurrentEmployee.PrimaryPosition();
            var pendingType = WorkflowHelper.GetPendingType(workflow);
            var nextAppraisal = WorkflowHelper.GetNextAppraiser(workflow, out pendingType);
            if (WorkflowHelper.GetNextAppraiser(workflow, out pendingType) == currentPosition)
                return Json(true, JsonRequestBehavior.AllowGet);
            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetRow(int workflowId, int PhaseWorkflowId)
        {
            return Json(PmsHelper.GetRow(workflowId, PhaseWorkflowId), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAppraisalViewModel(int positionId, int phaseWorkflowId)
        {

            var position = ServiceFactory.ORMService.GetById<Position>(positionId);
            var appraisalPhaseWorkflow = ServiceFactory.ORMService.GetById<AppraisalPhaseWorkflow>(phaseWorkflowId);
            var template = appraisalPhaseWorkflow.AppraisalPhase.AppraisalTemplateSetting.DefaultTemplate;
            var templateAppraisalPositions = appraisalPhaseWorkflow.AppraisalPhase.AppraisalTemplateSetting.AppraisalTemplatePositions.SingleOrDefault(x => x.Position == position);
            if (templateAppraisalPositions != null)
                template = templateAppraisalPositions.AppraisalTemplate;

            var result = AppraisalViewModelFactory.Create(position, template, appraisalPhaseWorkflow.AppraisalPhase, phaseWorkflowId);
            var peningType = WorkflowHelper.GetPendingType(appraisalPhaseWorkflow.WorkflowItem);
            if (peningType == WorkflowPendingType.PendingStep)
            {
                var appraisal =
                    appraisalPhaseWorkflow.Appraisals.SingleOrDefault(x => x.Step.Status == WorkflowStepStatus.Pending);
                AppraisalViewModelFactory.UpdateViewModelFromAppraisal(appraisal, result);
            }
            if (peningType == WorkflowPendingType.NewStep)
            {
                var appraisal =
                    appraisalPhaseWorkflow.Appraisals.LastOrDefault(x => x.Step.Status == WorkflowStepStatus.Accept);
                if (appraisal != null)
                    AppraisalViewModelFactory.UpdateViewModelFromAppraisal(appraisal, result);
                
            }
            result.ShowRejectButton = appraisalPhaseWorkflow.WorkflowItem.FirstUser == UserExtensions.CurrentUser ? true : false;
            return Json(result, JsonRequestBehavior.AllowGet);
            //  return Json(new { Data = PmsHelper.GetAppraisalViewModel(positionId, phaseWorkflowId) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetApprovalViewModel(int empId, int positionId, int phaseWorkflowId)
        {
            var position = ServiceFactory.ORMService.GetById<Position>(positionId);
            var appraisalPhaseWorkflow = ServiceFactory.ORMService.GetById<AppraisalPhaseWorkflow>(phaseWorkflowId);
            var template = appraisalPhaseWorkflow.AppraisalPhase.AppraisalTemplateSetting.DefaultTemplate;
            var templateAppraisalPositions = appraisalPhaseWorkflow.AppraisalPhase.AppraisalTemplateSetting.AppraisalTemplatePositions.SingleOrDefault(x => x.Position == position);
            if (templateAppraisalPositions != null)
                template = templateAppraisalPositions.AppraisalTemplate;
            var result = AppraisalViewModelFactory.CreateApprovalViewModel(position, appraisalPhaseWorkflow, template, appraisalPhaseWorkflow.AppraisalPhase);
            return Json(result, JsonRequestBehavior.AllowGet);
            //return Json(PmsHelper.GetApprovalViewModel(empId, phaseWorkflowId) , JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetDevelopmentInformation(AppraisalViewModel appraisalViewModel)
        {
            List<DevelopmentViewModel> result = null;
            result = DevelopmentViewModel.CreateInstance(appraisalViewModel);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AcceptAppraisal(AppraisalViewModel appraisalViewModel, int phaseWorkflowId, IList<CheckedDevelopmentItemViewModel> checkedItems)
        {
            var info = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.AcceptAppraisal);
            var appraisalPhaseWorkflow = ServiceFactory.ORMService.GetById<AppraisalPhaseWorkflow>(phaseWorkflowId);
            //SaveTrainingNeed(appraisalPhaseWorkflow, checkedItems);
            var appraisalValue = (float)System.Math.Round(SaveAppraisal(appraisalViewModel.WorkflowId, phaseWorkflowId, WorkflowStepStatus.Accept, appraisalViewModel.Note, info, appraisalViewModel),2);
            var result = new Dictionary<string, object>();
            result["AppraisalValue"] = appraisalValue;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult PeningAppraisal(AppraisalViewModel appraisalViewModel, int phaseWorkflowId)
        {
            var info = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.PeningAppraisal);
            var appraisalValue = (float)System.Math.Round(SaveAppraisal(appraisalViewModel.WorkflowId, phaseWorkflowId, WorkflowStepStatus.Pending, appraisalViewModel.Note, info, appraisalViewModel), 2);
   
            var result = new Dictionary<string, object>();
            result["AppraisalValue"] = appraisalValue;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RejectAppraisal(AppraisalViewModel appraisalViewModel, int phaseWorkflowId)
        {
            var start = DateTime.Now;
            var info = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.RejectAppraisal);

            var Success = false;
            var Msg = "";
            try
            {
                var appraisalPhaseWorkflow = new AppraisalPhaseWorkflow();
                if (phaseWorkflowId != 0)
                    appraisalPhaseWorkflow = ServiceFactory.ORMService.GetById<AppraisalPhaseWorkflow>(phaseWorkflowId);
                var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(appraisalViewModel.WorkflowId);
                var Notify = ServiceFactory.ORMService.All<Notify>().Where(x => x.DestinationData["WorkflowId"] == workflow.Id).OrderByDescending(x=>x.Id).FirstOrDefault();
                ServiceFactory.ORMService.Delete(Notify, UserExtensions.CurrentUser);
                var lastUser = workflow.Steps[workflow.Steps.Count - 1].User;
                if(lastUser == UserExtensions.CurrentUser)
                {
                    var appraisal= ServiceFactory.ORMService.All<HRIS.Domain.PMS.RootEntities.Appraisal>().FirstOrDefault(x=>x.Step== workflow.Steps[workflow.Steps.Count - 1]);
                    ServiceFactory.ORMService.Delete(appraisal, UserExtensions.CurrentUser);
                    workflow.Steps.RemoveAt(workflow.Steps.Count - 1);
                }
                   
                if (workflow.Steps.Count > 0)
                {
                    workflow.Steps[workflow.Steps.Count - 1].Status = WorkflowStepStatus.Pending;
                    var body = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.TheEvaluationOf) + " " + workflow.TargetUser.FullName + " " + PMSLocalizationHelper.GetResource(PMSLocalizationHelper.NeedsAReview) + "." + PMSLocalizationHelper.GetResource(PMSLocalizationHelper.PleaseReconsiderItAndResend);
                    var title = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.TheEvaluationOf) + " " + workflow.TargetUser.FullName + " " + PMSLocalizationHelper.GetResource(PMSLocalizationHelper.NeedsAReview) + "." + PMSLocalizationHelper.GetResource(PMSLocalizationHelper.PleaseReconsiderItAndResend);
                    var destinationTabName = NavigationTabName.Strategic;
                    var destinationModuleName = ModulesNames.PMS;
                    var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
                       ModulesNames.ResourceGroupName + "_" + ModulesNames.PMS);
                    var destinationControllerName = "PMS/Home";
                    var destinationActionName = "GetEmployeesAppraisal";
                    var destinationEntityId = "GetEmployeesAppraisal";
                    var destinationEntityTitle = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.EmployeesAppraisal);
                    var destinationEntityOperationType = OperationType.Nothing;
                    IDictionary<string, int> destinationData = new Dictionary<string, int>();
                    destinationData.Add("WorkflowId", workflow.Id);
                    if (appraisalViewModel != null)
                        destinationData.Add("ServiceId", appraisalViewModel.PhaseId);
                    else
                        destinationData.Add("ServiceId", 0);
                    Notify notify = new Notify()
                    {
                        Sender = UserExtensions.CurrentUser,
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
                        Receiver = workflow.Steps[workflow.Steps.Count - 1].User
                    });
                    var entities = new List<IAggregateRoot>() { workflow, notify };
                    ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, appraisalPhaseWorkflow?.Employee, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { appraisalPhaseWorkflow?.AppraisalPhase });

                }
                Success = true;
                Msg = @GlobalResource.SuccessMessage;
            }
            catch(Exception ex)
            {
                Success = false;
                Msg = ex.Message;
            }
            return Json(new { Success,Msg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AcceptApproval(int workflowId, int phaseWorkflowId, string note)
        {
            var info = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.AcceptApproval);
            SaveAppraisal(workflowId, phaseWorkflowId, WorkflowStepStatus.Accept, note, info);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult PeningApproval(int workflowId, int phaseWorkflowId, string note)
        {
            var info = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.PeningApproval);
            SaveAppraisal(workflowId, phaseWorkflowId, WorkflowStepStatus.Pending, note, info);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public ActionResult rejectApproval(int workflowId, int phaseWorkflowId, string note)
        {
            var start = DateTime.Now;
            var info = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.RejectApproval);
            var Success = false;
            var Msg = "";
            try
            {
                var appraisalPhaseWorkflow = new AppraisalPhaseWorkflow();
                var canSendNotify = false;
                var reciever = new User();
                Notify notify = new Notify();
                if (phaseWorkflowId != 0)
                    appraisalPhaseWorkflow = ServiceFactory.ORMService.GetById<AppraisalPhaseWorkflow>(phaseWorkflowId);
                var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
                var Notify = ServiceFactory.ORMService.All<Notify>().Where(x => x.DestinationData["WorkflowId"] == workflow.Id).OrderByDescending(x => x.Id).FirstOrDefault();
                if(Notify!=null)
                    ServiceFactory.ORMService.Delete(Notify, UserExtensions.CurrentUser);
                if (workflow.Steps.Count > 0 || workflow.Approvals.Count > 0)
                {
                    var currentApproval = workflow.Approvals.FirstOrDefault(x => x.User == UserExtensions.CurrentUser);
                    if (workflow.Approvals.Count > 1 && currentApproval.Order!=1)
                    {
                        workflow.Approvals.FirstOrDefault(x => x.Order == currentApproval.Order - 1).Status = WorkflowStepStatus.Pending;
                        reciever = workflow.Approvals.FirstOrDefault(x => x.Order == currentApproval.Order - 1).User;
                        canSendNotify = true;
                    }
                    else if(workflow.Steps.Count > 0)
                    {
                        workflow.Steps[workflow.Steps.Count - 1].Status = WorkflowStepStatus.Pending;
                        reciever = workflow.Steps[workflow.Steps.Count - 1].User;
                        canSendNotify = true;
                    }
                    if (canSendNotify)
                    {
                        var body = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.TheEvaluationOf) + " " + workflow.TargetUser.FullName + " " + PMSLocalizationHelper.GetResource(PMSLocalizationHelper.NeedsAReview) + "." + PMSLocalizationHelper.GetResource(PMSLocalizationHelper.PleaseReconsiderItAndResend);
                        var title = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.TheEvaluationOf) + " " + workflow.TargetUser.FullName + " " + PMSLocalizationHelper.GetResource(PMSLocalizationHelper.NeedsAReview) + "." + PMSLocalizationHelper.GetResource(PMSLocalizationHelper.PleaseReconsiderItAndResend);
                        var destinationTabName = NavigationTabName.Strategic;
                        var destinationModuleName = ModulesNames.PMS;
                        var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
                           ModulesNames.ResourceGroupName + "_" + ModulesNames.PMS);
                        var destinationControllerName = "PMS/Home";
                        var destinationActionName = "GetEmployeesAppraisal";
                        var destinationEntityId = "GetEmployeesAppraisal";
                        var destinationEntityTitle = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.EmployeesAppraisal);
                        var destinationEntityOperationType = OperationType.Nothing;
                        IDictionary<string, int> destinationData = new Dictionary<string, int>();
                        destinationData.Add("WorkflowId", workflow.Id);
                        destinationData.Add("ServiceId", 0);
                        notify = new Notify()
                        {
                            Sender = UserExtensions.CurrentUser,
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
                            Receiver = reciever
                        });
                    }
                    var entities = new List<IAggregateRoot>() { workflow, notify };
                    if (!canSendNotify)
                    {
                        entities.Remove(notify);
                    }
                    ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, appraisalPhaseWorkflow?.Employee, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { appraisalPhaseWorkflow?.AppraisalPhase });
                    Success = true;
                    Msg = @GlobalResource.SuccessMessage;
                }
                
            }
            catch (Exception ex)
            {
                Success = false;
                Msg = ex.Message;
            }
            return Json(new { Success, Msg }, JsonRequestBehavior.AllowGet);
        }
        public float SaveAppraisal(int workflowId, int phaseWorkflowId, WorkflowStepStatus status, string note, string info, AppraisalViewModel appraisalViewModel = null)
        {
            var start = DateTime.Now;
            var appraisalPhaseWorkflow = ServiceFactory.ORMService.GetById<AppraisalPhaseWorkflow>(phaseWorkflowId);
            var workflow = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
            var pendingType = WorkflowHelper.GetPendingType(workflow);
            var user = UserExtensions.CurrentUser;
            var body=PMSLocalizationHelper.GetResource(PMSLocalizationHelper.YouHaveANotifyToEvaluate) + " " + appraisalPhaseWorkflow.Employee.FullName + " " + PMSLocalizationHelper.GetResource(PMSLocalizationHelper.AndSeeItsRatingsSoFar);
            var title = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.YouHaveANotifyToEvaluate) + " " + appraisalPhaseWorkflow.Employee.FirstName + "  " + appraisalPhaseWorkflow.Employee.LastName + " " + PMSLocalizationHelper.GetResource(PMSLocalizationHelper.AndSeeItsRatingsSoFar);
            if (status == WorkflowStepStatus.Accept && ((pendingType == WorkflowPendingType.NewStep && workflow.StepCount==(workflow.Steps.Count+1)) || (pendingType == WorkflowPendingType.PendingStep && workflow.StepCount == workflow.Steps.Count)))
            {
                body = appraisalPhaseWorkflow.Employee.FullName + " " + PMSLocalizationHelper.GetResource(PMSLocalizationHelper.HasBeenEvaluated) + "." + PMSLocalizationHelper.GetResource(PMSLocalizationHelper.PleaseCheckTheEvaluationAndReconsiderIt);
                title = appraisalPhaseWorkflow.Employee.FirstName + "  " + appraisalPhaseWorkflow.Employee.LastName + " " + PMSLocalizationHelper.GetResource(PMSLocalizationHelper.HasBeenEvaluated) + "." + PMSLocalizationHelper.GetResource(PMSLocalizationHelper.PleaseCheckTheEvaluationAndReconsiderIt);
            }
            var destinationTabName = NavigationTabName.Strategic;
            var destinationModuleName = ModulesNames.PMS;
            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
               ModulesNames.ResourceGroupName + "_" + ModulesNames.PMS);
            var destinationControllerName = "PMS/Home";
            var destinationActionName = "GetEmployeesAppraisal";
            var destinationEntityId = "GetEmployeesAppraisal";
            var destinationEntityTitle = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.EmployeesAppraisal);
            var destinationEntityOperationType = OperationType.Nothing;
            IDictionary<string, int> destinationData = new Dictionary<string, int>();
            destinationData.Add("WorkflowId", workflowId);
            if(appraisalViewModel!=null)
               destinationData.Add("ServiceId", appraisalViewModel.PhaseId);
            else
               destinationData.Add("ServiceId", 0);
            WorkflowStatus workflowStatus;
            var notify = WorkflowHelper.UpdateDefaultWorkflow(workflow, note, status, user, title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
              destinationActionName, destinationEntityId, destinationEntityTitle, destinationEntityOperationType, destinationData, out workflowStatus);
            float appraisalValue = 0;
            if (pendingType == WorkflowPendingType.NewStep)
            {
                var appraisal = new HRIS.Domain.PMS.RootEntities.Appraisal()
                {
                    Step = workflow.Steps.Last(),
                    AppraisalDate = DateTime.Now,
                    Appraiser = user.Employee().PrimaryPosition()
                };
                appraisalPhaseWorkflow.AddAppraisal(appraisal);
                appraisalValue = (float)System.Math.Round(AppraisalViewModelFactory.UpdateAppraisalFromViewModel(appraisal, appraisalViewModel),2);
            }
            else if (pendingType == WorkflowPendingType.PendingStep)
            {
                WorkflowStep step=new WorkflowStep();
                if(status==WorkflowStepStatus.Pending)
                   step = workflow.Steps.Where(x => x.User == user && x.Status == WorkflowStepStatus.Pending)
                      .OrderBy(x => x.Date)
                       .LastOrDefault();
                if (status == WorkflowStepStatus.Accept)
                    step = workflow.Steps.Where(x => x.User == user && x.Status == WorkflowStepStatus.Accept)
                       .OrderBy(x => x.Date)
                        .LastOrDefault();
                var appraisal = appraisalPhaseWorkflow.Appraisals.SingleOrDefault(x => x.Step == step);

                appraisalValue = (float)System.Math.Round(AppraisalViewModelFactory.UpdateAppraisalFromViewModel(appraisal, appraisalViewModel),2);
            }

            var entities = new List<IAggregateRoot>() { workflow, appraisalPhaseWorkflow, notify };
            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser, appraisalPhaseWorkflow.Position != null ? appraisalPhaseWorkflow.Employee : null, Souccar.Domain.Audit.OperationType.Update, info, start, new List<Entity>() { appraisalPhaseWorkflow.AppraisalPhase });


            return appraisalValue;
        }

        
        public class CheckedDevelopmentItemViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }


    }
}
