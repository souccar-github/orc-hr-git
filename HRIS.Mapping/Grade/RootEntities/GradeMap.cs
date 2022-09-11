#region

using FluentNHibernate.Mapping;
using HRIS.Domain.OrganizationChart.RootEntities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.Grade.RootEntities
{
    public sealed class GradeMap : ClassMap<HRIS.Domain.Grades.RootEntities.Grade>
    {
        public GradeMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Category);
            References(x => x.OrganizationalLevel);
            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Order).Column("Grade_Order");
            References(x => x.GradeByEducation);
            Map(x => x.PayGroup).Length(GlobalConstant.SimpleStringMaxLength);

            Map(x => x.MinSalary);
            Map(x => x.MaxSalary);
            References(x => x.CurrencyType);
            References(x => x.LeaveTemplateMaster);
            References(x => x.AttendanceForm);
            References(x => x.LatenessForm);
            References(x => x.OvertimeForm);
            References(x => x.AbsenceForm);
            References(x => x.HealthInsuranceTypes);

            HasMany(x => x.Steps).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.NonCashBenefits).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.JobTitles).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.GradeBenefitDetails).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.GradeDeductionDetails).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}