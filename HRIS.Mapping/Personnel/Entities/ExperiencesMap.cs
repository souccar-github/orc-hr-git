#region

using FluentNHibernate.Mapping;
using HRIS.Domain.Personnel.Entities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.Personnel.Entities
{
    public sealed class ExperiencesMap : ClassMap<Experience>
    {
        public ExperiencesMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.JobTitle);

            References(x => x.Industry);

            Map(x => x.CompanyName).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.CompanyLocation).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.CompanyWebSite).Length(GlobalConstant.SimpleStringMaxLength);

            Map(x => x.StartDate);
            Map(x => x.EndDate);

            Map(x => x.LeaveReason).Length(GlobalConstant.MultiLinesStringMaxLength);

            Map(x => x.ReferenceFullName).Length(GlobalConstant.SimpleStringMaxLength);
            References(x => x.ReferenceJobTitle);
            Map(x => x.ReferenceContact).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.ReferenceEmail).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.ReferenceAddress).Length(GlobalConstant.SimpleStringMaxLength);

            Map(x => x.Notes).Length(GlobalConstant.MultiLinesStringMaxLength);

            References(x => x.Employee);
            HasMany(x => x.Attachments).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
        }
    }
}