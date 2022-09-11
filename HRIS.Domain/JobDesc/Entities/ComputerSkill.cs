#region

using System.ComponentModel.DataAnnotations;
using HRIS.Domain.JobDescription.Indexes;
using HRIS.Domain.JobDescription.RootEntities;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Domain.DomainModel;
using Souccar.Core.CustomAttribute;

#endregion

namespace HRIS.Domain.JobDescription.Entities
{
    public class ComputerSkill : Entity
    {
        [UserInterfaceParameter(Order = 10)]
        public virtual SkillType Type { get; set; }

        [UserInterfaceParameter(Order = 20)]
        public virtual Level Level { get; set; }

        [UserInterfaceParameter(Order = 30)]
        public virtual float Weight { get; set; }

        [UserInterfaceParameter(Order = 40)]
        public virtual bool Required { get; set; }

        public virtual HRIS.Domain.JobDescription.RootEntities.JobDescription JobDescription { get; set; }
    }
}