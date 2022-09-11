#region

using FluentNHibernate.Mapping;
using Souccar.ReportGenerator.Domain.QueryBuilder;

#endregion

namespace HRIS.Mapping.ReportGenerator.QueryBuilder
{
    public sealed class QueryTreeMap : ClassMap<Souccar.ReportGenerator.Domain.QueryBuilder.QueryTree>
    {
        public QueryTreeMap()
        {
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            DynamicUpdate();
            DynamicInsert();
            Map(x => x.FullClassName).Nullable();
            Map(x => x.FullClassPath).Nullable();
            Map(x => x.SelectOrder).Nullable();
            Map(x => x.Type).Nullable();
            Map(x => x.DefiningType).Nullable();
            HasMany(y => y.AggregateFilters).Component(com =>
            {
                com.Map(z => z.AggregateFunction).Nullable();
                com.Map(z => z.FilterOperator).Nullable();
                com.Map(z => z.PropertyName).Nullable();
                com.Map(z => z.SubPropertyName).Nullable();
                com.Map(z => z.StringValue).Nullable();
            });
            HasMany(y => y.AggregateOperations).Component(com =>
            {
                com.Map(z => z.AggregateFunction).Nullable();
                com.Map(z => z.PropertyName).Nullable();
                com.Map(z => z.SubPropertyName).Nullable();
                com.Map(z => z.DisplayName).Nullable();
            });
            HasMany(y => y.Leaves).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            References(x => x.Parent).Column("Parent_Id");
            References(x => x.Report);
            HasMany(y => y.Nodes).KeyColumn("Parent_Id").Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}
