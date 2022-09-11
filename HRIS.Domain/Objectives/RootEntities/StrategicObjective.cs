#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HRIS.Domain.Objectives.Entities;
using HRIS.Domain.Objectives.Enums;
using HRIS.Domain.Objectives.Indexes;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.Global.Constant;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.Objectives.RootEntities
{

    [Module(ModulesNames.Objectives)]
    public class StrategicObjective : AbstractObjective, IAggregateRoot
    {
        [UserInterfaceParameter(Order = 50)]
        public virtual Dimension Dimension { get; set; }

        [UserInterfaceParameter(Order = 100)]
        public virtual Period Period { get; set; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return Name; } }
        #region schedule


        [UserInterfaceParameter(Order = 130)]
        public virtual DateTime StartDate { get; set; }

        [UserInterfaceParameter(Order = 140)]
        public virtual DateTime EndDate { get; set; }

        #endregion

        #region Objectives

        public virtual IList<Objective> Objectives { get; set; }

        public virtual void AddObjective(Objective objective)
        {
            objective.StrategicObjective = this;
            Objectives.Add(objective);
            
        }

        #endregion

    }
}