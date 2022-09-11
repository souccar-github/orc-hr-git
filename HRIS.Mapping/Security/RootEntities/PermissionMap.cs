using FluentNHibernate.Mapping;
//using Souccar.Security.Domain;

namespace HRIS.Mapping.Security.RootEntities
{
    //public sealed class PermissionMap : ClassMap<Permission>
    //{
    //    public PermissionMap()
    //    {
    //        #region Basic Attributes

    //        Table("Security_Permission");

    //        DynamicUpdate();
    //        DynamicInsert();

    //        Id(x => x.Id);

    //        Map(x => x.Title);

    //        Map(x => x.Action);


    //        Map(x => x.Assembly);

    //        Map(x => x.Controller);

    //        Map(x => x.Description);

    //        #endregion

    //        #region References

    //        References(x => x.PermissionSet);

    //        HasMany(x => x.Roles).Inverse().Cascade.AllDeleteOrphan().Table("Security_PermissionRole");

    //        #endregion
    //    }
    //}
}
