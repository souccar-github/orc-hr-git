//using FluentNHibernate.Mapping;
//using HRIS.Domain.PayrollSystem.RootEntities;

//namespace HRIS.Mapping.PayrollSystem.RootEntities
//{
//    public class PositionBenefitMasterMap : ClassMap<PositionBenefitMaster>
//    {
//        public PositionBenefitMasterMap()
//        {
//            #region Default
//            DynamicUpdate();
//            DynamicInsert();
//            Id(x => x.Id);
//            Map(x => x.IsVertualDeleted);
//            #endregion


//            Map(x => x.AuditState);
//            References(x => x.Position).Unique();

//            HasMany(x => x.PositionBenefitDetails).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
//        }
//    }

//}
