using FluentNHibernate.Mapping;
using HRIS.Domain.Training.Entities;
using Souccar.Core;

namespace HRIS.Mapping.Training.Entities
{
    
    public sealed class AppraisalCourseMap : ClassMap<AppraisalCourse>
    {
        public AppraisalCourseMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.NumberOfTrainees);

            References(x => x.AppraisalKpi);
            References(x => x.AppraisalLevel);
            References(x => x.Course);
        }
    }
}