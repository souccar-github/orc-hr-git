using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.AttendanceSystem.Enums;
using HRIS.Domain.Global.Constant;
using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.AttendanceSystem.RootEntities
{
    public class FingerprintTransferredData : Entity, IAggregateRoot
    {

        [UserInterfaceParameter(Order = 2)]
        public virtual DateTime LogDateTime { get; set; } 

        [UserInterfaceParameter(Order = 3, IsTime = true)]
        public virtual LogType LogType { get; set; }

        [UserInterfaceParameter(Order = 4)]
        public virtual bool IsTransfered { get; set; }

        [UserInterfaceParameter(Order = 5)]
        public virtual bool IsOld { get; set; }

        [UserInterfaceParameter(Order = 6)]
        public virtual bool IsLogTypeIgnored { get; set; }

        [UserInterfaceParameter(Order = 4, IsReference = true)]
        public virtual Employee Employee { get; set; }
    }
}
