#region

using FluentNHibernate.Mapping;
using HRIS.Domain.Grades.Indexes;
using HRIS.Domain.OrganizationChart.Indexes;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.Grade.Indexes
{
    public sealed class NoneCashBenefitTypeMap : ClassMap<NoneCashBenefitType>
    {
        public NoneCashBenefitTypeMap()
        {
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength).Unique();
            Map(x => x.Order).Column("ValueOrder");
        }
    }
}