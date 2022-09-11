using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.EmployeeRelationServices.Configurations
{
    public sealed class EntranceExitRecordRequestMap : ClassMap<EntranceExitRecordRequest>
    {
        public EntranceExitRecordRequestMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.Creator);
            References(x => x.Employee);
            References(x => x.WorkflowItem);

            Map(x => x.LogDateTime);
            Map(x => x.LogType);
            Map(x => x.Note);
            Map(x => x.RecordDate);
            Map(x => x.RecordStatus);
            Map(x => x.RecordTime);
        }
    }
}
