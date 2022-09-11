using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Personnel.Indexes;
using HRIS.Domain.Recruitment.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.Recruitment.Entities
{
    public class PersonalSkill:Entity,IAggregateRoot
    {
        [UserInterfaceParameter(Order = 5)]
        public virtual SkillType SkillType { get; set; }

        [UserInterfaceParameter(Order = 10)]
        public virtual Level Level { get; set; }

        [UserInterfaceParameter(Order = 15)]
        public virtual string Description { get; set; }

        [UserInterfaceParameter(Order = 20)]
        public virtual string Comments { get; set; }

        public virtual JobApplication JobApplication { get; set; }
    }
}
