using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Objectives.Enums;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Validation.Specification.Objectives.Entities;
using System.Collections;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.Workflow.RootEntities;
using HRIS.Domain.Objectives.Entities;
using HRIS.Domain.Objectives.RootEntities;
using  Project.Web.Mvc4.Helpers.DomainExtensions;
using  Project.Web.Mvc4.Extensions;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;

namespace Project.Web.Mvc4.Areas.Objectives.Controllers
{
    public class ActionPlanController : Controller
    {
        //public ActionResult EvaluationService()
        //{
        //    return PartialView();
        //}

        //public ActionResult ApprovalService()
        //{
        //    return PartialView();
        //}

        //#region Evaluation

        ////Assistance class for JSON results
        //public class EvaluationResult
        //{
        //    public int ActionId { get; set; }
        //    public float Percentage { get; set; }
        //    public string Notes { get; set; }
        //}

        //[HttpPost]
        //public ActionResult Employees()
        //{
        //    var employees = GetManagerEmployees();

        //    var result = new ArrayList();
        //    foreach (var item in employees)
        //    {
        //        var temp = new Dictionary<string, object>();
        //        temp["Id"] = item.Id;
        //        temp["Name"] = item.NameForDropdown;
        //        result.Add(temp);
        //    }
        //    return Json(new { Data = result });
        //}

        //[HttpPost]
        //public ActionResult EvaluationPeriods()
        //{
        //    var evaluationPeriods = GetEvaluationPeriods();

        //    var result = new ArrayList();
        //    foreach (var item in evaluationPeriods)
        //    {
        //        var temp = new Dictionary<string, object>();
        //        temp["Id"] = item.Id;
        //        temp["Name"] = item.NameForDropdown;
        //        result.Add(temp);
        //    }
        //    return Json(new { Data = result });
        //}

        //[HttpPost]
        //public ActionResult EvaluationWorkflowObjectives(int empId, int evaluationPeriodId)
        //{
        //    //int empId = 0;
        //    var objectives = GetEvaluationWorkflowObjectives(empId,evaluationPeriodId);
        //    var result = new ArrayList();
        //    foreach (var item in objectives)
        //    {
        //        var temp = new Dictionary<string, object>();
        //        temp["Id"] = item.Id;
        //        temp["Name"] = item.Name;
        //        temp["Code"] = item.Code;
        //        temp["Type"] = item.Type;
        //        temp["Weight"] = item.Weight;
        //        temp["CreatedDate"] = item.CreationDate;
        //        result.Add(temp);
        //    }
        //    return Json(new { Data = result });
        //}

        //[HttpPost]
        //public ActionResult ObjectiveActionsInfo(int objectiveId,int evaluationPeriodId)
        //{
        //    string objectiveName = ServiceFactory.ORMService.GetById<HRIS.Domain.Objectives.RootEntities.Objective>(objectiveId).Name;
        //    float previousObjectiveEvaluationPercentage = CalculatePreviousObjectiveEvaluationPercentage(objectiveId,evaluationPeriodId);
        //    var actionsData = GetObjectiveActions(objectiveId);
        //    var result = new
        //                            {
        //                                Objective = new
        //                                                {
        //                                                    ObjectiveName = objectiveName,
        //                                                    PreviousObjectiveEvaluationPercentage = previousObjectiveEvaluationPercentage
        //                                                }
        //                                ,
        //                                Actions = new
        //                                                {
        //                                                    ActionsData = actionsData
        //                                                }
        //                            };
        //    return Json(new { Data = result });
        //}

        ////Need a db block transaction.
        //[HttpPost]
        //public ActionResult EvaluateObjective(int objectiveId,int evaluationPeriodId, IList<EvaluationResult> evaluationData,int workflowId,string description)
        //{
        //    HRIS.Domain.Objectives.RootEntities.Objective objective =
        //        ServiceFactory.ORMService.GetById<HRIS.Domain.Objectives.RootEntities.Objective>(objectiveId);
                                          
