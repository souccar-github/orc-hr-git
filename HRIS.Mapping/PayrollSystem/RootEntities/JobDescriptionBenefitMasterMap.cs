//using FluentNHibernate.Mapping;
//using HRIS.Domain.PayrollSystem.RootEntities;

//namespace HRIS.Mapping.PayrollSystem.RootEntities
//{
//    public class JobDescriptionBenefitMasterMap : ClassMap<JobDescriptionBenefitMaster>
//    {
//        public JobDescriptionBenefitMasterMap()
//        {
//            #region Default
//            DynamicUpdate();
//            DynamicInsert();
//            Id(x => x.Id);
//            Map(x => x.IsVertualDeleted);
//            #endregion


//            Map(x => x.AuditState);
//            References(x => x.JobDescription).Unique();

//            HasMany(x => x.JobDescriptionBenefitDetails).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
//        }
//    }

//}
