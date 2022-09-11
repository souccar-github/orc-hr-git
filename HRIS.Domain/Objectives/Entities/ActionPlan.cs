#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Objectives.Enums;
using HRIS.Domain.Objectives.RootEntities;

using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.Objectives.Entities
{
    public class ActionPlan : Entity,IAggregateRoot
    {
        public ActionPlan()
        {
            Status = ActionPlanStatus.Pending;
            ActualStartDate = DateTime.Now;
            ActualEndDate = DateTime.Now;
        }

        [UserInterfaceParameter(Order = 1)]
        public virtual string Description { get; set; }

        [UserInterfaceParameter(Order = 10,IsReference=true, ReferenceReadUrl = "Objectives/Reference/ReadPositions")]
        public virtual Position Owner { get; set; }

        #region Planning Info

        [UserInterfaceParameter(Order = 20)]
        public virtual DateTime PlannedStartDate { get; set; }

        [UserInterfaceParameter(Order =30)]
        public virtual DateTime PlannedEndDate { get; set; }

        #endregion

        [UserInterfaceParameter(Order = 35,IsNonEditable=true)]
        public virtual ActionPlanStatus Status { get; set; }

        [UserInterfaceParameter(Order = 37)]
        public virtual string ExpectedResult { get; set; }

        #region Tracking Info

        [UserInterfaceParameter(Order = 40,IsNonEditable=true)]
        public virtual DateTime ActualStartDate { get; set; }

        [UserInterfaceParameter(Order = 50, IsNonEditable = true)]
        public virtual DateTime ? ActualEndDate { get; set; }

        [UserInterfaceParameter(Order = 55, IsNonEditable = true)]
        public virtual float PercentageOfCompletion { get; set; }

        #endregion

        #region Appraisal

        [UserInterfaceParameter(Order = 60, IsNonEditable = true)]
        public virtual float Mark { get; set; }

        #endregion
       
        public virtual Objective Objective { get; set; }
    }
}