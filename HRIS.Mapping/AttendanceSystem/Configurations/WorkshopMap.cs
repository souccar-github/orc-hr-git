using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.AttendanceSystem.Configurations;
using Souccar.Core;

namespace HRIS.Mapping.AttendanceSystem.Configurations
{
    public class WorkshopMap : ClassMap<Workshop>
    {
        public WorkshopMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            Map(x => x.Number);
            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);

            HasMany(x => x.ParticularOvertimeShifts).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.TemporaryWorkshops).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.NormalShifts).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

        }
    }
}
