#region

using FluentNHibernate.Mapping;
using HRIS.Domain.JobDescription.Entities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.JobDescription.Entities
{
    public sealed class PositionMap : ClassMap<Position>
    {
        public PositionMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Code).Length(GlobalConstant.SimpleStringMaxLength).Not.Nullable();

            HasMany(x => x.Status).LazyLoad().Cascade.AllDeleteOrphan();

            References(x => x.Type);

            Map(x => x.DisabilityStatus);

            Map(x => x.PositionStatusType);

            Map(x => x.WorkingHours);

            References(x => x.Per);
          
            Map(x => x.Budget);

            References(x => x.CurrencyType);

            References(x => x.CostCenter);

            References(x => x.Step);

            References(x => x.JobDescription);

            References(x => x.AssigningEmployeeToPosition);
            Map(x => x.ManagerIsMandatory);
            References(x => x.Manager);

            References(x => x.ManagerJobTitle);

            HasMany(x => x.ManagerTo).LazyLoad().Cascade.AllDeleteOrphan().KeyColumn("ManagerPosition");
            HasMany(x => x.ReportingsTo).LazyLoad().Cascade.AllDeleteOrphan().KeyColumn("Position");
            HasMany(x => x.Delegates).LazyLoad().Cascade.AllDeleteOrphan().KeyColumn("PrimaryPosition_Id");
            HasMany(x => x.DelegatesTo).LazyLoad().Cascade.AllDeleteOrphan().KeyColumn("SecondaryPosition_Id");

            HasMany(x => x.DelegateAuthoritiesFromPositions).LazyLoad().Cascade.AllDeleteOrphan().KeyColumn("SourcePosition_Id");
            HasMany(x => x.DelegateAuthoritiesToPositions).LazyLoad().Cascade.AllDeleteOrphan().KeyColumn("DestinationPosition_Id");

            HasMany(x => x.DelegateRolesAsSuperiorPositions).LazyLoad().Cascade.AllDeleteOrphan().KeyColumn("Superior_Id");
            HasMany(x => x.DelegateRolesFromPositions).LazyLoad().Cascade.AllDeleteOrphan().KeyColumn("SourcePosition_Id");
            HasMany(x => x.DelegateRolesToPositions).LazyLoad().Cascade.AllDeleteOrphan().KeyColumn("DestinationPosition_Id");
            HasMany(x => x.PositionBenefitDetails).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.PositionDeductionDetails).Inverse().LazyLoad().Cascade.AllDeleteOrphan();


        }
    }
}
