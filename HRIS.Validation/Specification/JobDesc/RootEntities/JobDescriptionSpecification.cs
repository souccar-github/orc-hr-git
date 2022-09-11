
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.JobDescription.RootEntities
{
    public class JobDescriptionSpecification : Validates<HRIS.Domain.JobDescription.RootEntities.JobDescription>
    {
        public JobDescriptionSpecification()
        {
            IsDefaultForType();

            #region Primitive Types
            Check(x => x.Name).Required().MaxLength(GlobalConstant.SimpleStringMaxLength);
            Check(x => x.Summary).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            #endregion Primitive Types
            
            #region Ref
            Check(x => x.Node).Required().Expect((obj, property) => property.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            
            #endregion Indexes
            
            #region Indexes
            Check(x => x.JobTitle).Required().Expect((obj, property) => property.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
            #endregion Indexes
        }
    }
}
