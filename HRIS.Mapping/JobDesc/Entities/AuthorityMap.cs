#region

using FluentNHibernate.Mapping;
using HRIS.Domain.JobDescription.Entities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.JobDescription.Entities
{
    public sealed class AuthorityMap : ClassMap<Authority>
    {
        public AuthorityMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Code);
            Map(x => x.Name);

            References(x => x.Type);

            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);

            Map(x => x.RelatedActions);

            References(x => x.JobDescription);
        }
    }
}