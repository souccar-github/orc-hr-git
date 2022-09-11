#region

using FluentNHibernate.Mapping;
using HRIS.Domain.JobDescription.Entities;

#endregion

namespace HRIS.Mapping.JobDescription.Entities
{
    public sealed class JExperienceMap : ClassMap<JExperience>
    {
        public JExperienceMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.Industry);

            References(x => x.CareerLevel);

            Map(x => x.Weight);

            Map(x => x.Required);

            References(x => x.JobDescription);
        }
    }
}