using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using HRIS.Domain.JobDescription.Entities;
using Souccar.Core;

namespace HRIS.Mapping.JobDescription.Entities
{
    
    public sealed class PositionReportingMap : ClassMap<PositionReporting>
    {
        public PositionReportingMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.FromDate);
            Map(x => x.ExpireDate);
            Map(x => x.IsPrimary);

            Map(x => x.Comment).Length(GlobalConstant.MultiLinesStringMaxLength);

            References(x => x.Position).Column("Position");
            References(x => x.ManagerPosition).Column("ManagerPosition");
        }
    }
}
