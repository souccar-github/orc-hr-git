using HRIS.Domain.Recruitment.RootEntities;

namespace HRIS.Domain.Recruitment.Entities
{
    public class JobApplicationAttachment : AttachmentBase
    {
        public virtual JobApplication JobApplication { get; set; }
    }
}
