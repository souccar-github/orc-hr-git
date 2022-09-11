using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.PMS.Entities.JobDescription;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.PMS.Entities.JobDescription
{
    public class AppraisalJobDescriptionSpecification : Validates<AppraisalJobDescription>
    {
        public AppraisalJobDescriptionSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            Check(x => x.Weight).Required().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);

            #endregion Primitive Types


            #region Indexes

            Check(x => x.Responsibility)
                .Required()
                .Expect((obj, prop) => prop.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion Indexes

        }
    }
}
