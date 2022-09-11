using System.Linq;
using HRIS.Domain.Training.Enums;
using HRIS.Domain.Training.Indexes;
using Souccar.Core.CustomAttribute;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.Training.Entities
{
    public class CourseCost : Entity
    {
        [UserInterfaceParameter(Order = 1)]
        public virtual CostName Name { get; set; }

        [UserInterfaceParameter(Order = 2)]
        public virtual double Cost { get; set; }

        //مركز الكلفة
        [UserInterfaceParameter(Order = 4)]
        public virtual CostCenter CostCenter { get; set; }

        [UserInterfaceParameter(Order = 5)]
        public virtual string Description { get; set; }

        [UserInterfaceParameter(IsNonEditable = true)]
        public virtual double CostPerTrainee
        {
            get
            {
                var numberOfTrainees = Course.CourseEmployees?.Count(x => x.Type == CourseEmployeeType.Trainee) ?? 0;
                if (numberOfTrainees > 0)
                {
                    return Course != null ? System.Math.Round((Cost / numberOfTrainees), 2) : 0;
                }
                else
                {
                    return 0;
                }
                
            }
        }

        [UserInterfaceParameter(Order = 200, IsHidden = true)]
        public virtual string NameForDropdown
        {
            get
            {
                return Name.Name;
            }
        }

        public virtual Course Course { get; set; }
    }
}
