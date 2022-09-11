//using FluentNHibernate.Mapping;
//using HRIS.Domain.PayrollSystem.RootEntities;

//namespace HRIS.Mapping.PayrollSystem.RootEntities
//{
//    public class JobTitleBenefitMasterMap : ClassMap<JobTitleBenefitMaster>
//    {
//        public JobTitleBenefitMasterMap()
//        {
//            #region Default
//            DynamicUpdate();
//            DynamicInsert();
//            Id(x => x.Id);
//            Map(x => x.IsVertualDeleted);
//            #endregion


//            Map(x => x.AuditState);
//            References(x => x.JobTitle).Unique();

//            HasMany(x => x.JobTitleBenefitDetails).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
//        }
//    }
//}
