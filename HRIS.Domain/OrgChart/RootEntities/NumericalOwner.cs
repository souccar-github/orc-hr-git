using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.OrgChart.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.OrgChart.RootEntities
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    [Order(4)]
    //[Module(ModulesNames.Grade)]
    public class NumericalOwner : Entity, IAggregateRoot
    {
        [UserInterfaceParameter(Order = 1)]
        public virtual DateTime Date { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual OrganizationalLevel OrganizationalLevel { get; set; }

        [UserInterfaceParameter(Order = 3)]
        public virtual int Number { get; set; }
    }
}
