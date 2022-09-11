#region

using FluentNHibernate.Mapping;
using HRIS.Domain.JobDescription.Entities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.JobDescription.Entities
{
    public sealed class RestrictionMap : ClassMap<Restriction>
    {
        public RestrictionMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Code);

            References(x => x.Type);

            Map(x => x.Required);

            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);

            References(x => x.WorkingRestriction);
        }
    }
}