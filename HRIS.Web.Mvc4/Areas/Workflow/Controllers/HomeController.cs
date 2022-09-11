using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FluentNHibernate.Conventions;
using HRIS.Domain.Grades.Entities;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.JobDescription.RootEntities;
using HRIS.Domain.OrganizationChart.Indexes;
using HRIS.Domain.Workflow;
using Project.Web.Mvc4.Extensions;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Models;


using Souccar.Core.Extensions;
using Souccar.Core.Fasterflect;
using Souccar.Domain.Workflow.Entities;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;

using System.Collections;
using HRIS.Domain.OrganizationChart.RootEntities;

using HRIS.Domain.Global.Constant;
using Project.Web.Mvc4.Areas.Workflow.Models;
using Project.Web.Mvc4.Areas.Workflow.Models.EventHandler;
using HRIS.Domain.Grades.RootEntities;

namespace Project.Web.Mvc4.Areas.Workflow.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(RequestInformation.Navigation.Step moduleInfo)
        {
            if (TempData["Module"] == null)
                return RedirectToAction("Welcome", "Module", new { area = "", id = ModulesNames.Workflow });

            return View();
        }
        
        public ActionResult GetWorkflow(int workflowId)
        {
            var workflow = ((WorkflowItem) (typeof (WorkflowItem).GetById(workflowId)));
            var workflowViewModel = new WrokflowViewModel(workflow);
            if(workflowViewModel.Steps.Count!=0 && workflowViewModel.Steps[workflowViewModel.Steps.Count-1].Status== WorkflowStepStatus.Pending.ToString())
            {
                if (workflowViewModel.Approvals.Count > 0)
                    workflowViewModel.Approvals=new List<WrokflowStepViewModel>();
            }
            var eventHandlerName = string.Format("Project.Web.Mvc4.Models.Workflow.EventHandler.{0}WorkflowEventHandler"
                , workflow.Type.ToString());
            var eventHandlerType = eventHandlerName.ToType();
            if (eventHandlerType != null)
            {
                var handler = (WorkflowEventHandler) eventHandlerType.CreateInstance();
                handler.AfrerReadWorkflow(workflow, workflowViewModel,
                ServiceFactory.SecurityService.GetUserByUsername(User.Identity.Name));
            }
            workflowViewModel.CanAddNewStep = WorkflowHelper.AuthorizedAddStatus(workflow);
            return new Souccar.Web.Mvc.JsonNet.JsonNetResult(workflowViewModel);
        }

        public ActionResult AddTestWorkflow()
        {
            var workflow = new WorkflowItem();
            workflow.Date = DateTime.Today;
            workflow.Description = "laplaplap laplaplap laplaplap laplaplap";
            workflow.Status = WorkflowStatus.InProgress;
            workflow.StepCount = 3;
            workflow.Type = Souccar.Domain.Workflow.Enums.WorkflowType.Appraisal;
            workflow.Save();
            return new EmptyResult();
        }

        public ActionResult SaveStep(int workflowId, string status, string description)
        {
            var workflow = ((WorkflowItem) (typeof (WorkflowItem).GetById(workflowId)));
            var eventHandlerName = string.Format("Project.Web.Mvc4.Models.Workflow.EventHandler.{0}WorkflowEventHandler"
            , workflow.Type.ToString());
            var handler = new WorkflowEventHandler();
            var eventHandlerType = eventHandlerName.ToType();
            if (eventHandlerType != null)
            {
                handler = (WorkflowEventHandler) eventHandlerType.CreateInstance();
            }
            if (!User.Identity.IsAuthenticated)
                throw new Exception("User not authenticated.");
            var user = ServiceFactory.SecurityService.GetUserByUsername(User.Identity.Name);
            var acceptCount = workflow.Steps.Count(x => x.Status == WorkflowStepStatus.Accept) -
                              workflow.Steps.Count(x => x.Status == WorkflowStepStatus.Reject);
            if (acceptCount < workflow.StepCount)
            {
                var step = new WorkflowStep()
                {
                    Date = DateTime.Now,
                    User = user,
                    Description = description,
                    Status = (WorkflowStepStatus) Enum.Parse(typeof (WorkflowStepStatus), status)
                };
                handler.BeforeInsertStep(workflow,step);
                if(handler.PreventDefault)
                    return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new WrokflowViewModel(workflow));
                workflow.AddStep(step);
                workflow.Save();
                handler.AfterInsertStep(workflow, step);
                if (WorkflowHelper.IsFinish(workflow))
                {
                    workflow.Status=WorkflowStatus.Completed;
                    workflow.Save();
                    return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new WrokflowViewModel(workflow));
                }
            }
            var approval = workflow.Approvals.SingleOrDefault(x => x.User == user);
            if (approval != null)
            {
                approval.Status = (WorkflowStepStatus)Enum.Parse(typeof(WorkflowStepStatus), status);
                approval.Description = description;
                handler.BeforeInsertApproval(workflow, approval);
                if (handler.PreventDefault)
                    return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new WrokflowViewModel(workflow));
                workflow.Save();
                handler.AfterInsertApproval(workflow, approval);

                if (WorkflowHelper.IsFinish(workflow))
                {
                    handler.BeforeFinish(workflow);
                    if (handler.PreventDefault)
                        return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new WrokflowViewModel(workflow));
                    workflow.Status = WorkflowStatus.Completed;
                    workflow.Save();
                    return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new WrokflowViewModel(workflow));
                }
            }
            return new Souccar.Web.Mvc.JsonNet.JsonNetResult(new WrokflowViewModel(workflow));
        }

        #region workflow setting
        #region step
        public ActionResult GetOrgLevel()
        {
            var data = ServiceFactory.ORMService.All<OrganizationalLevel>().OrderBy(x=>x.Order).ToList();
            var result = new ArrayList();
            foreach (var item in data)
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = item.Id;
                temp["Name"] = item.Name;
                result.Add(temp);
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult GetGrade(int id)
        {
            var data = ServiceFactory.ORMService.All<HRIS.Domain.Grades.RootEntities.Grade>().
                Where(x => x.OrganizationalLevel.Id == id).OrderBy(x => x.Order).ToList();
            var result = new ArrayList();
            foreach (var item in data)
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = item.Id;
                temp["Name"] = item.Name;
                result.Add(temp);
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetJobTitle(int id)
        {
            var data = ServiceFactory.ORMService.All<JobTitle>().
                Where(x => x.Grade.Id == id).OrderBy(x => x.Order).ToList();
            var result = new ArrayList();
            foreach (var item in data)
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = item.Id;
                temp["Name"] = item.Name;
                result.Add(temp);
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetJobDescription(int id)
        {
            var data = ServiceFactory.ORMService.All<HRIS.Domain.JobDescription.RootEntities.JobDescription>().
                Where(x => x.JobTitle.Id == id).OrderBy(x => x.Name).ToList();
            var result = new ArrayList();
            foreach (var item in data)
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = item.Id;
                temp["Name"] = item.Name;
                result.Add(temp);
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetJobPosition(int id)
        {
            var data = ServiceFactory.ORMService.All<HRIS.Domain.JobDescription.RootEntities.JobDescription>().
                Where(x => x.Id == id).SelectMany(x=> x.Positions).ToList();
            
            var result = new ArrayList();
            foreach (var item in data)
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = item.Id;
                temp["Name"] = item.NameForDropdown;
                result.Add(temp);
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetStepByOrgLevel(int stepCount, int id, int workflowSettingId)
        {
            var workflowSettint = ServiceFactory.ORMService.GetById<WorkflowSetting>(workflowSettingId);
            var jobDescriptions = ServiceFactory.ORMService.All<HRIS.Domain.JobDescription.RootEntities.JobDescription>()
                .Where(x => x.JobTitle.Grade.OrganizationalLevel.Id == id);
            var positions = jobDescriptions.SelectMany(x => x.Positions);
            updateWorkflowSetting(1, stepCount, positions, workflowSettint, jobDescriptions.FirstOrDefault().JobTitle?.Grade?.OrganizationalLevel);
            workflowSettint.Save();
            return Content("");
        }

        public ActionResult SetStepByGrade(int stepCount, int id, int workflowSettingId)
        {
            var workflowSettint = ServiceFactory.ORMService.GetById<WorkflowSetting>(workflowSettingId);
            var jobDescriptions = ServiceFactory.ORMService.All<HRIS.Domain.JobDescription.RootEntities.JobDescription>()
                .Where(x => x.JobTitle.Grade.Id == id);
            var positions = jobDescriptions.SelectMany(x => x.Positions);
            updateWorkflowSetting(2, stepCount, positions, workflowSettint, null, jobDescriptions.FirstOrDefault().JobTitle?.Grade);
            return Content("");
            
        }

        public ActionResult SetStepByJobTitle(int stepCount, int id, int workflowSettingId)
        {
            var workflowSettint = ServiceFactory.ORMService.GetById<WorkflowSetting>(workflowSettingId);
            var jobDescriptions = ServiceFactory.ORMService.All<HRIS.Domain.JobDescription.RootEntities.JobDescription>()
                .Where(x => x.JobTitle.Id == id);
            var positions = jobDescriptions.SelectMany(x => x.Positions);
            updateWorkflowSetting(3, stepCount, positions, workflowSettint, null, null , jobDescriptions.FirstOrDefault().JobTitle);
            return Content("");
             
        }

        public ActionResult SetStepByJobDescription(int stepCount, int id, int workflowSettingId)
        {
            var workflowSettint = ServiceFactory.ORMService.GetById<WorkflowSetting>(workflowSettingId);
            var jobDesc = ServiceFactory.ORMService.GetById<HRIS.Domain.JobDescription.RootEntities.JobDescription>(id);
            var positions =jobDesc.Positions;
                
            updateWorkflowSetting(4,stepCount, positions, workflowSettint, null, null ,null , jobDesc);
            return Content("");
            
        }
        public ActionResult SetStepByJobPosition(int stepCount, int id, int workflowSettingId)
        {
            var workflowSettint = ServiceFactory.ORMService.GetById<WorkflowSetting>(workflowSettingId);
            var positions = ServiceFactory.ORMService.All<Position>().Where(x=> x.Id == id);
            updateWorkflowSetting(5, stepCount, positions, workflowSettint);
            return Content("");

        }
        private void updateWorkflowSetting(int overwriteType,int stepCount, IEnumerable<Position> positions, WorkflowSetting workflowSettint, OrganizationalLevel orgLevel = null, Grade grade = null, JobTitle jobTitle = null, HRIS.Domain.JobDescription.RootEntities.JobDescription jobDesc = null)
        {
            switch (overwriteType)
            {
                //for Org level
                case 1:
                    workflowSettint.AddOverwirte(new WorkflowSettingOverwritingHistory
                    {
                        OrganizationalLevel = orgLevel,
                        Count = stepCount
                    });
                    break;
                    //for Grade
                case 2:
                    workflowSettint.AddOverwirte(new WorkflowSettingOverwritingHistory
                    {
                        Grade = grade,
                        Count = stepCount
                    });
                    break;
                    // for Job Title
                case 3:
                    workflowSettint.AddOverwirte(new WorkflowSettingOverwritingHistory
                    {
                        JobTitle = jobTitle,
                        Count = stepCount
                    });
                    break;
                    //for job desc
                case 4:
                    workflowSettint.AddOverwirte(new WorkflowSettingOverwritingHistory
                    {
                        JobDescription = jobDesc,
                        Count = stepCount
                    });
                    break;
                default:
                    break;
            }
            foreach (var position in positions)
            {
                var temp = workflowSettint.SettingPositions.SingleOrDefault(x => x.Position == position);
                if (temp != null)
                {
                    temp.Count = stepCount;
                }
                else
                {
                    workflowSettint.AddPosition(new WorkflowSettingPosition() { Position = position, Count = stepCount });
                }
            }
            workflowSettint.Save();
        }

        #endregion
        #region approval
        public ActionResult GetAllJobDescription()
        {
            var jds = ServiceFactory.ORMService.All<HRIS.Domain.JobDescription.RootEntities.JobDescription>();
            var result = new ArrayList();
            foreach (var item in jds)
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = item.Id;
                temp["Name"] = item.Name;
                result.Add(temp);
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPosition(int id)
        {
            var positions = ServiceFactory.ORMService.GetById<HRIS.Domain.JobDescription.RootEntities.JobDescription>(id).Positions;

            var result = new ArrayList();
            foreach (var item in positions)
            {
                var temp = new Dictionary<string, object>();
                temp["Id"] = item.Id;
                temp["Name"] = item.NameForDropdown;
                result.Add(temp);
            }
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetWorkflowSettingApprovals(int workflowId)
        {
            var workflowSettint = ServiceFactory.ORMService.GetById<WorkflowSetting>(workflowId);
            var result = workflowSettint.SettingApprovals.Select(x => new WorkflowApprovalViewModel()
            {
                PositionId = x.Position.Id,
                Order = x.Order,
                PositionName = x.Position.NameForDropdown
            }).OrderBy(x=>x.Order).ToList();
            return Json(new { Approvals = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveApprovalFromWorkflowSetting(int positionId, int workflowId)
        {
            var workflowSettint = ServiceFactory.ORMService.GetById<WorkflowSetting>(workflowId);
            var temp = workflowSettint.SettingApprovals.SingleOrDefault(x => x.Position.Id == positionId);
            if (temp != null)
            {
                workflowSettint.SettingApprovals.Remove(temp);
                var i = 1;
                foreach (var approval in workflowSettint.SettingApprovals.OrderBy(x=>x.Order))
                {
                    approval.Order = i++;
                }
                workflowSettint.Save();
            }
            return GetWorkflowSettingApprovals(workflowId);
        }

        public ActionResult AddApprovalToWorkflowSetting(int positionId, int workflowId)
        {
            var workflowSettint = ServiceFactory.ORMService.GetById<WorkflowSetting>(workflowId);
            var position = workflowSettint.SettingApprovals.SingleOrDefault(x => x.Position.Id == positionId);
            var maxOrder = 0;
            if (!workflowSettint.SettingApprovals.IsEmpty())
                maxOrder=workflowSettint.SettingApprovals.Max(x => x.Order);
            if (position == null)
            {
                workflowSettint.AddApprovals(new WorkflowSettingApproval()
                {
                    Position = ServiceFactory.ORMService.GetById<Position>(positionId),
                    Order = maxOrder + 1
                });
                workflowSettint.Save();
            }
            return GetWorkflowSettingApprovals(workflowId);
        }

        public ActionResult MoveApprovalUp(int positionId, int workflowId)
        {
            var workflowSettint = ServiceFactory.ORMService.GetById<WorkflowSetting>(workflowId);
            var position = workflowSettint.SettingApprovals.SingleOrDefault(x => x.Position.Id == positionId);
            if (position == null)
                return GetWorkflowSettingApprovals(workflowId);
            if(position.Order==1)
                return GetWorkflowSettingApprovals(workflowId);
            var prevPosition = workflowSettint.SettingApprovals.SingleOrDefault(x => x.Order == position.Order-1);
            prevPosition.Order++ ;
            position.Order--;    
            workflowSettint.Save();
            return GetWorkflowSettingApprovals(workflowId);
        }

        public ActionResult MoveApprovalDown(int positionId, int workflowId)
        {
            var workflowSettint = ServiceFactory.ORMService.GetById<WorkflowSetting>(workflowId);
            var position = workflowSettint.SettingApprovals.SingleOrDefault(x => x.Position.Id == positionId);
            if (position == null)
                return GetWorkflowSettingApprovals(workflowId);
            if (position.Order == workflowSettint.SettingApprovals.Count)
                return GetWorkflowSettingApprovals(workflowId);
            var nextPosition = workflowSettint.SettingApprovals.SingleOrDefault(x => x.Order == position.Order + 1);
            nextPosition.Order--;
            position.Order++;
            workflowSettint.Save();
            return GetWorkflowSettingApprovals(workflowId);
        }
        #endregion

        #endregion
    }

    public class WorkflowApprovalViewModel
    {
        public string PositionName { get; set; }
        public int PositionId { get; set; }
        public int Order { get; set; }
    }
}
