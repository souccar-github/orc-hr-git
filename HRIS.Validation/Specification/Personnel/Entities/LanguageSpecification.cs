using HRIS.Domain.Personnel.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Personnel.Entities
{
    public class LanguageSpecification : Validates<Language>
    {
        public LanguageSpecification()
        {
            IsDefaultForType();

            #region Primitive Types


            #endregion

            #region Indexes

            Check(x => x.LanguageName)
                .Required()
                .Expect((language, writing) => writing.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Writing)
                .Required()
                .Expect((language, writing) => writing.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Reading)
                .Required()
                .Expect((language, reading) => reading.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Speaking)
                .Required()
                .Expect((language, speaking) => speaking.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Listening)
                .Required()
                .Expect((language, listening) => listening.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));


            #endregion
        }
    }
}
