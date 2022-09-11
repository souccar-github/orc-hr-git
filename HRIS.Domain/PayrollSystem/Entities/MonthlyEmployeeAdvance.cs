using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.PayrollSystem.Entities
{
    public class MonthlyEmployeeAdvance : Entity, IAggregateRoot
    {
        [UserInterfaceParameter(Order = 5)]
        public virtual DateTime OperationDate { get; set; }
        [UserInterfaceParameter(Order = 10)]
        public virtual float AdvanceAmount
        {

            get; set;
        }
        [UserInterfaceParameter(Order = 15)]
        public virtual string Note { get; set; }
        [UserInterfaceParameter(IsHidden = true)]
        public virtual MonthlyCard MonthlyCard { get; set; }
    }
}
