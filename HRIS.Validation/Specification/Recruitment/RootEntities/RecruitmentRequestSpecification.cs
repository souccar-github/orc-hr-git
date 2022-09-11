using HRIS.Domain.Recruitment.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Validation.Specification.Recruitment.RootEntities
{
    public class RecruitmentRequestSpecification : Validates<RecruitmentRequest>
    {
        public RecruitmentRequestSpecification()
        {
            IsDefaultForType();

            #region Request Information

            Check(x => x.RequestDate).Required()
                .LessThanEqualTo(DateTime.Now).With(x =>
                    x.MessageKey =
                        CustomMessageKeysRecruitmentModule.GetFullKey(CustomMessageKeysRecruitmentModule
                            .TheValueMustBeLessThanOrEqualCurrentDate));

            Check(x => x.PositionBudget).Required();
            Check(x => x.SalaryRange).Required();
            Check(x => x.ExpectedHiringDate).Required();
            Check(x => x.DurationToFillPosition).Optional().MaxLength(255);
            
            Check(x => x.RequestType).Required()
                   .Expect((recruitmentRequest, requestType) => requestType.IsTransient() == false, "")
                   .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required)); 

            Check(x => x.VacancyReason).Required()
                .Expect((recruitmentRequest,vacancyReason) => vacancyReason.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.JobType).Required()
                .Expect((recruitmentRequest, jobType) => jobType.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            #endregion

            #region Job Description Info

            Check(x => x.RequestedPosition).Required()
                .Expect((recruitmentRequest, requestedPosition) => requestedPosition.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required)); ;

            #endregion

        }
    }
}
