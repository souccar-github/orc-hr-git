using HRIS.Domain.Training.RootEntities;
using Souccar.Domain.DomainModel;

namespace HRIS.Domain.Training.Entities
{
    public class CourseTrainingNeed: Entity
    {
        public virtual Course Course { get; set; }
        public virtual TrainingNeed TrainingNeed { get; set; }
    }
}
