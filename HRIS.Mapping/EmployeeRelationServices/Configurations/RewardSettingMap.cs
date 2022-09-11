#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 11/03/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using FluentNHibernate.Mapping;
using HRIS.Domain.EmployeeRelationServices.RootEntities;
using HRIS.Domain.EmployeeRelationServices.Configurations;
using Souccar.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
namespace HRIS.Mapping.EmployeeRelationServices.Configurations
{
    public sealed class RewardSettingMap : ClassMap<RewardSetting>
    {
        public RewardSettingMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.Name).Not.Nullable();
            Map(x => x.Order1);
            Map(x => x.IsAddedToSalary);
            Map(x => x.IsPercentage);
            Map(x => x.FixedValue);
            Map(x => x.Percentage);
            Map(x => x.Description).Nullable().Length(GlobalConstant.MultiLinesStringMaxLength);
            
            References(x => x.RewardType);
            References(x => x.WorkflowSetting);
        }
    }
}
