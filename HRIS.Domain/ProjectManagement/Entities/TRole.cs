using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.ProjectManagement.Helpers;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.ProjectManagement.Entities
{
    public class TRole : Entity,IAggregateRoot
    {
        public TRole()
        {
            Members = new List<Member>();
        }

        [UserInterfaceParameter(Order=5, Group = ProjectManagementGroupNames.BasicInformation,IsReference = true)]
        public virtual Role Role { get; set; }

        [UserInterfaceParameter(Order = 10, Group = ProjectManagementGroupNames.BasicInformation,IsReference = true)]
        public virtual Role ParentRole { get; set; }

        [UserInterfaceParameter(Order = 15, Group = ProjectManagementGroupNames.BasicInformation)]
        public virtual int Number { get; set; }

        [UserInterfaceParameter(Order = 20, Group = ProjectManagementGroupNames.BasicInformation)]
        public virtual float Weight { get; set; }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown
        {
            get
            {
                return Role == null ? string.Empty : Role.Name;
            }
        }

        public virtual Team Team { get; set; }

        public virtual IList<Member> Members {get; set; }

        public virtual void AddMember(Member member)
        {
            member.TRole = this;
            Members.Add(member);
        }

        public virtual IList<IndirectManagerInfo> IndirectManagerInfos { get; set; }

        public virtual void AddIndirectManagerInfo(IndirectManagerInfo indirectManager)
        {
            indirectManager.TRole = this;
            IndirectManagerInfos.Add(indirectManager);
        }
    }
}
