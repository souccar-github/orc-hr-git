using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.EmployeeRelationServices.Entities
{
   public class MorningLate : Entity, IAggregateRoot
    {
        public MorningLate()
        {
        }
        [UserInterfaceParameter(Order = 10)]
        public virtual DateTime Date { get; set; }
        [UserInterfaceParameter(Order = 20)]
        public virtual double NumberOfMinutes { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
