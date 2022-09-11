using HRIS.Domain.JobDesc.RootEntities;
using HRIS.Domain.OrgChart.Indexes;
using HRIS.Domain.OrgChart.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.OrgChart.Entities
{
    public class GradeStep : Entity,IAggregateRoot
    {
        public GradeStep()
        {
        }

        [UserInterfaceParameter(Order = 1)]
        public virtual string Name { get; set; }
        [UserInterfaceParameter(Order = 2)]
        public virtual int Order { get; set; }

        [UserInterfaceParameter(Order = 4, Step = 1000)]
        public virtual float MinSalary { get; set; }
        [UserInterfaceParameter(Order = 5, Step = 1000)]
        public virtual float MaxSalary { get; set; }

        public virtual float MidSalary
        {
            get { return (MaxSalary + MinSalary)/2; }
        }

        [UserInterfaceParameter(Order = 6)]
        public virtual CurrencyType CurrencyType { get; set; }

        [UserInterfaceParameter(Order = 7)]
        public virtual string Description { get; set; }
        [UserInterfaceParameter(Order = 155, IsHidden = true)]
        public virtual string NameForDropdown { get { return Name; } }
        public virtual Grade Grade { get; set; }
    }
}