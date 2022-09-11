using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.Entities;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using Souccar.Core;

namespace HRIS.Mapping.EmployeeRelationServices.Entities
{
    /// <summary>
    /// Author: Khaled Alsaadi
    /// </summary>

    public sealed class LeaveTemplateDetailMap : ClassMap<LeaveTemplateDetail>
    {
        public LeaveTemplateDetailMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            References(x => x.LeaveSetting).Not.Nullable();
            References(x => x.LeaveTemplateMaster);
            
        }
    }

}
