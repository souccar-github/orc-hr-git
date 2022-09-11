using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Recruitment.Indexes;
using HRIS.Domain.Recruitment.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.Recruitment.Entities
{
    public class RecruitmentComputerSkill:Entity,IAggregateRoot
    {
        [UserInterfaceParameter(Order = 5)]
        public virtual SkillType SkillType { get; set; }

        [UserInterfaceParameter(Order = 10)]
        public virtual Level Level { get; set; }

        public virtual JobApplication JobApplication { get; set; }
    }
}
