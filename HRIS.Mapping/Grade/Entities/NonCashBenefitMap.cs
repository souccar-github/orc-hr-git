#region

using FluentNHibernate.Mapping;
using HRIS.Domain.Grades.Entities;

#endregion

namespace HRIS.Mapping.Grade.Entities
{
    public sealed class NonCashBenefitMap : ClassMap<NonCashBenefit>
    {
        public NonCashBenefitMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Description);

            References(x => x.Type);

            References(x => x.Grade);
        }
    }
}