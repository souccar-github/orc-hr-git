using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.AttendanceSystem.Entities;

namespace HRIS.Mapping.AttendanceSystem.Entities
{
    public class NormalShiftMap : ClassMap<NormalShift>
    {
        public NormalShiftMap()
        {

            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.NormalShiftOrder).Nullable();
            Map(x => x.EntryTime).Nullable();
            Map(x => x.ExitTime).Nullable();
            Map(x => x.ShiftRangeStartTime).Nullable();
            Map(x => x.ShiftRangeEndTime).Nullable();
            Map(x => x.ShiftRelatedToNextDay);
            Map(x => x.IgnoredPeriodBeforeEntryTime);
            Map(x => x.IgnoredPeriodAfterEntryTime);
            Map(x => x.IgnoredPeriodBeforeExitTime);
            Map(x => x.IgnoredPeriodAfterExitTime);
            Map(x => x.RestPeriod);
            Map(x => x.RestRangeStartTime).Nullable();
            Map(x => x.RestRangeEndTime).Nullable();

            References(x => x.Workshop);

        }
    }
}
