#region

using FluentNHibernate.Mapping;
using HRIS.Domain.Personnel.Entities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.Personnel.Entities
{
    public sealed class CertificationMap : ClassMap<Certification>
    {
        public CertificationMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.Type);

            Map(x => x.DateOfIssuance);
            References(x => x.PlaceOfIssuance);
            Map(x => x.ExpirationDate);

            Map(x => x.Notes).Length(GlobalConstant.MultiLinesStringMaxLength);

            //References(x => x.Status);

            References(x => x.Employee);
            HasMany(x => x.Attachments).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

        }
    }
}