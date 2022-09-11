using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.Recruitment.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Recruitment.Entities
{
    public class TrainingCourseSpecification : Validates<TrainingCourse>
    {
        public TrainingCourseSpecification()
        {
            IsDefaultForType();

            Check(x => x.CourseName).Required().MaxLength(255);
            Check(x => x.AttendanceCertificateIssuanceDate).Optional().LessThanEqualTo(DateTime.Now)
                .With(x=>x.MessageKey= CustomMessageKeysRecruitmentModule.GetFullKey(CustomMessageKeysRecruitmentModule.TheValueMustBeLessThanCurrentDate));

            Check(x => x.CompetencyName)
                .Required()
                .Expect((trainingCourse, competencyName) => competencyName.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.CompetencyLevel)
                .Required()
                .Expect((trainingCourse, competencyLevel) => competencyLevel.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }
}
