#region

using FluentNHibernate.Mapping;
using HRIS.Domain.Personnel.Entities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.Personnel.Entities
{
    public sealed class PassportMap : ClassMap<Passport>
    {
        public PassportMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Number).Column("No").Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.FirstName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.LastName).Length(GlobalConstant.SimpleStringMaxLength);
            //Map(x => x.SecondName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.FirstNameL2).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.LastNameL2).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.MotherName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.FatherName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.IssuanceDate);
            Map(x => x.ExpiryDate);
            References(x => x.PlaceOfIssuance);
        
            References(x => x.Employee);
            HasMany(x => x.Attachments).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}

#warning to review