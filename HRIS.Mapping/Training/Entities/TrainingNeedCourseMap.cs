using FluentNHibernate.Mapping;
using HRIS.Domain.Training.Entities;

namespace HRIS.Mapping.Training.Entities
{

    public sealed class TrainingNeedCourseMap : ClassMap<TrainingNeedCourse>
    {
        public TrainingNeedCourseMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.AppraisalPhase);
            References(x => x.Employee);
            References(x => x.TrainingNeed);

        }
    }
}