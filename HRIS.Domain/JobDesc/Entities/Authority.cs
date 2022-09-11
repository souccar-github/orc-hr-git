#region

using System.ComponentModel.DataAnnotations;
using HRIS.Domain.JobDescription.Indexes;
using HRIS.Domain.JobDescription.RootEntities;
using HRIS.Domain.OrganizationChart.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

#endregion

namespace HRIS.Domain.JobDescription.Entities
{
    public class Authority : Entity,IAggregateRoot
    {

        [UserInterfaceParameter(Order = 1)]
        public virtual string Code { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual AuthorityType Type { get; set; }

        [UserInterfaceParameter(Order = 3)]
        public virtual string Name { get; set; }
        
        [UserInterfaceParameter(Order = 4)]
        public virtual string Description { get; set; }

        [UserInterfaceParameter(Order = 5)]
        public virtual string RelatedActions { get; set; }

        public virtual HRIS.Domain.JobDescription.RootEntities.JobDescription JobDescription { get; set; }

    }
}