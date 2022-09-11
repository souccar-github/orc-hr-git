using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Recruitment.Entities;
using HRIS.Validation.MessageKeys;
using Souccar.Infrastructure.Extenstions;
using SpecExpress;

namespace HRIS.Validation.Specification.Recruitment.Entities
{
    public class WorkingExperienceSpecification : Validates<WorkingExperience>
    {
        public WorkingExperienceSpecification()
        {
            IsDefaultForType();

            Check(x => x.CompanyName).Required().MaxLength(255);
            Check(x => x.ReferenceEmail).Optional().MaxLength(GlobalConstant.SimpleStringMaxLength).And.Matches(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$").With(x => x.Message = CustomMessageKeysRecruitmentModule.GetFullKey(CustomMessageKeysRecruitmentModule.EmailNotValid));
            Check(x => x.StartDate).Optional().LessThan(x => x.EndDate)
                .With(x => x.MessageKey = CustomMessageKeysRecruitmentModule.GetFullKey(CustomMessageKeysRecruitmentModule.StartWorkingDateMustBeLessThanEndWorkingDate));

            Check(x => x.EndDate).Optional().LessThan(DateTime.Now.Date)
                .With(x => x.MessageKey = CustomMessageKeysRecruitmentModule.GetFullKey(CustomMessageKeysRecruitmentModule.TheValueMustBeLessThanCurrentDate));

            Check(x => x.JobTitle)
                .Required()
                .Expect((workingExperience, jobTitle) => jobTitle.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

        }
    }
}