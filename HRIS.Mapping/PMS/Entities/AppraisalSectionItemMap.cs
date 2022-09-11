#region

using FluentNHibernate.Mapping;
using HRIS.Domain.PMS.Entities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.PMS.Entities
{
    public sealed class AppraisalSectionItemMap : ClassMap<AppraisalSectionItem>
    {
        public AppraisalSectionItemMap()
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
            Map(x => x.Weight);

            #endregion

            #region References

            References(x => x.AppraisalSection).Column("AppraisalSection_id");

            #endregion

            #region Appraisal Section Item Kpi

            HasMany(x => x.Kpis).Inverse().LazyLoad().Cascade.AllDeleteOrphan().KeyColumn("AppraisalSectionItem_id");

            #endregion

        }
    }
}