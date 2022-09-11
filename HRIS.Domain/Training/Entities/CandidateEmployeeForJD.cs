using HRIS.Domain.JobDesc.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Training.Entities
{
    public class CandidateEmployeeForJD : Entity, IAggregateRoot
    {
        public CandidateEmployeeForJD()
        {
            CreateDate = DateTime.Today;
            CandidateEmployeeForJDItems = new List<CandidateEmployeeForJDItem>();
        }
        [UserInterfaceParameter(Order = 1)]
        public virtual string Name { get; set; }
        [UserInterfaceParameter(Order = 2)]
        public virtual DateTime CreateDate { get; set; }
        [UserInterfaceParameter(Order = 3)]
        public virtual JobDescription JobDescription { get; set; }
        public virtual IList<CandidateEmployeeForJDItem> CandidateEmployeeForJDItems { get; set; }
        public virtual void AddCandidateEmployeeForJDItem(CandidateEmployeeForJDItem candidateEmployeeForJDItem)
        {
            candidateEmployeeForJDItem.CandidateEmployeeForJD = this;
            CandidateEmployeeForJDItems.Add(candidateEmployeeForJDItem);
        }
    }
}
