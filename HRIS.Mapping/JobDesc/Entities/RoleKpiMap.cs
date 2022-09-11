#region

using System.Security.Cryptography.X509Certificates;
using FluentNHibernate.Mapping;
using HRIS.Domain.JobDescription.Entities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.JobDescription.Entities
{
    public sealed class RoleKpiMap : ClassMap<RoleKpi>
    {
        public RoleKpiMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            #region Basic Info.

          
            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);

            Map(x => x.Weight);

            Map(x => x.Value);

            References(x => x.Role);

            #endregion

        }
    }
}