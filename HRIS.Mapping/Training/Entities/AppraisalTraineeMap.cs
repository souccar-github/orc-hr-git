using FluentNHibernate.Mapping;
using HRIS.Domain.Training.Entities;
using Souccar.Core;

namespace HRIS.Mapping.Training.Entities
{
    public sealed class AppraisalTraineeMap : ClassMap<AppraisalTrainee>
    {
        public AppraisalTraineeMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.ExamDate);
            Map(x => x.Score);
            Map(x => x.AbsenceReason).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.TrainerNote).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.NumberOfHoursOfAbsence);
            Map(x => x.ResponsibilityOfEmployee);
            
            References(x => x.Employee);
            References(x => x.Course);
            References(x => x.Level);

            HasMany(x => x.Attachment).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }

}
