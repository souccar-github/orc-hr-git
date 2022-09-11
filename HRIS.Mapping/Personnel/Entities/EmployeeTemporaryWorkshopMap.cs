using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.Personnel.Entities;

namespace HRIS.Mapping.Personnel.Entities
{
    class EmployeeTemporaryWorkshopMap : ClassMap<EmployeeTemporaryWorkshop>
    {
        public EmployeeTemporaryWorkshopMap()
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
            References(x => x.EmployeeCard);
        }
    }
}
