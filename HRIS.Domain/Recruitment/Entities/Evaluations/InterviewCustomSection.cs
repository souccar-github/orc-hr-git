using HRIS.Domain.PMS.RootEntities;
using Souccar.Domain.DomainModel;
using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.Recruitment.RootEntities;

namespace HRIS.Domain.Recruitment.Entities.Evaluations
{
    public class InterviewCustomSection : Entity,IAggregateRoot
    {
        public InterviewCustomSection()
        {
            InterviewCustomSectionItems = new List<InterviewCustomSectionItem>();
        }
        public virtual float Weight { get; set; }
        public virtual float Rate { get; set; }
        public virtual string Description { get; set; }
        public virtual void UpdateValue()
        {
            Rate = InterviewCustomSectionItems.Sum(x => x.Weight * x.Rate / 100);
        }
        public virtual IList<InterviewCustomSectionItem> InterviewCustomSectionItems { get; set; }
        public virtual void AddInterviewCustomSectionItem(InterviewCustomSectionItem interviewCustomSectionItem)
        {
            interviewCustomSectionItem.InterviewCustomSection = this;
            InterviewCustomSectionItems.Add(interviewCustomSectionItem);
        }

        public virtual Evaluator Evaluator { get; set; }
        public virtual AppraisalSection AppraisalSection { get; set; }
    }
}
