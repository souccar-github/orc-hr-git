using System;
using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.Entities;

namespace HRIS.Mapping.EmployeeRelationServices.Entities
{
    public sealed class AssigningEmployeeToPositionMap : ClassMap<AssigningEmployeeToPosition>
    {
        public AssigningEmployeeToPositionMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            #region Basic Info.

            Map(x => x.IsPrimary);
            Map(x => x.Weight);
            Map(x => x.CreationDate);
         
            References(x => x.Position).Column("Position_Id");
            //References(x => x.JobDescription);
            References(x => x.Employee);

            #endregion

        }
    }
}