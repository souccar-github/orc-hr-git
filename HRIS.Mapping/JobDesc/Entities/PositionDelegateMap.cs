#region

using FluentNHibernate.Mapping;
using HRIS.Domain.JobDescription.Entities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.JobDescription.Entities
{
    public sealed class PositionDelegateMap : ClassMap<PositionDelegate>
    {
        public PositionDelegateMap()
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

            References(x => x.AuthorityType);
            References(x => x.PrimaryPosition).Column("PrimaryPosition_Id");
            References(x => x.SecondaryPosition).Column("SecondaryPosition_Id");
        }
    }
}
