using System;
using System.Linq;
using System.Collections.Generic;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Objectives.Entities;
using HRIS.Domain.Objectives.Enums;
using HRIS.Domain.Objectives.RootEntities;

using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Workflow.Enums;
using Souccar.Domain.Workflow.RootEntities;
using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.JobDescription.RootEntities;

//using FluentNHibernate.Data;

namespace HRIS.Domain.Objectives.RootEntities
{
    // [Module(ModulesNames.Objective)]
    public class Objective : AbstractObjective, IAggregateRoot
    {
        public Objective()
        {
            SharedWiths = new List<SharedWith>();
            Kpis = new List<ObjectiveKpi>();
            Constraints = new List<ObjectiveConstraint>();
            ActionPlans = new List<ActionPlan>();
            Objectives = new List<Objective>();
            Status = ObjectiveStatus.Waiting;
        }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual bool IsFinishedAndEvaluated { get; set; }
        #region objective info

        [UserInterfaceParameter(Order = 10, IsReference = true, IsNonEditable = true)]
        public virtual Employee Creator { get; set; }
        [UserInterfaceParameter(Order = 15, IsNonEditable = true)]
        public virtual string FatherName
        {
            get
            {
                return Creator.FatherName;
            }
        }

            [UserInterfaceParameter(Order = 20)]
        public virtual DateTime CreationDate { get; set; }

        [UserInterfaceParameter(Order = 30, IsReference = true)]
        public virtual Node Department { get; set; }

        [UserInterfaceParameter(Order = 60)]
        public virtual ObjectiveType Type { get; set; }

        [UserInterfaceParameter(Order = 70, IsReference = true, CascadeFrom = "Department", ReferenceReadUrl = "Objectives/Reference/ReadJDByNode")]
        public virtual HRIS.Domain.JobDescription.RootEntities.JobDescription JopDescription { get; set; }

        [UserInterfaceParameter(Order = 70, IsReference = true,CascadeFrom= "JopDescription", ReferenceReadUrl = "Objectives/Reference/ReadPositionsByJD")]
        public virtual Position Owner { get; set; }

        [UserInterfaceParameter(Order = 85, IsNonEditable = true)]
        public virtual ObjectiveStatus Status { get; set; }

        [UserInterfaceParameter(Order = 86)]
        public virtual double Mark
        {
            get
            {
                if (ActionPlans.Count == 0)
                    return 0;
                return ActionPlans.Sum(x=>x.Mark)/ActionPlans.Count;
            }
        }
        public virtual void UpdateStatusByActionPlan()
        {
            if (ActionPlans.Count == 0)
                return;
            if (ActionPlans.Any(x => x.Status == ActionPlanStatus.InProgress))
            {
                Status = ObjectiveStatus.InProcess;
                return;
            }
            if (ActionPlans.All(x => x.Status == ActionPlanStatus.Cancelled))
            {
                Status = ObjectiveStatus.Canceled;
                return;
            }
            if (ActionPlans.All(x => x.Status == ActionPlanStatus.Accepted || x.Status == ActionPlanStatus.Closed || x.Status == ActionPlanStatus.Cancelled))
            {
                Status = ObjectiveStatus.Finished;
                return;
            }
        }

        [UserInterfaceParameter(Order = 110)]
        public virtual Priority Priority { get; set; }

        [UserInterfaceParameter(Order = 120)]
        public virtual float Weight { get; set; }

        [UserInterfaceParameter(Order = 180, IsReference = true)]
        public virtual Objective ParentObjective { get; set; }
        #region Planning Date

        [UserInterfaceParameter(Order = 190)]
        public virtual DateTime PlannedStartingDate { get; set; }

        [UserInterfaceParameter(Order = 200)]
        public virtual DateTime PlannedClosingDate { get; set; }


        [UserInterfaceParameter(Order = 203)]
        public virtual DateTime ActualStartDate
        {
            get 
            {
                return (ActionPlans.Count == 0) ? PlannedStartingDate : ActionPlans.Min(x => x.ActualStartDate);//ActionPlans.Where(x=>x.Status==ActionPlanStatus.Pending).Min(x => x.ActualStartDate);
            }
        }
       
        [UserInterfaceParameter(Order = 204)]
        public virtual DateTime ?ActualEndDate
        {
            get 
            {
                return (ActionPlans.Count == 0) ? PlannedClosingDate : ActionPlans.Max(x => x.ActualEndDate);//ActionPlans.Where(x => x.Status == ActionPlanStatus.Closed).Max(x => x.ActualEndDate);
            }
        }
        [UserInterfaceParameter(Order = 206)]
        public virtual WorkflowStatus WorkflowStatus
        {
            get
            {
                
                return CreationWorkflow==null?WorkflowStatus.Pending:CreationWorkflow.Status;
            }
        }
        #endregion

        public virtual StrategicObjective StrategicObjective { get; set; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return Name; } }
        #endregion

        #region creation
        public virtual ObjectiveCreationPhase CreationPhase { get; set; }
        public virtual WorkflowItem CreationWorkflow { get; set; }
        public virtual WorkflowItem EvaluationWorkflow { get; set; }
        #endregion

        #region Strategic Objective


        #endregion

        #region Shared Withs

        public virtual IList<SharedWith> SharedWiths { get; set; }

        public virtual void AddSharedWith(SharedWith sharedWith)
        {
            sharedWith.Objective = this;
            SharedWiths.Add(sharedWith);
        }

        #endregion

        #region Objective Kpis

        public virtual IList<ObjectiveKpi> Kpis { get; set; }

        public virtual void AddKpi(ObjectiveKpi objectiveKpi)
        {
            objectiveKpi.Objective = this;
            Kpis.Add(objectiveKpi);
        }

        #endregion

        #region Constraints

        public virtual IList<ObjectiveConstraint> Constraints { get; set; }

        public virtual void Addconstraint(ObjectiveConstraint objectiveConstraint)
        {
            objectiveConstraint.Objective = this;
            Constraints.Add(objectiveConstraint);
        }

        #endregion

        #region ActionPlans

        public virtual IList<ActionPlan> ActionPlans { get; set; }

        public virtual void AddActionPlan(ActionPlan actionPlan)
        {
            actionPlan.Objective = this;
            ActionPlans.Add(actionPlan);
        }

        #endregion

        #region Objectives (recursive relationship //need discussion)


        public virtual IList<Objective> Objectives { get; set; }

        public virtual void AddObjective(Objective objective)
        {
            objective.ParentObjective = this;
            Objectives.Add(objective);
        }

        #endregion
    }
}