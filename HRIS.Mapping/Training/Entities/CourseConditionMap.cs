
using FluentNHibernate.Mapping;
using HRIS.Domain.Training.Entities;
using Souccar.Core;

namespace HRIS.Mapping.Training.Entities
{
    public sealed class CourseConditionMap : ClassMap<CourseCondition>
    {
        public CourseConditionMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Level);
            Map(x => x.Description).Length(GlobalConstant.SimpleStringMaxLength);

            References(x => x.Condition);
            References(x => x.Course);
        }
    }

}
