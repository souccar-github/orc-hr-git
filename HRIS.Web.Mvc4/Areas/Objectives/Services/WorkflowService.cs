using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using HRIS.Domain.JobDesc.Entities;
using HRIS.Domain.Objectives.Entities;
using HRIS.Domain.Objectives.Enums;
using HRIS.Domain.OrgChart.Entities;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Validation.MessageKeys;
using HRIS.Web.Mvc4.Areas.Objective.Models;
using HRIS.Web.Mvc4.Extensions.Domain;
using HRIS.Web.Mvc4.Helpers;
using HRIS.Web.Mvc4.Models.Workflow.EventHandler;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Workflow.Entities;
using Souccar.Domain.Workflow.Enums;
using Souccar.Infrastructure.Extenstions;
using Souccar.Infrastructure.Core;
using HRIS.Domain.Objectives.RootEntities;
using HRIS.Domain.OrgChart.Indexes;
using Souccar.Domain.Workflow.RootEntities;
using Souccar.Domain.Notification;
using Souccar.Domain.Security;

namespace HRIS.Web.Mvc4.Areas.Objective.Services
{
    public class WorkflowService
    {

        #region Workflow Tree

        public static bool ApplyToAll(IDictionary<string, object> /*model{"per","stepCount","Id"}*/ model, int Id)
        {
            var positions = ServiceFactory.ORMService.All<Position>().ToList();
            SaveWorkflowConfiguration(model, positions, Id);
            return true;
        }

        public static bool ApplyTo(IDictionary<string, object> /*model{"per","stepCount","dropDownName","Id"}*/ model, int Id)
        {
            var positions = new List<Position>();
            if (!string.IsNullOrEmpty(model["Id"].ToString()))
            {
                switch (model["dropDownName"].ToString())
                {
                    case "OrganizationalLevel":
                        if (int.Parse(model["Id"].ToString()) != 0)
                            positions =
                                ServiceFactory.ORMService.All<Position>().Where(
                                    x =>
                                    x.JobDescription.JobTitle.Grade.OrganizationalLevel.Id ==
                                    int.Parse(model["Id"].ToString())).ToList();
                        else
                            return false;
                        break;

                    case "Grade":
                        if (int.Parse(model["Id"].ToString()) != 0)
                            positions =
                                ServiceFactory.ORMService.All<Position>().Where(
                                    x => x.JobDescription.JobTitle.Grade.Id == int.Parse(model["Id"].ToString())).ToList
                                    ();
                        else
                            return false;
                        break;

                    case "JobTitle":
                        if (int.Parse(model["Id"].ToString()) != 0)
                            positions =
                                ServiceFactory.ORMService.All<Position>().Where(
                                    x => x.JobDescription.JobTitle.Id == int.Parse(model["Id"].ToString())).ToList();
                        else
                            return false;
                        break;

                    case "JobDescription":
                        if (int.Parse(model["Id"].ToString()) != 0)
                            positions =
                                ServiceFactory.ORMService.All<Position>().Where(
                                    x => x.JobDescription.Id == int.Parse(model["Id"].ToString())).ToList();
                        else
                            return false;
                        break;

                    case "Position":
                        if (int.Parse(model["Id"].ToString()) != 0)
                            positions =
                                ServiceFactory.ORMService.All<Position>().Where(
                                    x => x.Id == int.Parse(model["Id"].ToString())).ToList();
                        else
                            return false;
                        break;
                }
            }
            SaveWorkflowConfiguration(model, positions, Id);
            return true;
        }

        public static IList<WorkflowTreeViewModel> ViewTree(int Id)
        {
            var groupedData =
                ServiceFactory.ORMService.All<PhaseConfigurationWorkflow>().ToList().Where(
                    x => x.PhaseConfiguration.Id == Id).GroupBy(x => x.OperationNo).Select(
                        x => new {x, Count = x.Count()}).ToList(); //for grouping tree according to OperationNo.

            var organizationalTree = new List<WorkflowTreeViewModel>();

            foreach (var item in groupedData)
            {
                var requiredLevels =
                    item.x.Where(x => x.PhaseConfiguration.Id == Id).Select(
                        x => x.FirstPosition.JobDescription.JobTitle.Grade.OrganizationalLevel).Distinct().ToList();

                foreach (OrganizationalLevel organizationalLevel in requiredLevels)
                    //organizationalLevels for each parent node
                {
                    var treeItem = new WorkflowTreeViewModel();
                    treeItem.Id = organizationalLevel.Id;
                    treeItem.LevelNumber = (int) WorkflowApplyFlag.OrganizationalLevel;
                    treeItem.Name = organizationalLevel.Name;

                    if (item.x.ElementAt(0).WorkflowApplyFlag ==
                        HRIS.Domain.Objectives.Enums.WorkflowApplyFlag.OrganizationalLevel)
                        //To stop adding children items.
                    {
                        string stepCount =
                            item.x.ToList().Select(x => x.StepCount.ToString()).Distinct().FirstOrDefault();
                        if (!string.IsNullOrEmpty(stepCount))
                            treeItem.Name += (" ( " + stepCount + " )");
                        organizationalTree.Add(treeItem);
                        continue;
                    }

                    treeItem.HasChildren = true;
                    treeItem.Items = GetGradeByOrganizationalLevel(organizationalLevel.Id, Id, item.x.ToList());
                    organizationalTree.Add(treeItem);
                }
            }
            return organizationalTree;
        }

        public static int DeleteTreeNode(int nodeId, int levelNumber, int Id)
        {
            var deletedRowsNumber = 0;
            List<PhaseConfigurationWorkflow> phaseConfigurationWorkflows;
            switch (levelNumber)
            {
                case 0: //Org Level
                    phaseConfigurationWorkflows =
                        ServiceFactory.ORMService.All<PhaseConfigurationWorkflow>().Where(
                            x =>
                            x.FirstPosition.JobDescription.JobTitle.Grade.OrganizationalLevel.Id == nodeId &&
                            x.PhaseConfiguration.Id == Id).ToList();
                    deletedRowsNumber = DeleteWorkflow(phaseConfigurationWorkflows);
                    break;
                case 1: //Grade
                    phaseConfigurationWorkflows =
                        ServiceFactory.ORMService.All<PhaseConfigurationWorkflow>().Where(
                            x =>
                            x.FirstPosition.JobDescription.JobTitle.Grade.Id == nodeId && x.PhaseConfiguration.Id == Id)
                            .ToList();
                    deletedRowsNumber = DeleteWorkflow(phaseConfigurationWorkflows);
                    break;
                case 2: //J.T
                    phaseConfigurationWorkflows =
                        ServiceFactory.ORMService.All<PhaseConfigurationWorkflow>().Where(
                            x => x.FirstPosition.JobDescription.JobTitle.Id == nodeId && x.PhaseConfiguration.Id == Id).
                            ToList();
                    deletedRowsNumber = DeleteWorkflow(phaseConfigurationWorkflows);
                    break;
                case 3: //J.D
                    phaseConfigurationWorkflows =
                        ServiceFactory.ORMService.All<PhaseConfigurationWorkflow>().Where(
                            x => x.FirstPosition.JobDescription.Id == nodeId && x.PhaseConfiguration.Id == Id).ToList();
                    deletedRowsNumber = DeleteWorkflow(phaseConfigurationWorkflows);
                    break;
                case 4: //Position
                    phaseConfigurationWorkflows =
                        ServiceFactory.ORMService.All<PhaseConfigurationWorkflow>().Where(
                            x => x.FirstPosition.Id == nodeId && x.PhaseConfiguration.Id == Id).ToList();
                    deletedRowsNumber = DeleteWorkflow(phaseConfigurationWorkflows);
                    break;
            }
            return deletedRowsNumber;
        }

