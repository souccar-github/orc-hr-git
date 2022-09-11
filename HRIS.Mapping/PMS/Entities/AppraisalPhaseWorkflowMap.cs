#region

using FluentNHibernate.Mapping;
using HRIS.Domain.PMS.Entities;

#endregion

namespace HRIS.Mapping.PMS.Entities
{
    public sealed class AppraisalPhaseWorkflowMap : ClassMap<AppraisalPhaseWorkflow>
    {
        public AppraisalPhaseWorkflowMap()
        {

            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion


            References(x => x.WorkflowItem);
            References(x => x.Position);
            References(x => x.AppraisalPhase).Column("Phase_Id");
            References(x => x.Employee);
            HasMany(x => x.Appraisals).Inverse().LazyLoad().Cascade.AllDeleteOrphan().KeyColumn("PhaseWorkflow_id");

        }
    }
}