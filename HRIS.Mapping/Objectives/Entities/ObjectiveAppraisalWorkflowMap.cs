using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Objectives.Entities;

namespace HRIS.Mapping.Objectives.Entities
{
    public sealed class ObjectiveAppraisalWorkflowMap : ClassMap<ObjectiveAppraisalWorkflow>
    {
        public ObjectiveAppraisalWorkflowMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.Objective);
            References(x => x.WorkflowItem);
            References(x => x.Phase).Column("Phase_Id");
        }
    }
}
