#region

using FluentNHibernate.Mapping;
using HRIS.Domain.JobDescription.Entities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.JobDescription.Entities
{
    class DelegateAuthoritiesToPositionAuthorityMap : ClassMap<DelegateAuthoritiesToPositionAuthority>
    {
        public DelegateAuthoritiesToPositionAuthorityMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            #region Basic Info.

            References(x => x.Authorities).Column("Authorities_Id");

            References(x => x.Delegate).Column("DelegateAuthority_Id");

            #endregion
        }
    }
}
