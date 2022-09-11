#region

using FluentNHibernate.Mapping;
using HRIS.Domain.JobDescription.Entities;

#endregion

namespace HRIS.Mapping.JobDescription.Entities
{
    public sealed class JobDescriptionDelegateMap : ClassMap<JobDescriptionDelegate>
    {
        public JobDescriptionDelegateMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.AuthorityType);
            References(x => x.PrimaryJobDescription).Column("PrimaryJobDescription");
            References(x => x.SecondaryJobDescription).Column("SecondaryJobDescription");
        }
    }
}