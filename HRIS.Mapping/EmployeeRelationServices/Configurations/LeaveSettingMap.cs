using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using Souccar.Core;

namespace HRIS.Mapping.EmployeeRelationServices.Configurations
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    public sealed class LeaveSettingMap : ClassMap<LeaveSetting>
    {
        public LeaveSettingMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Name).Not.Nullable();
            Map(x => x.AliasName);
            Map(x => x.MaturityPeriod);
            References(x => x.Type).Not.Nullable();
            Map(x => x.IntervalDays);
            Map(x => x.Balance);
            Map(x => x.HasMonthlyBalance);
            Map(x => x.MonthlyBalance);
            Map(x => x.PaidPercentage);
            Map(x => x.HasMaximumNumber);
            Map(x => x.MaximumNumber);
            Map(x => x.IsIndivisible);
            Map(x => x.IsContinuous);
            Map(x => x.RoundPercentage);
            Map(x => x.IsDivisibleToHours);
            Map(x => x.MaximumHoursPerDay);
            Map(x => x.HoursEquivalentToOneLeaveDay);
            Map(x => x.IsAffectedByAssigningDate);
            Map(x => x.IsAffectedByStartWorkingDate);
            Map(x => x.MaximumRoundedLeaveDays);
            Map(x => x.Description);
            References(x => x.WorkflowSetting).Not.Nullable();
            References(x => x.DeductionCard);

            HasMany(x => x.BalanceSlices).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.PaidSlices).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.Recycles).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }

}
