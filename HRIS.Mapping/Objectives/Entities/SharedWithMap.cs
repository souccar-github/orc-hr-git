#region

using System;
using FluentNHibernate.Mapping;
using HRIS.Domain.Objectives.Entities;

#endregion

namespace HRIS.Mapping.Objectives.Entities
{
    public sealed class SharedWithMap : ClassMap<SharedWith>
    {
        public SharedWithMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.Department);
            References(x => x.Position);

            Map(x => x.Percentage);
            Map(x => x.Description);

            References(x => x.Objective).Column("Objective_Id");
          
        }
    }
}