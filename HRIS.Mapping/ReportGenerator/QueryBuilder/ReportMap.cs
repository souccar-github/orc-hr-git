#region

using FluentNHibernate.Mapping;
using Souccar.ReportGenerator.Domain.QueryBuilder;

#endregion

namespace HRIS.Mapping.ReportGenerator.QueryBuilder
{
    public sealed class ReportMap : ClassMap<Souccar.ReportGenerator.Domain.QueryBuilder.Report>
    {
        public ReportMap()
        {
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            DynamicUpdate();
            DynamicInsert();
            Map(x => x.Name);
            Map(x => x.ReportResourceName);
            //Map(x => x.ReportType);

            HasMany(x => x.QueryTreesList).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            References(x => x.Template);
        }
    }
}