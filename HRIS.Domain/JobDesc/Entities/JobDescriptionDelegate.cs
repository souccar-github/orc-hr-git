using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.JobDescription.Indexes;
using HRIS.Domain.JobDescription.RootEntities;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.JobDescription.Entities
{
    public class JobDescriptionDelegate : Entity
    {
        public virtual AuthorityType AuthorityType { get; set; }

        public virtual HRIS.Domain.JobDescription.RootEntities.JobDescription PrimaryJobDescription { get; set; }
        public virtual HRIS.Domain.JobDescription.RootEntities.JobDescription SecondaryJobDescription { get; set; }
    }
}
