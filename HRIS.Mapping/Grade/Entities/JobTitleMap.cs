#region

using FluentNHibernate.Mapping;
using HRIS.Domain.Grades.Entities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.Grade.Entities
{
    public sealed class JobTitleMap : ClassMap<JobTitle>
    {
        public JobTitleMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.EmployeeCount);
            Map(x => x.Vacancies);
            Map(x => x.Order).Column("ValueOrder");
            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);
            References(x => x.Grade);
            HasMany(x => x.JobTitleBenefitDetails).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            HasMany(x => x.JobTitleDeductionDetails).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}