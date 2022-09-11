using FluentNHibernate.Mapping;
using HRIS.Domain.Training.Entities;

namespace HRIS.Mapping.Training.Entities
{

    public sealed class CourseEmployeeMap : ClassMap<CourseEmployee>
    {
        public CourseEmployeeMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.Employee);
            References(x => x.Course);
            Map(x => x.Type);

        }
    }

}
