#region

using FluentNHibernate.Mapping;
using HRIS.Domain.Personnel.Indexes;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.Personnel.Indexes
{
    public sealed class DrivingLicenseTypeMap : ClassMap<DrivingLicenseType>
    {
        public DrivingLicenseTypeMap()
        {
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength).Unique();
            Map(x => x.Order).Column("ValueOrder");
        }
    }
}