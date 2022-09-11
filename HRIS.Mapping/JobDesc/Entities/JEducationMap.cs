#region

using FluentNHibernate.Mapping;
using HRIS.Domain.JobDescription.Entities;

#endregion

namespace HRIS.Mapping.JobDescription.Entities
{
    public sealed class JEducationMap : ClassMap<JEducation>
    {
        public JEducationMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.Type);

            References(x => x.Major);

            Map(x => x.Rank);

            Map(x => x.Score);
            

            Map(x => x.Weight);

            Map(x => x.Required);

            References(x => x.JobDescription);
        }
    }
}