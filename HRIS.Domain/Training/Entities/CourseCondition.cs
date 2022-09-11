using HRIS.Domain.Training.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.Training.Entities
{
    public class CourseCondition : Entity
    {
        [UserInterfaceParameter(Order = 1)]
        public virtual ConditionTitle Condition { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual int Level { get; set; }

        [UserInterfaceParameter(Order = 3)]
        public virtual string Description { get; set; }

        public virtual Course Course { get; set; }
       
    }
}
