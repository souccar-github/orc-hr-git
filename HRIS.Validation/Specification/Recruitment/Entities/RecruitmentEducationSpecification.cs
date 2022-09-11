using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Recruitment.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Recruitment.Entities
{
    public class RecruitmentEducationSpecification : Validates<RecruitmentEducation>
    {
        public RecruitmentEducationSpecification()
        {
            IsDefaultForType();

            Check(x => x.Comments).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            Check(x => x.DateOfIssuance).Optional()
                .LessThan(DateTime.Now)
                .With(x=>x.MessageKey = CustomMessageKeysRecruitmentModule.GetFullKey(CustomMessageKeysRecruitmentModule.TheValueMustBeLessThanCurrentDate));

            Check(x => x.Type)
                .Required()
                .Expect((recruitmentEducation, type) => type.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Major)
                .Required()
                .Expect((recruitmentEducation, major) => major.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));


        }
    }
}
