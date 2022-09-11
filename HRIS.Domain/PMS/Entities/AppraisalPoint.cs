using HRIS.Domain.Personnel.RootEntities;
using HRIS.Domain.PMS.Enums;
using HRIS.Domain.PMS.RootEntities;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.PMS.Entities
{
    public class AppraisalPoint: Entity,IAggregateRoot
    {
        public virtual Employee Appraiser { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual AppraisalPhase Phase { get; set; }
        public virtual string Note { get; set; }
        public virtual string Name { get; set; }
        public virtual bool TrainingNeed { get; set; }
        public virtual AppraisalPointType Type { get; set; }
    }
}
