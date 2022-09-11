using FluentNHibernate.Mapping;
using HRIS.Domain.Training.RootEntities;
using Souccar.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.Training.RootEntities
{

    public sealed class CareerPathFamilyMap : ClassMap<CareerPathFamily>
    {
        public CareerPathFamilyMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            Map(x => x.Name).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.CreationDate);
            References(x => x.Node).Nullable();
            HasMany(x => x.CareerPathNodes).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

        }
    }
}
