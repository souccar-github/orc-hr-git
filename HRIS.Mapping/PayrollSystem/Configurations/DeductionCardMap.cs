using FluentNHibernate.Mapping;
using HRIS.Domain.PayrollSystem.Configurations;
using HRIS.Domain.PayrollSystem.RootEntities;

namespace HRIS.Mapping.PayrollSystem.Configurations
{
    public class DeductionCardMap : ClassMap<DeductionCard>
    {
        public DeductionCardMap()
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
            Map(x => x.FinalRound);
            Map(x => x.InitialRound);
            Map(x => x.IsPrimaryDeduction);
            Map(x => x.BankAccountNumber);
            Map(x => x.EffectableByPartialWorkDays);
            //Map(x => x.AuditState);
            //Map(x => x.Status);

            References(x => x.ParentDeductionCard);

            HasMany(x => x.CrossDeductionWithDeductions).KeyColumn("ParentDeductionCard_id").Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}