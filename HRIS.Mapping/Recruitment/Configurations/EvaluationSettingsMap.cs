using FluentNHibernate.Mapping;
using HRIS.Domain.Recruitment.Configurations;
using HRIS.Domain.Recruitment.RootEntities;

namespace HRIS.Mapping.Recruitment.Configurations
{

    public sealed class EvaluationSettingsMap : ClassMap<EvaluationSettings>
    {
        public EvaluationSettingsMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.RecruitmentType).Not.Nullable();

            References(x => x.Grade).Not.Nullable();

            Map(x => x.FullSuccessMark).Not.Nullable(); 
            Map(x => x.MinSuccessMark).Not.Nullable(); 
            Map(x => x.WrittenWeightFactor).Not.Nullable(); 
            Map(x => x.MinWrittenMark).Not.Nullable(); 
            Map(x => x.OralWeightFactor).Not.Nullable();
            Map(x => x.MinOralMark).Not.Nullable();
            Map(x => x.OldnessWeightFactor).Not.Nullable();
            Map(x => x.LaborOfficeStartingDate).Not.Nullable();
            Map(x => x.RoundType).Not.Nullable();
            Map(x => x.MartyrSonFactor).Not.Nullable();
            
        }
    }
}