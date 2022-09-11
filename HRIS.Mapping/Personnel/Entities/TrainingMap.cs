#region

using FluentNHibernate.Mapping;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.Personnel.Entities
{
    public sealed class TrainingMap : ClassMap<Domain.Personnel.Entities.Training>
    {
        public TrainingMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            //CourseDetails
            References(x => x.Specialize);
            Map(x => x.CourseName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.CourseDurationPerHour);
            Map(x => x.CourseStartDate);
            Map(x => x.CourseEndDate);

            //TrainingCenterDetails
            References(x => x.TrainingCenter);
            Map(x => x.TrainingCenterLocation).Length(GlobalConstant.MultiLinesStringMaxLength);

            Map(x => x.instructor).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.instructorPhone).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.CertificateIssuanceDate);
            References(x => x.Status);
            Map(x => x.Notes).Length(GlobalConstant.MultiLinesStringMaxLength);
            
            References(x => x.Employee); 
            HasMany(x => x.Attachments).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}