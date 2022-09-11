
//using System.Web.Mvc;

//namespace Project.Web.Mvc4.Controllers
//{
//    /// <summary>
//    /// Author: Yaseen Alrefaee
//    /// </summary>
//    public class WorkflowController : Controller
//    {
//        public ActionResult Index()
//        {
//            return View();
//        }

//        //public ActionResult GetWorkflow(int workflowId)
//        //{
//        //    var workflow = ((WorkflowItem) (typeof (WorkflowItem).GetById(workflowId)));
//        //    var workflowViewModel = new WrokflowViewModel(workflow);
//        //    var eventHandlerName = string.Format("HRIS.Web.Mvc4.Models.Workflow.EventHandler.{0}WorkflowEventHandler"
//        //        , workflow.Type.ToString());
//        //    var eventHandlerType = eventHandlerName.ToType();
//        //    if (eventHandlerType != null)
//        //    {
//        //        var handler = (WorkflowEventHandler) eventHandlerType.CreateInstance();
//        //        handler.AfrerReadWorkflow(workflow, workflowViewModel,
//        //        ServiceFactory.SecurityService.GetUserByUsername(User.Identity.Name));
//        //    }
//        //    workflowViewModel.CanAddNewStep = WorkflowHelper.AuthorizedAddStatus(workflow);
//        //    return new Souccar.Web.Mvc.JsonNet.JsonNetResult(workflowViewModel);
//        //}

//        //public ActionResult AddTestWorkflow()
//        //{
//        //    var workflow = new WorkflowItem();
//        //    workflow.Date = DateTime.Today;
//        //    workflow.Description = "laplaplap laplaplap laplaplap laplaplap";
//        //    workflow.Status = WorkflowStatus.InProgress;
//        //    workflow.StepCount = 3;
//        //    workflow.Type = WorkflowType.Appraisal;
//        //    workflow.Save();
//        //    return new EmptyResult();
//        //}

//        //public ActionResult SaveStep(int workflowId, string status, string description)
//        //{
//        //    var workflow = ((WorkflowItem) (typeof (WorkflowItem).GetById(workflowId)));
//        //    var eventHandlerName = string.Format("HRIS.Web.Mvc4.Models.Workflow.EventHandler.{0}WorkflowEventHandler"
//        //    , workflow.Type.ToString());
//        //    var handler = new WorkflowEventHandler();
//        //    var eventHandlerType = eventHandlerName.ToType();
//        //    if (eventHandlerType != null)
//        //    {
//        //        handler = (WorkflowEventHandler) eventHandlerType.CreateInstance();
//        //    }
//        //    if (!User.Identity.IsAuthenticated)
//        //        throw new Exception("User not authenticated.");
//        //    var user = ServiceFactory.SecurityService.GetUserByUsername(User.Identity.Name);
//        //    var acceptCount = workflow.Steps.Count(x => x.Status == WorkflowStepStatus.Accept) -
//        //                      workflow.Steps.Count(x => x.Status == WorkflowStepStatus.Reject);
//        //    if (acceptCount < workflow.StepCount)
//        //    {
//        //        var step = new WorkflowStep()
//        //        {
//        //            Date = DateTime.Now,
//        //            User = user,
//        //            Description = description,
//        //            Status = (WorkflowStepStatus) Enum.Parse(typeof (WorkflowStepStatus), status)
//        //        };
//        //        handler.BeforeInsertStep(workflow,step);
//        //        if(handler.PreventDefault)
//        //            return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new WrokflowViewModel(workflow));
//        //        workflow.AddStep(step);
//        //        workflow.Save();
//        //        handler.AfterInsertStep(workflow, step);
//        //        if (WorkflowHelper.IsFinish(workflow))
//        //        {
//        //            workflow.Status=WorkflowStatus.Completed;
//        //            workflow.Save();
//        //            return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new WrokflowViewModel(workflow));
//        //        }
//        //    }
//        //    var approval = workflow.Approvals.SingleOrDefault(x => x.User == user);
//        //    if (approval != null)
//        //    {
//        //        approval.Status = (WorkflowStepStatus)Enum.Parse(typeof(WorkflowStepStatus), status);
//        //        approval.Description = description;
//        //        handler.BeforeInsertApproval(workflow, approval);
//        //        if (handler.PreventDefault)
//        //            return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new WrokflowViewModel(workflow));
//        //        workflow.Save();
//        //        handler.AfterInsertApproval(workflow, approval);

//        //        if (WorkflowHelper.IsFinish(workflow))
//        //        {
//        //            handler.BeforeFinish(workflow);
//        //            if (handler.PreventDefault)
//        //                return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new WrokflowViewModel(workflow));
//        //            workflow.Status = WorkflowStatus.Completed;
//        //            workflow.Save();
//        //            return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new WrokflowViewModel(workflow));
//        //        }
//        //    }
//        //    return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new WrokflowViewModel(workflow));
//        //}

