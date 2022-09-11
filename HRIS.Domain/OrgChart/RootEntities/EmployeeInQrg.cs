using HRIS.Domain.Global.Constant;
using HRIS.Domain.OrgChart.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.OrgChart.RootEntities
{
    #warning to delete

    // [Module(ModulesNames.OrganizationChart)]
    public class EmployeeInQrg : Entity, IAggregateRoot
    {
        public virtual int Number{ get; set; }
        public virtual DateTime Date { get; set; }
        public virtual OrganizationalLevel Level { get; set; }
    }
}
