using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.OrganizationChart.RootEntities;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.ProjectManagement.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.ProjectManagement.Entities
{
    public class Member : Entity,IAggregateRoot
    {
        [UserInterfaceParameter(Order = 1,IsReference = true)]
        public virtual Node Node { get; set; }


        [UserInterfaceParameter(Order = 2, IsReference = true)]
        public virtual Employee Employee { get; set; }

        [UserInterfaceParameter(Order = 3)]
        public virtual bool IsEvaluator { get; set; }

        public virtual TRole TRole { get; set; }
    }
}
