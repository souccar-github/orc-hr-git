#region

using FluentNHibernate.Mapping;
using HRIS.Domain.Personnel.Entities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.Personnel.Entities
{
    public sealed class DependentMap : ClassMap<Dependent>
    {
        public DependentMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.FirstName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.LastName).Length(GlobalConstant.SimpleStringMaxLength);

            References(x => x.KinshipType);
            Map(x => x.KinshipLevel);

            Map(x => x.DateOfBirth);
            References(x => x.PlaceOfBirth);

            References(x => x.Nationality);

            Map(x => x.ContactNumber).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.DeathDate);
            Map(x => x.IsDeath);

            References(x => x.Employee);
            HasMany(x => x.Attachments).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

        }
    }
}