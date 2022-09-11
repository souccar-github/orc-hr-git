using HRIS.Domain.Incentive.RootEntities;
using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Core.CustomAttribute;

namespace HRIS.Domain.PayrollSystem.Entities
{

    public class MonthIncentivesFiltering : Entity, IAggregateRoot
    {
        public virtual Month Month { get; set; }
        public virtual IncentivesFiltering IncentivesFiltering { get; set; }
    }
}
