#region

using FluentNHibernate.Mapping;
using HRIS.Domain.Personnel.Entities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.Personnel.Entities
{
    public sealed class MilitaryServiceMap : ClassMap<MilitaryService>
    {
        public MilitaryServiceMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Status);
            Map(x => x.IsPermamentExemption);
            Map(x => x.ExemptionReason);
            Map(x => x.DateOfExemption).Nullable();
            Map(x => x.DelayReason);
            Map(x => x.DateOfDelay).Nullable();
            Map(x => x.SendDelayExpirationNotification);
            Map(x => x.MilitiryServiceNo).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.MilitiryServiceDocIssuance).Nullable();

            Map(x => x.Months);
            Map(x => x.Years);
            Map(x => x.Days);

            Map(x => x.ServiceStartDate).Nullable();
            Map(x => x.ServiceEndDate).Nullable();
            Map(x => x.HoldDate).Nullable();
            Map(x => x.ReserveStartDate).Nullable();
            Map(x => x.Notes).Length(GlobalConstant.MultiLinesStringMaxLength);

            References(x => x.Granter);
            References(x => x.Employee);
            HasMany(x => x.Attachments).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}
