using FluentNHibernate.Mapping;
using HRIS.Domain.Training.Entities;

namespace HRIS.Mapping.Training.Entities
{
    public class CourseTrainingNeedMap:ClassMap<CourseTrainingNeed>
    {
        public CourseTrainingNeedMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.TrainingNeed);
            References(x => x.Course);
        }
    }
}
