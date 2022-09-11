#region

using FluentNHibernate.Mapping;
using HRIS.Domain.JobDescription.Configurations;
using HRIS.Domain.JobDescription.RootEntities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.JobDescription.Configurations
{
    public sealed class CompetencyCategoryMap : ClassMap<CompetenceCategory>
    {
        public CompetencyCategoryMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.Name);

            References(x => x.Type);
            HasMany(x => x.LevelDescriptions).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
           

        }
    }
}