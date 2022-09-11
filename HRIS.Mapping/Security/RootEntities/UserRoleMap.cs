using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Souccar.Domain.Security;

namespace HRIS.Mapping.Security.RootEntities
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public sealed class UserRoleMap : ClassMap<UserRole>
    {
        public UserRoleMap()
        {
            Table("Souccar_Security_UserRole");
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion


            #region Referances

            References(x => x.Role);
            References(x => x.User);

            #endregion
        }
    }
}
