using FluentNHibernate.Mapping;
using HRIS.Domain.PayrollSystem.Entities;

namespace HRIS.Mapping.PayrollSystem.Entities
{
    public class MonthlyCardMap : ClassMap<MonthlyCard>
    {
        public MonthlyCardMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Salary);
            Map(x => x.PackageSalary);
            Map(x => x.InsuranceSalary);
            Map(x => x.TempSalary1);
            Map(x => x.TempSalary2);
            Map(x => x.BenefitSalary);
            Map(x => x.Threshold);
            Map(x => x.IsCalculated);
            Map(x => x.WorkDays);
            Map(x => x.TaxableAmount);
            //Map(x => x.TotalDeducationsValue);
            //Map(x => x.TotalLoanPayments);
            //Map(x => x.TotalBenefitsValue);
            Map(x => x.HourlyMissionValue);
            Map(x => x.InternalTravelMissionValue);
            Map(x => x.ExternalTravelMissionValue);
            Map(x => x.TotalWorkingHours);
            Map(x => x.FinalMonthSalary);
            Map(x => x.ActualMonthSalary);
            //Map(x => x.AuditState);
            //Map(x => x.NegativeSalary);


            References(x => x.Month);
            References(x => x.PrimaryCard);

            HasMany(x => x.MonthlyEmployeeBenefits).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.MonthlyEmployeeDeductions).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.LoanPayments).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.MonthlyEmployeeAdvances).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}