using FluentNHibernate.Mapping;
using HRIS.Domain.Recruitment.Entities;
using Souccar.Core;

namespace HRIS.Mapping.Recruitment.Entities
{
    public class PersonalSkillMap:ClassMap<PersonalSkill>
    {
        public PersonalSkillMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x=>x.Description).Length(GlobalConstant.MultiLinesStringMaxLength); 
            Map(x=>x.Comments).Length(GlobalConstant.MultiLinesStringMaxLength); 

            References(x=>x.SkillType);
            References(x=>x.Level);
            References(x=>x.JobApplication);


        }

    }
}
