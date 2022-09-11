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
    public sealed class DefineCustodiesDetailsMap : ClassMap<CustodieDetails>
    {
        public DefineCustodiesDetailsMap()
        {
            #region Default
            DynamicUpdate();
            DynamicInsert();
            Id(x => x.Id);
            Map(x => x.IsVertualDeleted);
            #endregion
            
            Map(x => x.Name).Length(GlobalConstant.SimpleStringMaxLength);
            Map(x => x.Description).Length(GlobalConstant.MultiLinesStringMaxLength);
            Map(x => x.Cost);
            Map(x => x.PurchaseDate);
            Map(x => x.DepreciationPeriod);
            Map(x => x.Period);

            References(x => x.CustodiesType);
            References(x => x.Currency);
            References(x => x.Supplier);
        }
    }
}