        #endregion

        #region Notifications

        public static void NotifyFirstUsersEvaluationPhaseWorkflows(EvaluationPeriod evaluationPeriod)
        {
            var notifiedPositions =
                ServiceFactory.ORMService.All<ObjectiveEvaluationWorkflow>().Where(
                    x => x.EvaluationPeriod.Id == evaluationPeriod.Id).Select(x => x.Position);

            var notify = new Notify();

            notify.Subject = ServiceFactory.LocalizationService.GetResource(
                CustomMessageKeysObjectiveModule.GetFullKey(
                    CustomMessageKeysObjectiveModule.EvaluationPhaseNotificationSubjectMessage)) ?? "UnKnown Subject";

            var notificationMessageBody = ServiceFactory.LocalizationService.GetResource(
                CustomMessageKeysObjectiveModule.GetFullKey(
                    CustomMessageKeysObjectiveModule.FromDateSignatureNotificationMessage)) +
                                             DateTime.Parse(evaluationPeriod.StartDate.ToShortDateString()) + "\t" +
                                             ServiceFactory.LocalizationService.GetResource(
                                                 CustomMessageKeysObjectiveModule.GetFullKey(
                                                     CustomMessageKeysObjectiveModule.ToDateSignatureNotificationMessage)) +
                                             DateTime.Parse(evaluationPeriod.EndDate.ToShortDateString()) + "\n" +
                                             ServiceFactory.LocalizationService.GetResource(
                                                 CustomMessageKeysObjectiveModule.GetFullKey(
                                                     CustomMessageKeysObjectiveModule.
                                                         EvaluationPhaseNotificationBodyMessage));

            notify.Body = notificationMessageBody; 
            notify.Type = NotificationType.Information;
            notify.Sender = EmployeeExtensions.CurrentEmployee.User();

            bool isNotificationReceiverAdded = false;
            foreach (var position in notifiedPositions)
            {
                notify.AddNotifyReceiver(new NotifyReceiver()
                                             {Date = evaluationPeriod.StartDate, Receiver = position.Employee.User()});
                isNotificationReceiverAdded = true;
            }
            if (isNotificationReceiverAdded)
            {
                //notify.Save();
                var evaluationPeriodNotify = new EvaluationPeriodNotify
                                                 {
                                                     EvaluationPeriod = evaluationPeriod,
                                                     Notify = notify
                                                 };
                //evaluationPeriodNotify.Save();
                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot> {notify, evaluationPeriodNotify});
            }
        }

