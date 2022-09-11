
using FluentNHibernate.Mapping;
using Souccar.Domain.Security;
//using Souccar.Security.Domain;

namespace HRIS.Mapping.Security.RootEntities
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public sealed class AuthorizableElementRoleMap : ClassMap<AuthorizableElementRole>
    {
        public AuthorizableElementRoleMap()
        {
            Table("Souccar_Security_AuthorizableElementRole");
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.AuthorizeType);
            Map(x => x.AuthorizableElementId);
            Map(x => x.AuthorizableElementType);
            Map(x => x.ModuleName);

            #region Referances

            References(x => x.Role);
            #endregion
        }
    }
}
