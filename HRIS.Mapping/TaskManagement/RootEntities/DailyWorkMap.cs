using FluentNHibernate.Mapping;
using HRIS.Domain.TaskManagement.RootEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.TaskManagement.RootEntities
{
    public class DailyWorkMap : ClassMap<DailyWork>
    {
        public DailyWorkMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.CreationDate);
            Map(x => x.Date);
            Map(x => x.Description);

            Map(x => x.Progress);

            References(x => x.Task);
            References(x => x.Employee);
        }
    }
}
