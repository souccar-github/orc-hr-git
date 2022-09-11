#region

using FluentNHibernate.Mapping;
using HRIS.Domain.JobDescription.Entities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.JobDescription.Entities
{
    public sealed class PositionStatusMap : ClassMap<PositionStatus>
    {
        public PositionStatusMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.FromDate);

            Map(x => x.ExpireDate);

            Map(x => x.Comment).Length(GlobalConstant.MultiLinesStringMaxLength);

            Map(x => x.PositionStatusType);

            References(x => x.Position);
        }
    }
}