using HRIS.Validation.MessageKeys;
using SpecExpress;
using HRIS.Domain.PMS.Configurations;

namespace HRIS.Validation.Specification.PMS.Configurations
{

    public class AppraisalTemplateSettingSpecification : Validates<AppraisalTemplateSetting>
    {
        public AppraisalTemplateSettingSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.Name).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.CreationDate).Required();
            
            #endregion Primitive Types
            
            #region Indexes

            Check(x => x.DefaultTemplate)
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
