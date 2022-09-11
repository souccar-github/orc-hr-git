using HRIS.Domain.Recruitment.Entities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.Training.Entities
{
    [Module("Training")]
    public class AppraisalTraineeAttachment: AttachmentBase 
    {
        public virtual AppraisalTrainee AppraisalTrainee { get; set; }

    }
}
