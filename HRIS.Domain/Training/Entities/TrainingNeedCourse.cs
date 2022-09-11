using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HRIS.Domain.PMS.RootEntities;
using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.Training.RootEntities;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.Training.Entities
{
    public class TrainingNeedCourse : Entity,IAggregateRoot
    {
        public virtual AppraisalPhase AppraisalPhase { get; set; }
        public virtual TrainingNeed TrainingNeed { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
