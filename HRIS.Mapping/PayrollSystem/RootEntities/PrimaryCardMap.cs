//using FluentNHibernate.Mapping;
//using HRIS.Domain.PayrollSystem.Entities;
//using HRIS.Domain.PayrollSystem.RootEntities;

//namespace HRIS.Mapping.PayrollSystem.RootEntities
//{
//    public class PrimaryCardMap : ClassMap<PrimaryCard>
//    {
//        public PrimaryCardMap()
//        {
//            #region Default
//            DynamicUpdate();
//            DynamicInsert();
//            Id(x => x.Id);
//            Map(x => x.IsVertualDeleted);
//            #endregion


//            Map(x => x.Salary);
//            Map(x => x.InsuranceSalary);
//            Map(x => x.TempSalary1);
//            Map(x => x.TempSalary2);
//            Map(x => x.BenefitSalary);
//            Map(x => x.Threshold);
//            Map(x => x.SalaryDeservableType);
//            Map(x => x.AuditState);
//            //Map(x => x.Status);

//            References(x => x.Employee).Unique();
//            References(x => x.NominationSystem);

//            HasMany(x => x.BankingInformations).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
//            HasMany(x => x.PrimaryEmployeeBenefits).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
//            HasMany(x => x.PrimaryEmployeeDeductions).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
//            HasMany(x => x.EmployeeLoans).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
//        }
//    }
//}