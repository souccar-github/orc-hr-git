#region About
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
//*******company name: souccar for electronic industries*******//
//author: Ammar Alziebak
//description:
//start date: 26/02/2015
//end date:
//last update:
//update by:
//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//-//
#endregion
#region Namespace Reference
using FluentNHibernate.Mapping;
using HRIS.Domain.Personnel.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Souccar.Core;
#endregion
namespace HRIS.Mapping.Personnel.Configurations
{
    public sealed class EmployeeCodeSettingMap : ClassMap<EmployeeCodeSetting>
    {
        public EmployeeCodeSettingMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion

            Map(x => x.FixedPrefix).Length(GlobalConstant.SimpleStringMaxLength); 
            Map(x => x.FixedSuffix).Length(GlobalConstant.SimpleStringMaxLength); 
            Map(x => x.CustomPrefix);
            Map(x => x.CustomPrefixLength);
            Map(x => x.CustomPrefixStartingPosition);
            Map(x => x.CustomSuffix);
            Map(x => x.CustomSuffixLength);
            Map(x => x.CustomSuffixStartingPosition);
            Map(x => x.SeparatorSymbol);
            Map(x => x.Description);

        }
    }
}