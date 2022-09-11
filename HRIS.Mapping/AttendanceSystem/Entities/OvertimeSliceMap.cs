using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.AttendanceSystem.Entities;

namespace HRIS.Mapping.AttendanceSystem.Entities
{
    public class OvertimeSliceMap : ClassMap<OvertimeSlice>
    {
        public OvertimeSliceMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            Map(x => x.StartSlice);
            Map(x => x.EndSlice);
            Map(x => x.NormalPercentage);
            Map(x => x.NormalValue);
            Map(x => x.HolidayPercentage);
            Map(x => x.HolidayValue);
            Map(x => x.ParticularShiftPercentage);
            Map(x => x.ParticularShiftValue);

            References(x => x.OvertimeForm);

            
        }
    }
}
