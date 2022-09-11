using FluentNHibernate.Mapping;
using HRIS.Domain.PayrollSystem.RootEntities;

namespace HRIS.Mapping.PayrollSystem.RootEntities
{
    public class FamilyBenefitOptionMap : ClassMap<FamilyBenefitOption>
    {
        public FamilyBenefitOptionMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion


            Map(x => x.SpousePay);
            Map(x => x.FirstChildPay);
            Map(x => x.SecondChildPay);
            Map(x => x.ThirdChildPay);
            Map(x => x.UpperThreeChildPay);
            Map(x => x.UpperThreeChildPayConditionalYear);
            //Map(x => x.AuditState);
            //Map(x => x.Status);
        }
    }
}