        //    foreach (var item in evaluationData)
        //    {
        //        //Get current logged employee position.
        //        var position = EmployeeExtensions.CurrentEmployee.Positions.SingleOrDefault(x => x.IsPrimary).Position;
        //        //Position position = (Position)typeof(Position).GetById(1);

        //        Evaluation evaluation = new Evaluation();
        //        evaluation.EvaluationDate = DateTime.Now;
        //        evaluation.EvaluationPercentage = item.Percentage;//dynamic attribute.
        //        evaluation.Notes = item.Notes;//dynamic attribute.
        //        evaluation.EvaluationActionPlan = objective.ActionPlans.SingleOrDefault(x=>x.Id==item.ActionId/*dynamic attribute*/);
        //        evaluation.EvaluationPeriod = ServiceFactory.ORMService.GetById<EvaluationPeriod>(evaluationPeriodId);
        //        evaluation.EvaluationPosition = position;
        //        evaluation.Save();
        //    }
            
        //    //Save workflow acceptance.
        //  //  WorkflowService.SaveObjectiveStep(workflowId, WorkflowStepStatus.Accept, description,EmployeeExtensions.CurrentEmployee.User());

        //    return Json(new { Data = "Success" });
        //}

        //#endregion

        //#region Tracking & Closing

        //[HttpPost]
        //public ActionResult TrackActionPlan(int Id/*Action plan Id*/, string actualStartDate, string actualClosingDate)
        //{
        //    var actionPlan=ServiceFactory.ORMService.GetById<ActionPlan>(Id);
        //    var startDate = DateTime.Now;
        //    var closingDate = DateTime.Now;
        //    if (actionPlan.Status != ActionPlanStatus.Pending && actionPlan.Status != ActionPlanStatus.Cancelled)//You can track it.
        //    {
        //        if (!string.IsNullOrEmpty(actualStartDate))
        //        {
        //            startDate = DateTime.Parse(actualStartDate);
        //            actionPlan.ActualStartDate = startDate;
        //            actionPlan.Status = ActionPlanStatus.InProgress;
        //        }
        //        if (!string.IsNullOrEmpty(actualClosingDate))
        //        {
        //            closingDate = DateTime.Parse(actualClosingDate);
        //            actionPlan.ActualEndDate = closingDate;
        //            actionPlan.Status = ActionPlanStatus.InProgress;
        //        }
        //        //Temperory using (SparcExpress.dll doesn't support [0] validation).
        //        if(actionPlan.PercentageOfCompletion ==0) actionPlan.PercentageOfCompletion = 1;
        //        var result=actionPlan.Save(new ActionPlanTrackingSpecification());
        //        var resultMessages = "";
        //        //Process the message view.
        //        for (int i = 0; i < result.Count; i++)
        //        {
        //            resultMessages += result[i].Message;
        //            if (i != result.Count - 1)
        //                resultMessages += "/r/n";
        //        }
        //        return Json(result.Count==0 ? new { success = "Track this action plan succeed" } : new { success = resultMessages});
        //    }
        //    return Json(new { success = "You can't track unaccepted action plan." });//You can't track it.
        //}

        //[HttpPost]
        //public ActionResult ViewActionPlanTracking(int Id/*Action plan Id*/)
        //{
        //    ActionPlan actionPlan = ServiceFactory.ORMService.GetById<ActionPlan>(Id);
        //    var data = new {actualStartDate = actionPlan.ActualStartDate.ToShortDateString(), actualClosingDate = actionPlan.ActualEndDate.ToShortDateString()};
        //    return Json(new {Data=data} );
        //}

        //[HttpPost]
        //public ActionResult CloseActionPlan(int Id/*Action plan Id*/)
        //{
        //    ActionPlan actionPlan = ServiceFactory.ORMService.GetById<ActionPlan>(Id);
        //    if (actionPlan.Status != ActionPlanStatus.Pending && actionPlan.Status != ActionPlanStatus.Cancelled)//You can close it.
        //    {
        //        actionPlan.Status = ActionPlanStatus.Closed;
        //        actionPlan.Save();
        //        return Json(new {success = "success"});
        //    }
        //    else
        //        return Json(new { success = "You can't close unaccepted action plan." });
        //}

