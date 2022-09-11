using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.AttendanceSystem.Entities;
using HRIS.Domain.AttendanceSystem.Configurations;

namespace HRIS.Mapping.AttendanceSystem.Configurations
{
    public class TemporaryWorkshopMap : ClassMap<TemporaryWorkshop>
    {
        public TemporaryWorkshopMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            Map(x => x.FromDate).Nullable();
            Map(x => x.ToDate).Nullable();

            References(x => x.Workshop);
            References(x => x.AlternativeWorkshop);
            References(x => x.EmployeeCard);
        }
    }
}
