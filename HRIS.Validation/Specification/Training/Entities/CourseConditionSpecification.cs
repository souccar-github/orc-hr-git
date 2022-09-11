using HRIS.Domain.Training.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Training.Entities
{
    public class CourseConditionSpecification:Validates<CourseCondition>
    {
        public CourseConditionSpecification()
        {
            IsDefaultForType();

            Check(x => x.Level).Required().GreaterThan(0);
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            Check(x => x.Condition)
                  .Required()
                  .Expect((courseCondition, condition) => condition.IsTransient() == false, "")
                  .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }
}
