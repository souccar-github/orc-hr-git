using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.JobDescription.Entities;
using HRIS.Domain.Recruitment.RootEntities;
using Souccar.Domain.DomainModel;
using Souccar.Domain.Security;
using Souccar.Domain.Workflow.Entities;

namespace HRIS.Domain.Recruitment.Entities.Evaluations
{
    public class Evaluator : Entity, IAggregateRoot
    {
        public Evaluator()
        {
            InterviewCustomSections = new List<InterviewCustomSection>();
        }

        public virtual DateTime Date { get; set; }
        public virtual User User { get; set; }
        public virtual float Mark { get; set; }

        public virtual IList<InterviewCustomSection> InterviewCustomSections { get; set; }

        public virtual void AddInterviewCustomSection(InterviewCustomSection interviewCustomSection)
        {
            InterviewCustomSections.Add(interviewCustomSection);
            interviewCustomSection.Evaluator = this;
        }

        public virtual float EvaluationValue()
        {
            float value = 0;
            foreach (var custom in InterviewCustomSections)
            {
                if (custom.InterviewCustomSectionItems != null)
                    value += custom.InterviewCustomSectionItems.Sum(x => x.Weight * x.Rate / 100) * custom.Weight;
            }
            value /= 100;

            return value;
        }

        public virtual Interview Interview { get; set; }
    }
}