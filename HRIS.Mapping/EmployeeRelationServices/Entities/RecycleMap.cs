using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.Entities;
using Souccar.Core;
using Souccar.Core.Extensions;


namespace HRIS.Mapping.EmployeeRelationServices.Entities
{
    public sealed class RecycleMap : ClassMap<Recycle>
    {
        public RecycleMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.RecycleType);
            Map(x => x.Year);
            Map(x => x.RequestDate);

            References(x => x.LeaveSetting);




        }
    }
}