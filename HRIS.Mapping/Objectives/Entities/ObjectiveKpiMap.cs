#region

using FluentNHibernate.Mapping;
using HRIS.Domain.Objectives.Entities;

#endregion

namespace HRIS.Mapping.Objectives.Entities
{
    public sealed class ObjectiveKpiMap : ClassMap<ObjectiveKpi>
    {
        public ObjectiveKpiMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            #region Basic Info

      
            #endregion

            #region Objective Kpi

            Map(x => x.Value);
            Map(x => x.Weight);
            Map(x => x.Description);

            //Objective
            References(x => x.Objective);

            #endregion
        }
    }
}