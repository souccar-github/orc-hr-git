//using FluentNHibernate.Mapping;
//using HRIS.Domain.Services;

//namespace HRIS.Mapping.Services
//{
//    public sealed class DelegationMap : ClassMap<Delegation>
//    {
//        public DelegationMap()
//        {
//            DynamicUpdate();
//            DynamicInsert();

//            #region Basic Info

//            Id(x => x.Id);

//            #region References

//            References(x => x.Position);

//            #endregion

//            #region

//            Map(x => x.From).Column("FromDate");
//            Map(x => x.To).Column("ToDate");
//            Map(x => x.FromDate).Column("CreatedDate");
//            Map(x => x.ExpireDate);
//            Map(x => x.Appraisable);

//            #endregion

//            Map(x => x.Reason);
//            Map(x => x.Comment);

//            #endregion
//        }
//    }
//}