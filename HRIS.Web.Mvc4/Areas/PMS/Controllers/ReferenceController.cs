using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.PMS.Entities;
using HRIS.Domain.PMS.RootEntities;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using Project.Web.Mvc4.Helpers;
using Project.Web.Mvc4.Helpers.Resource;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using Project.Web.Mvc4.Extensions;
using HRIS.Domain.Workflow;
using Project.Web.Mvc4.Models;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Domain.Workflow.Enums;
using Souccar.Infrastructure.Extenstions;
using HRIS.Domain.Global.Constant;
using Project.Web.Mvc4.ProjectModels;
using Souccar.Domain.Notification;

namespace Project.Web.Mvc4.Areas.PMS.Controllers
{
    public class ReferenceController : Controller
    {
        //
        // GET: /PMS/Reference/

        public ActionResult ReadWorkflowSetting(string typeName, RequestInformation requestInformation)
        {
            var temp=  ServiceFactory.ORMService.All<WorkflowSetting>();
            var result = temp.Select(x => new { Id = x.Id, Name = x.Title }).ToList();
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateAppraisalPhase(int phaseId)
        {
            var notify = new Notify();
            if (UserExtensions.CurrentUser == null)
                return Json(false, JsonRequestBehavior.AllowGet);

            var phase = ServiceFactory.ORMService.GetById<AppraisalPhase>(phaseId);

            var workflowItems = phase.PhaseWorkflows.Select(xx => xx.WorkflowItem).ToList();

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
                if (employeePosition.Value.Position==null)
                    continue;

                var workflowItemIsExist = workflowItems.Any(x => x.TargetUser == employeePosition.Value.Position.User() && x.Type ==(int)Souccar.Domain.Workflow.Enums.WorkflowType.Appraisal);

                if (!workflowItemIsExist)
                {
                    var body = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.YouHaveANotifyToEvaluate) + " " + employeePosition.Key.User().FullName;
                    var title = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.YouHaveANotifyToEvaluate) + " " + employeePosition.Key.User().FullName;
                    var destinationTabName = NavigationTabName.Strategic;
                    var destinationModuleName = ModulesNames.PMS;
                    var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
                      ModulesNames.ResourceGroupName + "_" + ModulesNames.PMS);
                    var destinationControllerName = "PMS/Home";
                    var destinationActionName = "GetEmployeesAppraisal";
                    var destinationEntityId = "GetEmployeesAppraisal";
                    var destinationEntityTitle = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.EmployeesAppraisal);
                    var destinationData = new Dictionary<string, int>();
                    // destinationData.Add("WorkflowId", workflowItem.Id);
                    var workflow = WorkflowHelper.InitWithSetting(
                        phase.AppraisalPhaseSetting.WorkflowSetting,
                        employeePosition.Key.User(),
                        title, body, destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                        destinationActionName, destinationEntityId, destinationEntityTitle, OperationType.Nothing, destinationData,
                        employeePosition.Value.Position,
                         Souccar.Domain.Workflow.Enums.WorkflowType.Appraisal,
                        PMSLocalizationHelper.GetResource(PMSLocalizationHelper.PhaseDescription),
                        out notify);

                    entities.Add(workflow);
                    phase.AddPhaseWorkflow(new AppraisalPhaseWorkflow()
                    {
                        Position = employeePosition.Value.Position,
                        WorkflowItem = workflow,
                        Employee = employeePosition.Value.Position.Employee
                    });
                }
                else
                {
                    var workflowItem = workflowItems.FirstOrDefault(x => x.TargetUser == employeePosition.Key.User() && x.Type == (int)Souccar.Domain.Workflow.Enums.WorkflowType.Appraisal);
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
            notify.DestinationData.Add("ServiceId", phase.Id);
            notify.DestinationData.Add("WorkflowId", workflowitem.Id);
            ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>() { notify }, UserExtensions.CurrentUser);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
