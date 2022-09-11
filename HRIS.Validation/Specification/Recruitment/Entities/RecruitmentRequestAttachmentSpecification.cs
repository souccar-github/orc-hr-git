using HRIS.Domain.Recruitment.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Recruitment.Entities
{
    public class RecruitmentRequestAttachmentSpecification : Validates<RecruitmentRequestAttachment>
    {
        public RecruitmentRequestAttachmentSpecification()
        {
            IsDefaultForType();

            Check(x=>x.AttachmentType)
               .Required()
               .Expect((recruitmentRequestAttachment, attachmentType) => attachmentType.IsTransient() == false, "")
               .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x=>x.Attachment).Required();

            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
        }
    }
}
