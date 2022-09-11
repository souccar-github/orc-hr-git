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
    public class TrainingCourseLanguage : Entity,IAggregateRoot
    {

        [UserInterfaceParameter(Order = 5)]
        public virtual LanguageName LanguageName { get; set; }

        [UserInterfaceParameter(Order = 10)]
        public virtual Level Writing { get; set; }

        [UserInterfaceParameter(Order = 20)]
        public virtual Level Reading { get; set; }

        [UserInterfaceParameter(Order = 25)]
        public virtual Level Listening { get; set; }

        [UserInterfaceParameter(Order = 30)]
        public virtual Level Speaking { get; set; }

        public virtual JobApplication JobApplication { get; set; }
    }
}
