using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.PayrollSystem.Enums;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.PayrollSystem.Entities
{
    public class LeaveDeduction : Entity
    {
        public LeaveDeduction()
        {
        }

        public virtual int LeaveId { get; set; }
        public virtual MonthlyEmployeeDeduction MonthlyEmployeeDeduction { get; set; }

    }
}
