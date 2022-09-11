//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//project manager:
//supervisor:
//author: Ammar Alziebak
//description:
//start date:
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//using FluentNHibernate.Mapping;
//using HRIS.Domain.Training.RootEntities;

//namespace HRIS.Mapping.Training.RootEntities
//{

//    public sealed class TrainingNeedsPoolMap : ClassMap<TrainingNeedsPool>
//    {
//        public TrainingNeedsPoolMap()
//        {
//            DynamicUpdate();
//            DynamicInsert();

//            Id(x => x.Id);

//            Map(x => x.Description).Length(250);
//            Map(x => x.Name).Length(50);

//            References(x => x.Department).Nullable();
//            References(x => x.Level).Nullable();

//            HasMany(x => x.Courses).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
//        }
//    }

//}
