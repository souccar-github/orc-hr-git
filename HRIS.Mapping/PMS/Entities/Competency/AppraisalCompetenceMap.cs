#region

using FluentNHibernate.Mapping;
using HRIS.Domain.PMS.Entities.Competency;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.PMS.Entities.Competency
{
    /// <summary>
    /// Ammar Alziebak
    /// </summary>
    public sealed class AppraisalCompetenceMap : ClassMap<AppraisalCompetence>
    {
        public AppraisalCompetenceMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.Competence);
            Map(x => x.Weight);
            Map(x => x.Rate);
            Map(x => x.Description);

            References(x => x.Appraisal).Column("PhaseAppraisal_id");

        }
    }
}