        //#endregion

        //#region Approval

        //[HttpPost]
        //public ActionResult ApprovalPeriods()
        //{
        //    var phasePeriods = GetPhasePeriods();

        //    var result = new ArrayList();
        //    foreach (var item in phasePeriods)
        //    {
        //        var temp = new Dictionary<string, object>();
        //        temp["Id"] = item.Id;
        //        temp["Name"] = item.NameForDropdown;
        //        result.Add(temp);
        //    }
        //    return Json(new { Data = result });
        //}

        //[HttpPost]
        //public ActionResult GetObjectiveWorkflowId(int objectiveId,int phaseId,bool isEvaluation)
        //{
        //    var currentEmp = EmployeeExtensions.CurrentEmployee; //GetEmployeeByCode(User.Identity.Name);
        //    Position currentPosition = currentEmp.Positions.SingleOrDefault(x => x.IsPrimary).Position;
        //    if(isEvaluation)//Evaluation Phase workflow
        //    {
        //        List<ObjectiveEvaluationWorkflow> result =
        //            ServiceFactory.ORMService.All<ObjectiveEvaluationWorkflow>().Where(x =>
        //                                                                               x.EvaluationPeriod.Id == phaseId &&
        //                                                                               x.Objective.Id == objectiveId).ToList();
        //        var selectedWorkflow = SearchAboutWorkflowInEvaluationPeriod(result, currentEmp);
        //        if (selectedWorkflow != null)
        //            return Json(new { Data = selectedWorkflow.Id });
        //        return Json(new {Data = ""});
        //    }
        //    else
        //    {
        //        List<ObjectivePhaseWorkflow> result = ServiceFactory.ORMService.All<ObjectivePhaseWorkflow>().
        //                            Where(x => x.PhasePeriod.Id == phaseId && x.Objective.Id == objectiveId).ToList();
        //        WorkflowItem selectedWorkflow = SearchAboutWorkflowInPhasePeriod(result, currentEmp);
        //        if (selectedWorkflow != null)
        //            return Json(new { Data = selectedWorkflow.Id });
        //        return Json(new { Data = "" });
        //    }
        //}

        //[HttpPost]
        //public ActionResult ApprovalWorkflowObjectives(int phasePeriodId)
        //{
        //    //int empId = 0;
        //    var objectives = GetApprovalWorkflowObjectives(phasePeriodId);
        //    var result = new ArrayList();
        //    foreach (var item in objectives)
        //    {
        //        var temp = new Dictionary<string, object>();
        //        temp["Id"] = item.Id;
        //        temp["Name"] = item.Name;
        //        temp["Code"] = item.Code;
        //        temp["Type"] = item.Type;
        //        temp["Weight"] = item.Weight;
        //        temp["CreatedDate"] = item.CreationDate;
        //        result.Add(temp);
        //    }
        //    return Json(new { Data = result });
        //}

        //[HttpPost]
        //public ActionResult ApproveWorkflow(int workflowId,string status)
        //{
        //    var workflowItem = ServiceFactory.ORMService.GetById<WorkflowItem>(workflowId);
        //    workflowItem.Status =(WorkflowStatus)Enum.Parse(typeof (WorkflowStatus), status);
        //    workflowItem.Save();
        //    return Json(new {Data = "Success"});
        //}

        //#endregion

        //#region Assistance Methods

        //#region Workflow <-> Objective

