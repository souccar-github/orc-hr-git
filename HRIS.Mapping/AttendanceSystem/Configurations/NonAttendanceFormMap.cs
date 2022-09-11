using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.AttendanceSystem.Configurations;
using Souccar.Core;

namespace HRIS.Mapping.AttendanceSystem.Configurations
{
    public class NonAttendanceFormMap : ClassMap<NonAttendanceForm>
    {
        public NonAttendanceFormMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            Map(x => x.Number);
            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.ResetCounterRecurrence);
            
            Map(x => x.LastReset);
            

            References(x => x.InfractionForm);

            HasMany(x => x.NonAttendanceSlices).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}
