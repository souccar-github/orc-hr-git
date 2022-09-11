#region

using FluentNHibernate.Mapping;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.Personnel.Indexes
{
    public sealed class CountryMap : ClassMap<Country>
    {
        public CountryMap()
        {
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Order).Column("ValueOrder");
        }
    }
}