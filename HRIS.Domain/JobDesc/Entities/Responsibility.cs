#region

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HRIS.Domain.JobDescription.Indexes;
using HRIS.Domain.OrganizationChart.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using Souccar.Infrastructure.Core;
using System.Linq;
using System.Collections.Generic;

#endregion

namespace HRIS.Domain.JobDescription.Entities
{
    public class Responsibility : Entity,IAggregateRoot
    {
        #region Basic Info.

        public Responsibility()
        {
            ResponsibilityKpis=new List<ResponsibilityKpi>();
        }

        [UserInterfaceParameter(Order = 1)]
        public virtual string Description { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual float Weight { get; set; }

        [UserInterfaceParameter(Order = 3)]
        public virtual float ActuallyWeight { get 
        {
            if(this.Role==null)
                return 0;
            var role = ServiceFactory.ORMService.All<Role>().SingleOrDefault(x => x.Id == this.Role.Id);

            var sum = role.Responsibilities.Sum(x => x.Weight);
            if (sum == 0)
                return 100;
            return 100 * Weight / sum;
        } }
        

        public virtual Role Role { get; set; }

        #endregion

        public virtual IList<ResponsibilityKpi> ResponsibilityKpis{ get; set; }
        public virtual void AddResponsibilityKpi(ResponsibilityKpi kpi)
        {
            kpi.Responsibility = this;
            ResponsibilityKpis.Add(kpi);
        } 

    }
}