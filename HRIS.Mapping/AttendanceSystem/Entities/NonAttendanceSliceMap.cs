using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.AttendanceSystem.Entities;

namespace HRIS.Mapping.AttendanceSystem.Entities
{
    public class NonAttendanceSliceMap : ClassMap<NonAttendanceSlice>
    {
        public NonAttendanceSliceMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            Map(x => x.StartSlice);
            Map(x => x.EndSlice);
            Map(x => x.Value);

            References(x => x.NonAttendanceForm);
            References(x => x.InfractionForm);

            HasMany(x => x.NonAttendanceSlicePercentages).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}
