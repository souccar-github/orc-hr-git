using HRIS.Domain.Recruitment.Entities;
using HRIS.Domain.Training.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using HRIS.Domain.Recruitment.Indexes;

namespace HRIS.Domain.Training.Entities
{
    [Module("Training")]
    public class CourseAttachment :AttachmentBase 
    {
        public virtual Course Course { get; set; }

    }
}
