using HRIS.Domain.Training.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Training.Entities
{
    public class AppraisalTraineeAttachmentSpecification: Validates<AppraisalTraineeAttachment>
    {
        public AppraisalTraineeAttachmentSpecification()
        {
            IsDefaultForType();

            Check(x => x.Attachment).Required();
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);

            Check(x => x.AttachmentType)
                  .Required()
                  .Expect((Attachment, type) => type.IsTransient() == false, "")
                  .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));
        }
    }
}
