using HRIS.Domain.EmployeeRelationServices.Enums;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.JobDesc.Entities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.EmployeeRelationServices.RootEntities
{
    //[Module(ModulesNames.EmployeeRelationServices)]
    public class IncedenseDefinitionNotification : Entity, IAggregateRoot
    {
        [UserInterfaceParameter(Order = 1)]
        public virtual IncidenceType IncedenceType { set; get; }
        [UserInterfaceParameter(Order = 2)]
        public virtual FieldName FieldName { set; get; }
        [UserInterfaceParameter(Order = 3)]
        public virtual IncedenceNotificationFactor Factor { set; get; }
        [UserInterfaceParameter(Order = 4)]
        public virtual int Days { set; get; }

        [UserInterfaceParameter(Order = 5, IsReference=true)]
        public virtual Position Position { set; get; }

    }
}