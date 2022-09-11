using HRIS.Domain.PayrollSystem.RootEntities;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.RootEntities
{
    public class FamilyBenefitOptionSpecification : Validates<FamilyBenefitOption>
    {
        public FamilyBenefitOptionSpecification()
        {
            IsDefaultForType();

            Check(x => x.SpousePay, y => typeof(FamilyBenefitOption).GetProperty("SpousePay").GetTitle()).Required().GreaterThan(0);
            Check(x => x.FirstChildPay, y => typeof(FamilyBenefitOption).GetProperty("FirstChildPay").GetTitle()).Required().GreaterThan(0);
            Check(x => x.SecondChildPay, y => typeof(FamilyBenefitOption).GetProperty("SecondChildPay").GetTitle()).Required().GreaterThan(0);
            Check(x => x.ThirdChildPay, y => typeof(FamilyBenefitOption).GetProperty("ThirdChildPay").GetTitle()).Required().GreaterThan(0);
            Check(x => x.UpperThreeChildPay, y => typeof(FamilyBenefitOption).GetProperty("UpperThreeChildPay").GetTitle()).Required().GreaterThan(0);
            Check(x => x.UpperThreeChildPayConditionalYear, y => typeof(FamilyBenefitOption).GetProperty("UpperThreeChildPayConditionalYear").GetTitle()).Required().GreaterThan(0).And.LessThanEqualTo(9999);
        }
    }
}
