#region

using FluentNHibernate.Mapping;
using HRIS.Domain.Personnel.Entities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.Personnel.Entities
{
    public sealed class DrivingLicenseMap : ClassMap<DrivingLicense>
    {
        public DrivingLicenseMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Number).Column("No").Length(GlobalConstant.SimpleStringMaxLength);

            References(x => x.Type);

            Map(x => x.IssuanceDate);
            Map(x => x.ExpiryDate);

            References(x => x.PlaceOfIssuance);

            Map(x => x.LegalCondition);

            References(x => x.Employee);
            HasMany(x => x.Attachments).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}