using FluentNHibernate.Mapping;
using HRIS.Domain.PayrollSystem.Entities;

namespace HRIS.Mapping.PayrollSystem.Entities
{//todo : Mhd Update changeset no.2
    public class CrossDeductionWithBenefitMap : ClassMap<CrossDeductionWithBenefit>
    {
        public CrossDeductionWithBenefitMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion


            Map(x => x.CrossType);
            Map(x => x.Value);
            Map(x => x.CrossFormula);

            References(x => x.BenefitCard);
            References(x => x.DeductionCard);

            HasMany(x => x.CrossDependencys).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}