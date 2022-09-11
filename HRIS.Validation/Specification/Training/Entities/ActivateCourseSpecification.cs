using HRIS.Domain.Training.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Training.Entities
{
    public class ActivateCourseSpecification : Validates<Course>
    {
        public ActivateCourseSpecification()
        {
            //IsDefaultForType();

            Check(x => x.CourseTitle).Required();
            Check(x => x.Duration).Required().GreaterThan(0);
            Check(x => x.NumberOfSession).Required().GreaterThan(0);
            Check(x => x.StartDate).Required();
            Check(x => x.EndDate).Required().GreaterThan(x => x.StartDate).With(x => x.MessageKey =
                CustomMessageKeysTrainingModule.GetFullKey(CustomMessageKeysTrainingModule
                    .EndDateMustBeGreaterThanStartDate));

            Check(x => x.TrainingCenterName)
                  .Required()
                  .Expect((course, trainingCenterName) => trainingCenterName.IsTransient() == false, "")
                  .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.TrainingPlace)
                  .Required()
                  .Expect((course, trainingPlace) => trainingPlace.IsTransient() == false, "")
                  .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Sponsor)
                  .Required()
                  .Expect((course, sponsor) => sponsor.IsTransient() == false, "")
                  .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Trainer)
                  .Required()
                  .Expect((course, trainer) => trainer.IsTransient() == false, "")
                  .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

        }
    }
}
