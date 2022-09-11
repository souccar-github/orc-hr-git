#region

using FluentNHibernate.Mapping;
using HRIS.Domain.Personnel.Entities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.Personnel.Entities
{
    public sealed class ConvictionMap : ClassMap<Conviction>
    {
        public ConvictionMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Number).Column("No").Length(GlobalConstant.SimpleStringMaxLength);

            //Map(x => x.ConvictionDate);
            Map(x => x.IsConvicted);
            Map(x => x.ReleaseDate);
            Map(x => x.ExpirationDate);

            Map(x => x.Reason).Length(GlobalConstant.MultiLinesStringMaxLength);

            References(x => x.Type);

            Map(x => x.Notes).Length(GlobalConstant.SimpleStringMaxLength);

            References(x => x.Employee);
            HasMany(x => x.Attachments).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}
