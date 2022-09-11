#region

using FluentNHibernate.Mapping;
using HRIS.Domain.PMS.Entities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.PMS.Entities
{
    public sealed class AppraisalSectionItemKpiMap : ClassMap<AppraisalSectionItemKpi>
    {
        public AppraisalSectionItemKpiMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            #region Basic Info.
            Map(x => x.Value).Column("KpiValue");
            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.Weight);

            #endregion

            #region References

            References(x => x.AppraisalSectionItem).Column("AppraisalSectionItem_id");

            #endregion

        }
    }
}