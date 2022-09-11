using HRIS.Domain.Recruitment.Entities;
using FluentNHibernate.Mapping;

namespace HRIS.Mapping.Recruitment.Entities
{
    public class TrainingCourseLanguageMap : ClassMap<TrainingCourseLanguage>
    {
        public TrainingCourseLanguageMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x=>x.LanguageName);
            References(x=>x.Writing);
            References(x=>x.Reading);
            References(x=>x.Listening);
            References(x=>x.Speaking);
            References(x=>x.JobApplication);
        }

    }
}
