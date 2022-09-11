#region

using FluentNHibernate.Mapping;
using HRIS.Domain.PMS.RootEntities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.PMS.RootEntities
{
    public sealed class AppraisalPhaseMap : ClassMap<AppraisalPhase>
    {
        public AppraisalPhaseMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
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
            Map(x => x.Name);
            #endregion
           
            #region References

            References(x => x.AppraisalTemplateSetting).Column("TemplateSetting_id");
            References(x => x.AppraisalPhaseSetting);

            #endregion

            #region Phase Workflows

            HasMany(x => x.PhaseWorkflows).Inverse().LazyLoad().Cascade.AllDeleteOrphan().KeyColumn("Phase_Id");

            #endregion

        }
    }
}