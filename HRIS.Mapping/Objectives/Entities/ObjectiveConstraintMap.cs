#region

using FluentNHibernate.Mapping;
using HRIS.Domain.Objectives.Entities;

#endregion

namespace HRIS.Mapping.Objectives.Entities
{
    public sealed class ObjectiveConstraintMap : ClassMap<ObjectiveConstraint>
    {
        public ObjectiveConstraintMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            #region Basic Info

            

            #endregion

            #region Objective Constraint

            Map(x => x.Description);

            References(x => x.Type);
            References(x => x.Objective);

            #endregion

        }
    }
}