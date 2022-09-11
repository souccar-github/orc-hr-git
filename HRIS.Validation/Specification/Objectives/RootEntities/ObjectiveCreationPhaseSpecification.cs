
using HRIS.Domain.Objectives.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Objectives.RootEntities
{
    public class ObjectiveCreationPhaseSpecification : Validates<ObjectiveCreationPhase>
    {
        public ObjectiveCreationPhaseSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.StartDate).Required();
            Check(x => x.EndDate).Required().GreaterThanEqualTo(x => x.StartDate);
            Check(x => x.Period).Required();
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            
            #endregion Primitive Types


            #region Indexes

            Check(x => x.WorkflowSetting)
                .Required()
                .Expect((obj, prop) => prop.IsTransient() == false, "")
                .With(
                    x =>
                        x.MessageKey =
                            PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion Indexes

        }
    }
}
