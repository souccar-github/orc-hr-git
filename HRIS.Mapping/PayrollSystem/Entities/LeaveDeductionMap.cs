using FluentNHibernate.Mapping;
using HRIS.Domain.PayrollSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.PayrollSystem.Entities
{
    public sealed class LeaveDeductionMap : ClassMap<LeaveDeduction>
    {
        public LeaveDeductionMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.LeaveId);
            References(x => x.MonthlyEmployeeDeduction);
        }
    }
}
