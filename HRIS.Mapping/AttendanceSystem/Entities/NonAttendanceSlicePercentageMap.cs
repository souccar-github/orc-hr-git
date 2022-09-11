using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.AttendanceSystem.Entities;

namespace HRIS.Mapping.AttendanceSystem.Entities
{
    public class NonAttendanceSlicePercentageMap : ClassMap<NonAttendanceSlicePercentage>
    {
        public NonAttendanceSlicePercentageMap()
        {

            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            Map(x => x.PercentageOrder);
            Map(x => x.Percentage);

            References(x => x.NonAttendanceSlice);

        }
    }
}
