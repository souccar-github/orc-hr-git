using FluentNHibernate.Mapping;
using HRIS.Domain.Training.Entities;
using Souccar.Core;

namespace HRIS.Mapping.Training.Entities
{

    public sealed class CourseMap : ClassMap<Course>
    {
        public CourseMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.CourseObjective).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.CourseTitle).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.CancellationDescription).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.Duration);
            Map(x => x.ExpectedNumberOfEmployees);
            Map(x => x.NumberOfSession);
            Map(x => x.PlannedStartDate);
            Map(x => x.PlannedEndDate);
            Map(x => x.StartDate);
            Map(x => x.EndDate);
            Map(x => x.StartHour);
            Map(x => x.Status);
            
            Map(x => x.Saturday);
            Map(x => x.Sunday);
            Map(x => x.Monday);
            Map(x => x.Tuesday);
            Map(x => x.Wednesday);
            Map(x => x.Thursday);
            Map(x => x.Friday);

            References(x => x.TrainingPlan);
            References(x => x.CourseName);
            References(x => x.Priority);
            References(x => x.Specialize);
            References(x => x.CourseLevel);
            References(x => x.CourseType);
            References(x => x.LanguageName).Nullable();
            References(x => x.TrainingCenterName).Nullable();
            References(x => x.Sponsor).Nullable();
            References(x => x.Trainer).Nullable();
            References(x => x.TrainingPlace).Nullable();

            HasMany(x => x.AppraisalTrainees).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.Conditions).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.AppraisalCourses).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.CourseCosts).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.Attachments).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.CourseEmployees).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.CourseTrainingNeeds).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

        }
    }

}