        public static void NotifyFirstUsersPhasePeriodWorkflows(PhasePeriod phasePeriod)
        {
            var notifiedPositions =
                ServiceFactory.ORMService.All<ObjectivePhaseWorkflow>().Where(
                    x => x.PhasePeriod.Id == phasePeriod.Id).Select(x => x.Position);

            var notify = new Notify();
            notify.Subject = ServiceFactory.LocalizationService.GetResource(
                CustomMessageKeysObjectiveModule.GetFullKey(
                    CustomMessageKeysObjectiveModule.PhasePeriodNotificationSubjectMessage)) ?? "UnKnown Subject";

            var notificationMessageBody = ServiceFactory.LocalizationService.GetResource(
                CustomMessageKeysObjectiveModule.GetFullKey(
                    CustomMessageKeysObjectiveModule.FromDateSignatureNotificationMessage)) +
                                             DateTime.Parse(phasePeriod.StartDate.ToShortDateString()) + "\t" +
                                             ServiceFactory.LocalizationService.GetResource(
                                                 CustomMessageKeysObjectiveModule.GetFullKey(
                                                     CustomMessageKeysObjectiveModule.ToDateSignatureNotificationMessage)) +
                                             DateTime.Parse(phasePeriod.EndDate.ToShortDateString()) + "\n" +
                                             ServiceFactory.LocalizationService.GetResource(
                                                 CustomMessageKeysObjectiveModule.GetFullKey(
                                                     CustomMessageKeysObjectiveModule.
                                                         PhasePeriodNotificationBodyMessage));

            notify.Body = notificationMessageBody;
            notify.Type = NotificationType.Information;
            notify.Sender = EmployeeExtensions.CurrentEmployee.User();

            var isNotificationReceiverAdded = false;
            foreach (var position in notifiedPositions)
            {
                notify.AddNotifyReceiver(new NotifyReceiver()
                                             {Date = phasePeriod.StartDate, Receiver = position.Employee.User()});
                isNotificationReceiverAdded = true;
            }
            if (isNotificationReceiverAdded)
            {
                //notify.Save();
                var phasePeriodNotify = new PhasePeriodNotify
                                            {
                                                PhasePeriod = phasePeriod, 
                                                Notify = notify
                                            };
                //phasePeriodNotify.Save();
                ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot> {notify, phasePeriodNotify});
            }
        }

        public static void NotifyCurrentUserManager(WorkflowItem workflow)
        {
            //----------------------------Tasks----------------------------
            //1- Examine that the workflow is gotten from (phase OR evaluation).
            //2- Save notify for this user.
            //--------------------------------------------------------------
            var notify = new Notify
                             {Type = NotificationType.Information, Sender = EmployeeExtensions.CurrentEmployee.User()};
            var manager = EmployeeExtensions.CurrentEmployee.Manager();
            if (manager != null)
                notify.AddNotifyReceiver(new NotifyReceiver()
                                             {
                                                 Date = DateTime.Now,
                                                 Receiver = manager.User()
                                             });

            var phase = FindObjectiveWorkflowPhaseByWorkflow(workflow);

            if (phase is PhasePeriod)
            {
                notify.Subject = ServiceFactory.LocalizationService.GetResource(
                    CustomMessageKeysObjectiveModule.GetFullKey(
                        CustomMessageKeysObjectiveModule.PhasePeriodNotificationSubjectMessage)) ?? "UnKnown Subject";

                notify.Body = ServiceFactory.LocalizationService.GetResource(
                    CustomMessageKeysObjectiveModule.GetFullKey(
                        CustomMessageKeysObjectiveModule.FromDateSignatureNotificationMessage)) +
                              DateTime.Parse(DateTime.Now.ToShortDateString()) + "\t" +
                              ServiceFactory.LocalizationService.GetResource(
                                  CustomMessageKeysObjectiveModule.GetFullKey(
                                      CustomMessageKeysObjectiveModule.ToDateSignatureNotificationMessage)) +
                              DateTime.Parse(phase.EndDate.ToShortDateString()) + "\n" +
                              ServiceFactory.LocalizationService.GetResource(
                                  CustomMessageKeysObjectiveModule.GetFullKey(
                                      CustomMessageKeysObjectiveModule.PhasePeriodNotificationBodyMessage
                                      ));
                if (notify.Receivers.Count > 0)
                    notify.Save();
            }

            if (phase is EvaluationPeriod)
            {
                notify.Subject = ServiceFactory.LocalizationService.GetResource(
                    CustomMessageKeysObjectiveModule.GetFullKey(
                        CustomMessageKeysObjectiveModule.EvaluationPhaseNotificationSubjectMessage)) ??
                                 "UnKnown Subject";

                notify.Body = ServiceFactory.LocalizationService.GetResource(
                    CustomMessageKeysObjectiveModule.GetFullKey(
                        CustomMessageKeysObjectiveModule.FromDateSignatureNotificationMessage)) +
                              DateTime.Parse(DateTime.Now.ToShortDateString()) + "\t" +
                              ServiceFactory.LocalizationService.GetResource(
                                  CustomMessageKeysObjectiveModule.GetFullKey(
                                      CustomMessageKeysObjectiveModule.ToDateSignatureNotificationMessage)) +
                              DateTime.Parse(phase.EndDate.ToShortDateString()) + "\n" +
                              ServiceFactory.LocalizationService.GetResource(
                                  CustomMessageKeysObjectiveModule.GetFullKey(
                                      CustomMessageKeysObjectiveModule.EvaluationPhaseNotificationBodyMessage
                                      ));
                if (notify.Receivers.Count > 0)
                    notify.Save();
            }
        }

        public static void NotifyRejectedUserFromCurrentUser(WorkflowItem workflow)
        {
            WorkflowPendingType type;
            var position = WorkflowHelper.GetNextAppraiser(workflow,out type);//The current appraiser that the workflow waiting his approval.

            if (position != null)
            {
                var notify = new Notify
                                 {
                                     Type = NotificationType.Information,
                                     Sender = EmployeeExtensions.CurrentEmployee.User()
                                 };
                notify.AddNotifyReceiver(new NotifyReceiver()
                                             {
                                                 Date = DateTime.Now,
                                                 Receiver = position.Employee.User()
                                             });

                notify.Subject = ServiceFactory.LocalizationService.GetResource(
                    CustomMessageKeysObjectiveModule.GetFullKey(
                        CustomMessageKeysObjectiveModule.RejectedObjectiveSubjectMessage)) ?? "UnKnown Subject";

                notify.Body = ServiceFactory.LocalizationService.GetResource(
                    CustomMessageKeysObjectiveModule.GetFullKey(
                        CustomMessageKeysObjectiveModule.RejectedObjectiveBodyMessage)) + " " + position.Manager.NameForDropdown;
                         
                if (notify.Receivers.Count > 0)
                    notify.Save();
            }
        }

        //Need a delete transaction.
        public static void UnNotifyFirstUsersPhasePeriodWorkflows(PhasePeriod phasePeriod)
        {
            var phasePeriodNotify =
                ServiceFactory.ORMService.All<PhasePeriodNotify>().SingleOrDefault(
                    x => x.PhasePeriod.Id == phasePeriod.Id);
            if (phasePeriodNotify != null)
            {
                var notify = phasePeriodNotify.Notify;
                ServiceFactory.ORMService.Delete(phasePeriodNotify);
                ServiceFactory.ORMService.Delete(notify);
            }
        }

        //Need a delete transaction.
        public static void UnNotifyFirstUsersEvaluationPeriodWorkflows(EvaluationPeriod evaluationPeriod)
        {
            var evaluationPeriodNotify =
                ServiceFactory.ORMService.All<EvaluationPeriodNotify>().SingleOrDefault(
                    x => x.EvaluationPeriod.Id == evaluationPeriod.Id);
            if (evaluationPeriodNotify != null)
            {
                var notify = evaluationPeriodNotify.Notify;
                ServiceFactory.ORMService.Delete(evaluationPeriodNotify);
                ServiceFactory.ORMService.Delete(notify);
            }
        }

        #endregion

        #region Apply workflows

        public static bool ApplyObjectiveEvaluationPeriodWorkflows(
            HRIS.Domain.Objectives.RootEntities.EvaluationPeriod evaluationPhase)
        {
            //Insert new workflow Items (Phase).

            //Get Actual objectives. (Which action plans are (InProgress && Closed)).
            var correspondingActualObjectives =
                ServiceFactory.ORMService.All<HRIS.Domain.Objectives.RootEntities.Objective>().
                    Where(x => x.ActionPlans.Count > 0).
                    Where(x =>
                          (
                                //(x.ActionPlans.Min(y => y.ActualStartingDate) >= evaluationPhase.StartDate && x.ActionPlans.Max(y => y.ActualClosingDate) <= evaluationPhase.StartDate) ||
                                //(evaluationPhase.EndDate >= x.ActionPlans.Min(y => y.ActualStartingDate) && evaluationPhase.EndDate <= x.ActionPlans.Max(y => y.ActualClosingDate))

                                 (x.ActionPlans.Min(y => y.ActualStartingDate) >= evaluationPhase.StartDate && x.ActionPlans.Min(y => y.ActualStartingDate) <= evaluationPhase.EndDate) ||
                                (evaluationPhase.StartDate >= x.ActionPlans.Min(y => y.ActualStartingDate) && evaluationPhase.StartDate <= x.ActionPlans.Max(y => y.ActualClosingDate)) ||
                                (x.ActionPlans.Max(y => y.ActualClosingDate) >= evaluationPhase.StartDate && x.ActionPlans.Max(y => y.ActualClosingDate) <= evaluationPhase.EndDate) ||
                                (evaluationPhase.EndDate >= x.ActionPlans.Min(y => y.ActualStartingDate) && evaluationPhase.EndDate <= x.ActionPlans.Max(y => y.ActualClosingDate))

                              //CompareDatesApplyingConvention(x.ActionPlans.Min(y => y.ActualStartingDate),
                              //                       x.ActionPlans.Max(y => y.ActualClosingDate),
                              //                       evaluationPhase.StartDate, evaluationPhase.EndDate)==true
                          )
                          &&
                          (x.ActionPlans.Count(
                              y => y.Status == ActionPlanStatus.InProgress || y.Status == ActionPlanStatus.Closed) > 0)
                    )
                    .ToList();

            //Get planned objectives. (Which action plans are (Accepted -Is not tracked or closed)).
            var correspondingObjectives =
                ServiceFactory.ORMService.All
                    <HRIS.Domain.Objectives.RootEntities.Objective>().
                    Where(
                        x =>
                        x.ActionPlans.Count(y => y.Status == ActionPlanStatus.Accepted) > 0 &&

                        // (
                        //        (x.PlannedStartingDate >= evaluationPhase.StartDate && x.PlannedClosingDate <= evaluationPhase.StartDate) ||
                        //        (evaluationPhase.EndDate >= x.PlannedStartingDate && evaluationPhase.EndDate <= x.PlannedClosingDate)
                        //)

                          (x.PlannedStartingDate >= evaluationPhase.StartDate && x.PlannedStartingDate <= evaluationPhase.EndDate) ||
                                (evaluationPhase.StartDate >= x.PlannedStartingDate && evaluationPhase.StartDate <= x.PlannedClosingDate) ||
                                (x.PlannedClosingDate >= evaluationPhase.StartDate && x.PlannedClosingDate <= evaluationPhase.EndDate) ||
                                (evaluationPhase.EndDate >= x.PlannedStartingDate && evaluationPhase.EndDate <= x.PlannedClosingDate)

                        //CompareDatesApplyingConvention(x.PlannedStartingDate, x.PlannedClosingDate,
                        //                       evaluationPhase.StartDate, evaluationPhase.EndDate)==true
                    ).ToList();

            correspondingObjectives.AddRange(correspondingActualObjectives);

            foreach (var correspondingObjective in correspondingObjectives)
            {
                var phaseWorkflows = evaluationPhase.PhaseConfiguration.PhaseConfigurationWorkflows;
                foreach (var phaseConfigurationWorkflow in phaseWorkflows)
                {
                    //New workflow initialization & save.
                    var workflow = new WorkflowItem
                                       {
                                           Status = WorkflowStatus.Pending,
                                           StepCount = phaseConfigurationWorkflow.StepCount,
                                           Type = WorkflowType.Objective,
                                           Description =
                                               DoesNotMeetCriteriaMessage + correspondingObjective.DoesNotMeet + "<br/>" +
                                               MeetCriteriaMessage + correspondingObjective.Meet +
                                               "<br/>" + AboveMeetCriteriaMessage + correspondingObjective.Above,
                                           Date = evaluationPhase.EndDate,
                                           Creator = EmployeeExtensions.CurrentEmployee.User()
                                       };
                    if(phaseConfigurationWorkflow.FirstPosition.Employee==null)
                        continue;
                    workflow.FirstUser = phaseConfigurationWorkflow.FirstPosition.Employee.User();
                    //workflow.Save();

                    var objectiveEvaluationWorkflow = new ObjectiveEvaluationWorkflow
                                                          {
                                                              EvaluationPeriod = evaluationPhase,
                                                              Position = phaseConfigurationWorkflow.FirstPosition,
                                                              Objective = correspondingObjective,
                                                              Workflow = workflow,
                                                              OperationNo = phaseConfigurationWorkflow.OperationNo,
                                                              WorkflowApplyFlag =
                                                               phaseConfigurationWorkflow.WorkflowApplyFlag
                                                          };
                    //objectiveEvaluationWorkflow.Save();
                    ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>
                                                                  {workflow, objectiveEvaluationWorkflow});
                }
            }
            return true;
        }

        public static bool ApplyObjectivePhasePeriodWorkflows(
            HRIS.Domain.Objectives.RootEntities.PhasePeriod phasePeriod)
        {
            //Insert new workflow Items (Phase).
            //Get planned objectives.
            var correspondingObjectives =
                ServiceFactory.ORMService.All<HRIS.Domain.Objectives.RootEntities.Objective>().
                    Where(
                        x =>
                        x.ActionPlans.All(y => y.Status == ActionPlanStatus.Pending) &&

                        //   (
                        //        (x.PlannedStartingDate >= phasePeriod.StartDate && x.PlannedClosingDate <= phasePeriod.StartDate) ||
                        //        (phasePeriod.EndDate >= x.PlannedStartingDate && phasePeriod.EndDate <= x.PlannedClosingDate)
                        //)

                                (x.PlannedStartingDate >= phasePeriod.StartDate && x.PlannedStartingDate <= phasePeriod.EndDate) ||
                                (phasePeriod.StartDate >= x.PlannedStartingDate && phasePeriod.StartDate <= x.PlannedClosingDate) ||
                                (x.PlannedClosingDate >= phasePeriod.StartDate && x.PlannedClosingDate <= phasePeriod.EndDate) ||
                                (phasePeriod.EndDate >= x.PlannedStartingDate && phasePeriod.EndDate <= x.PlannedClosingDate)

                        //CompareDatesApplyingConvention(x.PlannedStartingDate, x.PlannedClosingDate, phasePeriod.StartDate,
                        //                       phasePeriod.EndDate)==true
                    ).ToList();

            foreach (var correspondingObjective in correspondingObjectives)
            {
                var phaseWorkflows = phasePeriod.PhaseConfiguration.PhaseConfigurationWorkflows;
                foreach (var phaseConfigurationWorkflow in phaseWorkflows)
                {
                    //New workflow initialization & save.
                    var workflow = new WorkflowItem
                                       {
                                           Status = WorkflowStatus.Pending,
                                           StepCount = phaseConfigurationWorkflow.StepCount,
                                           Type = WorkflowType.Objective,
                                           Description =
                                               DoesNotMeetCriteriaMessage
                                                + correspondingObjective.DoesNotMeet + "<br/>" +
                                               MeetCriteriaMessage + correspondingObjective.Meet +
                                                                      "<br/>" + AboveMeetCriteriaMessage
                                                + correspondingObjective.Above,
                                           Date = phasePeriod.EndDate,
                                           Creator = EmployeeExtensions.CurrentEmployee.User()
                                       };
                    if (phaseConfigurationWorkflow.FirstPosition.Employee == null)
                        continue;
                    workflow.FirstUser = phaseConfigurationWorkflow.FirstPosition.Employee.User();
                    //workflow.Save();

                    var objectivePhaseWorkflow = new ObjectivePhaseWorkflow
                                                     {
                                                         PhasePeriod = phasePeriod,
                                                         Position = phaseConfigurationWorkflow.FirstPosition,
                                                         Objective = correspondingObjective,
                                                         Workflow = workflow,
                                                         OperationNo = phaseConfigurationWorkflow.OperationNo,
                                                         WorkflowApplyFlag = phaseConfigurationWorkflow.WorkflowApplyFlag
                                                     };
                    //objectivePhaseWorkflow.Save();
                    ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>
                                                                  {workflow, objectivePhaseWorkflow});
                }
            }   
            return true;
        }

        //Need a multiple entity transaction.
        public static bool ApplyObjectiveAllPhasesWorkflows(HRIS.Domain.Objectives.RootEntities.Objective objective)
        {
            //Insert new workflow Items (Phase).
            var correspondingPhasePeriods = new List<PhasePeriod>();
            var correspondingEvaluationPeriods = new List<EvaluationPeriod>();

            if (objective.ActionPlans.Count(x => x.Status == ActionPlanStatus.InProgress) == 0)
                //Comparing by planned date.
            {
                correspondingPhasePeriods = ServiceFactory.ORMService.All<PhasePeriod>().Where(x =>

                        //                           (
                    //        (objective.PlannedStartingDate >= x.StartDate && objective.PlannedClosingDate <= x.StartDate) ||
                    //        (x.EndDate >= objective.PlannedStartingDate && x.EndDate <= objective.PlannedClosingDate)
                    //)

                                                    (objective.PlannedStartingDate >= x.StartDate && objective.PlannedStartingDate <= x.EndDate) ||
                                (x.StartDate >= objective.PlannedStartingDate && x.StartDate <= objective.PlannedClosingDate) ||
                                (objective.PlannedClosingDate >= x.StartDate && objective.PlannedClosingDate <= x.EndDate) ||
                                (x.EndDate >= objective.PlannedStartingDate && x.EndDate <= objective.PlannedClosingDate)

                                                                       //CompareDatesApplyingConvention(
                                                                       //    objective.PlannedStartingDate,
                                                                       //    objective.PlannedClosingDate, x.StartDate,
                                                                       //    x.EndDate) == true
                        ).
                        ToList();

                correspondingEvaluationPeriods =
                    ServiceFactory.ORMService.All<EvaluationPeriod>().Where(x =>

                        //                                                   (
                        //        (objective.PlannedStartingDate >= x.StartDate && objective.PlannedClosingDate <= x.StartDate) ||
                        //        (x.EndDate >= objective.PlannedStartingDate && x.EndDate <= objective.PlannedClosingDate)
                        //)

                                     (objective.PlannedStartingDate >= x.StartDate && objective.PlannedStartingDate <= x.EndDate) ||
                                (x.StartDate >= objective.PlannedStartingDate && x.StartDate <= objective.PlannedClosingDate) ||
                                (objective.PlannedClosingDate >= x.StartDate && objective.PlannedClosingDate <= x.EndDate) ||
                                (x.EndDate >= objective.PlannedStartingDate && x.EndDate <= objective.PlannedClosingDate)

                        //CompareDatesApplyingConvention(
                        //                                                   objective.PlannedStartingDate,
                        //                                                   objective.PlannedClosingDate, x.StartDate,
                        //                                                   x.EndDate)==true
                ).
                        ToList();
            }
            else //Comparing by actual date.
            {
                correspondingPhasePeriods =
                    ServiceFactory.ORMService.All<PhasePeriod>().Where(
                        x =>

                        //                                                       (
                        //        (objective.ActionPlans.Min(y => y.ActualStartingDate) >= x.StartDate && objective.ActionPlans.Max(y => y.ActualClosingDate) <= x.StartDate) ||
                        //        (x.EndDate >= objective.ActionPlans.Min(y => y.ActualStartingDate) && x.EndDate <= objective.ActionPlans.Max(y => y.ActualClosingDate))
                        //)

                        (objective.ActionPlans.Min(y => y.ActualStartingDate) >= x.StartDate && objective.ActionPlans.Min(y => y.ActualStartingDate) <= x.EndDate) ||
                                (x.StartDate >= objective.ActionPlans.Min(y => y.ActualStartingDate) && x.StartDate <= objective.ActionPlans.Max(y => y.ActualClosingDate)) ||
                                (objective.ActionPlans.Max(y => y.ActualClosingDate) >= x.StartDate && objective.ActionPlans.Max(y => y.ActualClosingDate) <= x.EndDate) ||
                                (x.EndDate >= objective.ActionPlans.Min(y => y.ActualStartingDate) && x.EndDate <= objective.ActionPlans.Max(y => y.ActualClosingDate))


                        //CompareDatesApplyingConvention(
                        //    objective.ActionPlans.Min(y => y.ActualStartingDate),
                        //    objective.ActionPlans.Max(y => y.ActualClosingDate),
                        //    x.StartDate,
                        //    x.EndDate)==true
                        ).
                        ToList();

                correspondingEvaluationPeriods =
                    ServiceFactory.ORMService.All<EvaluationPeriod>().Where(
                        x =>

                        //(
                        //        (objective.ActionPlans.Min(y => y.ActualStartingDate) >= x.StartDate && objective.ActionPlans.Max(y => y.ActualClosingDate) <= x.StartDate) ||
                        //        (x.EndDate >= objective.ActionPlans.Min(y => y.ActualStartingDate) && x.EndDate <= objective.ActionPlans.Max(y => y.ActualClosingDate))
                        //)

                               (objective.ActionPlans.Min(y => y.ActualStartingDate) >= x.StartDate && objective.ActionPlans.Min(y => y.ActualStartingDate) <= x.EndDate) ||
                                (x.StartDate >= objective.ActionPlans.Min(y => y.ActualStartingDate) && x.StartDate <= objective.ActionPlans.Max(y => y.ActualClosingDate)) ||
                                (objective.ActionPlans.Max(y => y.ActualClosingDate) >= x.StartDate && objective.ActionPlans.Max(y => y.ActualClosingDate) <= x.EndDate) ||
                                (x.EndDate >= objective.ActionPlans.Min(y => y.ActualStartingDate) && x.EndDate <= objective.ActionPlans.Max(y => y.ActualClosingDate))

                        //CompareDatesApplyingConvention(
                        //    objective.ActionPlans.Min(y => y.ActualStartingDate),
                        //    objective.ActionPlans.Max(y => y.ActualClosingDate),
                        //    x.StartDate,
                        //    x.EndDate)==true
                        ).
                        ToList();
            }

            foreach (var correspondingPhasePeriod in correspondingPhasePeriods)
                ApplySpecificObjectiveWorkflows(correspondingPhasePeriod, objective);

            foreach (var correspondingEvaluationPeriod in correspondingEvaluationPeriods)
                ApplySpecificObjectiveWorkflows(correspondingEvaluationPeriod, objective);
            return true;
        }

        #endregion

        #region Unapply workflows

        //Need a delete transaction.
        public static bool UnapplyObjectiveAllPhasesWorkflowItems(
            HRIS.Domain.Objectives.RootEntities.Objective objective)
        {

            var objectivePhaseWorkflows =
                ServiceFactory.ORMService.All<ObjectivePhaseWorkflow>().Where(x => x.Objective.Id == objective.Id);
            foreach (var objectivePhaseWorkflow in objectivePhaseWorkflows)
            {
                ServiceFactory.ORMService.Delete(objectivePhaseWorkflow);
                ServiceFactory.ORMService.Delete(objectivePhaseWorkflow.Workflow);
            }

            var objectiveEvaluationWorkflows =
                ServiceFactory.ORMService.All<ObjectiveEvaluationWorkflow>().Where(
                    x => x.Objective.Id == objective.Id);
            foreach (var objectiveEvaluationWorkflow in objectiveEvaluationWorkflows)
            {
                ServiceFactory.ORMService.Delete(objectiveEvaluationWorkflow);
                ServiceFactory.ORMService.Delete(objectiveEvaluationWorkflow.Workflow);
            }

            return true;
        }

        //Need a delete transaction.
        public static bool UnapplyObjectiveEvaluationPeriodWorkflowItems(
            HRIS.Domain.Objectives.RootEntities.EvaluationPeriod evaluationPhase)
        {
            var objectiveEvaluationWorkflows =
                ServiceFactory.ORMService.All<ObjectiveEvaluationWorkflow>().Where(
                    x => x.EvaluationPeriod.Id == evaluationPhase.Id);
            foreach (var objectiveEvaluationWorkflow in objectiveEvaluationWorkflows)
            {
                ServiceFactory.ORMService.Delete(objectiveEvaluationWorkflow);
                ServiceFactory.ORMService.Delete(objectiveEvaluationWorkflow.Workflow);
            }
            return true;
        }

        //Need a delete transaction.
        public static bool UnapplyObjectivePhasePeriodWorkflowItems(
            HRIS.Domain.Objectives.RootEntities.PhasePeriod phasePeriod)
        {
            var objectivePhaseWorkflows =
                ServiceFactory.ORMService.All<ObjectivePhaseWorkflow>().Where(
                    x => x.PhasePeriod.Id == phasePeriod.Id);
            foreach (var objectivePhaseWorkflow in objectivePhaseWorkflows)
            {
                ServiceFactory.ORMService.Delete(objectivePhaseWorkflow);
                ServiceFactory.ORMService.Delete(objectivePhaseWorkflow.Workflow);
            }
            return true;
        }

        #endregion

        #region Workflow approval operations

        public static AbstractPhase FindObjectiveWorkflowPhaseByWorkflow(WorkflowItem workflow)
        {
            var objectivePhaseWorkflow = ServiceFactory.ORMService.All
                <HRIS.Domain.Objectives.Entities.ObjectivePhaseWorkflow>().
                SingleOrDefault(x => x.Workflow.Id == workflow.Id);
            if (objectivePhaseWorkflow != null)
                return objectivePhaseWorkflow.PhasePeriod;

            var objectiveEvaluationWorkflow = ServiceFactory.ORMService.All
                <HRIS.Domain.Objectives.Entities.ObjectiveEvaluationWorkflow>().
                SingleOrDefault(x => x.Workflow.Id == workflow.Id);
            return objectiveEvaluationWorkflow != null ? objectiveEvaluationWorkflow.EvaluationPeriod : null;
        }

        public static bool IsAllRelatedWorkflowsAccepted(WorkflowItem workflow)
        {
            //Search in "ObjectivePhaseWorkflow"
            var objectivePhaseWorkflow = ServiceFactory.ORMService.All<ObjectivePhaseWorkflow>().SingleOrDefault(x => x.Workflow.Id == workflow.Id);
            if (objectivePhaseWorkflow != null)
            {
                var phasePeriodRelatedWorkflows =
                    ServiceFactory.ORMService.All<ObjectivePhaseWorkflow>().Where(
                        x =>
                        x.PhasePeriod.Id == objectivePhaseWorkflow.PhasePeriod.Id &&
                        x.Objective.Id == objectivePhaseWorkflow.Objective.Id &&
                        x.Workflow.Id != workflow.Id).Select(y => y.Workflow);//Except the current workflow.

                var isNotAllWorkflowsAccepted =
                    phasePeriodRelatedWorkflows.Any(x => x.Status != WorkflowStatus.Completed);

                if (!isNotAllWorkflowsAccepted)
                    return true;
            }
            else//Search in "ObjectiveEvaluationWorkflow"
            {
                var objectiveEvaluationWorkflow = ServiceFactory.ORMService.All<ObjectiveEvaluationWorkflow>().SingleOrDefault(x => x.Workflow.Id == workflow.Id);
                if (objectiveEvaluationWorkflow != null)
                {
                    var evaluationPeriodRelatedWorkflows =
                    ServiceFactory.ORMService.All<ObjectiveEvaluationWorkflow>().Where(
                        x =>
                        x.EvaluationPeriod.Id == objectiveEvaluationWorkflow.EvaluationPeriod.Id &&
                        x.Objective.Id == objectiveEvaluationWorkflow.Objective.Id &&
                        x.Workflow.Id != workflow.Id).Select(y => y.Workflow);//Except the current workflow.

                    var isNotAllWorkflowsAccepted =
                        evaluationPeriodRelatedWorkflows.Any(x => x.Status != WorkflowStatus.Completed);

                    if (!isNotAllWorkflowsAccepted)
                        return true;
                }
            }
            return false;
        }

        public static void DeclineObjective(WorkflowItem workflow)
        {
            ////-------------------------Tasks-------------------------
            ////1- Search about the objective by workflow.
            ////2- Change the whole objective action plan status to be "Cancelled".
            ////3- Change the workflow status to be "InProgress".
            ////---------------------------------------------------------

            //1- Search about the objective by workflow.
            var objective = FindObjectivePhasePeriodByWorkflow(workflow);
            //2- Change the whole objective action plan status to be "InProgress".
            if (objective != null)
            {
                foreach (var actionPlan in objective.ActionPlans)
                    actionPlan.Status = ActionPlanStatus.InProgress;
                objective.Save();
            }
            //3- Change the workflow status to be "InProgress".
            workflow.Status = WorkflowStatus.InProgress;
            workflow.Save();
        }

        public static void PendingObjective(WorkflowItem workflow)
        {
            //-------------------------Tasks-------------------------
            //1- Search about the objective by workflow.
            //2- Change the whole objective action plan status to be "Pending".
            //3- Change the workflow status to be "Pending".
            //---------------------------------------------------------

            //1- Search about the objective by workflow.
            var objective = FindObjectivePhasePeriodByWorkflow(workflow);
            //2- Change the whole objective action plan status to be "Pending".
            if (objective != null)
            {
                foreach (var actionPlan in objective.ActionPlans)
                    actionPlan.Status = ActionPlanStatus.Pending;
                objective.Save();
            }
            //3- Change the workflow status to be "Pending".
            workflow.Status = WorkflowStatus.Pending;
            workflow.Save();
        }

        public static void AcceptObjective(WorkflowItem workflow)
        {
            //-------------------------Tasks-------------------------
            //1- Search about the objective by workflow.
            //2- Change the whole objective action plan status to be "Accepted".
            //3- Change the workflow status to be "Completed".
            //---------------------------------------------------------
            //1- Search about the objective by workflow.
            var objective = FindObjectivePhasePeriodByWorkflow(workflow);
            //2- Change the whole objective action plan status to be "Accepted".
            if (objective != null)
            {
                foreach (var actionPlan in objective.ActionPlans)
                    actionPlan.Status = ActionPlanStatus.Accepted;
                objective.Save();
            }
            //3- Change the workflow status to be "Completed".
            workflow.Status = WorkflowStatus.Completed;
            workflow.Save();
        }

        public static bool IsWorkflowFinished(WorkflowItem workflow)
        {
            //Address finish workflow manually
                var acceptCount = workflow.Steps.Count(x => x.Status == WorkflowStepStatus.Accept) -
                             workflow.Steps.Count(x => x.Status == WorkflowStepStatus.Reject);
                if (acceptCount < workflow.StepCount)
                    return false;
            return workflow.Steps.Count(x => x.Status == WorkflowStepStatus.Accept) != 0;
        }

        public static bool IsPhaseFinished(AbstractPhase phase)
        {
            var isClosed = false;
            if (phase is PhasePeriod)
                isClosed =
                    ((PhasePeriod) phase).ObjectivePhaseWorkflows.All(
                        x => x.Workflow.Status == WorkflowStatus.Completed);
            if (phase is EvaluationPeriod)
                isClosed =
                    ((EvaluationPeriod) phase).ObjectiveEvaluationWorkflows.All(
                        x => x.Workflow.Status == WorkflowStatus.Completed);

            return isClosed;
        }

        //need a review.
        public static void SaveObjectiveStep(int workflowId, WorkflowStepStatus status, string description, User currentUser)
        {
            var workflow = ((WorkflowItem)(typeof(WorkflowItem).GetById(workflowId)));
            const string eventHandlerName = "HRIS.Web.Mvc4.Models.Workflow.EventHandler.ObjectiveWorkflowEventHandler";
            var eventHandlerType = Type.GetType(eventHandlerName);            
            var handler = new WorkflowEventHandler();

            if (eventHandlerType != null)
                handler = (WorkflowEventHandler)Activator.CreateInstance(eventHandlerType);
            
            var acceptCount = workflow.Steps.Count(x => x.Status == WorkflowStepStatus.Accept) -
                              workflow.Steps.Count(x => x.Status == WorkflowStepStatus.Reject);
            if (acceptCount < workflow.StepCount)
            {
                var step = new WorkflowStep()
                {
                    Date = DateTime.Now,
                    User = currentUser,
                    Description = description,
                    Status = status
                };
                handler.BeforeInsertStep(workflow, step);
                workflow.AddStep(step);
                workflow.Save();
                handler.AfterInsertStep(workflow, step);
            }
        }

        #endregion

        #region Prevented operations

        public static bool PreventedPhaseConfigurationOperation(PhaseConfiguration phaseConfiguration)
        {
            var isInPhasePeriod =
                ServiceFactory.ORMService.All<PhasePeriod>().Any(
                    x => x.PhaseConfiguration.Id == phaseConfiguration.Id);
            var isInEvaluationPeriod =
                ServiceFactory.ORMService.All<EvaluationPeriod>().Any(
                    x => x.PhaseConfiguration.Id == phaseConfiguration.Id);

            return isInPhasePeriod || isInEvaluationPeriod;
        }

        public static bool PreventedObjectiveOperation(HRIS.Domain.Objectives.RootEntities.Objective objective)
        {
            var isPhaseObjectiveWorkflowBegan =
                objective.ObjectivePhaseWorkflows.Any(
                    x => (x.Workflow.Status != WorkflowStatus.Pending  || DateTime.Now >= x.PhasePeriod.StartDate));
            var isEvaluationObjectiveWorkflowBegan =
                objective.ObjectiveEvaluationWorkflows.Any(
                    x => (x.Workflow.Status != WorkflowStatus.Pending || DateTime.Now >= x.EvaluationPeriod.StartDate));
            return isEvaluationObjectiveWorkflowBegan || isPhaseObjectiveWorkflowBegan;
        }

        public static bool PreventedActionPlanOperation(ActionPlan actionPlan)
        {
            var isEvaluationObjectiveWorkflowBegan = actionPlan.Objective.ObjectiveEvaluationWorkflows.Any(
                    x => (x.Workflow.Status != WorkflowStatus.Pending || DateTime.Now >= x.EvaluationPeriod.StartDate));
            return isEvaluationObjectiveWorkflowBegan;
        }

        public static bool PreventedPhaseOperation(AbstractPhase phase)
        {
            var result=false;
            if(phase is PhasePeriod)//Phase Period
            {
                result=
                    ((PhasePeriod) phase).ObjectivePhaseWorkflows.Any(
                        x =>
                        (x.Workflow.Status != WorkflowStatus.Pending || DateTime.Now >= x.PhasePeriod.StartDate));
            }
            if (phase is EvaluationPeriod)//Evaluation Period
            {
                result =
                    ((EvaluationPeriod)phase).ObjectiveEvaluationWorkflows.Any(
                        x =>
                        (x.Workflow.Status != WorkflowStatus.Pending || DateTime.Now >= x.EvaluationPeriod.StartDate));
            }
            return result;
        }

        #endregion

        #region Assistant Methods

        #region Workflow Tree

        //Need a review about transaction scope.
        private static void SaveWorkflowConfiguration(IDictionary<string, object>/*model{"per","stepCount"}*/ model, IList<Position> positions, int Id/*Phase config Id*/)
        {
                var maxOperationNo = 0;
                var lastPhaseConfigurationWorkflow =
                    ServiceFactory.ORMService.All<PhaseConfigurationWorkflow>().Where(x => x.PhaseConfiguration.Id == Id)
                        .OrderByDescending(x => x.OperationNo).FirstOrDefault(); //Max operation number
                if (lastPhaseConfigurationWorkflow != null)
                    maxOperationNo = lastPhaseConfigurationWorkflow.OperationNo;
                foreach (var position in positions)
                {
                    var template = ServiceFactory.ORMService.All<PhaseConfigurationWorkflow>().
                        Where(x => x.PhaseConfiguration.Id == Id).
                        SingleOrDefault(x => x.FirstPosition.Id == position.Id);
                    PhaseConfigurationWorkflow phaseConfigurationWorkflow;
                    if (template == null) //for insert
                    {
                        phaseConfigurationWorkflow = new PhaseConfigurationWorkflow();
                        if (model["per"] != null)
                            phaseConfigurationWorkflow.WorkflowApplyFlag =
                                (HRIS.Domain.Objectives.Enums.WorkflowApplyFlag)
                                Enum.Parse(typeof (HRIS.Domain.Objectives.Enums.WorkflowApplyFlag),
                                           model["per"].ToString());
                        phaseConfigurationWorkflow.PhaseConfiguration =
                            ServiceFactory.ORMService.GetById<PhaseConfiguration>(Id);
                        phaseConfigurationWorkflow.StepCount = int.Parse(model["stepCount"].ToString());
                        phaseConfigurationWorkflow.FirstPosition = position;
                        phaseConfigurationWorkflow.OperationNo = maxOperationNo + 1;
                        phaseConfigurationWorkflow.Save();
                    }
                    else //for edit
                    {
                        phaseConfigurationWorkflow =
                            ServiceFactory.ORMService.GetById<PhaseConfigurationWorkflow>(template.Id);

                        if (template.StepCount != int.Parse(model["stepCount"].ToString()))
                            template.OperationNo = maxOperationNo + 1;

                        if (model["per"] != null)
                            template.WorkflowApplyFlag =
                                (HRIS.Domain.Objectives.Enums.WorkflowApplyFlag)
                                Enum.Parse(typeof (HRIS.Domain.Objectives.Enums.WorkflowApplyFlag),
                                           model["per"].ToString());
   
                        phaseConfigurationWorkflow.StepCount = int.Parse(model["stepCount"].ToString());
                        phaseConfigurationWorkflow.Save();
                    }
                }
        }

        //Need a delete transaction (foreach).
        private static int DeleteWorkflow(IList<PhaseConfigurationWorkflow> phaseConfigurationWorkflows)
        {
            var deletedRowsNumber = 0;
            foreach (var item in phaseConfigurationWorkflows)
            {
                ServiceFactory.ORMService.Delete(item);
                deletedRowsNumber++;
            }
            return deletedRowsNumber;
        }//Should to handle (delete internal node belonged to applied master node).

        private static IList<WorkflowTreeViewModel> GetGradeByOrganizationalLevel(int organizationalLevelId, int Id, IList<PhaseConfigurationWorkflow> groupedPhaseConfigurationWorkflows)// method to get child nodes
        {
            var requiredGrades = groupedPhaseConfigurationWorkflows.Where(x => x.PhaseConfiguration.Id == Id).Select(x => x.FirstPosition.JobDescription.JobTitle.Grade).Distinct().ToList();
            var gradeTree = new List<WorkflowTreeViewModel>();

            foreach (HRIS.Domain.OrgChart.RootEntities.Grade grade in requiredGrades)
            {
                var treeItem = new WorkflowTreeViewModel();
                if (grade.OrganizationalLevel.Id == organizationalLevelId)//may be
                {
                    treeItem.Id = grade.Id;
                    treeItem.Name = grade.Name;
                    treeItem.LevelNumber = (int)WorkflowApplyFlag.Grade;

                    if (groupedPhaseConfigurationWorkflows.ElementAt(0).WorkflowApplyFlag == HRIS.Domain.Objectives.Enums.WorkflowApplyFlag.Grade)//To stop adding children items.
                    {
                        string stepCount = groupedPhaseConfigurationWorkflows.Select(x => x.StepCount.ToString()).Distinct().FirstOrDefault();
                        if (!string.IsNullOrEmpty(stepCount))
                            treeItem.Name += (" ( " + stepCount + " )");
                        gradeTree.Add(treeItem);
                        continue;
                    }

                    treeItem.HasChildren = true;
                    treeItem.Items = GetJobTitleByGrade(grade.Id, Id, groupedPhaseConfigurationWorkflows);
                    gradeTree.Add(treeItem);
                }
            }
            return gradeTree;
        }

        private static IList<WorkflowTreeViewModel> GetJobTitleByGrade(int gradeId, int Id, IList<PhaseConfigurationWorkflow> groupedPhaseConfigurationWorkflows)// method to get child nodes
        {
            var jobTitleTree = new List<WorkflowTreeViewModel>();
            var requiredJobTitles = groupedPhaseConfigurationWorkflows.Where(x => x.PhaseConfiguration.Id == Id).Select(x => x.FirstPosition.JobDescription.JobTitle).Distinct().ToList();

            foreach (JobTitle jobTitle in requiredJobTitles)
            {
                var treeItem = new WorkflowTreeViewModel();
                if (jobTitle.Grade.Id == gradeId)//may be
                {
                    treeItem.Id = jobTitle.Id;
                    treeItem.Name = jobTitle.Name;
                    treeItem.LevelNumber = (int)WorkflowApplyFlag.JobTitle;

                    if (groupedPhaseConfigurationWorkflows.ElementAt(0).WorkflowApplyFlag == HRIS.Domain.Objectives.Enums.WorkflowApplyFlag.JobTitle)//To stop adding children items.
                    {
                        string stepCount = groupedPhaseConfigurationWorkflows.Select(x => x.StepCount.ToString()).Distinct().FirstOrDefault();
                        if (!string.IsNullOrEmpty(stepCount))
                            treeItem.Name += (" ( " + stepCount + " )");
                        jobTitleTree.Add(treeItem);
                        continue;
                    }

                    treeItem.HasChildren = true;
                    treeItem.Items = GetJobDescriptionByJobTitle(jobTitle.Id, Id, groupedPhaseConfigurationWorkflows);
                    jobTitleTree.Add(treeItem);
                }
            }
            return jobTitleTree;
        }

        private static IList<WorkflowTreeViewModel> GetJobDescriptionByJobTitle(int jobTitleId, int Id, IList<PhaseConfigurationWorkflow> groupedPhaseConfigurationWorkflows)// method to get child nodes
        {
            var requiredJobDescriptions = groupedPhaseConfigurationWorkflows.Where(x => x.PhaseConfiguration.Id == Id).Select(x => x.FirstPosition.JobDescription).Distinct().ToList();
            var jobDescriptionTree = new List<WorkflowTreeViewModel>();

            foreach (HRIS.Domain.JobDesc.RootEntities.JobDescription jobDescription in requiredJobDescriptions)
            {
                var treeItem = new WorkflowTreeViewModel();
                if (jobDescription.JobTitle.Id == jobTitleId)
                {
                    treeItem.Id = jobDescription.Id;
                    treeItem.Name = jobDescription.Name;
                    treeItem.LevelNumber = (int)WorkflowApplyFlag.JobDescription;

                    if (groupedPhaseConfigurationWorkflows.ElementAt(0).WorkflowApplyFlag == HRIS.Domain.Objectives.Enums.WorkflowApplyFlag.JobDescription)//To stop adding children items.
                    {
                        string stepCount = groupedPhaseConfigurationWorkflows.Select(x => x.StepCount.ToString()).Distinct().FirstOrDefault();
                        if (!string.IsNullOrEmpty(stepCount))
                            treeItem.Name += (" ( " + stepCount + " )");
                        jobDescriptionTree.Add(treeItem);
                        continue;
                    }

                    treeItem.HasChildren = true;
                    treeItem.Items = GetPositionByJobDescription(jobDescription.Id, Id, groupedPhaseConfigurationWorkflows);
                    jobDescriptionTree.Add(treeItem);
                }
            }
            return jobDescriptionTree;
        }

        private static IList<WorkflowTreeViewModel> GetPositionByJobDescription(int jobDescriptionId, int Id, IList<PhaseConfigurationWorkflow> groupedPhaseConfigurationWorkflows)// method to get child nodes
        {
            var requiredPositions = groupedPhaseConfigurationWorkflows.Where(x => x.PhaseConfiguration.Id == Id).Select(x => x.FirstPosition).Distinct().ToList();
            var positionTree = new List<WorkflowTreeViewModel>();

            foreach (Position position in requiredPositions)
            {
                var treeItem = new WorkflowTreeViewModel();
                if (position.JobDescription.Id == jobDescriptionId)
                {
                    treeItem.Id = position.Id;
                    treeItem.LevelNumber = (int)WorkflowApplyFlag.Position;
                    treeItem.Name = position.NameForDropdown;
                    string stepCount = groupedPhaseConfigurationWorkflows.Select(x => x.StepCount.ToString()).Distinct().FirstOrDefault();
                    if (!string.IsNullOrEmpty(stepCount))
                        treeItem.Name += (" ( " + stepCount + " )");

                    positionTree.Add(treeItem);
                }
            }
            return positionTree;
        }

        #endregion

        #region Workflow approval

        private static HRIS.Domain.Objectives.RootEntities.Objective FindObjectivePhasePeriodByWorkflow(WorkflowItem workflow)
        {

            HRIS.Domain.Objectives.RootEntities.Objective objective = null;

            var objectivePhaseWorkflow = ServiceFactory.ORMService.All
                <HRIS.Domain.Objectives.Entities.ObjectivePhaseWorkflow>().
                SingleOrDefault(x => x.Workflow.Id == workflow.Id);

            if (objectivePhaseWorkflow != null)
                objective = objectivePhaseWorkflow.Objective;

            return objective;

        }

        #endregion

        #region Specific objective applies.

        //Used when objective case...
        private static void ApplySpecificObjectiveWorkflows(
            HRIS.Domain.Objectives.RootEntities.AbstractPhase phase,
            HRIS.Domain.Objectives.RootEntities.Objective objective)
        {
            //Insert new workflow Items (Phase).
            var phaseWorkflows = phase.PhaseConfiguration.PhaseConfigurationWorkflows;
            foreach (var phaseConfigurationWorkflow in phaseWorkflows)
            {
                //New workflow initialization & save.
                var workflow = new WorkflowItem
                                   {
                                       Status = WorkflowStatus.Pending,
                                       StepCount = phaseConfigurationWorkflow.StepCount,
                                       Type = WorkflowType.Objective,
                                       Description = DoesNotMeetCriteriaMessage + objective.DoesNotMeet + "<br/>" +
                                                     MeetCriteriaMessage + objective.Meet +
                                                     "<br/>" + AboveMeetCriteriaMessage + objective.Above,
                                       Date = phase.EndDate
                                   };
                if (phaseConfigurationWorkflow.FirstPosition.Employee==null)
                    continue;
                workflow.FirstUser = phaseConfigurationWorkflow.FirstPosition.Employee.User();//Todo login the system
                workflow.Creator = EmployeeExtensions.CurrentEmployee.User();//Todo login the system
                //workflow.FirstUser = ServiceFactory.ORMService.GetById<User>(1);
                //workflow.Save();

                if (phase is PhasePeriod)
                {
                    var objectivePhaseWorkflow = new ObjectivePhaseWorkflow
                                                     {
                                                         PhasePeriod = (PhasePeriod) phase,
                                                         Position = phaseConfigurationWorkflow.FirstPosition,
                                                         Objective = objective,
                                                         Workflow = workflow,
                                                         OperationNo = phaseConfigurationWorkflow.OperationNo,
                                                         WorkflowApplyFlag = phaseConfigurationWorkflow.WorkflowApplyFlag
                                                     };
                    //objectivePhaseWorkflow.Save();
                    ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>
                                                                  {workflow, objectivePhaseWorkflow});
                }
                else
                {
                    var objectiveEvaluationWorkflow = new ObjectiveEvaluationWorkflow
                                                          {
                                                              EvaluationPeriod = (EvaluationPeriod) phase,
                                                              Position = phaseConfigurationWorkflow.FirstPosition,
                                                              Objective = objective,
                                                              Workflow = workflow,
                                                              OperationNo = phaseConfigurationWorkflow.OperationNo,
                                                              WorkflowApplyFlag =
                                                                  phaseConfigurationWorkflow.WorkflowApplyFlag
                                                          };
                    //objectiveEvaluationWorkflow.Save();
                    ServiceFactory.ORMService.SaveTransaction(new List<IAggregateRoot>
                                                                  {workflow, objectiveEvaluationWorkflow});
                }

            }
        }

        //I need to fix the calling problem for this method.
        private static bool CompareDatesApplyingConvention(DateTime startItemDate, DateTime closingItemDate, DateTime startPhaseDate, DateTime endPhaseDate)
        {
            if (
                    //(startItemDate >= startPhaseDate && closingItemDate <= startPhaseDate) ||
                    //(endPhaseDate >= startItemDate && endPhaseDate <= closingItemDate)
                (startItemDate>=startPhaseDate && startItemDate<=endPhaseDate) ||
                (startPhaseDate >=startItemDate && startPhaseDate<=closingItemDate) || 
                (closingItemDate >=startPhaseDate && closingItemDate<=endPhaseDate) ||
                (endPhaseDate >=startItemDate && endPhaseDate <=closingItemDate)
                )
                return true;
            return false;
        }

        #endregion

        #endregion

        #region Assistant Properties

        private static string MeetCriteriaMessage
        {
            get
            {
                var  meetCriteriaMessage=ServiceFactory.LocalizationService.GetResource(
                   CustomMessageKeysObjectiveModule.GetFullKey(
                       CustomMessageKeysObjectiveModule.MeetCriteriaMessage));
                return meetCriteriaMessage ?? "";
            }
        }

        private static string DoesNotMeetCriteriaMessage
        {
            get
            {
                var doesNotMeetCriteriaMessage= ServiceFactory.LocalizationService.GetResource(
                   CustomMessageKeysObjectiveModule.GetFullKey(
                       CustomMessageKeysObjectiveModule.DoesNotMeetCriteriaMessage));
                return doesNotMeetCriteriaMessage ?? "";
            }
        }

        private static string AboveMeetCriteriaMessage
        {
            get
            {
               var aboveMeetCriteriaMessage=ServiceFactory.LocalizationService.GetResource(
                   CustomMessageKeysObjectiveModule.GetFullKey(
                       CustomMessageKeysObjectiveModule.AboveMeetCriteriaMessage));
                return aboveMeetCriteriaMessage ?? "";
            }
        }

        #endregion
    }
}