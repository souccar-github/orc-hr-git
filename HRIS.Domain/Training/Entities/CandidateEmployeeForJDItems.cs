using HRIS.Domain.Personnel.RootEntities;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Training.Entities
{
    public class CandidateEmployeeForJDItem : Entity
    {

        [UserInterfaceParameter(Order = 1)]
        public virtual Employee Employee { get; set; }
        [UserInterfaceParameter(Order = 2)]
        public virtual double Mark { get; set; }
        public virtual CandidateEmployeeForJD CandidateEmployeeForJD { get; set; }

    }
}
