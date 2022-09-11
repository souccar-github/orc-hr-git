using HRIS.Domain.Recruitment.Entities;
using FluentNHibernate.Mapping;

namespace HRIS.Mapping.Recruitment.Entities
{
    public class RecruitmentComputerSkillMap:ClassMap<RecruitmentComputerSkill>
    {
        public RecruitmentComputerSkillMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.SkillType);
            References(x => x.Level);
            References(x => x.JobApplication);
        }
       
    }
}
