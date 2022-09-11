using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Recruitment.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Recruitment.Entities
{
    public class TrainingCourseLanguageSpecification: Validates<TrainingCourseLanguage>
    {
        public TrainingCourseLanguageSpecification()
        {
            IsDefaultForType();

            Check(x => x.LanguageName)
                .Required()
                .Expect((TrainingCourseLanguage, languageName) => languageName.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Reading)
                .Required()
                .Expect((TrainingCourseLanguage, readingLevel) => readingLevel.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Writing)
                .Required()
                .Expect((TrainingCourseLanguage, writingLevel) => writingLevel.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Listening)
                .Required()
                .Expect((TrainingCourseLanguage, listeningLevel) => listeningLevel.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Speaking)
                .Required()
                .Expect((TrainingCourseLanguage, speakingLevel) => speakingLevel.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }
}
