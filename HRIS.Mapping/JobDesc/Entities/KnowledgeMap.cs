#region

using FluentNHibernate.Mapping;
using HRIS.Domain.JobDescription.Entities;

#endregion

namespace HRIS.Mapping.JobDescription.Entities
{
    public sealed class KnowledgeMap : ClassMap<Knowledge>
    {
        public KnowledgeMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.KnowledgeType);

            References(x => x.Level);

            Map(x => x.Weight);

            Map(x => x.Required);

            References(x => x.JobDescription);
        }
    }
}