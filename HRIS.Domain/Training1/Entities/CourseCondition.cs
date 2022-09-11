using HRIS.Domain.Training1.RootEntities;
using HRIS.Domain.Training1.Indexes;
using Souccar.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Domain.Training1.Entities
{
    public class CourseCondition : Entity
    {
        public virtual ConditionTitle Title{ get; set; }
        public virtual int Level { get; set; }
        public virtual string Description { get; set; }

        public virtual Course Course { get; set; }
    }
}
