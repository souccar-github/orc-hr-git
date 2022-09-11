using HRIS.Domain.Training.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Training.RootEntities
{
    public class TrainingPlanSpecification: Validates<TrainingPlan>
    {
        public TrainingPlanSpecification()
        {
            IsDefaultForType();

            Check(x => x.PlanName).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.StartDate).Required().LessThan(x => x.EndDate)
                .With(x => x.MessageKey =
                    CustomMessageKeysTrainingModule.GetFullKey(CustomMessageKeysTrainingModule
                        .StartDateMustBeLessThanEndDate));

            Check(x => x.EndDate).Required();
            //Check(x => x.Quarter).Required();
        }
    }
}