        //private List<HRIS.Domain.Objectives.RootEntities.Objective> GetApprovalWorkflowObjectives(int phasePeriodId)
        //{
        //    //----------------------Tasks----------------------
        //    //1- Get all objectives which have a pending status with take selected phase in account.
        //    //---------------------------------------------------
        //    //Get current logged employee position.
        //    //var position  = EmployeeExtensions.CurrentEmployee.Positions.SingleOrDefault(x => x.IsPrimary).Position;
        //    //Position position = (Position)typeof(Position).GetById(1);
        //    var objectivePhaseWorkflows = ServiceFactory.ORMService.All<ObjectivePhaseWorkflow>().Where(x => x.PhasePeriod.Id == phasePeriodId).ToList();
        //    var conditionalObjectives =
        //        objectivePhaseWorkflows.Where(
        //            x =>
        //            x.Objective.ActionPlans.All(
        //                z => z.Status == ActionPlanStatus.Pending || z.Status == ActionPlanStatus.Cancelled)).Select(r=>r.Objective).ToList();
        //        //&&
        //        //                                         y.Objective.ActionPlans.All(
        //        //                                             z =>
        //        //                                             z.Status == ActionPlanStatus.Pending || z.Status == ActionPlanStatus.Cancelled))).ToList();

        //    //var conditionalObjectives= ServiceFactory.ORMService.All<HRIS.Domain.Objectives.RootEntities.Objective>().
        //    //    Where(x =>
        //    //          x.ObjectivePhaseWorkflows.Any(y => y.PhasePeriod.Id == phasePeriodId &&
        //    //                                             y.Objective.ActionPlans.All(
        //    //                                                 z =>
        //    //                                                 z.Status == ActionPlanStatus.Pending || z.Status == ActionPlanStatus.Cancelled))).ToList();

        //    //Need a performance approvment.
        //    var currentEmployee = EmployeeExtensions.CurrentEmployee;
        //    return conditionalObjectives.Where(conditionalObjective => SearchAboutWorkflowInPhasePeriod(ServiceFactory.ORMService.All<ObjectivePhaseWorkflow>().Where(x=>x.Objective.Id==conditionalObjective.Id).ToList(), currentEmployee) != null).ToList();
        //}


        //private List<HRIS.Domain.Objectives.RootEntities.Objective> GetEvaluationWorkflowObjectives(int empId, int evaluationPeriodId)
        //{
        //    //----------------------Tasks----------------------
        //    //1- If empId not selected, get all objectives which haven't a (pending && cancelled) status with take selected evaluation phase in account.
        //    //2- If empId selected, get all objectives which haven't a (pending || cancelled) status && (selected employee owned & selected employee shared) with take selected evaluation phase in account.
        //    //---------------------------------------------------
        //    if (empId == 0)//the logged user is an individual employee.(empId shouldn't be taken in account)
        //    {
        //        //Get current logged employee position.
        //        //var position  = EmployeeExtensions.CurrentEmployee.Positions.SingleOrDefault(x => x.IsPrimary).Position;
        //        //Position position = (Position)typeof(Position).GetById(1);

        //        var objectiveEvaluationWorkflow = ServiceFactory.ORMService.All<ObjectiveEvaluationWorkflow>().Where(x => x.EvaluationPeriod.Id == evaluationPeriodId).ToList();
        //        var conditionalObjectives =
        //            objectiveEvaluationWorkflow.Where(
        //                x =>
        //                x.Objective.ActionPlans.All(
        //                    z =>
        //                          z.Status != ActionPlanStatus.Pending && z.Status != ActionPlanStatus.Cancelled)).Select(r => r.Objective).ToList();


        //        //var conditionalObjectives = ServiceFactory.ORMService.All<HRIS.Domain.Objectives.RootEntities.Objective>().
        //        //    Where(x =>
        //        //          x.ObjectiveEvaluationWorkflows.Any(y => y.EvaluationPeriod.Id == evaluationPeriodId &&
        //        //                                             y.Objective.ActionPlans.All(
        //        //                                                 z =>
        //        //                                                 z.Status != ActionPlanStatus.Pending && z.Status!=ActionPlanStatus.Cancelled))).ToList();
        //        //Need a performance approvment.
        //        var currentEmployee = EmployeeExtensions.CurrentEmployee;
        //        return conditionalObjectives.Where(conditionalObjective => SearchAboutWorkflowInEvaluationPeriod(ServiceFactory.ORMService.All<ObjectiveEvaluationWorkflow>().Where(x=>x.Objective.Id==conditionalObjective.Id).ToList(), currentEmployee) != null).ToList();
        //    }
        //    else//the logged user is a manager.
        //    {
        //            var emp = ServiceFactory.ORMService.All<Employee>().SingleOrDefault(x => x.Id == empId);

