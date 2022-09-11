using HRIS.Domain.JobDescription.Entities;
using HRIS.Validation.MessageKeys;
using HRIS.Validation.Specification.Index;
using SpecExpress;

namespace HRIS.Validation.Specification.JobDescription.Entities
{
    public class CompetenceSpecification : Validates<Competence>
    {
        public CompetenceSpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            Check(x => x.Weight).Required().Between(GlobalConstant.MinimumPercentageValue, GlobalConstant.MaximumPercentageValue);
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
            #endregion Primitive Types

            #region Indexes
            Check(x => x.Type)
                .Required()
                .Expect((x, y) => !y.IsTransient(), "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.CompetenceCategory)
                .Required()
                .Expect((x, y) => !y.IsTransient(), "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Level)
                .Required()
                .Expect((x,y)=>!y.IsTransient(), "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
           
            #endregion Indexes

        }
    }
}




