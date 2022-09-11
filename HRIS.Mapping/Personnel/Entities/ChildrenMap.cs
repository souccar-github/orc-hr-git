#region

using FluentNHibernate.Mapping;
using HRIS.Domain.Personnel.Entities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.Personnel.Entities
{
    public sealed class ChildrenMap : ClassMap<Child>
    {
        public ChildrenMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.FirstName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.LastName).Length(GlobalConstant.SimpleStringMaxLength);

            Map(x => x.OrderInFamily);

            Map(x => x.Gender);
            Map(x => x.MaritalStatus);
            Map(x => x.IsEmployed);
            Map(x => x.IsStudying);
            Map(x => x.DisabilityExist);
            Map(x => x.IsDeath);
            Map(x => x.DeathDate);
            Map(x => x.DateOfBirth);
            References(x => x.PlaceOfBirth);

            References(x => x.Spouse);
            References(x => x.Nationality);

            Map(x => x.ResidencyNo).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.ResidencyExpiryDate).Nullable();

            Map(x => x.PassportNo).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.PassportExpiryDate).Nullable();

            Map(x => x.HasChildBenefit);
            Map(x => x.ChildBenefitStartDate);
            Map(x => x.ChildBenefitEndDate).Nullable();

            References(x => x.Employee);
            HasMany(x => x.Attachments).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}