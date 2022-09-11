#region

using FluentNHibernate.Mapping;
using Souccar.ReportGenerator.Domain.QueryBuilder;

#endregion
namespace HRIS.Mapping.ReportGenerator.QueryBuilder
{
    public sealed class QueryLeafMap : ClassMap<Souccar.ReportGenerator.Domain.QueryBuilder.QueryLeaf>
    {
        public QueryLeafMap()
        {
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            DynamicUpdate();
            DynamicInsert();
            Map(x => x.IsPrimitive).Nullable();
            Map(x => x.IsReference).Nullable();
            Map(x => x.Position).Nullable();
            Map(x => x.PropertyFullPath).Nullable();
            Map(x => x.PropertyName).Nullable();
            Map(x => x.DefiningType).Nullable();
            Map(x => x.ParentType).Nullable();
            Map(x => x.PropertyType).Nullable();
            Map(x => x.Selected).Nullable();
            HasMany(x => x.FilterDescriptors).Component(com =>
            {
                com.Map(y => y.FilterOperator);
                com.Map(y => y.StringValue);
            });
            Component(x => x.SortDescriptor, m =>
            {
                m.Map(y => y.SortDirection);
                m.Map(y => y.SortOrder);
            });
            Component(x => x.GroupDescriptor, m =>
            {
                m.Map(y => y.GroupByOrder);
            });
            References(x => x.QueryTree).Column("Parent_Id");
        }
    }

}
