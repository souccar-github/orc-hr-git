#region

using FluentNHibernate;
using FluentNHibernate.Mapping;
using HRIS.Domain.Objectives.RootEntities;

#endregion

namespace HRIS.Mapping.Objectives.RootEntities
{
    public sealed class StrategicObjectiveMap : ClassMap<StrategicObjective>
    {
        public StrategicObjectiveMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            #region Basic Info

            //Id(x => x.Id).GeneratedBy.Foreign("Objective");//When use one-to-one case.

            
            #endregion

            #region Abstract Basic Info

            Map(x => x.Name);
            Map(x => x.Description);
            Map(x => x.Code);

            Map(x => x.StartDate);
            Map(x => x.EndDate);


            Map(x => x.DoesNotMeetExpectation);
            Map(x => x.MeetExpectation);
            Map(x => x.AboveExpectation);

            #endregion

            #region Strategic Objective Info

            Map(x => x.Period);
           
            HasMany(x => x.Objectives).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            References(x => x.Dimension);

            #endregion
        }
    }
}