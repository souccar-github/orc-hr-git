using FluentNHibernate.Mapping;
using HRIS.Domain.Objectives.RootEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.Objectives.RootEntities
{
    
    public sealed class ObjectiveCreationPhaseMap : ClassMap<ObjectiveCreationPhase>
    {
        public ObjectiveCreationPhaseMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            #region Basic Info

            #endregion

            #region Abstract Phase Info
            Map(x => x.CreationDate);
            Map(x => x.StartDate);
            Map(x => x.EndDate);
            Map(x => x.Description);
            Map(x => x.Period);
            Map(x => x.Month);
            Map(x => x.Quarter);
            Map(x => x.SemiAnnual);
            Map(x => x.Year);
            
            #endregion

            References(x => x.WorkflowSetting);
        }
    }
}
