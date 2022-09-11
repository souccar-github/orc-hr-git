using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.AttendanceSystem.RootEntities;
using Souccar.Core;

namespace HRIS.Mapping.AttendanceSystem.RootEntities
{
    public class TravelMissionMap : ClassMap<TravelMission>
    {
        public TravelMissionMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            Map(x => x.FromDate).Nullable();
            Map(x => x.ToDate).Nullable();
            Map(x => x.Note).Nullable().Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.CreationDate);
            Map(x => x.Status);
            Map(x => x.Type).Nullable();
            Map(x => x.IsTransferedToPayroll);
            References(x => x.WorkflowItem).Nullable();
            References(x => x.Employee);

        }
    }
}
