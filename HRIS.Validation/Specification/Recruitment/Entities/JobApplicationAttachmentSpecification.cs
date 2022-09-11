using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HRIS.Domain.Recruitment.Entities;
using HRIS.Validation.MessageKeys;
using SpecExpress;

namespace HRIS.Validation.Specification.Recruitment.Entities
{
    public class JobApplicationAttachmentSpecification : Validates<JobApplicationAttachment>
    {
        public JobApplicationAttachmentSpecification()
        {
            IsDefaultForType();

            Check(x => x.AttachmentType)
                .Required()
                .Expect((recruitmentRequestAttachment, attachmentType) => attachmentType.IsTransient() == false, "")
                .With(x => x.MessageKey = PreDefinedMessageKeysSpecExpress.GetFullKey(PreDefinedMessageKeysSpecExpress.Required));

            Check(x => x.Attachment).Required();
            Check(x => x.Description).Optional().MaxLength(GlobalConstant.MultiLinesStringMaxLength);
        }
    }
}
