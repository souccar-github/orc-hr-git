using HRIS.Domain.JobDescription.Configurations;
using HRIS.Domain.JobDescription.RootEntities;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.JobDescription.Entities
{
    public class CompetenceCategoryLevelDescription : Entity,IAggregateRoot
    {
        [UserInterfaceParameter(Order = 1, IsHidden = true)]
        public virtual string NameForDropdown
        {
            get
            {
                return CompetenceCategory != null ? Level.Name : string.Empty;
            }
        }
        [UserInterfaceParameter(Order = 3)]
        public virtual Level Level { get; set; }
        [UserInterfaceParameter(Order = 4)]
        public virtual string Description { get; set; }
        public virtual CompetenceCategory CompetenceCategory { get; set; }

    }
}
