using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Recruitment.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Recruitment.Entities
{
    public class PersonalSkillSpecification:Validates<PersonalSkill>
    {
        public PersonalSkillSpecification()
        {
            IsDefaultForType();

            Check(x => x.SkillType)
                .Required()
                .Expect((personalSkill, skillType) => skillType.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Level)
                .Required()
                .Expect((personalSkill, skillLevel) => skillLevel.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }
}
