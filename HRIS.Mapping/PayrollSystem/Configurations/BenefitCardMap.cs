using FluentNHibernate.Mapping;
using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.PayrollSystem.RootEntities;

namespace HRIS.Mapping.PayrollSystem.Configurations
{
    public class BenefitCardMap : ClassMap<BenefitCard>
    {
        public BenefitCardMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion


            Map(x => x.Name);
            Map(x => x.Value);
            Map(x => x.Formula);
            Map(x => x.ExtraValue);
            Map(x => x.ExtraValueFormula);
            Map(x => x.CeilValue);
            Map(x => x.CeilFormula);
            Map(x => x.FinalRound);
            Map(x => x.InitialRound);
            Map(x => x.EffectableByPartialWorkDays);
            Map(x => x.TaxValue);
            Map(x => x.TaxFormula);
            //Map(x => x.AuditState);

            References(x => x.ParentBenefitCard);

            HasMany(x => x.CrossDeductions).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}