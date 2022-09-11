#region

using FluentNHibernate.Mapping;
using HRIS.Domain.Personnel.Entities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.Personnel.Entities
{
    public sealed class ResidencyMap : ClassMap<Residency>
    {
        public ResidencyMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.No).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.FirstName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.LastName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.SecondName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.MotherName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.FatherName).Length(GlobalConstant.SimpleStringMaxLength);
            References(x => x.Type);
            References(x => x.Nationality);
            Map(x => x.IssuanceDate);
            Map(x => x.ExpiryDate);
            Map(x => x.Address).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.Tel).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Notes).Length(GlobalConstant.MultiLinesStringMaxLength);

            References(x => x.Employee);
            HasMany(x => x.Attachments).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}