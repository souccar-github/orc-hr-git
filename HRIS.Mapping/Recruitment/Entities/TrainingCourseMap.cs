using HRIS.Domain.Recruitment.Entities;
using FluentNHibernate.Mapping;
using Souccar.Core;

namespace HRIS.Mapping.Recruitment.Entities
{
    public class TrainingCourseMap : ClassMap<TrainingCourse>
    {
        public TrainingCourseMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x=>x.CourseName);
            Map(x=>x.CourseDuration);
            Map(x=>x.TrainingCenter);
            Map(x=>x.TrainingLocation);
            Map(x=>x.AttendanceCertificateIssuanceDate);
            Map(x=>x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);
           
            References(x=>x.CompetencyName);
            References(x=>x.CompetencyLevel);
            References(x=>x.Status).Nullable();
            References(x=>x.JobApplication);
        }
        
        
    }
}
