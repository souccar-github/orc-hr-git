using FluentNHibernate.Mapping;
using HRIS.Domain.Training.RootEntities;

namespace HRIS.Mapping.Training.RootEntities
{

    public sealed class TrainingPlanMap : ClassMap<TrainingPlan>
    {
        public TrainingPlanMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.PlanName);
            Map(x => x.StartDate);
            Map(x => x.EndDate);
            //Map(x => x.Quarter);
            Map(x => x.CreationDate);


            HasMany(x => x.Courses).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }

}
