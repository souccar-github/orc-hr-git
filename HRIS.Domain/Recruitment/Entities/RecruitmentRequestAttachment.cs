using HRIS.Domain.Recruitment.RootEntities;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Recruitment.Entities
{
    public class RecruitmentRequestAttachment: AttachmentBase
    {
        public virtual RecruitmentRequest RecruitmentRequest { get; set; }
    }
}
