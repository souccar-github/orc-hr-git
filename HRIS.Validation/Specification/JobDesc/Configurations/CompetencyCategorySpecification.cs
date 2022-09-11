using HRIS.Domain.JobDescription.Configurations;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.JobDescription.RootEntities;
using HRIS.Validation.MessageKeys;
using HRIS.Validation.Specification.Index;
using SpecExpress;

namespace HRIS.Validation.Specification.JobDescription.Configurations
{
    public class CompetenceCategorySpecification : Validates<CompetenceCategory>
    {
        public CompetenceCategorySpecification()
        {
            IsDefaultForType();

            #region Primitive Types

            #endregion Primitive Types

            #region Indexes

            Check(x => x.Type)
                .Required()
                .Expect(IndexSpecification.IsTransient, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Name)
               .Required()
               .Expect(IndexSpecification.IsTransient, "")
               .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion Indexes

        }
    }
}




