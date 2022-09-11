using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using Souccar.Core;

namespace HRIS.Mapping.EmployeeRelationServices.Configurations
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    public sealed class LeaveTemplateMasterMap : ClassMap<LeaveTemplateMaster>
    {
        public LeaveTemplateMasterMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Name).Not.Nullable();
            Map(x => x.Description);
            Map(x => x.InsertDate).Default("getDate()").Not.Nullable();

            HasMany(x => x.LeaveTemplateDetails).Inverse().LazyLoad().Cascade.AllDeleteOrphan();
            
        }
    }

}
