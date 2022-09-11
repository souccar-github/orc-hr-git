using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRIS.Mapping.EmployeeRelationServices.Entities
{
    public sealed class PositionsLogOfEmployeeMap : ClassMap<PositionsLogOfEmployee>
    {
        public PositionsLogOfEmployeeMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            #region Basic Info.

            Map(x => x.AssigmentType);
            Map(x => x.AssigningDate);
            Map(x => x.DisengagementType);
            Map(x => x.LeavingDate);

            Map(x => x.IsPrimary);
            Map(x => x.CreationDate);


            References(x => x.JobDescription).Column("JobDescription_Id");

            References(x => x.Position).Column("Position_Id");
            //References(x => x.JobDescription);
            References(x => x.Employee).Column("Employee_Id");

            #endregion

        }
    }
}
