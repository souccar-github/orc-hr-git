#region

using FluentNHibernate.Mapping;
using HRIS.Domain.JobDescription.Entities;
using Souccar.Core;

#endregion

namespace HRIS.Mapping.JobDescription.Entities
{
    class DelegateRolesToPositionRoleMap : ClassMap<DelegateRolesToPositionRole>
    {
        public DelegateRolesToPositionRoleMap ()
	    {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            #region Basic Info.

            References(x => x.Roles).Column("Roles_Id");

            References(x => x.Delegate).Column("DelegateRoles_Id");

            #endregion
	    }
          
    }
}
