using HRIS.Domain.Personnel.RootEntities;
using Project.Web.Mvc4.Areas.Appraisal;
using Project.Web.Mvc4.Areas.Objectives.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project.Web.Mvc4.Helpers.DomainExtensions;
using HRIS.Domain.PMS.RootEntities;
using Souccar.Infrastructure.Core;
using Souccar.Domain.Workflow.Enums;
using HRIS.Domain.JobDescription.Entities;
using Project.Web.Mvc4.Helpers;
using Souccar.Domain.Workflow.RootEntities;
using HRIS.Domain.PMS.Enums;
using Project.Web.Mvc4.Areas.Appraisals;
using Souccar.Domain.DomainModel;
using HRIS.Domain.JobDescription.Entities;
using Project.Web.Mvc4.Helpers.Resource;
using HRIS.Domain.PMS.Entities;
using HRIS.Domain.Personnel.Enums;
using Project.Web.Mvc4.Factories;
using HRIS.Domain.Workflow;
using Project.Web.Mvc4.Areas.Appraisals;
using Project.Web.Mvc4.ProjectModels;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.Notification;

namespace Project.Web.Mvc4.Areas.PMS.Helper
{
    public static class PmsHelper
    {
        public static bool requested = false;
        public static List<EmployeeInfoViewModel> GetEmployeeForAppraisal()
        {
            var result = new List<EmployeeInfoViewModel>();
            if (!requested)
            {

                requested = true;
                var currentPosition = EmployeeExtensions.CurrentEmployee.GetSecondaryPositionElsePrimary();

                if (currentPosition == null)
                {
                    return result;
                }


                var appraisalPhase = GetCurrentAppraisalPhase();
                if (appraisalPhase == null)
                    return result;
                var currentPositionList = new List<Position>();
                //  if (appraisalPhase.PhaseWorkflows.Where(x => x.WorkflowItem.FirstUser == EmployeeExtensions.CurrentEmployee.User()).ToList().Count() != 0)
                //  { 
                currentPositionList = EmployeeExtensions.CurrentEmployee.GetSecondaryPositionElsePrimaryForManager().ToList();

                //  }
                if (currentPositionList.Where(x => x.AssigningEmployeeToPosition.Position == currentPosition).ToList().Count() == 0)
                {
                    currentPositionList.Add(currentPosition);
                }
                var positionForEmployees = new List<Position>();
                foreach (var currentPositionItem in currentPositionList)
                {
                    GetChildren(currentPositionItem, positionForEmployees);
                }
                foreach (var item in appraisalPhase.PhaseWorkflows)
                {
                    WorkflowPendingType pendingType = WorkflowHelper.GetPendingType(item.WorkflowItem);
                    var _position = item.WorkflowItem.CurrentUser?.GetSecondaryPositionElsePrimary();

                    if (_position == null)
                    {
                        _position = WorkflowHelper.GetNextAppraiser(item.WorkflowItem, out pendingType);
                    }
                    if (_position == currentPosition && item.WorkflowItem.Status != WorkflowStatus.Completed &&
                            item.WorkflowItem.Status != WorkflowStatus.Canceled)
                    {
                        if ((pendingType == WorkflowPendingType.NotPending || pendingType == WorkflowPendingType.NewStep || pendingType == WorkflowPendingType.PendingStep) &&
                            ((item.WorkflowItem.Steps.Count > 1) || (item.WorkflowItem.Steps.Count == 1 && item.WorkflowItem.Steps.LastOrDefault().Status == WorkflowStepStatus.Accept)))
                        {
                            result.Add(GetPmsDataViewModel(appraisalPhase, item, pendingType));
                        }
                        else if (pendingType == WorkflowPendingType.PendingApproval || !positionForEmployees.Any(x => x.Employee == item.Employee))
                        {
                            result.Add(GetPmsDataViewModel(appraisalPhase, item, pendingType));
                        }
                    }

                }

                var entities = new List<IAggregateRoot>();
                foreach (var position in positionForEmployees)
                {
                    var employee = position.Employee;
                    if (IsExistPosition(position.Employee, position))
                    {
                        if (position.Employee?.User() == null)
                        {
                            continue;
                        }
                        var workflows = ServiceFactory.ORMService.All<AppraisalPhaseWorkflow>().Where(x => x.AppraisalPhase.Id == appraisalPhase.Id).ToList();
                        var workflowbyPositionAndEmployee = workflows.FirstOrDefault(x => x.Position == position && x.Employee == employee);
                        var workflowbyEmployee = workflows.FirstOrDefault(x => x.Employee == employee);
                        var workflowbyPosition = workflows.FirstOrDefault(x => x.Position == position || x.Employee == employee);
                        var workflow = workflowbyPositionAndEmployee != null ? workflowbyPositionAndEmployee : workflowbyEmployee != null ? workflowbyEmployee : workflowbyPosition;
                        if (workflow != null && workflow.WorkflowItem != null && workflow.WorkflowItem.TargetUser.Id != workflow.Employee.User().Id)
                            workflow = null;
                        if (workflow == null)
                        {
                            var notify = new Notify();
                            var body = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.YouHavePerformanceAppraisalNotifyFor) + " " + position.Employee.User().FullName;
                            var title = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.YouHavePerformanceAppraisalNotifyFor);
                            var destinationTabName = NavigationTabName.Strategic;
                            var destinationModuleName = ModulesNames.PMS;
                            var destinationLocalizationModuleName = ServiceFactory.LocalizationService.GetResource(
                              ModulesNames.ResourceGroupName + "_" + ModulesNames.PMS);
                            var destinationControllerName = "PMS/Home";
                            var destinationActionName = "GetEmployeesAppraisal";
                            var destinationEntityId = "GetEmployeesAppraisal";
                            var destinationEntityTitle = PMSLocalizationHelper.GetResource(PMSLocalizationHelper.EmployeesAppraisal);
                            var destinationData = new Dictionary<string, int>();

                            var workflowitem = WorkflowHelper.InitWithSetting(appraisalPhase.AppraisalPhaseSetting.WorkflowSetting, position.Employee.User(), body, body,
                                                  destinationTabName, destinationModuleName, destinationLocalizationModuleName, destinationControllerName,
                                                  destinationActionName, destinationEntityId, destinationEntityTitle, OperationType.Nothing, destinationData,
                                                  position, WorkflowType.Appraisal,
                                                  appraisalPhase.Description, out notify);
                            entities.Add(workflowitem);
                            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser);
                            var newPhaseWorkflow = new AppraisalPhaseWorkflow()
                            {
                                Position = position,
                                WorkflowItem = workflowitem,
                                Employee = position.Employee
                            };
                            notify.DestinationData.Add("WorkflowId", workflowitem.Id);
                            notify.DestinationData.Add("ServiceId", appraisalPhase.Id);
                            appraisalPhase.AddPhaseWorkflow(newPhaseWorkflow);
                            entities.Add(appraisalPhase);
                            entities.Add(notify);
                            ServiceFactory.ORMService.SaveTransaction(entities, UserExtensions.CurrentUser);
                            workflow = newPhaseWorkflow;
                        }

                        WorkflowPendingType pendingType;
                        var startPosition = WorkflowHelper.GetNextAppraiser(workflow.WorkflowItem, out pendingType);
                        //if (pendingType == WorkflowPendingType.PendingApproval && !workflow.HasFinalManagerApproval)
                        //    continue;
                        if (pendingType == WorkflowPendingType.PendingApproval || startPosition != currentPosition && workflow != null)
                        {
                            continue;
                        }
                        if (currentPositionList.Where(x => x.AssigningEmployeeToPosition.Position == startPosition).ToList().Count() != 0)
                        {

                            result.Add(GetPmsDataViewModel(appraisalPhase, workflow, pendingType));



                        }

                    }

                }
                requested = false;
            }


            return result;
        }



        public static EmployeeInfoViewModel GetRow(int workflowId, int appraisalPhaseId)
        {
            var result = new EmployeeInfoViewModel();
            var workflow = ServiceFactory.ORMService.All<AppraisalPhaseWorkflow>().FirstOrDefault(x => x.WorkflowItem.Id == workflowId);
            var AppraisalPhase = ServiceFactory.ORMService.GetById<AppraisalPhase>(appraisalPhaseId);
            var currentPosition = EmployeeExtensions.CurrentEmployee.PrimaryPosition();
            if (currentPosition == null)
            {
                return result;
            }
            WorkflowPendingType pendingType;
            var startPosition = WorkflowHelper.GetNextAppraiser(workflow.WorkflowItem, out pendingType);
            if (startPosition == currentPosition)
            {

                result = (GetPmsDataViewModel(AppraisalPhase, workflow, pendingType));

            }
            return result;
        }

        public static void GetChildren(Position position, List<Position> allPositions)
        {
            var positions = ServiceFactory.ORMService.All<Position>().ToList()
               .Where(x => x.Manager == position && x.AssigningEmployeeToPosition != null);

            if (positions != null && positions.Count() > 0)
            {
                allPositions.AddRange(positions);

                //foreach (var pos in positions)
                //{
                //    GetChildren(pos, allPositions);
                //}
            }
        }
        public static bool IsExistPosition(Employee employee, Position position)
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
        public static ApprovalViewModel GetApprovalViewModel(int empId, int phaseWorkflowId)
        {
            var phaseWorkflow = ServiceFactory.ORMService.GetById<AppraisalPhaseWorkflow>(phaseWorkflowId);

            return ApprovalViewModelFactory.Create(phaseWorkflow);
        }
        public static AppraisalViewModel GetAppraisalViewModel(int id, int phaseWorkflowId)
        {
            var position = ServiceFactory.ORMService.GetById<Position>(id);
            var appraisalPhaseWorkflow = ServiceFactory.ORMService.GetById<AppraisalPhaseWorkflow>(phaseWorkflowId);
            var template = appraisalPhaseWorkflow.AppraisalPhase.AppraisalTemplateSetting.DefaultTemplate;
            var templateAppraisalPositions = appraisalPhaseWorkflow.AppraisalPhase.AppraisalTemplateSetting.AppraisalTemplatePositions.SingleOrDefault(x => x.Position == position);
            if (templateAppraisalPositions != null)
                template = templateAppraisalPositions.AppraisalTemplate;

            var result = AppraisalViewModelFactory.Create(position, template, appraisalPhaseWorkflow.AppraisalPhase);
            var peningType = WorkflowHelper.GetPendingType(appraisalPhaseWorkflow.WorkflowItem);
            if (peningType == WorkflowPendingType.PendingStep)
            {
                var appraisal =
                    appraisalPhaseWorkflow.Appraisals.SingleOrDefault(x => x.Step.Status == WorkflowStepStatus.Pending);
                AppraisalViewModelFactory.UpdateViewModelFromAppraisal(appraisal, result);
            }

            return result;
        }
        public static EmployeeInfoViewModel GetPmsDataViewModel(AppraisalPhase appraisalPhase, AppraisalPhaseWorkflow workflow = null, WorkflowPendingType? pendingType = null, DelegateRolesToPosition delegateRole = null)
        {
            var result = new EmployeeInfoViewModel();
            result.TripleName = workflow.Employee.FullName;
            result.EmployeeStatus = ServiceFactory.LocalizationService.GetResource("HRIS.Domain.Personnel.Enums.EmployeeStatus." + Enum.GetName(typeof(EmployeeStatus), workflow.Employee.Status));
            result.Date = workflow.WorkflowItem.Date.ToShortDateString();
            result.Id = workflow.Employee.Id;
            result.PositionId = workflow.Position.Id;
            result.FullName = workflow.Employee.FullName;
            result.Age = workflow.Employee.Age;
            if (appraisalPhase != null)
                result.PhaseId = appraisalPhase.Id;
            result.Code = workflow.Employee.Code;
            result.PositionCode = workflow.Position.Code;
            result.PhaseWorkflowId = workflow.Id;
            result.WorkflowId = workflow.WorkflowItem.Id;
            result.PendingType = (WorkflowPendingType)pendingType;
            if (delegateRole != null)
            {
                result.Node = delegateRole.SourcePosition.JobDescription.Node.Name;
                result.JobTitle = delegateRole.SourcePosition.JobDescription.JobTitle.Name;
                result.JobDescription = delegateRole.SourcePosition.JobDescription.Name;

            }
            else
            {
                result.Node = workflow.Position.JobDescription.Node.Name;
                result.JobTitle = workflow.Position.JobDescription.JobTitle.Name;
                result.JobDescription = workflow.Position.JobDescription.Name;

            }

            return result;
        }
        public static AppraisalPhase GetCurrentAppraisalPhase()
        {
            return ServiceFactory.ORMService.All<AppraisalPhase>().ToList()
                .LastOrDefault(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now);
        }
        //public static List<EmployeeInfoViewModel> GetEmployeeForFinalApproval()
        //{
        //    var result = new List<EmployeeInfoViewModel>();
        //    Position currentPosition = null;
        //    var editMarkThreshold = 0.0;

        //    if (EmployeeExtensions.CurrentEmployee == null)
        //    {
        //        return result;
        //    }
        //    currentPosition = EmployeeExtensions.CurrentEmployee.PrimaryPosition();
        //    if (currentPosition == null)
        //    {
        //        return result;
        //    }
        //    var phase = GetCurrentAppraisalPhase();

        //    if (phase == null)
        //        return result;
        //    editMarkThreshold = (phase.AppraisalPhaseSetting.ToMark - phase.AppraisalPhaseSetting.FromMark) * 25 / (float)100;
        //    foreach (var item in phase.PhaseWorkflows.Where(x => !x.HasFinalManagerApproval))
        //    {
        //        var pendingType = WorkflowHelper.GetPendingType(item.WorkflowItem);
        //        if (pendingType != WorkflowPendingType.PendingStep && pendingType != WorkflowPendingType.NewStep)
        //        {
        //            if (item.WorkflowItem.StepCount == 0)
        //            {
        //                if (item.WorkflowItem.FirstUser.GetManagerAsPosition() != currentPosition)
        //                {
        //                    continue;
        //                }
        //            }
        //            else if (item.WorkflowItem.Steps.Last().User.GetManagerAsPosition() != currentPosition)
        //                continue;
        //            if (item.Position.Employee.EmployeeCard.PerformanceAppraisal)
        //                result.Add(GetPmsDataViewModel(phase, item, pendingType));

        //        }


        //    }

        //    return result;
        //}

        public static float GetEditMarkThreshold()
        {

            var phase = GetCurrentAppraisalPhase();
            return (phase.AppraisalPhaseSetting.ToMark - phase.AppraisalPhaseSetting.FromMark) * 25 / (float)100;
        }
    }
}
