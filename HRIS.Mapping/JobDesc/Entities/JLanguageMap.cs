#region

using FluentNHibernate.Mapping;
using HRIS.Domain.JobDescription.Entities;

#endregion

namespace HRIS.Mapping.JobDescription.Entities
{
    public sealed class JLanguageMap : ClassMap<JLanguage>
    {
        public JLanguageMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.LanguageName);

            References(x => x.Writing);

            References(x => x.Reading);

            References(x => x.Speaking);

            References(x => x.Listening);

            Map(x => x.Weight);

            Map(x => x.Required);

            References(x => x.JobDescription);
        }
    }
}