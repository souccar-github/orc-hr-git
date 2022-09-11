#region

using System.ComponentModel.DataAnnotations;
using HRIS.Domain.Objectives.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Objectives.Indexes;

#endregion

namespace HRIS.Domain.Objectives.Entities
{
    public class ObjectiveConstraint : Entity
    {
        [UserInterfaceParameter(Order=2)]
        public virtual string Description { get; set; }

        [UserInterfaceParameter(Order = 20)]
        public virtual ObjectiveConstraintType Type { get; set; }

        public virtual Objective Objective { get; set; }
    }
}