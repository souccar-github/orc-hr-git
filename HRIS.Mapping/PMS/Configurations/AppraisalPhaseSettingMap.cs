#region

using FluentNHibernate.Mapping;
using HRIS.Domain.PMS.Configurations;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.PMS.Configurations
{
    public sealed class AppraisalPhaseSettingMap : ClassMap<AppraisalPhaseSetting>
    {
        public AppraisalPhaseSettingMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            #region Basic Info

            Map(x => x.Title);

            Map(x => x.FromMark);
            Map(x => x.ToMark);
            Map(x => x.FullMark);
            Map(x => x.MarkStep);
            Map(x => x.FromMarkBelowExpected);
            Map(x => x.ToMarkBelowExpected);
            Map(x => x.FromMarkNeedTraining);
            Map(x => x.ToMarkNeedTraining);
            Map(x => x.FromMarkExpected);
            Map(x => x.ToMarkExpected);
            Map(x => x.FromMarkUpExpected);
            Map(x => x.ToMarkUpExpected);
            Map(x => x.FromMarkDistinct);
            Map(x => x.ToMarkDistinct);
            Map(x => x.GapThreshold);
            Map(x => x.SkillThreshold);

            #endregion

            #region References

            References(x => x.WorkflowSetting);

            #endregion

        }
    }
}