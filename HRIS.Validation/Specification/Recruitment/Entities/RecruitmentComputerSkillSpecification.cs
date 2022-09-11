using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Recruitment.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Recruitment.Entities
{
    public class RecruitmentComputerSkillSpecification:Validates<RecruitmentComputerSkill>
    {
        public RecruitmentComputerSkillSpecification()
        {
            IsDefaultForType();

            Check(x => x.SkillType)
                .Required()
                .Expect((recruitmentComputerSkill, skillType) => skillType.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Level)
                .Required()
                .Expect((recruitmentComputerSkill, level) => level.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }
}