//        //#region workflow setting
//        //#region step
//        //public ActionResult GetOrgLevel()
//        //{
//        //    var data = ServiceFactory.ORMService.All<OrganizationalLevel>().OrderBy(x=>x.Order).ToList();
//        //    var result = new ArrayList();
//        //    foreach (var item in data)
//        //    {
//        //        var temp = new Dictionary<string, object>();
//        //        temp["Id"] = item.Id;
//        //        temp["Name"] = item.Name;
//        //        result.Add(temp);
//        //    }
//        //    return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
//        //}

//        //public ActionResult GetGrade(int id)
//        //{
//        //    var data = ServiceFactory.ORMService.All<Grade>().
//        //        Where(x => x.OrganizationalLevel.Id == id).OrderBy(x => x.Order).ToList();
//        //    var result = new ArrayList();
//        //    foreach (var item in data)
//        //    {
//        //        var temp = new Dictionary<string, object>();
//        //        temp["Id"] = item.Id;
//        //        temp["Name"] = item.Name;
//        //        result.Add(temp);
//        //    }
//        //    return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
//        //}

//        //public ActionResult GetJobTitle(int id)
//        //{
//        //    var data = ServiceFactory.ORMService.All<JobTitle>().
//        //        Where(x => x.Grade.Id == id).OrderBy(x => x.Order).ToList();
//        //    var result = new ArrayList();
//        //    foreach (var item in data)
//        //    {
//        //        var temp = new Dictionary<string, object>();
//        //        temp["Id"] = item.Id;
//        //        temp["Name"] = item.Name;
//        //        result.Add(temp);
//        //    }
//        //    return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
//        //}

//        //public ActionResult GetJobDescription(int id)
//        //{
//        //    var data = ServiceFactory.ORMService.All<JobDescription>().
//        //        Where(x => x.JobTitle.Id == id).OrderBy(x => x.Name).ToList();
//        //    var result = new ArrayList();
//        //    foreach (var item in data)
//        //    {
//        //        var temp = new Dictionary<string, object>();
//        //        temp["Id"] = item.Id;
//        //        temp["Name"] = item.Name;
//        //        result.Add(temp);
//        //    }
//        //    return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
//        //}


//        //public ActionResult SetStepByOrgLevel(int stepCount, int id, int workflowSettingId)
//        //{
//        //    var workflowSettint = ServiceFactory.ORMService.GetById<WorkflowSetting>(workflowSettingId);
//        //    var jobDescriptions = ServiceFactory.ORMService.All<JobDescription>()
//        //        .Where(x => x.JobTitle.Grade.OrganizationalLevel.Id == id);
//        //    var positions = jobDescriptions.SelectMany(x => x.Positions);
//        //    updateWorkflowSetting(stepCount, positions, workflowSettint);
//        //    workflowSettint.Save();
//        //    return Content("");
//        //}

//        //public ActionResult SetStepByGrade(int stepCount, int id, int workflowSettingId)
//        //{
//        //    var workflowSettint = ServiceFactory.ORMService.GetById<WorkflowSetting>(workflowSettingId);
//        //    var jobDescriptions = ServiceFactory.ORMService.All<JobDescription>()
//        //        .Where(x => x.JobTitle.Grade.Id == id);
//        //    var positions = jobDescriptions.SelectMany(x => x.Positions);
//        //    updateWorkflowSetting(stepCount, positions, workflowSettint);
//        //    return Content("");
            
//        //}

//        //public ActionResult SetStepByJobTitle(int stepCount, int id, int workflowSettingId)
//        //{
//        //    var workflowSettint = ServiceFactory.ORMService.GetById<WorkflowSetting>(workflowSettingId);
//        //    var jobDescriptions = ServiceFactory.ORMService.All<JobDescription>()
//        //        .Where(x => x.JobTitle.Id == id);
//        //    var positions = jobDescriptions.SelectMany(x => x.Positions);
//        //    updateWorkflowSetting(stepCount, positions, workflowSettint);
//        //    return Content("");
             
//        //}

//        //public ActionResult SetStepByJobDescription(int stepCount, int id, int workflowSettingId)
//        //{
//        //    var workflowSettint = ServiceFactory.ORMService.GetById<WorkflowSetting>(workflowSettingId);
//        //    var positions = ServiceFactory.ORMService.GetById<JobDescription>(id).Positions;
                
//        //    updateWorkflowSetting(stepCount, positions, workflowSettint);
//        //    return Content("");
            
//        //}

//        //private void updateWorkflowSetting(int stepCount, IEnumerable<Position> positions, WorkflowSetting workflowSettint)
//        //{
//        //    foreach (var position in positions)
//        //    {
//        //        var temp = workflowSettint.SettingPositions.SingleOrDefault(x => x.Position == position);
//        //        if (temp != null)
//        //        {
//        //            temp.Count = stepCount;
//        //        }
//        //        else
//        //        {
//        //            workflowSettint.AddPosition(new WorkflowSettingPosition() { Position = position, Count = stepCount });
//        //        }
//        //    }
//        //    workflowSettint.Save();
//        //}

