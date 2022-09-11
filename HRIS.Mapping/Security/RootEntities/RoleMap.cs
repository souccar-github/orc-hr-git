using FluentNHibernate.Mapping;
using Souccar.Core;
using Souccar.Domain.Security;


namespace HRIS.Mapping.Security.RootEntities
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public sealed class RoleMap : ClassMap<Role>
    {
        public RoleMap()
        {
            Table("Souccar_Security_Role");
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            #region Basic Attributes

          
            Map(x => x.Name).Unique();
            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.Enabled);

            #endregion

        }
    }
}
