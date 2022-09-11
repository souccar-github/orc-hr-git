using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Core.CustomAttribute;
using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.PayrollSystem.RootEntities
{
    public class LeaveOfDeduction : Entity, IAggregateRoot
    {
        public LeaveOfDeduction(int leaveId, int deductionCard , int monthId)
        {
            this.LeaveId = leaveId;
            this.CardNo = deductionCard;
            this.MonthId = monthId;
        }
        public LeaveOfDeduction()
        {

        }

      [UserInterfaceParameter(Order = 10)]
        public virtual int LeaveId { get; set; }


        public virtual int CardNo { get; set; }
        public virtual int MonthId { get; set; }


    }
}
