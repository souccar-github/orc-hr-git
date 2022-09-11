using HRIS.Domain.Recruitment.RootEntities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Recruitment.RootEntities
{
    public class InterviewSpecification : Validates<Interview>
    {
        public InterviewSpecification()
        {
            IsDefaultForType();

            Check(x => x.InterviewDate).Required();
            Check(x => x.InterviewStartingTime).Required();
            Check(x => x.InterviewEndTime).Required();
            //Check(x => x.SubTopic).Required().MaxLength(255);
            Check(x => x.InterviewGuidelines).Required().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            Check(x=>x.InterviewType)
                .Required()
                .Expect((interview, interviewType) => interviewType.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.InterviewAppraisalSetting)
                .Required()
                .Expect((interview, interviewAppraisalSetting) => interviewAppraisalSetting.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.JobApplication)
                .Required()
                .Expect((interview, jobApplication) => jobApplication.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.InterviewAppraisalTemplate)
                .Required()
                .Expect((interview, interviewAppraisalTemplate) => interviewAppraisalTemplate.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }
}
