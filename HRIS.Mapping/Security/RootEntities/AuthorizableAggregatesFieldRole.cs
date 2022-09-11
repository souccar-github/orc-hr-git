
using FluentNHibernate.Mapping;
using Souccar.Domain.Security;
//using Souccar.Security.Domain;

namespace HRIS.Mapping.Security.RootEntities
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public sealed class AuthorizablFieldRoleMap : ClassMap<AuthorizableFieldRole>
    {
        public AuthorizablFieldRoleMap()
        {
            Table("Souccar_Security_AuthorizableAggregatesFieldRole");
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.IsHidden);
            Map(x => x.AuthorizableElementId);
            Map(x => x.AuthorizableFieldId);
            Map(x => x.ModuleName);

            #region Referances

            References(x => x.Role);
            #endregion
        }
    }
}
