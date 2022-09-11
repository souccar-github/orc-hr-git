//using FluentNHibernate.Mapping;
//using HRIS.Domain.PayrollSystem.RootEntities;

//namespace HRIS.Mapping.PayrollSystem.RootEntities
//{
//    public class GradeBenefitMasterMap : ClassMap<GradeBenefitMaster>
//    {
//        public GradeBenefitMasterMap()
//        {
//            #region Default
//            DynamicUpdate();
//            DynamicInsert();
//            Id(x => x.Id);
//            Map(x => x.IsVertualDeleted);
//            #endregion

//            Map(x => x.AuditState);

//            References(x => x.Grade).Unique();

//            HasMany(x => x.GradeBenefitDetails).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
//        }
//    }

//}
