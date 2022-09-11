using HRIS.Domain.Grades.Entities;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.Entities
{
    //*******************repeated************************
    //public class JobTitleBenefitDetailSpecification : Validates<JobTitleBenefitDetail>
    //{
    //    public JobTitleBenefitDetailSpecification()
    //    {
    //        IsDefaultForType();

    //        Check(x => x.Value, y => typeof(JobTitleBenefitDetail).GetProperty("Value").GetTitle()).Optional().GreaterThanEqualTo(0);
    //        Check(x => x.CeilValue, y => typeof(JobTitleBenefitDetail).GetProperty("CeilValue").GetTitle()).Optional().GreaterThanEqualTo(0);

    //        Check(x => x.Note).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
    //        Check(x => x.CeilFormula, y => typeof(JobTitleBenefitDetail).GetProperty("CeilFormula").GetTitle()).Required();
    //        Check(x => x.ExtraValueFormula, y => typeof(JobTitleBenefitDetail).GetProperty("ExtraValueFormula").GetTitle()).Required();
    //        Check(x => x.Formula, y => typeof(JobTitleBenefitDetail).GetProperty("Formula").GetTitle()).Required();


    //        Check(x => x.BenefitCard, y => typeof(JobTitleBenefitDetail).GetProperty("BenefitCard").GetTitle())
    //            .Required()
    //            .Expect((primaryEmployeeBenefit, benefitCard) => benefitCard.IsTransient() == false, "")
    //            .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
    //    }
    //}

}
