#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 04/03/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using Souccar.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
namespace HRIS.Mapping.EmployeeRelationServices.Configurations
{
    public sealed class DisciplinarySettingMap : ClassMap<DisciplinarySetting>
    {
        public DisciplinarySettingMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Name).Not.Nullable();
            Map(x => x.Order1);
            Map(x => x.IsDeductFromSalary);
            Map(x => x.DeductionType);
            Map(x => x.Value);
            Map(x => x.Description).Nullable().Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.DisciplinaryNumber);
            
            References(x => x.DisciplinaryType);
            References(x => x.WorkflowSetting);
        }
    }
}
