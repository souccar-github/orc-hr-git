using FluentNHibernate.Mapping;
using Souccar.Domain.Security;

namespace HRIS.Mapping.Security.RootEntities
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public sealed class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("Souccar_Security_User");
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted).Default("0");
            #endregion

            Map(x => x.Username);
            Map(x => x.FullName);
            Map(x => x.Email);
            Map(x => x.FCMToken);
            Map(x => x.MobilePhone);
            Map(x => x.Comment);
            Map(x => x.IsEnabled);
            Map(x => x.IsLockedOut);
            Map(x => x.ThemingType);
            Map(x => x.IsOnline);

            #region Referances
            HasMany(x => x.Roles).Inverse().LazyLoad().Cascade.AllDeleteOrphan();

            #endregion
        }
    }
}
