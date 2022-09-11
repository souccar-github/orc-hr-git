using HRIS.Domain.PMS.Entities;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.Recruitment.Entities.Evaluations
{
    public class InterviewCustomSectionItem : Entity,IAggregateRoot
    {
        public virtual AppraisalSectionItem AppraisalSectionItem { get; set; }
        public virtual float Weight { get; set; }
        public virtual float Rate { get; set; }
        public virtual bool CanNotEvaluate { get; set; }
        public virtual string Description { get; set; }

        public virtual InterviewCustomSection InterviewCustomSection { get; set; }
    }
}
