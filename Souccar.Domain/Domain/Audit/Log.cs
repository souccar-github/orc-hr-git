using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Souccar.Domain.Audit
{
    [Module("Audit")]
    public class Log : Entity, IAggregateRoot
    {


        public virtual string ClassName { get; set; }
        public virtual string AffecetedRow { get; set; }
        public virtual DateTime Date { get; set; }
        [UserInterfaceParameter(IsTime = true)]
        public virtual DateTime Time { get; set; }
        [UserInterfaceParameter(IsReference = true)]
        public virtual User User { get; set; }
        public virtual OperationType OperationType { get; set; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual string Description { get; set; }
        public virtual bool IsWithWorkFlow { get; set; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual double ProcessingPeriod { get; set; }

    }
}
