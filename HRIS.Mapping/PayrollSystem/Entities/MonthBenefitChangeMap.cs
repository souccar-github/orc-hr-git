using FluentNHibernate.Mapping;
using HRIS.Domain.PayrollSystem.Entities;

namespace HRIS.Mapping.PayrollSystem.Entities
{
    public class MonthBenefitChangeMap : ClassMap<MonthBenefitChange>
    {
        public MonthBenefitChangeMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion


            Map(x => x.ConflictOption);
            Map(x => x.Value);
            Map(x => x.Formula);
            Map(x => x.ExtraValue);
            Map(x => x.ExtraValueFormula);
            Map(x => x.CeilValue);
            Map(x => x.CeilFormula);
            Map(x => x.Note);
            //Map(x => x.AuditState);

            References(x => x.Month);
            References(x => x.BenefitCard);

            HasMany(x => x.MonthlyEmployeeBenefits).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}