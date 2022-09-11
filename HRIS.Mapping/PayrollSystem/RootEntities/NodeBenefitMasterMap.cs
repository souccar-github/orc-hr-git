//using FluentNHibernate.Mapping;
//using HRIS.Domain.PayrollSystem.RootEntities;

//namespace HRIS.Mapping.PayrollSystem.RootEntities
//{
//    public class NodeBenefitMasterMap : ClassMap<NodeBenefitMaster>
//    {
//        public NodeBenefitMasterMap()
//        {
//            #region Default
//            DynamicUpdate();
//            DynamicInsert();
//            Id(x => x.Id);
//            Map(x => x.IsVertualDeleted);
//            #endregion


//            Map(x => x.AuditState);
//            References(x => x.Node).Unique();

//            HasMany(x => x.NodeBenefitDetails).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
//        }
//    }

//}
