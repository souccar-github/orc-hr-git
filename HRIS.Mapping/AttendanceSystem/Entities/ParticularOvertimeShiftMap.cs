using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.AttendanceSystem.Entities;

namespace HRIS.Mapping.AttendanceSystem.Entities
{
    public class ParticularOvertimeShiftMap : ClassMap<ParticularOvertimeShift>
    {
        public ParticularOvertimeShiftMap()
        {

            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.StartTime).Nullable();
            Map(x => x.EndTime).Nullable();

            References(x => x.Workshop);
        }
    }
}
