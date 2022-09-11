using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.AttendanceSystem.RootEntities;
using Souccar.Core;

namespace HRIS.Mapping.AttendanceSystem.RootEntities
{
    public class OvertimeOrderMap : ClassMap<OvertimeOrder>
    {
        public OvertimeOrderMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            Map(x => x.Number);
            Map(x => x.FromDate).Nullable();
            Map(x => x.ToDate).Nullable();
            Map(x => x.OvertimeHoursPerDay);
            Map(x => x.TakeConsiderationHolidaysDeduction);
            Map(x => x.Note).Length(GlobalConstant.MultiLinesStringMaxLength);

            References(x => x.Employee);
            References(x => x.EmployeeManager);
        }
    }
}
