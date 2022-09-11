using HRIS.Domain.Training.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Training.Entities
{
    public class CourseSpecification:Validates<Course>
    {
        public CourseSpecification()
        {
            IsDefaultForType();

            Check(x => x.CourseTitle).Required();
            Check(x => x.CourseObjective).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Duration).Required().GreaterThan(0);
            Check(x => x.ExpectedNumberOfEmployees).Required().GreaterThan(0);
            Check(x => x.NumberOfSession).Required().GreaterThan(0);
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.PlannedStartDate).Required();
            Check(x => x.PlannedEndDate).Required().GreaterThan(x => x.PlannedStartDate).With(x => x.MessageKey =
                CustomMessageKeysTrainingModule.GetFullKey(CustomMessageKeysTrainingModule
                    .PlannedEndDateMustBeGreaterThanPlannedStartDate));

            Check(x => x.CourseName)
                  .Required()
                  .Expect((course, courseName) => courseName.IsTransient() == false, "")
                  .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Specialize)
                  .Required()
                  .Expect((course, specialize) => specialize.IsTransient() == false, "")
                  .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.CourseType)
                  .Required()
                  .Expect((course, type) => type.IsTransient() == false, "")
                  .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Priority)
                  .Required()
                  .Expect((course, priority) => priority.IsTransient() == false, "")
                  .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.CourseLevel)
                  .Required()
                  .Expect((course, level) => level.IsTransient() == false, "")
                  .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

        }
    }
}
