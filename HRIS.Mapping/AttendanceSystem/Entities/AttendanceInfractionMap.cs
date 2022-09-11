using FluentNHibernate.Mapping;
using HRIS.Domain.AttendanceSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.AttendanceSystem.Entities
{
    public sealed class AttendanceInfractionMap : ClassMap<AttendanceInfraction>
    {
        public AttendanceInfractionMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.CreationDate);
            Map(x => x.PenaltyDate);
            Map(x => x.IsActiveForNextPenalties);
            Map(x => x.RepeationNumber);
            References(x => x.AttendanceRecord);
            References(x => x.EmployeeCard);
            References(x => x.Infraction);
            References(x => x.Penalty);
        }
    }
}
