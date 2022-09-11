#region

using FluentNHibernate.Mapping;
using HRIS.Domain.PMS.RootEntities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.PMS.RootEntities
{
    public sealed class AppraisalSectionMap : ClassMap<AppraisalSection>
    {
        public AppraisalSectionMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            #region Basic Info.

            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.CreationDate);

            #endregion

            #region Appraisal Section Items

            HasMany(x => x.Items).Inverse().LazyLoad().Cascade.AllDeleteOrphan().KeyColumn("AppraisalSection_id");

            #endregion

        }
    }
}