
using HRIS.Domain.Objectives.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Objectives.RootEntities
{
    public class StrategicObjectiveSpecification:Validates<StrategicObjective>
    {
        public  StrategicObjectiveSpecification()
        {
            IsDefaultForType();

            #region Primitive types

            Check(x => x.Name).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Code).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);

            Check(x => x.MeetExpectation).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.DoesNotMeetExpectation).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.AboveExpectation).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            
            Check(x => x.Period).Required();
            Check(x => x.StartDate).Required().LessThanEqualTo(x => x.EndDate);
            Check(x => x.EndDate).Required();

            #endregion

            #region Indexs

            Check(x => x.Dimension)
                .Required()
                .Expect((dimension, type) => type.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion

        }
    }
}
