using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.Entities;
using Souccar.Core;
using Souccar.Core.Extensions;


namespace HRIS.Mapping.EmployeeRelationServices.Entities
{
    public sealed class PaidSliceMap : ClassMap<PaidSlice>
    {
        public PaidSliceMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.FromBalance);
            Map(x => x.ToBalance);
            Map(x => x.PaidPercentage);
            
            References(x => x.LeaveSetting);
            
            


        }
    }
}