using HRIS.Domain.JobDescription.RootEntities;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.JobDescription.Entities
{
    public class JobDescriptionReporting:Entity
    {
        public virtual HRIS.Domain.JobDescription.RootEntities.JobDescription JobDescription { get; set; }
        public virtual HRIS.Domain.JobDescription.RootEntities.JobDescription ManagerJobDescription { get; set; }
        public virtual bool IsPrimary { get; set; }
    }
}
