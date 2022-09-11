using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.Entities;

namespace HRIS.Mapping.EmployeeRelationServices.Entities
{
    public class InfractionSliceMap : ClassMap<InfractionSlice>
    {
        public InfractionSliceMap()
        {

            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            Map(x => x.MinimumRecurrence);
            Map(x => x.MaximumRecurrence);
            
            References(x => x.InfractionForm);
            References(x => x.Penalty);

        }
    }
}
