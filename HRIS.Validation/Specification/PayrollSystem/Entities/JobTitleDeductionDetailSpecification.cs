
using HRIS.Domain.Grades.Entities;
using HRIS.Domain.PayrollSystem.Entities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.PayrollSystem.Entities
{
    public class JobTitleDeductionDetailSpecification : Validates<JobTitleDeductionDetail>
    {
        public JobTitleDeductionDetailSpecification()
        {
            IsDefaultForType();

            Check(x => x.Value, y => typeof(JobTitleDeductionDetail).GetProperty("Value").GetTitle()).Optional().GreaterThanEqualTo(0);
            Check(x => x.ExtraValueFormula, y => typeof(JobTitleDeductionDetail).GetProperty("ExtraValueFormula").GetTitle()).Required();
            Check(x => x.Formula, y => typeof(JobTitleDeductionDetail).GetProperty("Formula").GetTitle()).Required();

            Check(x => x.DeductionCard, y => typeof(JobTitleDeductionDetail).GetProperty("DeductionCard").GetTitle())
                .Required()
                .Expect((primaryEmployeeDeduction, deductionCard) => deductionCard.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }

}