        //            //var position = ServiceFactory.ORMService.All<Position>().SingleOrDefault(x => x.Employee.Id == empId);
        //        var position = emp.PrimaryPosition();

        //        var objectiveEvaluationWorkflow = ServiceFactory.ORMService.All<ObjectiveEvaluationWorkflow>().Where(x => x.EvaluationPeriod.Id == evaluationPeriodId).ToList();
        //        var filteredObjectives = objectiveEvaluationWorkflow.Where(
        //            x =>
        //            x.Objective.ActionPlans.All(z => z.Status != ActionPlanStatus.Pending && z.Status != ActionPlanStatus.Cancelled)).Select(r => r.Objective).ToList();

        //        var conditionalObjectives =
        //            filteredObjectives.Where(
        //                x => x.Owner.Id == position.Id || x.SharedWiths.Any(y => y.Position.Id == position.Id)).ToList();

        //        //var conditionalObjectives = ServiceFactory.ORMService.All<HRIS.Domain.Objectives.RootEntities.Objective>().
        //        //        Where(
        //        //            x => x.Owner.Id == position.Id
        //        //                 ||
        //        //                 x.SharedWiths.Any(y => y.Position.Id == position.Id))
        //        //        .Where(
        //        //            z => z.ObjectiveEvaluationWorkflows.Any(o => o.EvaluationPeriod.Id == evaluationPeriodId) &&
        //        //                 z.ActionPlans.All(
        //        //                     y => y.Status != ActionPlanStatus.Pending && y.Status != ActionPlanStatus.Cancelled))
        //        //        .ToList();

        //            //Need a performance approvment.
        //            var currentEmployee = EmployeeExtensions.CurrentEmployee;
        //            return conditionalObjectives.Where(conditionalObjective => SearchAboutWorkflowInEvaluationPeriod(ServiceFactory.ORMService.All<ObjectiveEvaluationWorkflow>().Where(x => x.Objective.Id == conditionalObjective.Id).ToList(), currentEmployee) != null).ToList();
        //    }
        //}


        ////private WorkflowItem SearchAboutWorkflowInPhasePeriod(List<ObjectivePhaseWorkflow> objectivePhaseWorkflows,Employee currentEmp)
        ////{
        ////    WorkflowItem selectedWorkflow = null;

        ////    foreach (var objectiveWorkflow in objectivePhaseWorkflows)
        ////    {
        ////        var firstEmplyee = objectiveWorkflow.Workflow.FirstUser.Employee();
        ////        if (firstEmplyee.Id == currentEmp.Id)
        ////        {
        ////            selectedWorkflow = objectiveWorkflow.Workflow;
        ////            break;
        ////        }
        ////        //Search in the managers.
        ////        Employee currentManager = objectiveWorkflow.Workflow.FirstUser.Employee().Manager();
        ////        if (currentManager != null)
        ////        {
        ////            for (int i = 1; i < objectiveWorkflow.Workflow.StepCount; i++)
        ////            {
        ////                if (currentManager.Id == currentEmp.Id)
        ////                {
        ////                    selectedWorkflow = objectiveWorkflow.Workflow;
        ////                    break;
        ////                }
        ////                currentManager = currentManager.Manager();
        ////            }
        ////        }
        ////    }
        ////    return selectedWorkflow;
        ////}


        //private WorkflowItem SearchAboutWorkflowInEvaluationPeriod(List<ObjectiveEvaluationWorkflow> objectiveEvaluationWorkflows, Employee currentEmp)
        //{
        //    WorkflowItem selectedWorkflow = null;

