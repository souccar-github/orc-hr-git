using FluentNHibernate.Mapping;
using HRIS.Domain.AttendanceSystem.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.AttendanceSystem.Configurations
{
    public class GeneralSettingsMap : ClassMap<GeneralSettings>
    {
        public GeneralSettingsMap() 
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            Map(x => x.AttendanceCycleStartDay);
            References(x => x.AttendanceForm);
            References(x => x.LatenessForm);
            References(x => x.OvertimeForm);
            References(x => x.AbsenceForm);
        }
    }
}
