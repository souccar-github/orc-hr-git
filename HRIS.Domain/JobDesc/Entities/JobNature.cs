using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.JobDescription.Enum;
using HRIS.Domain.JobDescription.Indexes;
using HRIS.Domain.JobDescription.RootEntities;
using Souccar.Domain.DomainModel;
using Souccar.Core.CustomAttribute;

namespace HRIS.Domain.JobDescription.Entities
{
    public class JobNature : Entity
    {
        [UserInterfaceParameter(Order = 1)]
        public virtual NatureJobType Type { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual string Description { get; set; }

        public virtual HRIS.Domain.JobDescription.RootEntities.JobDescription JobDescription { get; set; }
    }
}
