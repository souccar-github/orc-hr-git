using FluentNHibernate.Mapping;
using Souccar.Domain.Audit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.Audit
{
    public sealed class LogMap : ClassMap<Log>
    {
        public LogMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.ClassName);
            Map(x => x.AffecetedRow);
            Map(x => x.Date);
            Map(x => x.Time);
            Map(x => x.Description);
            Map(x => x.OperationType);
            Map(x => x.IsWithWorkFlow);
            Map(x => x.ProcessingPeriod);

            References(x => x.User);
        }
    }

}
