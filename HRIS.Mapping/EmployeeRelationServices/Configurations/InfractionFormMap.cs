using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using Souccar.Core;

namespace HRIS.Mapping.EmployeeRelationServices.Configurations
{
    public class InfractionFormMap : ClassMap<InfractionForm>
    {
        public InfractionFormMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            Map(x => x.Number);
            References(x => x.Infraction);
            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);

            HasMany(x => x.InfractionSlices).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            
        }
    }
}
