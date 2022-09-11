using FluentNHibernate.Mapping;
using HRIS.Domain.PayrollSystem.Entities;

namespace HRIS.Mapping.PayrollSystem.Entities
{
    public class CrossDeductionWithDeductionMap : ClassMap<CrossDeductionWithDeduction>
    {
        public CrossDeductionWithDeductionMap()
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
            //Map(x => x.AuditState);

            References(x => x.ParentDeductionCard);
            References(x => x.DeductionCard);
        }
    }
}