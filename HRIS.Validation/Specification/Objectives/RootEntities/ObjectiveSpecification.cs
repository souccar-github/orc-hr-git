
using HRIS.Domain.Objectives.Entities;
using HRIS.Domain.Objectives.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Objectives.Entities
{
    public class ObjectiveSpecification:Validates<Objective>
    {
        public ObjectiveSpecification()
        {
            IsDefaultForType();

            #region Primitive types

            Check(x => x.Name).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Code).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.Type).Required();
            Check(x => x.CreationDate).Required();

            Check(x => x.Owner).Required().Expect((obj, property) => property.IsTransient() == false, "")
               .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.JopDescription).Required().Expect((obj, property) => property.IsTransient() == false, "")
               .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            //Check(x => x.Creator).Required().Expect((obj, property) => property.IsTransient() == false, "")
            //   .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Weight).Required().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);
            Check(x => x.Priority).Required();
            Check(x => x.PlannedClosingDate).Required();
            Check(x => x.PlannedStartingDate).Required().LessThan(x => x.PlannedClosingDate);
            Check(x => x.Department).Required();
            Check(x => x.MeetExpectation).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.DoesNotMeetExpectation).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.AboveExpectation).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            
            #endregion

            #region Indexs



            #endregion
        }
    }
}
