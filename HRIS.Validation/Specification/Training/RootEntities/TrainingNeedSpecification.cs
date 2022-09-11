using HRIS.Domain.Training.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Training.RootEntities
{
    public class TrainingNeedSpecification : Validates<TrainingNeed>
    {
        public TrainingNeedSpecification()
        {
            IsDefaultForType();
            Check(x => x.Name).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            Check(x => x.Level)
                  .Required()
                  .Expect((trainingNeed, level) => level.IsTransient() == false, "")
                  .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

        }
    }
}