        //    foreach (var objectiveWorkflow in objectiveEvaluationWorkflows)
        //    {
        //        var firstEmplyee = objectiveWorkflow.Workflow.FirstUser.Employee();
        //        if (firstEmplyee.Id == currentEmp.Id)
        //        {
        //            selectedWorkflow = objectiveWorkflow.Workflow;
        //            break;
        //        }
        //        //Search in the managers.
        //        Employee currentManager = objectiveWorkflow.Workflow.FirstUser.Employee().Manager();
        //        if (currentManager != null)
        //        {
        //            for (int i = 1; i < objectiveWorkflow.Workflow.StepCount; i++)
        //            {
        //                if (currentManager.Id == currentEmp.Id)
        //                {
        //                    selectedWorkflow = objectiveWorkflow.Workflow;
        //                    break;
        //                }
        //                currentManager = currentManager.Manager();
        //            }
        //        }
        //    }
        //    return selectedWorkflow;
        //}

        //#endregion

        ////Get the all employees under the logged director user, Or get (nothing).
        //private IList<Employee> GetManagerEmployees()
        //{
        //    //Get current logged employee position.
        //    var currentEmp = EmployeeExtensions.CurrentEmployee; //GetEmployeeByCode(User.Identity.Name);
        //    if (currentEmp == null)
        //        return new List<Employee>();
        //    Position currentPosition = currentEmp.Positions.SingleOrDefault(x => x.IsPrimary).Position;
        //    return currentPosition.ManagerTo.Select(x=>x.Position.Employee).ToList();
        //    //return typeof(Employee).GetAll<Employee>().ToList();
        //}

        //private float CalculatePreviousObjectiveEvaluationPercentage(int objectiveId,int evaluationPeriodId)
        //{
        //    /////--------------------Tasks--------------------
        //    //1- Select objective actions.
        //    //2-  Calculate average for each action plan evaluation.
        //    //3- Calculate average for objective evaluation.
        //    //-----------------------------------------------

        //    //1- Select objective actions.
        //    var actionsPlan = typeof(ActionPlan).GetAll<ActionPlan>().Where(x => x.Objective.Id == objectiveId);
        //    float actionPlanPercentage = 0;
        //    //2-  Calculate average for each action plan evaluation.
        //    foreach (var item in actionsPlan)
        //    {
        //        List<Evaluation> eval =typeof (Evaluation).GetAll<Evaluation>().Where(x => x.EvaluationActionPlan.Id == item.Id && x.EvaluationPeriod.Id == evaluationPeriodId).ToList();
        //        List<float> percentages = eval.Select(x => x.EvaluationPercentage).ToList();
        //        if (percentages.Count != 0)
        //            actionPlanPercentage += percentages.Average();
        //    }
        //    //3- Calculate average for objective evaluation.
        //    int actionsCount = actionsPlan.Count();
        //    if (actionsCount == 0)//prevent divided on zero.
        //        return 0;
        //    return actionPlanPercentage / actionsCount;
        //}

        //private dynamic GetObjectiveActions(int objectiveId)
        //{
        //    var actions = typeof(ActionPlan).GetAll<ActionPlan>().Where(x => x.Objective.Id == objectiveId).ToList();
        //    var requiredResult = from a in actions
                                
        //                         select
        //                             new
        //                                 {
        //                                     a.Id,
                                            
        //                                     Description = a.Description != null ? a.Description : "",
                                            
        //                                 };
        //    return requiredResult;
        //}

        //private List<EvaluationPeriod> GetEvaluationPeriods()
        //{
        //    return ServiceFactory.ORMService.All<EvaluationPeriod>().ToList();
        //}

        ////private List<PhasePeriod> GetPhasePeriods()
        ////{
        ////    return ServiceFactory.ORMService.All<PhasePeriod>().ToList();
        ////}

        //#endregion
    }
}
