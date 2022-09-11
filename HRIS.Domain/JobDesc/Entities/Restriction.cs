#region

using System.ComponentModel.DataAnnotations;
using HRIS.Domain.JobDescription.Indexes;
using Souccar.Domain.DomainModel;
using Souccar.Core.CustomAttribute;

#endregion

namespace HRIS.Domain.JobDescription.Entities
{
    public class Restriction : Entity
    {
        [UserInterfaceParameter(Order = 10)]
        public virtual string Code { get; set; }

        [UserInterfaceParameter(Order = 20)]
        public virtual ConditionType Type { get; set; }

        [UserInterfaceParameter(Order = 30)]
        public virtual bool Required { get; set; }

        [UserInterfaceParameter(Order = 40)]
        public virtual string Description { get; set; }

        public virtual WorkingRestriction WorkingRestriction { get; set; }
    }
}