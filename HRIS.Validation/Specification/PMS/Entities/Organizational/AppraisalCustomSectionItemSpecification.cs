
using HRIS.Domain.PMS.Entities.Organizational;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.PMS.Entities.Organizational
{
    
    public class AppraisalCustomSectionItemSpecification : Validates<AppraisalCustomSectionItem>
    {
        public AppraisalCustomSectionItemSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Weight).Required().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);

            #endregion Primitive Types


            #region Indexes

            Check(x => x.Section)
                .Required()
                .Expect((obj, prop) => prop.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Item)
                .Required()
                .Expect((obj, prop) => prop.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion Indexes

        }
    }
}