//        //#endregion
//        //#region approval
//        //public ActionResult GetAllJobDescription()
//        //{
//        //    var jds = ServiceFactory.ORMService.All<JobDescription>();
//        //    var result = new ArrayList();
//        //    foreach (var item in jds)
//        //    {
//        //        var temp = new Dictionary<string, object>();
//        //        temp["Id"] = item.Id;
//        //        temp["Name"] = item.Name;
//        //        result.Add(temp);
//        //    }
//        //    return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
//        //}

//        //public ActionResult GetPosition(int id)
//        //{
//        //    var positions = ServiceFactory.ORMService.GetById<JobDescription>(id).Positions;

//        //    var result = new ArrayList();
//        //    foreach (var item in positions)
//        //    {
//        //        var temp = new Dictionary<string, object>();
//        //        temp["Id"] = item.Id;
//        //        temp["Name"] = item.NameForDropdown;
//        //        result.Add(temp);
//        //    }
//        //    return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
//        //}

//        //public ActionResult GetWorkflowSettingApprovals(int workflowId)
//        //{
//        //    var workflowSettint = ServiceFactory.ORMService.GetById<WorkflowSetting>(workflowId);
//        //    var result = workflowSettint.SettingApprovals.Select(x => new WorkflowApprovalViewModel()
//        //    {
//        //        PositionId = x.Position.Id,
//        //        Order = x.Order,
//        //        PositionName = x.Position.NameForDropdown
//        //    }).OrderBy(x=>x.Order).ToList();
//        //    return Json(new { Approvals = result }, JsonRequestBehavior.AllowGet);
//        //}

//        //public ActionResult RemoveApprovalFromWorkflowSetting(int positionId, int workflowId)
//        //{
//        //    var workflowSettint = ServiceFactory.ORMService.GetById<WorkflowSetting>(workflowId);
//        //    var temp = workflowSettint.SettingApprovals.SingleOrDefault(x => x.Position.Id == positionId);
//        //    if (temp != null)
//        //    {
//        //        workflowSettint.SettingApprovals.Remove(temp);
//        //        var i = 1;
//        //        foreach (var approval in workflowSettint.SettingApprovals.OrderBy(x=>x.Order))
//        //        {
//        //            approval.Order = i++;
//        //        }
//        //        workflowSettint.Save();
//        //    }
//        //    return GetWorkflowSettingApprovals(workflowId);
//        //}

//        //public ActionResult AddApprovalToWorkflowSetting(int positionId, int workflowId)
//        //{
//        //    var workflowSettint = ServiceFactory.ORMService.GetById<WorkflowSetting>(workflowId);
//        //    var position = workflowSettint.SettingApprovals.SingleOrDefault(x => x.Position.Id == positionId);
//        //    var maxOrder = 0;
//        //    if (!workflowSettint.SettingApprovals.IsEmpty())
//        //        maxOrder=workflowSettint.SettingApprovals.Max(x => x.Order);
//        //    if (position == null)
//        //    {
//        //        workflowSettint.AddApprovals(new WorkflowSettingApproval()
//        //        {
//        //            Position = ServiceFactory.ORMService.GetById<Position>(positionId),
//        //            Order = maxOrder + 1
//        //        });
//        //        workflowSettint.Save();
//        //    }
//        //    return GetWorkflowSettingApprovals(workflowId);
//        //}

//        //public ActionResult MoveApprovalUp(int positionId, int workflowId)
//        //{
//        //    var workflowSettint = ServiceFactory.ORMService.GetById<WorkflowSetting>(workflowId);
//        //    var position = workflowSettint.SettingApprovals.SingleOrDefault(x => x.Position.Id == positionId);
//        //    if (position == null)
//        //        return GetWorkflowSettingApprovals(workflowId);
//        //    if(position.Order==1)
//        //        return GetWorkflowSettingApprovals(workflowId);
//        //    var prevPosition = workflowSettint.SettingApprovals.SingleOrDefault(x => x.Order == position.Order-1);
//        //    prevPosition.Order++ ;
//        //    position.Order--;    
//        //    workflowSettint.Save();
//        //    return GetWorkflowSettingApprovals(workflowId);
//        //}

//        //public ActionResult MoveApprovalDown(int positionId, int workflowId)
//        //{
//        //    var workflowSettint = ServiceFactory.ORMService.GetById<WorkflowSetting>(workflowId);
//        //    var position = workflowSettint.SettingApprovals.SingleOrDefault(x => x.Position.Id == positionId);
//        //    if (position == null)
//        //        return GetWorkflowSettingApprovals(workflowId);
//        //    if (position.Order == workflowSettint.SettingApprovals.Count)
//        //        return GetWorkflowSettingApprovals(workflowId);
//        //    var nextPosition = workflowSettint.SettingApprovals.SingleOrDefault(x => x.Order == position.Order + 1);
//        //    nextPosition.Order--;
//        //    position.Order++;
//        //    workflowSettint.Save();
//        //    return GetWorkflowSettingApprovals(workflowId);
//        //}
//        //#endregion

//        //#endregion
//    }

    
//}
