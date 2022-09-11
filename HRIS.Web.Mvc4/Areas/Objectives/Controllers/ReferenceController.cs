using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.Grades.Entities;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Objectives.Entities;
using HRIS.Domain.Objectives.RootEntities;

using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Workflow;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Helpers.Resource;
using Project.Web.Mvc4.Models;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Workflow.Enums;
using Souccar.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project.Web.Mvc4.ProjectModels;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.Notification;
using Souccar.Domain.Workflow.RootEntities;
using HRIS.Domain.JobDescription.RootEntities;


namespace Project.Web.Mvc4.Areas.Objectives.Controllers
{
    public class ReferenceController : Controller
    {
        [HttpPost]
        public ActionResult ReadPositions(string typeName, RequestInformation requestInformation)
        {
            var obj = ServiceFactory.ORMService.GetById<HRIS.Domain.Objectives.RootEntities.Objective>(requestInformation.NavigationInfo.Previous[1].RowId);
            var result = obj.SharedWiths.Select(x => new { Id = x.Position.Id, Name = string.Format("{0}",x.Position.NameForDropdown)}).ToList();
            result.Insert(0, new { Id = obj.Owner.Id, Name = obj.Owner.NameForDropdown });
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ReadPositionsByNode(string typeName, RequestInformation requestInformation)
        {
            var result = ServiceFactory.ORMService.All<Position>().ToList().Select(x => new { Id = x.Id, Name = x.NameForDropdown, ParentId = x.JobDescription?.Node?.Id }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ReadJDByNode(string typeName, RequestInformation requestInformation)
        {
            var result = ServiceFactory.ORMService.All<HRIS.Domain.JobDescription.RootEntities.JobDescription> ().ToList().Select(x => new { Id = x.Id, Name = x.NameForDropdown,ParentId=x.Node.Id}).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ReadPositionsByJD(string typeName, RequestInformation requestInformation)
        {
            var result = ServiceFactory.ORMService.All<Position>().ToList().Where(x=>x.Employee!=null).Select(x => new { Id = x.Id, Name = x.NameForDropdown, ParentId = x.JobDescription.Id }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReadWorkflowSetting(string typeName, RequestInformation requestInformation)
        {
            var temp = ServiceFactory.ORMService.All<WorkflowSetting>();
            var result = temp.Select(x => new { Id = x.Id, Name = x.Title }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult ReadJobTitleCascadeGrade(string typeName, RequestInformation requestInformation)
        {
            var result = ServiceFactory.ORMService.All<JobTitle>().Select(x => new { Id = x.Id, Name = x.Name, ParentId = x.Grade.Id }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReadJobDescriptionCascadeGrade(string typeName, RequestInformation requestInformation)
        {
            var result = ServiceFactory.ORMService.All<HRIS.Domain.JobDescription.RootEntities.JobDescription>().Select(x => new { Id = x.Id, Name = x.Name, ParentId = x.JobTitle.Id }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ReadPositionCascadeJobDescription(string typeName, RequestInformation requestInformation)
        {
            var result = ServiceFactory.ORMService.All<Position>().ToList().Select(x => new { Id = x.Id, Name = x.NameForDropdown, ParentId = x.JobDescription.Id }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateObjectivePhase(int phaseId)
        {
            var notify = new Notify();
            if (UserExtensions.CurrentUser == null)
                return Json(false, JsonRequestBehavior.AllowGet);

            var phase = ServiceFactory.ORMService.GetById<ObjectiveAppraisalPhase>(phaseId);

            var workflowItems = phase.Workflows.Select(xx => xx.WorkflowItem).ToList();

            var emps = ServiceFactory.ORMService.All<Employee>().ToList();

            var employeesPositions = new List<KeyValuePair<Employee, AssigningEmployeeToPosition>>();

            foreach (var emp in emps)
            {
                if (emp.Positions.Count == 1)
                    employeesPositions.Add(new KeyValuePair<Employee, AssigningEmployeeToPosition>(emp, emp.Positions[0]));
                else
                {
                    foreach (var position in emp.Positions.Where(x => !x.IsPrimary))
                    {
                        employeesPositions.Add(new KeyValuePair<Employee, AssigningEmployeeToPosition>(emp, position));
                    }
                }
            }

            var entities = new List<IAggregateRoot>();
            foreach (var employeePosition in employeesPositions)
            {
                var workflowItemIsExist = workflowItems.Any(x => x.TargetUser == employeePosition.Key.User() && x.Type == Souccar.Domain.Workflow.Enums.WorkflowType.Objective);

                if (!workflowItemIsExist)
                {
                    var body = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.BodyAppraisalNotify) + " " + employeePosition.Key.User().FullName;
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
                    
                    var workflow = WorkflowHelper.InitWithSetting(
                        phase.WorkflowSetting,
                        employeePosition.Key.User(),
                        title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                        destinationActionName, destinationEntityId, destinationEntityTitle, OperationType.Nothing, destinationData,
                        employeePosition.Value.Position,
                        Souccar.Domain.Workflow.Enums.WorkflowType.Objective,
                        ObjectiveLocalizationHelper.GetResource(ObjectiveLocalizationHelper.PhaseDescription),
                        out notify);

                    entities.Add(workflow);
                    phase.AddWorkflow(new ObjectiveAppraisalWorkflow()
                    {
                        //Position = employeePosition.Value.Position,
                        WorkflowItem = workflow
                    });
                }
                else
                {
                    var workflowItem = workflowItems.SingleOrDefault(x => x.TargetUser == employeePosition.Key.User() && x.Type == Souccar.Domain.Workflow.Enums.WorkflowType.Objective);
                    workflowItem.TargetUser = employeePosition.Key.User();
                    workflowItem.Date = DateTime.Today;
                    workflowItem.Creator = UserExtensions.CurrentUser;
                    workflowItem.FirstUser = (employeePosition.Key.User() != null && employeePosition.Value.Position != null && employeePosition.Value.Position.Manager != null) ? employeePosition.Value.Position.Manager.User() : null;
                    entities.Add(workflowItem);
                }
            }
            entities.Add(phase);
            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser);
            var workflowitem = ServiceFactory.ORMService.All<WorkflowItem>().OrderByDescending(x => x.Id).FirstOrDefault();
            notify.DestinationData.Add("WorkflowId", workflowitem.Id);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, UserExtensions.CurrentUser);
            return Json(phase, JsonRequestBehavior.AllowGet);
        }
    }
}
