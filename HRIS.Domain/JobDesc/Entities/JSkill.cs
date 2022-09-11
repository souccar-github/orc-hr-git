#region

using System.ComponentModel.DataAnnotations;
using HRIS.Domain.JobDescription.RootEntities;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.JobDescription.Entities
{
    public class JSkill : Entity
    {
        [UserInterfaceParameter(Order = 1)]
        public virtual SkillType Type { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual Level Level { get; set; }

        [UserInterfaceParameter(Order = 3)]
        public virtual float Weight { get; set; }

        [UserInterfaceParameter(Order = 4)]
        public virtual bool Required { get; set; }

        public virtual HRIS.Domain.JobDescription.RootEntities.JobDescription JobDescription { get; set; }
    }
}