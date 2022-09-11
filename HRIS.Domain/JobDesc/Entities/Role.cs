#region

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HRIS.Domain.JobDescription.Indexes;
using HRIS.Domain.JobDescription.RootEntities;
using HRIS.Domain.OrganizationChart.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace HRIS.Domain.JobDescription.Entities
{
    public class Role : Entity,IAggregateRoot
    {
        public Role()
        {
            RoleKpis = new List<RoleKpi>();
            Responsibilities = new List<Responsibility>();

        }

        #region Basic Info.

        [UserInterfaceParameter(Order = 1)]
        public virtual string Name { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual string Summary { get; set; }

        [UserInterfaceParameter(Order = 3)]
        public virtual float Weight{get;set;}

        [UserInterfaceParameter(Order = 4)]
        public virtual float ActuallyWeight
        {
            get
            {
                if (this.JobDescription == null)
                    return 0;
                var sum = this.JobDescription.Roles.Sum(x => x.Weight);
                if (sum == 0)
                    return 100;
                return 100 * Weight / sum;
            }
        }

        [UserInterfaceParameter(IsHidden = true)]
        public virtual string NameForDropdown { get { return Name; } }

        public virtual HRIS.Domain.JobDescription.RootEntities.JobDescription JobDescription { get; set; }

        #endregion

        #region RoleKpi

        public virtual IList<RoleKpi> RoleKpis { get; set; }

        public virtual void AddRoleKpi(RoleKpi kpi)
        {
            kpi.Role = this;
            RoleKpis.Add(kpi);
        }

        #endregion

        #region Responsibilities

        public virtual IList<Responsibility> Responsibilities { get; set; }

        public virtual void AddResponsibility(Responsibility responsibility)
        {
            responsibility.Role = this;
            Responsibilities.Add(responsibility);
        }

        #endregion
    }
}