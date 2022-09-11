using FluentNHibernate.Mapping;
using HRIS.Domain.Grades.Entities;
using HRIS.Domain.PayrollSystem.Entities;

namespace HRIS.Mapping.PayrollSystem.Entities
{
    public class GradeBenefitDetailMap : ClassMap<GradeBenefitDetail>
    {
        public GradeBenefitDetailMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion


            Map(x => x.Value);
            Map(x => x.Formula);
            Map(x => x.ExtraValue);
            Map(x => x.ExtraValueFormula);
            Map(x => x.CeilValue);
            Map(x => x.CeilFormula);
            Map(x => x.Note);
            //Map(x => x.AuditState);

            References(x => x.BenefitCard);
            References(x => x.Grade);
            //References(x => x.GradeBenefitMaster);
        }
    }
}
