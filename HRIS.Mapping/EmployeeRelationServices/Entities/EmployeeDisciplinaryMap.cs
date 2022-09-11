#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 08/03/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
namespace HRIS.Mapping.EmployeeRelationServices.Entities
{
    public sealed class EmployeeDisciplinaryMap : ClassMap<EmployeeDisciplinary>
    {
        public EmployeeDisciplinaryMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.DisciplinaryDate);
            Map(x => x.DisciplinaryReason);
            Map(x => x.Comment);
            Map(x => x.CreationDate);
            Map(x => x.DisciplinaryStatus);
            Map(x => x.IsTransferToPayroll).Default("0").Not.Nullable();

            References(x => x.WorkflowItem);
            References(x => x.DisciplinarySetting);
            References(x => x.Creator);
            References(x => x.EmployeeCard).Column("EmployeeCard_Id");
            References(x => x.AttendanceDailyAdjustment);
            References(x => x.AttendanceMonthlyAdjustment);
            References(x => x.AttendanceWithoutAdjustment);

        }
    }
}
