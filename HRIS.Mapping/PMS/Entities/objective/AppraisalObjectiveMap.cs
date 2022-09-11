#region

using FluentNHibernate.Mapping;
using HRIS.Domain.PMS.Entities.Competency;
using HRIS.Domain.PMS.Entities.objective;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.PMS.Entities.objective
{
    /// <summary>
    /// Ammar Alziebak
    /// </summary>
    public sealed class AppraisalObjectiveMap : ClassMap<AppraisalObjective>
    {
        public AppraisalObjectiveMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.Objective);
            Map(x => x.Weight);
            Map(x => x.Rate);
            Map(x => x.Description);

            References(x => x.Appraisal).Column("PhaseAppraisal_id");

        }
    }
}