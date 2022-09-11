#region

using FluentNHibernate.Mapping;
using HRIS.Domain.JobDescription.Entities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.JobDescription.Entities
{
    public sealed class RoleMap : ClassMap<Role>
    {
        public RoleMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            #region Basic Info.

            Map(x => x.Name);

            Map(x => x.Summary).Length(GlobalConstant.MultiLinesStringMaxLength);

            Map(x => x.Weight);
            

            References(x => x.JobDescription);

            #endregion

            #region Responsibilities

            HasMany(x => x.Responsibilities).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            #endregion

            #region RoleKpis

            HasMany(x => x.RoleKpis).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            #endregion

            Table("JDRole");
        }
    }
}